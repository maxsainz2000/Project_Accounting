Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Merchandising_Accounting_App.Data
Imports Merchandising_Accounting_App.Domain
Imports Merchandising_Accounting_App.Domain.Engines
Imports Merchandising_Accounting_App.Domain.Models
Imports Merchandising_Accounting_App.Services
Imports Merchandising_Accounting_App.Tests.Infrastructure

Namespace Services

    <TestClass>
    Public Class ProcureToPayServiceTests

        ' =========================================================================
        ' 1. Purchase Order Creation Tests
        ' =========================================================================

        <TestMethod>
        Public Sub CreatePurchaseOrder_ValidSingleLine_PersistsPOAndLines()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor = CreateTestVendor(context, "V-001", "Acme Supplies")
                    Dim product = CreateTestProduct(context, "SKU-TEST-1", "Test Widget", 10.0000D, 50)

                    Dim lines As New List(Of POLine) From {
                        New POLine(product.ProductId, 100, 12.5000D)
                    }

                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, lines)

                    Assert.IsNotNull(po)
                    Assert.IsTrue(po.POId > 0)
                    Assert.AreEqual(vendor.VendorId, po.VendorId)
                    Assert.AreEqual(1, po.Lines.Count)
                    Assert.AreEqual(1250.0000D, po.TotalAmount)
                    Assert.AreEqual("Approved", po.Status)
                    Assert.IsTrue(po.DocumentNumber.StartsWith("PO-"))

                    ' Verify persistence
                    Dim savedPo = context.PurchaseOrders.Find(po.POId)
                    Assert.IsNotNull(savedPo)
                    Assert.AreEqual(1250.0000D, savedPo.TotalAmount)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub CreatePurchaseOrder_ValidMultipleLinesWithExplicitDateAndNumber_PersistsCorrectly()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor = CreateTestVendor(context, "V-002", "Global Goods")
                    Dim product1 = CreateTestProduct(context, "SKU-A", "Product A", 5.0000D, 10)
                    Dim product2 = CreateTestProduct(context, "SKU-B", "Product B", 20.0000D, 20)

                    Dim lines As New List(Of POLine) From {
                        New POLine(product1.ProductId, 40, 5.5000D),  ' 220.00
                        New POLine(product2.ProductId, 15, 18.0000D)  ' 270.00
                    }

                    Dim orderDate As New DateTime(2026, 11, 10)
                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, lines, orderDate, "PO-CUSTOM-001")

                    Assert.AreEqual("PO-CUSTOM-001", po.DocumentNumber)
                    Assert.AreEqual(orderDate, po.OrderDate)
                    Assert.AreEqual(2, po.Lines.Count)
                    Assert.AreEqual(490.0000D, po.TotalAmount)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub CreatePurchaseOrder_InvalidInputs_ThrowsAppropriateExceptions()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)
                    Dim vendor = CreateTestVendor(context, "V-003", "Test Vendor")
                    Dim product = CreateTestProduct(context, "SKU-C", "Product C", 10.0000D, 0)

                    ' VendorId <= 0
                    Assert.Throws(Of ArgumentOutOfRangeException)(
                        Sub() service.CreatePurchaseOrder(0, New List(Of POLine) From {New POLine(product.ProductId, 10, 10D)}))

                    ' Non-existent vendor
                    Assert.Throws(Of ArgumentException)(
                        Sub() service.CreatePurchaseOrder(9999, New List(Of POLine) From {New POLine(product.ProductId, 10, 10D)}))

                    ' Lines is null
                    Assert.Throws(Of ArgumentNullException)(
                        Sub() service.CreatePurchaseOrder(vendor.VendorId, Nothing))

                    ' Lines is empty
                    Assert.Throws(Of ArgumentException)(
                        Sub() service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine)()))

                    ' Line with OrderedQty <= 0
                    Assert.Throws(Of ArgumentOutOfRangeException)(
                        Sub() service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {New POLine(product.ProductId, 0, 10D)}))

                    ' Line with negative price
                    Assert.Throws(Of ArgumentOutOfRangeException)(
                        Sub() service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {New POLine(product.ProductId, 10, -5D)}))

                    ' Non-existent product
                    Assert.Throws(Of ArgumentException)(
                        Sub() service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {New POLine(9999, 10, 10D)}))
                End Using
            End Using
        End Sub

        ' =========================================================================
        ' 2. Goods Receipt Note (GRN) Receiving Intake Tests
        ' =========================================================================

        <TestMethod>
        Public Sub ReceiveGoods_FullReceipt_IncrementsStockUpdatesCostAndEmitsBalancedJournal()
            ' CUJ 1: An approved PO exists for 100 units of SKU-A at $10.00/unit.
            ' Dock receiving confirms physical receipt. Stock increases by 100;
            ' Moving avg cost updates; Debit 1300 Inventory $1,000, Credit 2100 GRNI $1,000.
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor = CreateTestVendor(context, "V-010", "Coffee Importers")
                    ' Product starts with 50 units @ $8.00 = $400
                    Dim product = CreateTestProduct(context, "SKU-BEV-10", "Colombian Beans", 8.0000D, 50)

                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {
                        New POLine(product.ProductId, 100, 10.0000D)
                    })
                    Dim poLineId = po.Lines.First().POLineId

                    Dim eventDate As New DateTime(2026, 11, 12)
                    Dim receiptLines As New List(Of GRNLine) From {
                        New GRNLine(poLineId, product.ProductId, 100, 10.0000D)
                    }

                    Dim grn = service.ReceiveGoods(po.POId, receiptLines, eventDate, "GRN-2026-0001")

                    Assert.IsNotNull(grn)
                    Assert.IsTrue(grn.GRNId > 0)
                    Assert.AreEqual("GRN-2026-0001", grn.DocumentNumber)
                    Assert.AreEqual(eventDate, grn.ReceiptDate)
                    Assert.AreEqual(eventDate, grn.PostingDate)
                    Assert.AreEqual("Posted", grn.Status)

                    ' Product Stock: 50 + 100 = 150
                    ' Moving Avg Cost: ((50 * 8) + (100 * 10)) / 150 = (400 + 1000) / 150 = 1400 / 150 = 9.3333
                    Dim updatedProduct = context.Products.Find(product.ProductId)
                    Assert.AreEqual(150, updatedProduct.TotalQtyOnHand)
                    Assert.AreEqual(9.3333D, updatedProduct.CurrentUnitCost)

                    ' PO Status updated to Fully Received
                    Dim updatedPo = context.PurchaseOrders.Find(po.POId)
                    Assert.AreEqual("Fully Received", updatedPo.Status)

                    ' Inventory Transaction audit entry
                    Dim invTx = context.InventoryTransactions.FirstOrDefault(Function(t) t.SourceDocId = grn.GRNId)
                    Assert.IsNotNull(invTx)
                    Assert.AreEqual(product.ProductId, invTx.ProductId)
                    Assert.AreEqual(100, invTx.QtyChange)
                    Assert.AreEqual(10.0000D, invTx.UnitCost)
                    Assert.AreEqual(150, invTx.RunningQty)
                    Assert.AreEqual(9.3333D, invTx.RunningUnitCost)

                    ' Balanced Journal Entry
                    Dim journal = context.JournalEntries.Include(Function(j) j.Lines).FirstOrDefault(Function(j) j.SourceDocType = "GoodsReceiptNote" AndAlso j.SourceDocId = grn.GRNId)
                    Assert.IsNotNull(journal)
                    Assert.AreEqual(2, journal.Lines.Count)
                    Assert.AreEqual(grn.PostingDate, journal.PostingDate)
                    Assert.IsTrue(journal.PeriodId > 0)

                    Dim invDebit = journal.Lines.First(Function(l) l.DebitAmount > 0D)
                    Dim grniCredit = journal.Lines.First(Function(l) l.CreditAmount > 0D)

                    Dim accInventory = context.Accounts.First(Function(a) a.Code = "1300")
                    Dim accGrni = context.Accounts.First(Function(a) a.Code = "2100")

                    Assert.AreEqual(accInventory.AccountId, invDebit.AccountId)
                    Assert.AreEqual(1000.0000D, invDebit.DebitAmount)

                    Assert.AreEqual(accGrni.AccountId, grniCredit.AccountId)
                    Assert.AreEqual(1000.0000D, grniCredit.CreditAmount)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub ReceiveGoods_PartialReceipt_UpdatesPOStatusToPartiallyReceived()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor = CreateTestVendor(context, "V-011", "Parts Corp")
                    Dim product = CreateTestProduct(context, "SKU-PART-1", "Bolt Pack", 4.0000D, 0)

                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {
                        New POLine(product.ProductId, 100, 4.0000D)
                    })

                    ' Partial receipt: 40 of 100 units
                    Dim receipt1 As New List(Of GRNLine) From {
                        New GRNLine(0, product.ProductId, 40, 4.0000D)
                    }
                    Dim grn1 = service.ReceiveGoods(po.POId, receipt1, New DateTime(2026, 11, 5))

                    Dim poAfterFirst = context.PurchaseOrders.Find(po.POId)
                    Assert.AreEqual("Partially Received", poAfterFirst.Status)
                    Assert.AreEqual(40, context.Products.Find(product.ProductId).TotalQtyOnHand)

                    ' Second receipt: remaining 60 units
                    Dim receipt2 As New List(Of GRNLine) From {
                        New GRNLine(0, product.ProductId, 60, 4.0000D)
                    }
                    Dim grn2 = service.ReceiveGoods(po.POId, receipt2, New DateTime(2026, 11, 8))

                    Dim poAfterSecond = context.PurchaseOrders.Find(po.POId)
                    Assert.AreEqual("Fully Received", poAfterSecond.Status)
                    Assert.AreEqual(100, context.Products.Find(product.ProductId).TotalQtyOnHand)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub ReceiveGoods_MultipleLinesInSingleGRN_UpdatesAllProductsAndGeneratesCombinedJournal()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor = CreateTestVendor(context, "V-012", "Hardware Supply")
                    Dim productA = CreateTestProduct(context, "SKU-H1", "Hammer", 15.0000D, 10)
                    Dim productB = CreateTestProduct(context, "SKU-H2", "Nails 1kg", 3.0000D, 20)

                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {
                        New POLine(productA.ProductId, 20, 15.0000D), ' 300.00
                        New POLine(productB.ProductId, 50, 3.2000D)   ' 160.00
                    })

                    Dim receiptLines As New List(Of GRNLine) From {
                        New GRNLine(0, productA.ProductId, 20, 15.0000D),
                        New GRNLine(0, productB.ProductId, 50, 3.2000D)
                    }

                    Dim grn = service.ReceiveGoods(po.POId, receiptLines, New DateTime(2026, 11, 14))

                    Assert.AreEqual(30, context.Products.Find(productA.ProductId).TotalQtyOnHand)
                    Assert.AreEqual(70, context.Products.Find(productB.ProductId).TotalQtyOnHand)

                    Dim journal = context.JournalEntries.Include(Function(j) j.Lines).First(Function(j) j.SourceDocId = grn.GRNId)
                    Dim totalDebit = journal.Lines.Sum(Function(l) l.DebitAmount)
                    Dim totalCredit = journal.Lines.Sum(Function(l) l.CreditAmount)

                    ' Total = 300 + 160 = 460.0000
                    Assert.AreEqual(460.0000D, totalDebit)
                    Assert.AreEqual(460.0000D, totalCredit)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub ReceiveGoods_ClosedPeriodHandling_ResolvesStatutoryPostingDateToActivePeriodStart()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor = CreateTestVendor(context, "V-013", "Vendor Late")
                    Dim product = CreateTestProduct(context, "SKU-LATE", "Late Widget", 10.0000D, 0)
                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {
                        New POLine(product.ProductId, 10, 10.0000D)
                    })

                    ' Operational event happened on 2026-10-25 (outside active period FY2026-P11 2026-11-01..30)
                    Dim lateEventDate As New DateTime(2026, 10, 25)
                    Dim grn = service.ReceiveGoods(po.POId, New List(Of GRNLine) From {
                        New GRNLine(0, product.ProductId, 10, 10.0000D)
                    }, lateEventDate)

                    ' Physical receipt date preserved
                    Assert.AreEqual(lateEventDate, grn.ReceiptDate)
                    ' Statutory posting date resolved to 2026-11-01
                    Assert.AreEqual(New DateTime(2026, 11, 1), grn.PostingDate)

                    Dim journal = context.JournalEntries.First(Function(j) j.SourceDocId = grn.GRNId)
                    Assert.AreEqual(lateEventDate, journal.OperationalEventDate)
                    Assert.AreEqual(New DateTime(2026, 11, 1), journal.PostingDate)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub ReceiveGoods_ExplicitPostingDateInClosedPeriod_ThrowsClosedPeriodPostingException()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim periodService As New PeriodService(context)
                    Dim service As New ProcureToPayService(context, periodService)

                    ' Seal the current period
                    Dim activePeriod = periodService.GetActiveFiscalPeriod()
                    periodService.CloseFiscalPeriod(activePeriod.PeriodId)

                    Dim vendor = CreateTestVendor(context, "V-014", "Vendor Closed")
                    Dim product = CreateTestProduct(context, "SKU-CLOSED", "Closed Widget", 10.0000D, 0)
                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {
                        New POLine(product.ProductId, 10, 10.0000D)
                    })

                    Dim receiptLines As New List(Of GRNLine) From {
                        New GRNLine(0, product.ProductId, 10, 10.0000D)
                    }

                    ' Explicit posting date in closed period
                    Assert.Throws(Of ClosedPeriodPostingException)(
                        Sub() service.ReceiveGoods(po.POId, receiptLines, New DateTime(2026, 11, 15), postingDate:=New DateTime(2026, 11, 15)))
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub ReceiveGoods_InvalidInputs_ThrowsAppropriateExceptions()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)
                    Dim vendor = CreateTestVendor(context, "V-015", "Vendor Inv")
                    Dim product = CreateTestProduct(context, "SKU-INV", "Inv Widget", 10.0000D, 0)
                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {
                        New POLine(product.ProductId, 10, 10.0000D)
                    })

                    ' POId <= 0
                    Assert.Throws(Of ArgumentOutOfRangeException)(
                        Sub() service.ReceiveGoods(0, New List(Of GRNLine) From {New GRNLine(0, product.ProductId, 5, 10D)}, DateTime.Today))

                    ' Non-existent PO
                    Assert.Throws(Of ArgumentException)(
                        Sub() service.ReceiveGoods(9999, New List(Of GRNLine) From {New GRNLine(0, product.ProductId, 5, 10D)}, DateTime.Today))

                    ' receiptLines is null
                    Assert.Throws(Of ArgumentNullException)(
                        Sub() service.ReceiveGoods(po.POId, Nothing, DateTime.Today))

                    ' receiptLines is empty
                    Assert.Throws(Of ArgumentException)(
                        Sub() service.ReceiveGoods(po.POId, New List(Of GRNLine)(), DateTime.Today))

                    ' ReceivedQty <= 0
                    Assert.Throws(Of ArgumentOutOfRangeException)(
                        Sub() service.ReceiveGoods(po.POId, New List(Of GRNLine) From {New GRNLine(0, product.ProductId, 0, 10D)}, DateTime.Today))

                    ' Product not on PO
                    Dim unrelatedProduct = CreateTestProduct(context, "SKU-OTHER", "Other Widget", 5.0000D, 0)
                    Assert.Throws(Of ArgumentException)(
                        Sub() service.ReceiveGoods(po.POId, New List(Of GRNLine) From {New GRNLine(0, unrelatedProduct.ProductId, 5, 5D)}, DateTime.Today))
                End Using
            End Using
        End Sub

        ' =========================================================================
        ' 3. Supplier Bill 3-Way Matching Tests
        ' =========================================================================

        <TestMethod>
        Public Sub MatchSupplierBill_ExactMatch_GeneratesBalancedJournalAndCreditsAP()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor = CreateTestVendor(context, "V-020", "Supplier Exact", 30, 0D)
                    Dim product = CreateTestProduct(context, "SKU-EXACT", "Widget Exact", 10.0000D, 0)

                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {
                        New POLine(product.ProductId, 100, 10.0000D)
                    })
                    Dim grn = service.ReceiveGoods(po.POId, New List(Of GRNLine) From {
                        New GRNLine(0, product.ProductId, 100, 10.0000D)
                    }, New DateTime(2026, 11, 10))

                    Dim grnLineId = grn.Lines.First().GRNLineId
                    Dim billLines As New List(Of BillLine) From {
                        New BillLine() With {
                            .GRNLineId = grnLineId,
                            .BilledQty = 100,
                            .BilledUnitPrice = 10.0000D
                        }
                    }

                    Dim bill = service.MatchSupplierBill(vendor.VendorId, grn.GRNId, billLines, "BILL-2026-001", New DateTime(2026, 11, 12))

                    Assert.IsNotNull(bill)
                    Assert.AreEqual(1000.0000D, bill.TotalAmount)
                    Assert.AreEqual("BILL-2026-001", bill.DocumentNumber)
                    Assert.AreEqual("Posted", bill.Status)
                    Assert.AreEqual(New DateTime(2026, 12, 12), bill.DueDate) ' 30 days terms

                    ' Vendor payable balance incremented
                    Dim updatedVendor = context.Vendors.Find(vendor.VendorId)
                    Assert.AreEqual(1000.0000D, updatedVendor.CurrentPayableBalance)

                    ' GRN status updated to Settled
                    Dim updatedGrn = context.GoodsReceiptNotes.Find(grn.GRNId)
                    Assert.AreEqual("Settled", updatedGrn.Status)

                    ' Balanced Journal: Debit 2100 GRNI $1,000, Credit 2000 AP $1,000
                    Dim journal = context.JournalEntries.Include(Function(j) j.Lines).First(Function(j) j.SourceDocType = "SupplierBill" AndAlso j.SourceDocId = bill.BillId)
                    Assert.AreEqual(2, journal.Lines.Count)

                    Dim accGrni = context.Accounts.First(Function(a) a.Code = "2100")
                    Dim accAp = context.Accounts.First(Function(a) a.Code = "2000")

                    Dim grniLine = journal.Lines.First(Function(l) l.AccountId = accGrni.AccountId)
                    Dim apLine = journal.Lines.First(Function(l) l.AccountId = accAp.AccountId)

                    Assert.AreEqual(1000.0000D, grniLine.DebitAmount)
                    Assert.AreEqual(1000.0000D, apLine.CreditAmount)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub MatchSupplierBill_UnfavorablePriceVariance_RoutesDifferenceToDebitPPV()
            ' PO price: $10.00, Supplier billed: $12.50 across 100 units
            ' PPV = 100 * ($12.50 - $10.00) = +$250.00 (Unfavorable -> Debit 5100)
            ' GRNI Clearing = 100 * $10.00 = $1,000.00 (Debit 2100)
            ' Accounts Payable = 100 * $12.50 = $1,250.00 (Credit 2000)
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor = CreateTestVendor(context, "V-021", "Supplier Variance")
                    Dim product = CreateTestProduct(context, "SKU-PPV-1", "Widget PPV", 10.0000D, 0)

                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {
                        New POLine(product.ProductId, 100, 10.0000D)
                    })
                    Dim grn = service.ReceiveGoods(po.POId, New List(Of GRNLine) From {
                        New GRNLine(0, product.ProductId, 100, 10.0000D)
                    }, New DateTime(2026, 11, 10))

                    Dim grnLineId = grn.Lines.First().GRNLineId
                    Dim billLines As New List(Of BillLine) From {
                        New BillLine() With {
                            .GRNLineId = grnLineId,
                            .BilledQty = 100,
                            .BilledUnitPrice = 12.5000D
                        }
                    }

                    Dim bill = service.MatchSupplierBill(vendor.VendorId, grn.GRNId, billLines, "BILL-PPV-UNFAV", New DateTime(2026, 11, 12))

                    Assert.AreEqual(1250.0000D, bill.TotalAmount)

                    Dim journal = context.JournalEntries.Include(Function(j) j.Lines).First(Function(j) j.SourceDocType = "SupplierBill" AndAlso j.SourceDocId = bill.BillId)
                    Assert.AreEqual(3, journal.Lines.Count)

                    Dim accGrni = context.Accounts.First(Function(a) a.Code = "2100")
                    Dim accPpv = context.Accounts.First(Function(a) a.Code = "5100")
                    Dim accAp = context.Accounts.First(Function(a) a.Code = "2000")

                    Dim grniLine = journal.Lines.First(Function(l) l.AccountId = accGrni.AccountId)
                    Dim ppvLine = journal.Lines.First(Function(l) l.AccountId = accPpv.AccountId)
                    Dim apLine = journal.Lines.First(Function(l) l.AccountId = accAp.AccountId)

                    Assert.AreEqual(1000.0000D, grniLine.DebitAmount)
                    Assert.AreEqual(250.0000D, ppvLine.DebitAmount)
                    Assert.AreEqual(1250.0000D, apLine.CreditAmount)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub MatchSupplierBill_FavorablePriceVariance_RoutesDifferenceToCreditPPV()
            ' PO price: $10.00, Supplier billed: $8.50 across 100 units
            ' PPV = 100 * ($8.50 - $10.00) = -$150.00 (Favorable -> Credit 5100)
            ' GRNI Clearing = 100 * $10.00 = $1,000.00 (Debit 2100)
            ' Accounts Payable = 100 * $8.50 = $850.00 (Credit 2000)
            ' Total Debits ($1000) == Total Credits ($150 + $850 = $1000)
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor = CreateTestVendor(context, "V-022", "Supplier Discount")
                    Dim product = CreateTestProduct(context, "SKU-PPV-2", "Widget Fav", 10.0000D, 0)

                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {
                        New POLine(product.ProductId, 100, 10.0000D)
                    })
                    Dim grn = service.ReceiveGoods(po.POId, New List(Of GRNLine) From {
                        New GRNLine(0, product.ProductId, 100, 10.0000D)
                    }, New DateTime(2026, 11, 10))

                    Dim grnLineId = grn.Lines.First().GRNLineId
                    Dim billLines As New List(Of BillLine) From {
                        New BillLine() With {
                            .GRNLineId = grnLineId,
                            .BilledQty = 100,
                            .BilledUnitPrice = 8.5000D
                        }
                    }

                    Dim bill = service.MatchSupplierBill(vendor.VendorId, grn.GRNId, billLines, "BILL-PPV-FAV", New DateTime(2026, 11, 12))

                    Assert.AreEqual(850.0000D, bill.TotalAmount)

                    Dim journal = context.JournalEntries.Include(Function(j) j.Lines).First(Function(j) j.SourceDocType = "SupplierBill" AndAlso j.SourceDocId = bill.BillId)
                    Assert.AreEqual(3, journal.Lines.Count)

                    Dim accGrni = context.Accounts.First(Function(a) a.Code = "2100")
                    Dim accPpv = context.Accounts.First(Function(a) a.Code = "5100")
                    Dim accAp = context.Accounts.First(Function(a) a.Code = "2000")

                    Dim grniLine = journal.Lines.First(Function(l) l.AccountId = accGrni.AccountId)
                    Dim ppvLine = journal.Lines.First(Function(l) l.AccountId = accPpv.AccountId)
                    Dim apLine = journal.Lines.First(Function(l) l.AccountId = accAp.AccountId)

                    Assert.AreEqual(1000.0000D, grniLine.DebitAmount)
                    Assert.AreEqual(150.0000D, ppvLine.CreditAmount)
                    Assert.AreEqual(850.0000D, apLine.CreditAmount)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub MatchSupplierBill_WithInboundFreight_RoutesFreightTo5200()
            ' PO price: $10.00, Billed price: $10.00, Freight: $75.00
            ' Debit 2100 GRNI: $1,000.00
            ' Debit 5200 Freight: $75.00
            ' Credit 2000 AP: $1,075.00
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor = CreateTestVendor(context, "V-023", "Supplier Freight")
                    Dim product = CreateTestProduct(context, "SKU-FRT-1", "Widget Freight", 10.0000D, 0)

                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {
                        New POLine(product.ProductId, 100, 10.0000D)
                    })
                    Dim grn = service.ReceiveGoods(po.POId, New List(Of GRNLine) From {
                        New GRNLine(0, product.ProductId, 100, 10.0000D)
                    }, New DateTime(2026, 11, 10))

                    Dim grnLineId = grn.Lines.First().GRNLineId
                    Dim billLines As New List(Of BillLine) From {
                        New BillLine() With {
                            .GRNLineId = grnLineId,
                            .BilledQty = 100,
                            .BilledUnitPrice = 10.0000D,
                            .FreightAmount = 75.0000D
                        }
                    }

                    Dim bill = service.MatchSupplierBill(vendor.VendorId, grn.GRNId, billLines, "BILL-WITH-FREIGHT", New DateTime(2026, 11, 12))

                    Assert.AreEqual(1075.0000D, bill.TotalAmount)

                    Dim journal = context.JournalEntries.Include(Function(j) j.Lines).First(Function(j) j.SourceDocType = "SupplierBill" AndAlso j.SourceDocId = bill.BillId)
                    Dim accFreight = context.Accounts.First(Function(a) a.Code = "5200")
                    Dim freightLine = journal.Lines.First(Function(l) l.AccountId = accFreight.AccountId)

                    Assert.AreEqual(75.0000D, freightLine.DebitAmount)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub MatchSupplierBill_StandaloneFreightLine_RoutesTo5200()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor = CreateTestVendor(context, "V-024", "Carrier Logistics")
                    Dim product = CreateTestProduct(context, "SKU-CARRIER", "Carrier Widget", 10.0000D, 0)

                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {
                        New POLine(product.ProductId, 50, 10.0000D)
                    })
                    Dim grn = service.ReceiveGoods(po.POId, New List(Of GRNLine) From {
                        New GRNLine(0, product.ProductId, 50, 10.0000D)
                    }, New DateTime(2026, 11, 10))

                    ' Standalone freight bill line with BilledQty = 0
                    Dim billLines As New List(Of BillLine) From {
                        New BillLine() With {
                            .BilledQty = 0,
                            .LineTotal = 120.0000D,
                            .FreightAmount = 120.0000D
                        }
                    }

                    Dim bill = service.MatchSupplierBill(vendor.VendorId, grn.GRNId, billLines, "BILL-CARRIER-FREIGHT", New DateTime(2026, 11, 12))

                    Assert.AreEqual(120.0000D, bill.TotalAmount)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub MatchSupplierBill_OverbilledQuantity_ThrowsThreeWayMatchException()
            ' Received: 50 units. Supplier bills for 60 units -> Throws ThreeWayMatchException
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor = CreateTestVendor(context, "V-025", "Overbill Vendor")
                    Dim product = CreateTestProduct(context, "SKU-OVER", "Over Widget", 10.0000D, 0)

                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {
                        New POLine(product.ProductId, 50, 10.0000D)
                    })
                    Dim grn = service.ReceiveGoods(po.POId, New List(Of GRNLine) From {
                        New GRNLine(0, product.ProductId, 50, 10.0000D)
                    }, New DateTime(2026, 11, 10))

                    Dim grnLineId = grn.Lines.First().GRNLineId
                    Dim billLines As New List(Of BillLine) From {
                        New BillLine() With {
                            .GRNLineId = grnLineId,
                            .BilledQty = 60,
                            .BilledUnitPrice = 10.0000D
                        }
                    }

                    Dim caughtEx = Assert.Throws(Of ThreeWayMatchException)(
                        Sub() service.MatchSupplierBill(vendor.VendorId, grn.GRNId, billLines, "BILL-OVER", New DateTime(2026, 11, 12)))

                    Assert.AreEqual(60, caughtEx.BilledQty.Value)
                    Assert.AreEqual(50, caughtEx.ReceivedQty.Value)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub MatchSupplierBill_CumulativeOverbillingAcrossMultipleBills_ThrowsThreeWayMatchException()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor = CreateTestVendor(context, "V-026", "Split Bill Vendor")
                    Dim product = CreateTestProduct(context, "SKU-SPLIT", "Split Widget", 10.0000D, 0)

                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {
                        New POLine(product.ProductId, 50, 10.0000D)
                    })
                    Dim grn = service.ReceiveGoods(po.POId, New List(Of GRNLine) From {
                        New GRNLine(0, product.ProductId, 50, 10.0000D)
                    }, New DateTime(2026, 11, 10))

                    Dim grnLineId = grn.Lines.First().GRNLineId

                    ' First bill for 35 units (ok)
                    service.MatchSupplierBill(vendor.VendorId, grn.GRNId, New List(Of BillLine) From {
                        New BillLine() With {.GRNLineId = grnLineId, .BilledQty = 35, .BilledUnitPrice = 10D}
                    }, "BILL-PART-1", New DateTime(2026, 11, 12))

                    ' Second bill attempts to bill 25 units (35 + 25 = 60 > 50 received) -> Throws
                    Assert.Throws(Of ThreeWayMatchException)(
                        Sub() service.MatchSupplierBill(vendor.VendorId, grn.GRNId, New List(Of BillLine) From {
                            New BillLine() With {.GRNLineId = grnLineId, .BilledQty = 25, .BilledUnitPrice = 10D}
                        }, "BILL-PART-2", New DateTime(2026, 11, 14)))
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub MatchSupplierBill_VendorMismatch_ThrowsThreeWayMatchException()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor1 = CreateTestVendor(context, "V-027A", "Vendor A")
                    Dim vendor2 = CreateTestVendor(context, "V-027B", "Vendor B")
                    Dim product = CreateTestProduct(context, "SKU-MISMATCH", "Mismatch Widget", 10.0000D, 0)

                    Dim po = service.CreatePurchaseOrder(vendor1.VendorId, New List(Of POLine) From {
                        New POLine(product.ProductId, 50, 10.0000D)
                    })
                    Dim grn = service.ReceiveGoods(po.POId, New List(Of GRNLine) From {
                        New GRNLine(0, product.ProductId, 50, 10.0000D)
                    }, New DateTime(2026, 11, 10))

                    ' Attempt to bill vendor 2 against vendor 1's GRN
                    Assert.Throws(Of ThreeWayMatchException)(
                        Sub() service.MatchSupplierBill(vendor2.VendorId, grn.GRNId, New List(Of BillLine) From {
                            New BillLine() With {.GRNLineId = grn.Lines.First().GRNLineId, .BilledQty = 50, .BilledUnitPrice = 10D}
                        }, "BILL-WRONG-VEND", New DateTime(2026, 11, 12)))
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub MatchSupplierBill_GRNLineNotOnGRN_ThrowsThreeWayMatchException()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor = CreateTestVendor(context, "V-028", "Vendor LineBad")
                    Dim product = CreateTestProduct(context, "SKU-BADLINE", "Bad Line Widget", 10.0000D, 0)

                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {
                        New POLine(product.ProductId, 50, 10.0000D)
                    })
                    Dim grn = service.ReceiveGoods(po.POId, New List(Of GRNLine) From {
                        New GRNLine(0, product.ProductId, 50, 10.0000D)
                    }, New DateTime(2026, 11, 10))

                    ' Non-existent GRNLineId 9999
                    Assert.Throws(Of ThreeWayMatchException)(
                        Sub() service.MatchSupplierBill(vendor.VendorId, grn.GRNId, New List(Of BillLine) From {
                            New BillLine() With {.GRNLineId = 9999, .BilledQty = 10, .BilledUnitPrice = 10D}
                        }, "BILL-INVALID-LINE", New DateTime(2026, 11, 12)))
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub MatchSupplierBill_InvalidInputs_ThrowsAppropriateExceptions()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor = CreateTestVendor(context, "V-029", "Vendor Inputs")
                    Dim product = CreateTestProduct(context, "SKU-INP-B", "Inputs Widget", 10.0000D, 0)
                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {
                        New POLine(product.ProductId, 50, 10.0000D)
                    })
                    Dim grn = service.ReceiveGoods(po.POId, New List(Of GRNLine) From {
                        New GRNLine(0, product.ProductId, 50, 10.0000D)
                    }, New DateTime(2026, 11, 10))

                    Dim grnLineId = grn.Lines.First().GRNLineId
                    Dim validLines = New List(Of BillLine) From {New BillLine() With {.GRNLineId = grnLineId, .BilledQty = 10, .BilledUnitPrice = 10D}}

                    ' VendorId <= 0
                    Assert.Throws(Of ArgumentOutOfRangeException)(
                        Sub() service.MatchSupplierBill(0, grn.GRNId, validLines, "DOC-1", DateTime.Today))

                    ' GRNId <= 0
                    Assert.Throws(Of ArgumentOutOfRangeException)(
                        Sub() service.MatchSupplierBill(vendor.VendorId, 0, validLines, "DOC-1", DateTime.Today))

                    ' Empty doc num
                    Assert.Throws(Of ArgumentException)(
                        Sub() service.MatchSupplierBill(vendor.VendorId, grn.GRNId, validLines, "", DateTime.Today))

                    ' Bill lines is null
                    Assert.Throws(Of ArgumentNullException)(
                        Sub() service.MatchSupplierBill(vendor.VendorId, grn.GRNId, Nothing, "DOC-1", DateTime.Today))

                    ' Bill lines is empty
                    Assert.Throws(Of ArgumentException)(
                        Sub() service.MatchSupplierBill(vendor.VendorId, grn.GRNId, New List(Of BillLine)(), "DOC-1", DateTime.Today))

                    ' Non-existent vendor
                    Assert.Throws(Of ArgumentException)(
                        Sub() service.MatchSupplierBill(9999, grn.GRNId, validLines, "DOC-1", DateTime.Today))

                    ' Non-existent GRN
                    Assert.Throws(Of ArgumentException)(
                        Sub() service.MatchSupplierBill(vendor.VendorId, 9999, validLines, "DOC-1", DateTime.Today))

                    ' Negative billed qty
                    Assert.Throws(Of ArgumentOutOfRangeException)(
                        Sub() service.MatchSupplierBill(vendor.VendorId, grn.GRNId, New List(Of BillLine) From {
                            New BillLine() With {.GRNLineId = grnLineId, .BilledQty = -5, .BilledUnitPrice = 10D}
                        }, "DOC-1", DateTime.Today))

                    ' Negative billed price
                    Assert.Throws(Of ArgumentOutOfRangeException)(
                        Sub() service.MatchSupplierBill(vendor.VendorId, grn.GRNId, New List(Of BillLine) From {
                            New BillLine() With {.GRNLineId = grnLineId, .BilledQty = 5, .BilledUnitPrice = -10D}
                        }, "DOC-1", DateTime.Today))

                    ' Negative freight
                    Assert.Throws(Of ArgumentOutOfRangeException)(
                        Sub() service.MatchSupplierBill(vendor.VendorId, grn.GRNId, New List(Of BillLine) From {
                            New BillLine() With {.GRNLineId = grnLineId, .BilledQty = 5, .BilledUnitPrice = 10D, .FreightAmount = -10D}
                        }, "DOC-1", DateTime.Today))
                End Using
            End Using
        End Sub

        ' =========================================================================
        ' 4. Accounts Payable Cash Disbursement (Payment Voucher) Tests
        ' =========================================================================

        <TestMethod>
        Public Sub DisbursePaymentVoucher_FullPayment_DebitsAPCreditsBankAndSettlesBill()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor = CreateTestVendor(context, "V-030", "Vendor Pay Full", 30, 0D)
                    Dim product = CreateTestProduct(context, "SKU-PAY-1", "Pay Widget", 10.0000D, 0)

                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {
                        New POLine(product.ProductId, 50, 10.0000D)
                    })
                    Dim grn = service.ReceiveGoods(po.POId, New List(Of GRNLine) From {
                        New GRNLine(0, product.ProductId, 50, 10.0000D)
                    }, New DateTime(2026, 11, 5))

                    Dim bill = service.MatchSupplierBill(vendor.VendorId, grn.GRNId, New List(Of BillLine) From {
                        New BillLine() With {.GRNLineId = grn.Lines.First().GRNLineId, .BilledQty = 50, .BilledUnitPrice = 10D}
                    }, "BILL-PAY-01", New DateTime(2026, 11, 7))

                    Assert.AreEqual(500.0000D, context.Vendors.Find(vendor.VendorId).CurrentPayableBalance)

                    ' Operating Bank Account code: 1020
                    Dim bankAccount = context.Accounts.First(Function(a) a.Code = "1020")
                    Dim voucher = service.DisbursePaymentVoucher(vendor.VendorId, bill.BillId, 500.0000D, bankAccount.AccountId, New DateTime(2026, 11, 20), voucherNumber:="PV-FULL-001")

                    Assert.IsNotNull(voucher)
                    Assert.AreEqual("VendorPayment", voucher.VoucherType)
                    Assert.AreEqual(500.0000D, voucher.DisbursedAmount)
                    Assert.AreEqual("PV-FULL-001", voucher.VoucherNumber)

                    ' Vendor balance reduced to 0
                    Assert.AreEqual(0D, context.Vendors.Find(vendor.VendorId).CurrentPayableBalance)

                    ' Bill status is Settled
                    Assert.AreEqual("Settled", context.SupplierBills.Find(bill.BillId).Status)

                    ' Balanced Journal: Debit 2000 AP $500, Credit 1020 Bank $500
                    Dim journal = context.JournalEntries.Include(Function(j) j.Lines).First(Function(j) j.SourceDocType = "PaymentVoucher" AndAlso j.SourceDocId = voucher.VoucherId)
                    Assert.AreEqual(2, journal.Lines.Count)

                    Dim accAp = context.Accounts.First(Function(a) a.Code = "2000")
                    Dim apLine = journal.Lines.First(Function(l) l.AccountId = accAp.AccountId)
                    Dim bankLine = journal.Lines.First(Function(l) l.AccountId = bankAccount.AccountId)

                    Assert.AreEqual(500.0000D, apLine.DebitAmount)
                    Assert.AreEqual(500.0000D, bankLine.CreditAmount)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub DisbursePaymentVoucher_PartialPayment_DecrementsBalanceAndKeepsStatusPosted()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor = CreateTestVendor(context, "V-031", "Vendor Pay Part", 30, 0D)
                    Dim product = CreateTestProduct(context, "SKU-PAY-2", "Pay Widget 2", 20.0000D, 0)

                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {
                        New POLine(product.ProductId, 50, 20.0000D)
                    })
                    Dim grn = service.ReceiveGoods(po.POId, New List(Of GRNLine) From {
                        New GRNLine(0, product.ProductId, 50, 20.0000D)
                    }, New DateTime(2026, 11, 5))

                    Dim bill = service.MatchSupplierBill(vendor.VendorId, grn.GRNId, New List(Of BillLine) From {
                        New BillLine() With {.GRNLineId = grn.Lines.First().GRNLineId, .BilledQty = 50, .BilledUnitPrice = 20D}
                    }, "BILL-PAY-02", New DateTime(2026, 11, 7))

                    ' Bill total = $1,000.00
                    Dim bankAccount = context.Accounts.First(Function(a) a.Code = "1020")

                    ' Disburse partial $400.00
                    Dim voucher1 = service.DisbursePaymentVoucher(vendor.VendorId, bill.BillId, 400.0000D, bankAccount.AccountId, New DateTime(2026, 11, 10))

                    Assert.AreEqual("Posted", context.SupplierBills.Find(bill.BillId).Status)
                    Assert.AreEqual(600.0000D, context.Vendors.Find(vendor.VendorId).CurrentPayableBalance)

                    ' Disburse remaining $600.00
                    Dim voucher2 = service.DisbursePaymentVoucher(vendor.VendorId, bill.BillId, 600.0000D, bankAccount.AccountId, New DateTime(2026, 11, 20))

                    Assert.AreEqual("Settled", context.SupplierBills.Find(bill.BillId).Status)
                    Assert.AreEqual(0D, context.Vendors.Find(vendor.VendorId).CurrentPayableBalance)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub DisbursePaymentVoucher_UsingAccountCodeDirectly_ResolvesBankAccount()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor = CreateTestVendor(context, "V-032", "Vendor Code", 30, 0D)
                    Dim product = CreateTestProduct(context, "SKU-CODE", "Code Widget", 10.0000D, 0)
                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {New POLine(product.ProductId, 10, 10D)})
                    Dim grn = service.ReceiveGoods(po.POId, New List(Of GRNLine) From {New GRNLine(0, product.ProductId, 10, 10D)}, DateTime.Today)
                    Dim bill = service.MatchSupplierBill(vendor.VendorId, grn.GRNId, New List(Of BillLine) From {
                        New BillLine() With {.GRNLineId = grn.Lines.First().GRNLineId, .BilledQty = 10, .BilledUnitPrice = 10D}
                    }, "BILL-CODE-01", DateTime.Today)

                    ' Pass COA integer code 1020 directly instead of AccountId
                    Dim voucher = service.DisbursePaymentVoucher(vendor.VendorId, bill.BillId, 100.0000D, 1020)

                    Assert.IsNotNull(voucher)
                    Dim accBank = context.Accounts.First(Function(a) a.Code = "1020")
                    Assert.AreEqual(accBank.AccountId, voucher.BankAccountId)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub DisbursePaymentVoucher_Overpayment_ThrowsArgumentException()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor = CreateTestVendor(context, "V-033", "Vendor Overpay", 30, 0D)
                    Dim product = CreateTestProduct(context, "SKU-OVERPAY", "Overpay Widget", 10.0000D, 0)
                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {New POLine(product.ProductId, 10, 10D)})
                    Dim grn = service.ReceiveGoods(po.POId, New List(Of GRNLine) From {New GRNLine(0, product.ProductId, 10, 10D)}, DateTime.Today)
                    Dim bill = service.MatchSupplierBill(vendor.VendorId, grn.GRNId, New List(Of BillLine) From {
                        New BillLine() With {.GRNLineId = grn.Lines.First().GRNLineId, .BilledQty = 10, .BilledUnitPrice = 10D}
                    }, "BILL-OVERPAY", DateTime.Today)

                    ' Bill total is $100. Attempt to pay $100.01 -> Throws
                    Assert.Throws(Of ArgumentException)(
                        Sub() service.DisbursePaymentVoucher(vendor.VendorId, bill.BillId, 100.0100D, 1020))
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub DisbursePaymentVoucher_AlreadySettledBill_ThrowsInvalidOperationException()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor = CreateTestVendor(context, "V-034", "Vendor DoublePay", 30, 0D)
                    Dim product = CreateTestProduct(context, "SKU-DBLPAY", "Double Widget", 10.0000D, 0)
                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {New POLine(product.ProductId, 10, 10D)})
                    Dim grn = service.ReceiveGoods(po.POId, New List(Of GRNLine) From {New GRNLine(0, product.ProductId, 10, 10D)}, DateTime.Today)
                    Dim bill = service.MatchSupplierBill(vendor.VendorId, grn.GRNId, New List(Of BillLine) From {
                        New BillLine() With {.GRNLineId = grn.Lines.First().GRNLineId, .BilledQty = 10, .BilledUnitPrice = 10D}
                    }, "BILL-DBLPAY", DateTime.Today)

                    ' Settle in full
                    service.DisbursePaymentVoucher(vendor.VendorId, bill.BillId, 100.0000D, 1020)

                    ' Second disbursement attempt on settled bill
                    Assert.Throws(Of InvalidOperationException)(
                        Sub() service.DisbursePaymentVoucher(vendor.VendorId, bill.BillId, 50.0000D, 1020))
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub DisbursePaymentVoucher_VendorMismatch_ThrowsArgumentException()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor1 = CreateTestVendor(context, "V-035A", "Vendor Real")
                    Dim vendor2 = CreateTestVendor(context, "V-035B", "Vendor Wrong")
                    Dim product = CreateTestProduct(context, "SKU-MIS-P", "Mis Widget", 10.0000D, 0)
                    Dim po = service.CreatePurchaseOrder(vendor1.VendorId, New List(Of POLine) From {New POLine(product.ProductId, 10, 10D)})
                    Dim grn = service.ReceiveGoods(po.POId, New List(Of GRNLine) From {New GRNLine(0, product.ProductId, 10, 10D)}, DateTime.Today)
                    Dim bill = service.MatchSupplierBill(vendor1.VendorId, grn.GRNId, New List(Of BillLine) From {
                        New BillLine() With {.GRNLineId = grn.Lines.First().GRNLineId, .BilledQty = 10, .BilledUnitPrice = 10D}
                    }, "BILL-VEND-MIS", DateTime.Today)

                    ' Pass vendor2 for vendor1's bill
                    Assert.Throws(Of ArgumentException)(
                        Sub() service.DisbursePaymentVoucher(vendor2.VendorId, bill.BillId, 100.0000D, 1020))
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub DisbursePaymentVoucher_InvalidInputs_ThrowsAppropriateExceptions()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor = CreateTestVendor(context, "V-036", "Vendor BadInputs", 30, 0D)
                    Dim product = CreateTestProduct(context, "SKU-BADINP", "BadInp Widget", 10.0000D, 0)
                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {New POLine(product.ProductId, 10, 10D)})
                    Dim grn = service.ReceiveGoods(po.POId, New List(Of GRNLine) From {New GRNLine(0, product.ProductId, 10, 10D)}, DateTime.Today)
                    Dim bill = service.MatchSupplierBill(vendor.VendorId, grn.GRNId, New List(Of BillLine) From {
                        New BillLine() With {.GRNLineId = grn.Lines.First().GRNLineId, .BilledQty = 10, .BilledUnitPrice = 10D}
                    }, "BILL-BAD-INP", DateTime.Today)

                    ' VendorId <= 0
                    Assert.Throws(Of ArgumentOutOfRangeException)(
                        Sub() service.DisbursePaymentVoucher(0, bill.BillId, 50D, 1020))

                    ' BillId <= 0
                    Assert.Throws(Of ArgumentOutOfRangeException)(
                        Sub() service.DisbursePaymentVoucher(vendor.VendorId, 0, 50D, 1020))

                    ' Amount <= 0
                    Assert.Throws(Of ArgumentOutOfRangeException)(
                        Sub() service.DisbursePaymentVoucher(vendor.VendorId, bill.BillId, 0D, 1020))
                    Assert.Throws(Of ArgumentOutOfRangeException)(
                        Sub() service.DisbursePaymentVoucher(vendor.VendorId, bill.BillId, -25D, 1020))

                    ' Non-existent vendor
                    Assert.Throws(Of ArgumentException)(
                        Sub() service.DisbursePaymentVoucher(9999, bill.BillId, 50D, 1020))

                    ' Non-existent bill
                    Assert.Throws(Of ArgumentException)(
                        Sub() service.DisbursePaymentVoucher(vendor.VendorId, 9999, 50D, 1020))

                    ' Non-existent bank account
                    Assert.Throws(Of ArgumentException)(
                        Sub() service.DisbursePaymentVoucher(vendor.VendorId, bill.BillId, 50D, 9999))
                End Using
            End Using
        End Sub

        ' =========================================================================
        ' 5. Query Helper Methods & Constructor Tests
        ' =========================================================================

        <TestMethod>
        Public Sub QueryHelperMethods_RetrieveEntitiesWithNavigations()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim service As New ProcureToPayService(context)

                    Dim vendor = CreateTestVendor(context, "V-040", "Vendor Query", 30, 0D)
                    Dim product = CreateTestProduct(context, "SKU-QUERY", "Query Widget", 10.0000D, 0)
                    Dim po = service.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {New POLine(product.ProductId, 10, 10D)})
                    Dim grn = service.ReceiveGoods(po.POId, New List(Of GRNLine) From {New GRNLine(0, product.ProductId, 10, 10D)}, DateTime.Today)
                    Dim bill = service.MatchSupplierBill(vendor.VendorId, grn.GRNId, New List(Of BillLine) From {
                        New BillLine() With {.GRNLineId = grn.Lines.First().GRNLineId, .BilledQty = 10, .BilledUnitPrice = 10D}
                    }, "BILL-QUERY", DateTime.Today)
                    Dim voucher = service.DisbursePaymentVoucher(vendor.VendorId, bill.BillId, 100D, 1020)

                    Dim loadedPo = service.GetPurchaseOrder(po.POId)
                    Assert.IsNotNull(loadedPo)
                    Assert.AreEqual(po.POId, loadedPo.POId)
                    Assert.IsNotNull(loadedPo.Vendor)
                    Assert.AreEqual(1, loadedPo.Lines.Count)

                    Dim loadedGrn = service.GetGoodsReceiptNote(grn.GRNId)
                    Assert.IsNotNull(loadedGrn)
                    Assert.AreEqual(grn.GRNId, loadedGrn.GRNId)
                    Assert.IsNotNull(loadedGrn.PurchaseOrder)
                    Assert.AreEqual(1, loadedGrn.Lines.Count)

                    Dim loadedBill = service.GetSupplierBill(bill.BillId)
                    Assert.IsNotNull(loadedBill)
                    Assert.AreEqual(bill.BillId, loadedBill.BillId)
                    Assert.IsNotNull(loadedBill.Vendor)
                    Assert.AreEqual(1, loadedBill.Lines.Count)

                    Dim loadedVoucher = service.GetPaymentVoucher(voucher.VoucherId)
                    Assert.IsNotNull(loadedVoucher)
                    Assert.AreEqual(voucher.VoucherId, loadedVoucher.VoucherId)
                    Assert.IsNotNull(loadedVoucher.Vendor)
                    Assert.IsNotNull(loadedVoucher.BankAccount)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub Constructors_ValidationAndDefaultInstantiation()
            ' Null context throws ArgumentNullException
            Assert.Throws(Of ArgumentNullException)(
                Sub()
                    Dim s = New ProcureToPayService(Nothing)
                End Sub)

            Assert.Throws(Of ArgumentNullException)(
                Sub()
                    Dim s = New ProcureToPayService(Nothing, Nothing)
                End Sub)

            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    ' Null periodService throws ArgumentNullException
                    Assert.Throws(Of ArgumentNullException)(
                        Sub()
                            Dim s = New ProcureToPayService(context, Nothing)
                        End Sub)

                    ' Valid two-parameter constructor
                    Dim periodService As New PeriodService(context)
                    Dim s1 As New ProcureToPayService(context, periodService)
                    Assert.IsNotNull(s1)

                    ' Valid single-parameter constructor
                    Dim s2 As New ProcureToPayService(context)
                    Assert.IsNotNull(s2)
                End Using
            End Using
        End Sub

        ' =========================================================================
        ' Private Test Fixture Helpers
        ' =========================================================================

        Private Function CreateTestVendor(context As ErpDbContext, code As String, name As String, Optional paymentTerms As Integer = 30, Optional balance As Decimal = 0D) As Vendor
            Dim vendor As New Vendor(code, name, paymentTerms, balance)
            context.Vendors.Add(vendor)
            context.SaveChanges()
            Return vendor
        End Function

        Private Function CreateTestProduct(context As ErpDbContext, sku As String, name As String, unitCost As Decimal, qtyOnHand As Integer) As Product
            Dim product As New Product(sku, name, "Units", unitCost, qtyOnHand)
            context.Products.Add(product)
            context.SaveChanges()
            Return product
        End Function

    End Class

End Namespace
