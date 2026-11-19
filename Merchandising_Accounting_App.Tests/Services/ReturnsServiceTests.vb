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
    Public Class ReturnsServiceTests

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
        Public Sub Constructor_Parameterless_InitializesCorrectly()
            Dim service As New ReturnsService()
            Assert.IsNotNull(service)
        End Sub

        ' =========================================================================
        ' ReceiveReturnToQuarantine Tests
        ' =========================================================================

        <TestMethod>
        Public Sub ReceiveReturnToQuarantine_ValidLines_IncrementsQuarantineQtyWithoutGLPosting()
            Using context = _fixture.CreateContext()
                Dim o2cService As New OrderToCashService(context)
                Dim returnService As New ReturnsService(context)

                Dim customer As New Customer("CUST-RET-01", "Return Customer 1", 5000.0D, 0D)
                context.Customers.Add(customer)
                Dim product As New Product("SKU-RET-10", "Widget Returnable", "Each", 10.0D, 50)
                context.Products.Add(product)
                context.SaveChanges()

                ' Initial Sales Invoice: 10 units @ $25.00
                Dim postingDate As New DateTime(2026, 11, 15)
                Dim invLines As New List(Of SalesInvoiceLine) From {
                    New SalesInvoiceLine() With {.ProductId = product.ProductId, .FulfilledQty = 10, .UnitPrice = 25.0D}
                }
                Dim invoice = o2cService.FulfillSalesInvoice(0, invLines, postingDate, customer.CustomerId)

                Dim glCountBeforeReturn As Integer = context.JournalEntries.Count()

                ' Receive return into quarantine
                Dim returnLines As New List(Of CustomerReturnLine) From {
                    New CustomerReturnLine() With {.ProductId = product.ProductId, .ReturnedQty = 10}
                }

                Dim retDoc = returnService.ReceiveReturnToQuarantine(customer.CustomerId, invoice.InvoiceId, returnLines, postingDate, "RMA-TEST-001")

                Assert.IsNotNull(retDoc)
                Assert.AreEqual("RMA-TEST-001", retDoc.DocumentNumber)
                Assert.AreEqual("Quarantine", retDoc.Status)
                Assert.AreEqual(1, retDoc.Lines.Count)

                Dim retLine = retDoc.Lines.First()
                Assert.AreEqual(10, retLine.ReturnedQty)
                Assert.AreEqual(0, retLine.RestockQty)
                Assert.AreEqual(0, retLine.ScrapQty)
                Assert.AreEqual(25.0D, retLine.SellingUnitPrice)
                Assert.AreEqual(10.0D, retLine.OriginalUnitCost)
                Assert.AreEqual("Quarantine", retLine.DispositionStatus)

                ' Product stock checks:
                ' TotalQtyOnHand is unchanged (still 40 from prior invoice fulfillment)
                ' QuarantineQty incremented to 10
                Dim reloadedProduct = context.Products.Find(product.ProductId)
                Assert.AreEqual(40, reloadedProduct.TotalQtyOnHand)
                Assert.AreEqual(10, reloadedProduct.QuarantineQty)

                ' CRUCIAL INVARIANT: 0 General Ledger entries generated during quarantine intake
                Dim glCountAfterReturn As Integer = context.JournalEntries.Count()
                Assert.AreEqual(glCountBeforeReturn, glCountAfterReturn)
            End Using
        End Sub

        <TestMethod>
        Public Sub ReceiveReturnToQuarantine_NullOrEmptyLines_ThrowsExceptions()
            Using context = _fixture.CreateContext()
                Dim returnService As New ReturnsService(context)

                Assert.Throws(Of ArgumentNullException)(Sub()
                    returnService.ReceiveReturnToQuarantine(1, 1, Nothing)
                End Sub)

                Assert.Throws(Of ArgumentException)(Sub()
                    returnService.ReceiveReturnToQuarantine(1, 1, New List(Of CustomerReturnLine)())
                End Sub)
            End Using
        End Sub

        <TestMethod>
        Public Sub ReceiveReturnToQuarantine_InvalidCustomerOrInvoice_ThrowsExceptions()
            Using context = _fixture.CreateContext()
                Dim returnService As New ReturnsService(context)
                Dim validLines As New List(Of CustomerReturnLine) From {New CustomerReturnLine() With {.ProductId = 1, .ReturnedQty = 5}}

                ' Invalid CustomerId <= 0
                Assert.Throws(Of ArgumentException)(Sub()
                    returnService.ReceiveReturnToQuarantine(0, 1, validLines)
                End Sub)

                ' Invalid InvoiceId <= 0
                Assert.Throws(Of ArgumentException)(Sub()
                    returnService.ReceiveReturnToQuarantine(1, 0, validLines)
                End Sub)

                ' Customer not found
                Assert.Throws(Of ArgumentException)(Sub()
                    returnService.ReceiveReturnToQuarantine(9999, 1, validLines)
                End Sub)

                ' Invoice not found
                Dim customer As New Customer("CUST-RET-02", "Customer 2", 5000.0D, 0D)
                context.Customers.Add(customer)
                context.SaveChanges()

                Assert.Throws(Of ArgumentException)(Sub()
                    returnService.ReceiveReturnToQuarantine(customer.CustomerId, 9999, validLines)
                End Sub)

                ' Invoice belongs to another customer
                Dim customer3 As New Customer("CUST-RET-03", "Customer 3", 5000.0D, 0D)
                context.Customers.Add(customer3)
                Dim prod As New Product("SKU-RET-20", "Widget X", "Each", 10.0D, 50)
                context.Products.Add(prod)
                context.SaveChanges()

                Dim o2cService As New OrderToCashService(context)
                Dim inv = o2cService.FulfillSalesInvoice(0, New List(Of SalesInvoiceLine) From {
                    New SalesInvoiceLine() With {.ProductId = prod.ProductId, .FulfilledQty = 5, .UnitPrice = 20.0D}
                }, New DateTime(2026, 11, 15), customer3.CustomerId)

                Assert.Throws(Of InvalidOperationException)(Sub()
                    returnService.ReceiveReturnToQuarantine(customer.CustomerId, inv.InvoiceId, validLines)
                End Sub)
            End Using
        End Sub

        <TestMethod>
        Public Sub ReceiveReturnToQuarantine_InvalidLineProperties_ThrowsExceptions()
            Using context = _fixture.CreateContext()
                Dim o2cService As New OrderToCashService(context)
                Dim returnService As New ReturnsService(context)

                Dim customer As New Customer("CUST-RET-04", "Customer 4", 5000.0D, 0D)
                context.Customers.Add(customer)
                Dim product As New Product("SKU-RET-30", "Widget Y", "Each", 10.0D, 50)
                context.Products.Add(product)
                context.SaveChanges()

                Dim invoice = o2cService.FulfillSalesInvoice(0, New List(Of SalesInvoiceLine) From {
                    New SalesInvoiceLine() With {.ProductId = product.ProductId, .FulfilledQty = 5, .UnitPrice = 20.0D}
                }, New DateTime(2026, 11, 15), customer.CustomerId)

                ' Zero/Negative ProductId
                Assert.Throws(Of ArgumentException)(Sub()
                    returnService.ReceiveReturnToQuarantine(customer.CustomerId, invoice.InvoiceId, New List(Of CustomerReturnLine) From {
                        New CustomerReturnLine() With {.ProductId = 0, .ReturnedQty = 5}
                    })
                End Sub)

                ' Zero/Negative ReturnedQty
                Assert.Throws(Of ArgumentOutOfRangeException)(Sub()
                    returnService.ReceiveReturnToQuarantine(customer.CustomerId, invoice.InvoiceId, New List(Of CustomerReturnLine) From {
                        New CustomerReturnLine() With {.ProductId = product.ProductId, .ReturnedQty = 0}
                    })
                End Sub)

                ' Product not found
                Assert.Throws(Of ArgumentException)(Sub()
                    returnService.ReceiveReturnToQuarantine(customer.CustomerId, invoice.InvoiceId, New List(Of CustomerReturnLine) From {
                        New CustomerReturnLine() With {.ProductId = 9999, .ReturnedQty = 5}
                    })
                End Sub)
            End Using
        End Sub

        ' =========================================================================
        ' ProcessReturnInspection Tests (CUJ 3 Matching)
        ' =========================================================================

        <TestMethod>
        Public Sub ProcessReturnInspection_CUJ3_RestockAndScrap_RecalculatesMovingAverageAndPostsCreditNote()
            Using context = _fixture.CreateContext()
                Dim o2cService As New OrderToCashService(context)
                Dim returnService As New ReturnsService(context)

                ' CUJ 3 Specification:
                ' Given: Customer returns 10 units of SKU-A originally purchased at $15.00/unit (original unit cost $10.00/unit).
                ' Product on hand has 40 units at $10.00/unit.
                Dim customer As New Customer("CUST-CUJ3", "CUJ3 Retailer", 10000.0D, 0D)
                context.Customers.Add(customer)
                Dim product As New Product("SKU-A", "Standard Pack A", "Each", 10.0D, 50)
                context.Products.Add(product)
                context.SaveChanges()

                ' Sales Invoice for 10 units @ $15.00 (COGS $10.00)
                Dim postingDate As New DateTime(2026, 11, 15)
                Dim invoice = o2cService.FulfillSalesInvoice(0, New List(Of SalesInvoiceLine) From {
                    New SalesInvoiceLine() With {.ProductId = product.ProductId, .FulfilledQty = 10, .UnitPrice = 15.0D}
                }, postingDate, customer.CustomerId)

                ' Product TotalQtyOnHand is now 40, CurrentUnitCost is $10.00. Customer AR balance is $150.00.
                Assert.AreEqual(40, context.Products.Find(product.ProductId).TotalQtyOnHand)
                Assert.AreEqual(150.0D, context.Customers.Find(customer.CustomerId).CurrentARBalance)

                ' Step 1: Intake 10 units to Quarantine
                Dim rma = returnService.ReceiveReturnToQuarantine(customer.CustomerId, invoice.InvoiceId, New List(Of CustomerReturnLine) From {
                    New CustomerReturnLine() With {.ProductId = product.ProductId, .ReturnedQty = 10}
                }, postingDate, "RMA-2026-0012")

                Assert.AreEqual(10, context.Products.Find(product.ProductId).QuarantineQty)
                Assert.AreEqual(40, context.Products.Find(product.ProductId).TotalQtyOnHand)

                ' Step 2: Inspection disposition: 8 units restockable, 2 units damaged (scrap)
                Dim dispositions As New List(Of ReturnDispositionLine) From {
                    New ReturnDispositionLine(product.ProductId, restockQty:=8, scrapQty:=2)
                }

                Dim inspectedReturn = returnService.ProcessReturnInspection(rma.ReturnId, dispositions, postingDate)

                Assert.IsNotNull(inspectedReturn)
                Assert.AreEqual("Posted", inspectedReturn.Status)

                ' Check Product updates:
                ' Quarantine stock decremented from 10 to 0
                ' TotalQtyOnHand incremented by 8 (from 40 to 48)
                ' Moving average cost: ((40 * 10) + (8 * 10)) / 48 = 10.00
                Dim finalProduct = context.Products.Find(product.ProductId)
                Assert.AreEqual(0, finalProduct.QuarantineQty)
                Assert.AreEqual(48, finalProduct.TotalQtyOnHand)
                Assert.AreEqual(10.0D, finalProduct.CurrentUnitCost)

                ' Check InventoryTransaction for restocked 8 units
                Dim invTx = context.InventoryTransactions.FirstOrDefault(Function(t) t.SourceDocId = inspectedReturn.ReturnId AndAlso t.SourceDocType = "CustomerReturn")
                Assert.IsNotNull(invTx)
                Assert.AreEqual(8, invTx.QtyChange)
                Assert.AreEqual(48, invTx.RunningQty)
                Assert.AreEqual(10.0D, invTx.UnitCost)

                ' Check Credit Note General Ledger Journal Entry:
                ' Debit: 4100 Sales Returns & Allowances $150.00
                ' Credit: 1200 Accounts Receivable $150.00
                ' Debit: 1300 Merchandise Inventory $80.00 (8 * $10.00)
                ' Debit: 5300 Inventory Shrinkage & Damaged Stock $20.00 (2 * $10.00)
                ' Credit: 5000 Cost of Goods Sold $100.00 (10 * $10.00)
                Dim creditNoteJournal = context.JournalEntries.FirstOrDefault(Function(j) j.SourceDocId = inspectedReturn.ReturnId AndAlso j.SourceDocType = "CustomerReturn")
                Assert.IsNotNull(creditNoteJournal)
                Assert.AreEqual(5, creditNoteJournal.Lines.Count)

                Dim retAllowanceLine = creditNoteJournal.Lines.First(Function(l) l.Account.Code = "4100")
                Assert.AreEqual(150.0D, retAllowanceLine.DebitAmount)
                Assert.AreEqual(0D, retAllowanceLine.CreditAmount)

                Dim arLine = creditNoteJournal.Lines.First(Function(l) l.Account.Code = "1200")
                Assert.AreEqual(0D, arLine.DebitAmount)
                Assert.AreEqual(150.0D, arLine.CreditAmount)

                Dim inventoryLine = creditNoteJournal.Lines.First(Function(l) l.Account.Code = "1300")
                Assert.AreEqual(80.0D, inventoryLine.DebitAmount)
                Assert.AreEqual(0D, inventoryLine.CreditAmount)

                Dim shrinkageLine = creditNoteJournal.Lines.First(Function(l) l.Account.Code = "5300")
                Assert.AreEqual(20.0D, shrinkageLine.DebitAmount)
                Assert.AreEqual(0D, shrinkageLine.CreditAmount)

                Dim cogsLine = creditNoteJournal.Lines.First(Function(l) l.Account.Code = "5000")
                Assert.AreEqual(0D, cogsLine.DebitAmount)
                Assert.AreEqual(100.0D, cogsLine.CreditAmount)

                ' Verify strict balance: Total Debits (150 + 80 + 20 = 250) == Total Credits (150 + 100 = 250)
                Assert.IsTrue(LedgerPostingEngine.IsBalanced(creditNoteJournal.Lines))

                ' Verify Customer AR subledger: $150 invoice - $150 credit note = $0 balance
                Dim finalCustomer = context.Customers.Find(customer.CustomerId)
                Assert.AreEqual(0D, finalCustomer.CurrentARBalance)
            End Using
        End Sub

        <TestMethod>
        Public Sub ProcessReturnInspection_MovingAverageCostBlending_WithDifferentCosts()
            Using context = _fixture.CreateContext()
                Dim o2cService As New OrderToCashService(context)
                Dim returnService As New ReturnsService(context)

                Dim customer As New Customer("CUST-BLEND", "Blend Customer", 10000.0D, 0D)
                context.Customers.Add(customer)
                ' Product currently has 10 units on hand at $20.00/unit
                Dim product As New Product("SKU-BLEND", "Blended Widget", "Each", 20.0D, 20)
                context.Products.Add(product)
                context.SaveChanges()

                ' Sell 10 units at original cost $10.00
                ' Set product current cost to $10 first for the invoice
                product.CurrentUnitCost = 10.0D
                context.SaveChanges()

                Dim invoice = o2cService.FulfillSalesInvoice(0, New List(Of SalesInvoiceLine) From {
                    New SalesInvoiceLine() With {.ProductId = product.ProductId, .FulfilledQty = 10, .UnitPrice = 30.0D}
                }, New DateTime(2026, 11, 15), customer.CustomerId)

                ' Now simulate that subsequent purchase increased current cost to $20.00 for the remaining 10 units
                product.CurrentUnitCost = 20.0D
                context.SaveChanges()

                ' Receive return of 10 units (original cost was $10.00)
                Dim rma = returnService.ReceiveReturnToQuarantine(customer.CustomerId, invoice.InvoiceId, New List(Of CustomerReturnLine) From {
                    New CustomerReturnLine() With {.ProductId = product.ProductId, .ReturnedQty = 10}
                }, New DateTime(2026, 11, 15))

                ' Restock all 10 units:
                ' Formula: ((10 * $20.00) + (10 * $10.00)) / (10 + 10) = 300 / 20 = $15.00
                returnService.ProcessReturnInspection(rma.ReturnId, New List(Of ReturnDispositionLine) From {
                    New ReturnDispositionLine(product.ProductId, restockQty:=10, scrapQty:=0)
                }, New DateTime(2026, 11, 15))

                Dim updatedProduct = context.Products.Find(product.ProductId)
                Assert.AreEqual(20, updatedProduct.TotalQtyOnHand)
                Assert.AreEqual(15.0D, updatedProduct.CurrentUnitCost)
            End Using
        End Sub

        <TestMethod>
        Public Sub ProcessReturnInspection_AllScrap_DoesNotIncrementOnHandStock()
            Using context = _fixture.CreateContext()
                Dim o2cService As New OrderToCashService(context)
                Dim returnService As New ReturnsService(context)

                Dim customer As New Customer("CUST-SCRAP", "Scrap Customer", 10000.0D, 0D)
                context.Customers.Add(customer)
                Dim product As New Product("SKU-SCRAP", "Fragile Glass", "Each", 12.0D, 30)
                context.Products.Add(product)
                context.SaveChanges()

                Dim invoice = o2cService.FulfillSalesInvoice(0, New List(Of SalesInvoiceLine) From {
                    New SalesInvoiceLine() With {.ProductId = product.ProductId, .FulfilledQty = 5, .UnitPrice = 25.0D}
                }, New DateTime(2026, 11, 15), customer.CustomerId)

                ' 25 on hand
                Assert.AreEqual(25, context.Products.Find(product.ProductId).TotalQtyOnHand)

                Dim rma = returnService.ReceiveReturnToQuarantine(customer.CustomerId, invoice.InvoiceId, New List(Of CustomerReturnLine) From {
                    New CustomerReturnLine() With {.ProductId = product.ProductId, .ReturnedQty = 5}
                }, New DateTime(2026, 11, 15))

                ' Inspect: all 5 scrapped (0 restock)
                returnService.ProcessReturnInspection(rma.ReturnId, New List(Of ReturnDispositionLine) From {
                    New ReturnDispositionLine(product.ProductId, restockQty:=0, scrapQty:=5)
                }, New DateTime(2026, 11, 15))

                ' TotalQtyOnHand remains 25!
                Dim reloadedProduct = context.Products.Find(product.ProductId)
                Assert.AreEqual(25, reloadedProduct.TotalQtyOnHand)
                Assert.AreEqual(0, reloadedProduct.QuarantineQty)

                ' Journal contains 5300 Shrinkage debit of $60 (5 * $12) and 5000 COGS credit of $60
                Dim journal = context.JournalEntries.First(Function(j) j.SourceDocId = rma.ReturnId AndAlso j.SourceDocType = "CustomerReturn")
                Dim shrinkageLine = journal.Lines.First(Function(l) l.Account.Code = "5300")
                Assert.AreEqual(60.0D, shrinkageLine.DebitAmount)
                Dim cogsLine = journal.Lines.First(Function(l) l.Account.Code = "5000")
                Assert.AreEqual(60.0D, cogsLine.CreditAmount)
            End Using
        End Sub

        <TestMethod>
        Public Sub ProcessReturnInspection_ClosedPeriod_ThrowsClosedPeriodPostingException()
            Using context = _fixture.CreateContext()
                Dim periodService As New PeriodService(context)
                Dim o2cService As New OrderToCashService(context, periodService)
                Dim returnService As New ReturnsService(context, periodService)

                Dim customer As New Customer("CUST-CLOSED", "Closed Period Customer", 5000.0D, 0D)
                context.Customers.Add(customer)
                Dim product As New Product("SKU-CLOSED", "Item Z", "Each", 10.0D, 50)
                context.Products.Add(product)
                context.SaveChanges()

                Dim invoice = o2cService.FulfillSalesInvoice(0, New List(Of SalesInvoiceLine) From {
                    New SalesInvoiceLine() With {.ProductId = product.ProductId, .FulfilledQty = 5, .UnitPrice = 20.0D}
                }, New DateTime(2026, 11, 15), customer.CustomerId)

                Dim rma = returnService.ReceiveReturnToQuarantine(customer.CustomerId, invoice.InvoiceId, New List(Of CustomerReturnLine) From {
                    New CustomerReturnLine() With {.ProductId = product.ProductId, .ReturnedQty = 5}
                }, New DateTime(2026, 11, 15))

                ' Close period
                Dim activePeriod = periodService.GetActiveFiscalPeriod()
                periodService.CloseFiscalPeriod(activePeriod.PeriodId)

                ' Attempt to inspect and post in closed period
                Assert.Throws(Of ClosedPeriodPostingException)(Sub()
                    returnService.ProcessReturnInspection(rma.ReturnId, New List(Of ReturnDispositionLine) From {
                        New ReturnDispositionLine(product.ProductId, 5, 0)
                    }, New DateTime(2026, 11, 15))
                End Sub)
            End Using
        End Sub

        <TestMethod>
        Public Sub ProcessReturnInspection_OverloadWithCustomerReturnLines_Succeeds()
            Using context = _fixture.CreateContext()
                Dim o2cService As New OrderToCashService(context)
                Dim returnService As New ReturnsService(context)

                Dim customer As New Customer("CUST-OVERLOAD", "Overload Customer", 5000.0D, 0D)
                context.Customers.Add(customer)
                Dim product As New Product("SKU-OVERLOAD", "Item W", "Each", 10.0D, 50)
                context.Products.Add(product)
                context.SaveChanges()

                Dim invoice = o2cService.FulfillSalesInvoice(0, New List(Of SalesInvoiceLine) From {
                    New SalesInvoiceLine() With {.ProductId = product.ProductId, .FulfilledQty = 4, .UnitPrice = 20.0D}
                }, New DateTime(2026, 11, 15), customer.CustomerId)

                Dim rma = returnService.ReceiveReturnToQuarantine(customer.CustomerId, invoice.InvoiceId, New List(Of CustomerReturnLine) From {
                    New CustomerReturnLine() With {.ProductId = product.ProductId, .ReturnedQty = 4}
                }, New DateTime(2026, 11, 15))

                ' Call overload passing CustomerReturnLine instances
                Dim dispLines As New List(Of CustomerReturnLine) From {
                    New CustomerReturnLine() With {.ProductId = product.ProductId, .RestockQty = 3, .ScrapQty = 1}
                }
                Dim result = returnService.ProcessReturnInspection(rma.ReturnId, dispLines, New DateTime(2026, 11, 15))

                Assert.AreEqual("Posted", result.Status)
                Assert.AreEqual(49, context.Products.Find(product.ProductId).TotalQtyOnHand) ' 46 + 3 = 49
            End Using
        End Sub

        <TestMethod>
        Public Sub ProcessReturnInspection_InvalidParameters_ThrowsAppropriateExceptions()
            Using context = _fixture.CreateContext()
                Dim o2cService As New OrderToCashService(context)
                Dim returnService As New ReturnsService(context)

                ' Return ID <= 0
                Assert.Throws(Of ArgumentException)(Sub()
                    returnService.ProcessReturnInspection(0, New List(Of ReturnDispositionLine) From {New ReturnDispositionLine(1, 1, 0)})
                End Sub)

                ' Return ID not found
                Assert.Throws(Of ArgumentException)(Sub()
                    returnService.ProcessReturnInspection(9999, New List(Of ReturnDispositionLine) From {New ReturnDispositionLine(1, 1, 0)})
                End Sub)

                ' Null dispositions
                Assert.Throws(Of ArgumentNullException)(Sub()
                    returnService.ProcessReturnInspection(1, CType(Nothing, IEnumerable(Of ReturnDispositionLine)))
                End Sub)
                Assert.Throws(Of ArgumentNullException)(Sub()
                    returnService.ProcessReturnInspection(1, CType(Nothing, IEnumerable(Of CustomerReturnLine)))
                End Sub)

                ' Empty dispositions
                Assert.Throws(Of ArgumentException)(Sub()
                    returnService.ProcessReturnInspection(1, New List(Of ReturnDispositionLine)())
                End Sub)

                ' Create valid return
                Dim customer As New Customer("CUST-INV-DISP", "Invalid Disp", 5000.0D, 0D)
                context.Customers.Add(customer)
                Dim prod As New Product("SKU-INV-DISP", "Product D", "Each", 10.0D, 50)
                context.Products.Add(prod)
                context.SaveChanges()

                Dim invoice = o2cService.FulfillSalesInvoice(0, New List(Of SalesInvoiceLine) From {
                    New SalesInvoiceLine() With {.ProductId = prod.ProductId, .FulfilledQty = 5, .UnitPrice = 20.0D}
                }, New DateTime(2026, 11, 15), customer.CustomerId)

                Dim rma = returnService.ReceiveReturnToQuarantine(customer.CustomerId, invoice.InvoiceId, New List(Of CustomerReturnLine) From {
                    New CustomerReturnLine() With {.ProductId = prod.ProductId, .ReturnedQty = 5}
                }, New DateTime(2026, 11, 15))

                ' Negative restock or scrap
                Assert.Throws(Of ArgumentOutOfRangeException)(Sub()
                    returnService.ProcessReturnInspection(rma.ReturnId, New List(Of ReturnDispositionLine) From {
                        New ReturnDispositionLine(prod.ProductId, -1, 0)
                    })
                End Sub)
                Assert.Throws(Of ArgumentOutOfRangeException)(Sub()
                    returnService.ProcessReturnInspection(rma.ReturnId, New List(Of ReturnDispositionLine) From {
                        New ReturnDispositionLine(prod.ProductId, 0, -1)
                    })
                End Sub)

                ' Total inspected == 0
                Assert.Throws(Of ArgumentException)(Sub()
                    returnService.ProcessReturnInspection(rma.ReturnId, New List(Of ReturnDispositionLine) From {
                        New ReturnDispositionLine(prod.ProductId, 0, 0)
                    })
                End Sub)

                ' Product not on return
                Assert.Throws(Of ArgumentException)(Sub()
                    returnService.ProcessReturnInspection(rma.ReturnId, New List(Of ReturnDispositionLine) From {
                        New ReturnDispositionLine(9999, 1, 0)
                    })
                End Sub)

                ' Inspecting more than returned
                Assert.Throws(Of InvalidOperationException)(Sub()
                    returnService.ProcessReturnInspection(rma.ReturnId, New List(Of ReturnDispositionLine) From {
                        New ReturnDispositionLine(prod.ProductId, 6, 0)
                    })
                End Sub)

                ' Once successfully inspected, second inspection throws InvalidOperationException
                returnService.ProcessReturnInspection(rma.ReturnId, New List(Of ReturnDispositionLine) From {
                    New ReturnDispositionLine(prod.ProductId, 5, 0)
                }, New DateTime(2026, 11, 15))

                Assert.Throws(Of InvalidOperationException)(Sub()
                    returnService.ProcessReturnInspection(rma.ReturnId, New List(Of ReturnDispositionLine) From {
                        New ReturnDispositionLine(prod.ProductId, 5, 0)
                    })
                End Sub)
            End Using
        End Sub

        ' =========================================================================
        ' DisburseCustomerRefund Tests
        ' =========================================================================

        <TestMethod>
        Public Sub DisburseCustomerRefund_ValidVoucher_ClearsNegativeARAndPostsJournal()
            Using context = _fixture.CreateContext()
                Dim returnService As New ReturnsService(context)

                ' Customer with credit balance of $150 (CurrentARBalance = -150)
                Dim customer As New Customer("CUST-REFUND", "Refund Customer", 5000.0D, -150.0D)
                context.Customers.Add(customer)
                context.SaveChanges()

                Dim bankAccount = context.Accounts.First(Function(a) a.Code = "1020")
                Dim postingDate As New DateTime(2026, 11, 15)

                Dim voucher = returnService.DisburseCustomerRefund(customer.CustomerId, Nothing, 150.0D, bankAccount.AccountId, postingDate, postingDate, "REF-001")

                Assert.IsNotNull(voucher)
                Assert.AreEqual("CustomerRefund", voucher.VoucherType)
                Assert.AreEqual(150.0D, voucher.DisbursedAmount)

                ' Verify customer AR balance brought back to 0
                Dim updatedCustomer = context.Customers.Find(customer.CustomerId)
                Assert.AreEqual(0D, updatedCustomer.CurrentARBalance)

                ' Verify journal entry: Debit 1200 AR, Credit 1020 Bank
                Dim journal = context.JournalEntries.First(Function(j) j.SourceDocId = voucher.VoucherId AndAlso j.SourceDocType = "PaymentVoucher")
                Assert.IsNotNull(journal)
                Assert.AreEqual(2, journal.Lines.Count)

                Dim arLine = journal.Lines.First(Function(l) l.Account.Code = "1200")
                Assert.AreEqual(150.0D, arLine.DebitAmount)
                Assert.AreEqual(0D, arLine.CreditAmount)

                Dim bankLine = journal.Lines.First(Function(l) l.Account.Code = "1020")
                Assert.AreEqual(0D, bankLine.DebitAmount)
                Assert.AreEqual(150.0D, bankLine.CreditAmount)

                Assert.IsTrue(LedgerPostingEngine.IsBalanced(journal.Lines))
            End Using
        End Sub

        <TestMethod>
        Public Sub DisburseCustomerRefund_InvalidParameters_ThrowsAppropriateExceptions()
            Using context = _fixture.CreateContext()
                Dim returnService As New ReturnsService(context)

                ' CustomerId <= 0
                Assert.Throws(Of ArgumentException)(Sub()
                    returnService.DisburseCustomerRefund(0, Nothing, 100.0D, 1)
                End Sub)

                ' Customer not found
                Assert.Throws(Of ArgumentException)(Sub()
                    returnService.DisburseCustomerRefund(9999, Nothing, 100.0D, 1)
                End Sub)

                ' Amount <= 0
                Dim customer As New Customer("CUST-REF-INV", "Customer", 5000.0D, 0D)
                context.Customers.Add(customer)
                context.SaveChanges()

                Assert.Throws(Of ArgumentOutOfRangeException)(Sub()
                    returnService.DisburseCustomerRefund(customer.CustomerId, Nothing, 0D, 1)
                End Sub)
                Assert.Throws(Of ArgumentOutOfRangeException)(Sub()
                    returnService.DisburseCustomerRefund(customer.CustomerId, Nothing, -10.0D, 1)
                End Sub)
            End Using
        End Sub

        <TestMethod>
        Public Sub DisburseCustomerRefund_DefaultBankResolution_WhenBankAccountIdZero()
            Using context = _fixture.CreateContext()
                Dim returnService As New ReturnsService(context)

                Dim customer As New Customer("CUST-REF-DEF", "Customer Def Bank", 5000.0D, -50.0D)
                context.Customers.Add(customer)
                context.SaveChanges()

                Dim postingDate As New DateTime(2026, 11, 15)
                Dim voucher = returnService.DisburseCustomerRefund(customer.CustomerId, Nothing, 50.0D, 0, postingDate, postingDate)

                Assert.IsNotNull(voucher)
                Dim bank = context.Accounts.First(Function(a) a.Code = "1020")
                Assert.AreEqual(bank.AccountId, voucher.BankAccountId)
            End Using
        End Sub

    End Class

End Namespace
