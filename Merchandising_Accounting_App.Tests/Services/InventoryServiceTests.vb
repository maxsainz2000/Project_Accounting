Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Merchandising_Accounting_App.Data
Imports Merchandising_Accounting_App.Domain
Imports Merchandising_Accounting_App.Domain.Engines
Imports Merchandising_Accounting_App.Domain.Models
Imports Merchandising_Accounting_App.Services
Imports Merchandising_Accounting_App.Tests.Infrastructure

Namespace Services

    <TestClass>
    Public Class InventoryServiceTests

        Private _fixture As TestDatabaseFixture

        <TestInitialize>
        Public Sub Initialize()
            _fixture = New TestDatabaseFixture()
        End Sub

        <TestCleanup>
        Public Sub Cleanup()
            _fixture?.Dispose()
        End Sub

        <TestMethod>
        Public Sub Constructor_InitializesCorrectly()
            Using context = _fixture.CreateContext()
                Dim service As New InventoryService(context)
                Assert.IsNotNull(service)
            End Using
        End Sub

        <TestMethod>
        Public Sub RecordInventoryAdjustment_Shrinkage_DecrementsStockPostsBalancedGLAndRecordsAudit()
            Using context = _fixture.CreateContext()
                Dim service As New InventoryService(context)

                Dim product As New Product("SKU-ADJ-01", "Widget Shrink", "Each", 15.0D, 100)
                context.Products.Add(product)
                context.SaveChanges()

                Dim lines As New List(Of InventoryAdjustmentLine) From {
                    New InventoryAdjustmentLine() With {
                        .ProductId = product.ProductId,
                        .QtyChange = -5,
                        .UnitCost = 15.0D,
                        .Notes = "Damaged during handling"
                    }
                }

                Dim adj = service.RecordInventoryAdjustment("ADJ-TEST-001", "Shrinkage", lines, New DateTime(2026, 11, 10), "Monthly cycle count")

                Assert.IsNotNull(adj)
                Assert.AreEqual("ADJ-TEST-001", adj.DocumentNumber)
                Assert.AreEqual("Shrinkage", adj.ReasonCode)
                Assert.AreEqual("Posted", adj.Status)
                Assert.AreEqual(1, adj.Lines.Count)

                ' Verify physical stock decremented atomically: 100 - 5 = 95
                Dim reloadedProduct = context.Products.Find(product.ProductId)
                Assert.AreEqual(95, reloadedProduct.TotalQtyOnHand)

                ' Verify InventoryTransaction audit record appended
                Dim tx = context.InventoryTransactions.FirstOrDefault(Function(t) t.SourceDocId = adj.AdjustmentId AndAlso t.SourceDocType = "InventoryAdjustment")
                Assert.IsNotNull(tx)
                Assert.AreEqual(-5, tx.QtyChange)
                Assert.AreEqual(95, tx.RunningQty)
                Assert.AreEqual(15.0D, tx.UnitCost)

                ' Verify General Ledger Journal Entry: Debit 5300 ($75.00), Credit 1300 ($75.00)
                Dim je = context.JournalEntries.FirstOrDefault(Function(j) j.SourceDocId = adj.AdjustmentId AndAlso j.SourceDocType = "InventoryAdjustment")
                Assert.IsNotNull(je)
                Assert.AreEqual(2, je.Lines.Count)

                Dim drLine = je.Lines.FirstOrDefault(Function(l) l.DebitAmount > 0D)
                Dim crLine = je.Lines.FirstOrDefault(Function(l) l.CreditAmount > 0D)
                Assert.IsNotNull(drLine)
                Assert.IsNotNull(crLine)

                Dim shrinkageAcct = context.Accounts.First(Function(a) a.Code = "5300")
                Dim inventoryAcct = context.Accounts.First(Function(a) a.Code = "1300")
                Assert.AreEqual(shrinkageAcct.AccountId, drLine.AccountId)
                Assert.AreEqual(75.0D, drLine.DebitAmount)
                Assert.AreEqual(inventoryAcct.AccountId, crLine.AccountId)
                Assert.AreEqual(75.0D, crLine.CreditAmount)
            End Using
        End Sub

        <TestMethod>
        Public Sub RecordInventoryAdjustment_Surplus_IncrementsStockPostsBalancedGLAndRecordsAudit()
            Using context = _fixture.CreateContext()
                Dim service As New InventoryService(context)

                Dim product As New Product("SKU-ADJ-02", "Widget Surplus", "Each", 20.0D, 50)
                context.Products.Add(product)
                context.SaveChanges()

                Dim lines As New List(Of InventoryAdjustmentLine) From {
                    New InventoryAdjustmentLine() With {
                        .ProductId = product.ProductId,
                        .QtyChange = 10,
                        .UnitCost = 20.0D,
                        .Notes = "Physical count surplus found"
                    }
                }

                Dim adj = service.RecordInventoryAdjustment("ADJ-TEST-002", "CountSurplus", lines, New DateTime(2026, 11, 10))

                Assert.IsNotNull(adj)
                Assert.AreEqual("CountSurplus", adj.ReasonCode)

                ' Verify physical stock incremented atomically: 50 + 10 = 60
                Dim reloadedProduct = context.Products.Find(product.ProductId)
                Assert.AreEqual(60, reloadedProduct.TotalQtyOnHand)

                ' Verify General Ledger Journal Entry: Debit 1300 ($200.00), Credit 5400 ($200.00)
                Dim je = context.JournalEntries.FirstOrDefault(Function(j) j.SourceDocId = adj.AdjustmentId AndAlso j.SourceDocType = "InventoryAdjustment")
                Assert.IsNotNull(je)
                Assert.AreEqual(2, je.Lines.Count)

                Dim drLine = je.Lines.FirstOrDefault(Function(l) l.DebitAmount > 0D)
                Dim crLine = je.Lines.FirstOrDefault(Function(l) l.CreditAmount > 0D)
                Assert.IsNotNull(drLine)
                Assert.IsNotNull(crLine)

                Dim inventoryAcct = context.Accounts.First(Function(a) a.Code = "1300")
                Dim surplusAcct = context.Accounts.First(Function(a) a.Code = "5400")
                Assert.AreEqual(inventoryAcct.AccountId, drLine.AccountId)
                Assert.AreEqual(200.0D, drLine.DebitAmount)
                Assert.AreEqual(surplusAcct.AccountId, crLine.AccountId)
                Assert.AreEqual(200.0D, crLine.CreditAmount)
            End Using
        End Sub

        <TestMethod>
        Public Sub RecordInventoryAdjustment_ShrinkageExceedingStock_ThrowsInsufficientStockException()
            Using context = _fixture.CreateContext()
                Dim service As New InventoryService(context)

                Dim product As New Product("SKU-ADJ-03", "Widget Deficit", "Each", 10.0D, 5)
                context.Products.Add(product)
                context.SaveChanges()

                ' Attempt to reduce 10 units when only 5 exist
                Dim lines As New List(Of InventoryAdjustmentLine) From {
                    New InventoryAdjustmentLine() With {
                        .ProductId = product.ProductId,
                        .QtyChange = -10,
                        .UnitCost = 10.0D
                    }
                }

                Assert.Throws(Of InsufficientStockException)(Sub()
                    service.RecordInventoryAdjustment("ADJ-FAIL", "Shrinkage", lines)
                End Sub)

                ' Verify physical stock unchanged
                Dim reloaded = context.Products.Find(product.ProductId)
                Assert.AreEqual(5, reloaded.TotalQtyOnHand)
            End Using
        End Sub

        <TestMethod>
        Public Sub RecordInventoryAdjustment_InvalidInputs_ThrowsAppropriateExceptions()
            Using context = _fixture.CreateContext()
                Dim service As New InventoryService(context)

                ' Empty reason
                Assert.Throws(Of ArgumentException)(Sub()
                    service.RecordInventoryAdjustment("DOC-01", "", New List(Of InventoryAdjustmentLine) From {
                        New InventoryAdjustmentLine() With {.ProductId = 1, .QtyChange = 1}
                    })
                End Sub)

                ' Null lines
                Assert.Throws(Of ArgumentNullException)(Sub()
                    service.RecordInventoryAdjustment("DOC-02", "Shrinkage", Nothing)
                End Sub)

                ' Empty lines
                Assert.Throws(Of ArgumentException)(Sub()
                    service.RecordInventoryAdjustment("DOC-03", "Shrinkage", New List(Of InventoryAdjustmentLine)())
                End Sub)

                ' All zero quantity lines
                Assert.Throws(Of ArgumentException)(Sub()
                    service.RecordInventoryAdjustment("DOC-04", "Shrinkage", New List(Of InventoryAdjustmentLine) From {
                        New InventoryAdjustmentLine() With {.ProductId = 1, .QtyChange = 0}
                    })
                End Sub)

                ' Non-existent product
                Assert.Throws(Of ArgumentException)(Sub()
                    service.RecordInventoryAdjustment("DOC-05", "Shrinkage", New List(Of InventoryAdjustmentLine) From {
                        New InventoryAdjustmentLine() With {.ProductId = 99999, .QtyChange = -1}
                    })
                End Sub)
            End Using
        End Sub

        <TestMethod>
        Public Sub RecordInventoryAdjustment_ClosedPeriod_AutoShiftsToFirstOpenDay()
            Using context = _fixture.CreateContext()
                Dim periodService As New PeriodService(context)
                Dim service As New InventoryService(context, periodService)

                ' Ensure period 10 (Oct) is closed and period 11 (Nov) is active
                Dim p10 = context.AccountingPeriods.FirstOrDefault(Function(p) p.FiscalYear = 2026 AndAlso p.PeriodNumber = 10)
                If p10 Is Nothing Then
                    p10 = New AccountingPeriod(2026, 10, New DateTime(2026, 10, 1), New DateTime(2026, 10, 31, 23, 59, 59))
                    context.AccountingPeriods.Add(p10)
                    context.SaveChanges()
                End If
                periodService.CloseFiscalPeriod(p10.PeriodId)

                Dim product As New Product("SKU-ADJ-OCT", "October Product", "Each", 10.0D, 50)
                context.Products.Add(product)
                context.SaveChanges()

                Dim lines As New List(Of InventoryAdjustmentLine) From {
                    New InventoryAdjustmentLine() With {
                        .ProductId = product.ProductId,
                        .QtyChange = 5,
                        .UnitCost = 10.0D
                    }
                }

                ' Physical count occurred in closed October period
                Dim octDate As New DateTime(2026, 10, 15)
                Dim adj = service.RecordInventoryAdjustment("ADJ-OCT-01", "CountSurplus", lines, octDate)

                Assert.IsNotNull(adj)
                Assert.AreEqual(octDate, adj.AdjustmentDate)
                ' Statutory posting date auto-shifted to Nov 1 (StartDate of open active period)
                Assert.AreEqual(New DateTime(2026, 11, 1), adj.PostingDate)
            End Using
        End Sub

        <TestMethod>
        Public Sub QueryMethods_GetAdjustmentById_And_GetAllAdjustments()
            Using context = _fixture.CreateContext()
                Dim service As New InventoryService(context)

                Dim product As New Product("SKU-ADJ-QRY", "Query Product", "Each", 25.0D, 100)
                context.Products.Add(product)
                context.SaveChanges()

                Dim lines As New List(Of InventoryAdjustmentLine) From {
                    New InventoryAdjustmentLine() With {.ProductId = product.ProductId, .QtyChange = -2, .UnitCost = 25.0D}
                }

                Dim created = service.RecordInventoryAdjustment("ADJ-QRY-001", "Shrinkage", lines)

                Dim fetched = service.GetAdjustmentById(created.AdjustmentId)
                Assert.IsNotNull(fetched)
                Assert.AreEqual("ADJ-QRY-001", fetched.DocumentNumber)
                Assert.AreEqual(1, fetched.Lines.Count)

                Dim all = service.GetAllAdjustments()
                Assert.IsTrue(all.Any(Function(a) a.AdjustmentId = created.AdjustmentId))
            End Using
        End Sub

    End Class

End Namespace
