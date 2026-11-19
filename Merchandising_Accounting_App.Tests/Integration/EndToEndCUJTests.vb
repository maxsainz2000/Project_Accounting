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

Namespace Integration

    ''' <summary>
    ''' Statutory Accounting End-to-End Critical User Journey (CUJ) Integration Tests.
    ''' Verifies the complete lifecycle across Procurement, Inventory, Sales, Returns,
    ''' 3-Way Matching, and Fiscal Period Locking against an embedded SQLite database.
    ''' </summary>
    <TestClass>
    Public Class EndToEndCUJTests

        ' =========================================================================
        ' CUJ 1: Dock Intake (PO -> GRN -> stock increase, 1300 debit, 2100 credit, moving average cost)
        ' =========================================================================
        <TestMethod>
        Public Sub CUJ1_DockIntake_PO_GRN_StockIncrease_And_MovingAverageCost()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim periodService As New PeriodService(context)
                    Dim p2pService As New ProcureToPayService(context, periodService)

                    ' Given: Approved Purchase Order exists for 100 units of SKU-A at $10.00/unit with supplier terms net-30
                    Dim vendor = CreateTestVendor(context, "V-CUJ1-01", "Acme Wholesale Corp", paymentTermsDays:=30)
                    Dim product = CreateTestProduct(context, "SKU-CUJ1-A", "Standard Pack A", baseUnitCost:=0D, initialQty:=0)

                    Dim poLines As New List(Of POLine) From {
                        New POLine(product.ProductId, 100, 10.0000D)
                    }
                    Dim po = p2pService.CreatePurchaseOrder(vendor.VendorId, poLines, New DateTime(2026, 11, 1), "PO-2026-0001")

                    Assert.IsNotNull(po)
                    Assert.AreEqual("Approved", po.Status)
                    Assert.AreEqual(1000.0000D, po.TotalAmount)

                    ' When: Warehouse clerk receives delivery and confirms physical receipt via Goods Receipt Note (GRN)
                    Dim receiptLines As New List(Of GRNLine) From {
                        New GRNLine(po.Lines.First().POLineId, product.ProductId, 100, 10.0000D)
                    }
                    Dim grnEventDate As New DateTime(2026, 11, 5)
                    Dim grn = p2pService.ReceiveGoods(po.POId, receiptLines, grnEventDate, "GRN-2026-0001")

                    ' Then:
                    ' 1. On-hand inventory increases by 100 units
                    Dim updatedProduct = context.Products.Find(product.ProductId)
                    Assert.IsNotNull(updatedProduct)
                    Assert.AreEqual(100, updatedProduct.TotalQtyOnHand)

                    ' 2. Moving average cost initialized/updated to $10.0000
                    Assert.AreEqual(10.0000D, updatedProduct.CurrentUnitCost)

                    ' 3. Balanced Journal Entry: Debit 1300 Merchandise Inventory $1,000.00, Credit 2100 GRNI $1,000.00
                    Dim journal = context.JournalEntries _
                        .Include(Function(j) j.Lines) _
                        .ThenInclude(Function(l) l.Account) _
                        .FirstOrDefault(Function(j) j.SourceDocType = "GoodsReceiptNote" AndAlso j.SourceDocId = grn.GRNId)

                    Assert.IsNotNull(journal, "Expected GoodsReceiptNote journal entry was not found.")
                    Assert.AreEqual(2, journal.Lines.Count)

                    Dim invLine = journal.Lines.First(Function(l) l.Account.Code = "1300")
                    Dim grniLine = journal.Lines.First(Function(l) l.Account.Code = "2100")

                    Assert.AreEqual(1000.0000D, invLine.DebitAmount)
                    Assert.AreEqual(0D, invLine.CreditAmount)
                    Assert.AreEqual(0D, grniLine.DebitAmount)
                    Assert.AreEqual(1000.0000D, grniLine.CreditAmount)
                    Assert.IsTrue(LedgerPostingEngine.IsBalanced(journal.Lines))

                    ' 4. Audit transaction trail recorded
                    Dim invTx = context.InventoryTransactions.FirstOrDefault(Function(t) t.SourceDocType = "GoodsReceiptNote" AndAlso t.SourceDocId = grn.GRNId)
                    Assert.IsNotNull(invTx)
                    Assert.AreEqual(100, invTx.QtyChange)
                    Assert.AreEqual(100, invTx.RunningQty)
                    Assert.AreEqual(10.0000D, invTx.UnitCost)

                    ' Step B: Subsequent delivery at higher price to verify moving weighted average cost recalculation
                    ' Receive second PO for 50 units @ $16.0000
                    ' Expected new weighted cost: ((100 * $10.00) + (50 * $16.00)) / (100 + 50) = (1000 + 800) / 150 = 1800 / 150 = $12.0000
                    Dim po2Lines As New List(Of POLine) From {
                        New POLine(product.ProductId, 50, 16.0000D)
                    }
                    Dim po2 = p2pService.CreatePurchaseOrder(vendor.VendorId, po2Lines, New DateTime(2026, 11, 6), "PO-2026-0002")
                    Dim receipt2Lines As New List(Of GRNLine) From {
                        New GRNLine(po2.Lines.First().POLineId, product.ProductId, 50, 16.0000D)
                    }
                    Dim grn2 = p2pService.ReceiveGoods(po2.POId, receipt2Lines, New DateTime(2026, 11, 7), "GRN-2026-0002")

                    context.Entry(updatedProduct).Reload()
                    Assert.AreEqual(150, updatedProduct.TotalQtyOnHand)
                    Assert.AreEqual(12.0000D, updatedProduct.CurrentUnitCost)

                    Dim journal2 = context.JournalEntries _
                        .Include(Function(j) j.Lines) _
                        .ThenInclude(Function(l) l.Account) _
                        .FirstOrDefault(Function(j) j.SourceDocType = "GoodsReceiptNote" AndAlso j.SourceDocId = grn2.GRNId)

                    Assert.IsNotNull(journal2)
                    Dim invLine2 = journal2.Lines.First(Function(l) l.Account.Code = "1300")
                    Dim grniLine2 = journal2.Lines.First(Function(l) l.Account.Code = "2100")
                    Assert.AreEqual(800.0000D, invLine2.DebitAmount)
                    Assert.AreEqual(800.0000D, grniLine2.CreditAmount)
                    Assert.IsTrue(LedgerPostingEngine.IsBalanced(journal2.Lines))
                End Using
            End Using
        End Sub

        ' =========================================================================
        ' CUJ 2: Sales Stock Guard (attempt oversell -> assert InsufficientStockException; post valid invoice -> assert stock decrement & double-entry GL)
        ' =========================================================================
        <TestMethod>
        Public Sub CUJ2_SalesStockGuard_OversellBlock_And_ValidFulfillmentDoubleEntry()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim periodService As New PeriodService(context)
                    Dim o2cService As New OrderToCashService(context, periodService)

                    ' Given: SKU-A has 100 units on hand at a moving average cost of $10.00/unit
                    Dim customer = CreateTestCustomer(context, "CUST-CUJ2-01", "Metro Merchandising Ltd", creditLimit:=20000.0D)
                    Dim product = CreateTestProduct(context, "SKU-CUJ2-A", "Sales Guard Widget", baseUnitCost:=10.0000D, initialQty:=100)

                    ' -----------------------------------------------------------------
                    ' Part 1: Attempt Oversell (Customer orders 110 units, shortfall of 10 units)
                    ' -----------------------------------------------------------------
                    Dim oversellLines As New List(Of SalesInvoiceLine) From {
                        New SalesInvoiceLine() With {.ProductId = product.ProductId, .FulfilledQty = 110, .UnitPrice = 15.0000D}
                    }

                    Dim postingDate As New DateTime(2026, 11, 12)
                    Dim oversellBlocked As Boolean = False

                    Try
                        o2cService.FulfillSalesInvoice(0, oversellLines, postingDate, customer.CustomerId)
                    Catch ex As InsufficientStockException
                        oversellBlocked = True
                        Assert.AreEqual(product.ProductId, ex.ProductId.GetValueOrDefault())
                        Assert.AreEqual(110, ex.RequestedQty.GetValueOrDefault())
                        Assert.AreEqual(100, ex.AvailableQty.GetValueOrDefault())
                    End Try

                    Assert.IsTrue(oversellBlocked, "Expected InsufficientStockException when fulfilling 110 units against 100 units on hand.")

                    ' Verify database state was unmutated: stock remains 100, customer balance remains 0, 0 invoices created
                    Dim reloadedProduct = context.Products.Find(product.ProductId)
                    Assert.AreEqual(100, reloadedProduct.TotalQtyOnHand)

                    Dim reloadedCustomer = context.Customers.Find(customer.CustomerId)
                    Assert.AreEqual(0D, reloadedCustomer.CurrentARBalance)
                    Assert.AreEqual(0, context.SalesInvoices.Count())

                    ' -----------------------------------------------------------------
                    ' Part 2: Post Valid Invoice for 40 units @ $15.00/unit
                    ' -----------------------------------------------------------------
                    Dim validSoLines As New List(Of SOLine) From {
                        New SOLine(product.ProductId, 40, 15.0000D)
                    }
                    Dim so = o2cService.CreateSalesOrder(customer.CustomerId, validSoLines, New DateTime(2026, 11, 12), "SO-2026-0040")

                    Dim invoice = o2cService.FulfillSalesInvoice(so.SOId, postingDate)

                    Assert.IsNotNull(invoice)
                    Assert.AreEqual("Posted", invoice.Status)
                    Assert.AreEqual(600.0000D, invoice.TotalAmount)

                    ' Verify physical stock decremented from 100 to 60 (100 - 40 = 60)
                    context.Entry(reloadedProduct).Reload()
                    Assert.AreEqual(60, reloadedProduct.TotalQtyOnHand)

                    ' Verify customer subledger updated: AR balance = $600.00
                    context.Entry(reloadedCustomer).Reload()
                    Assert.AreEqual(600.0000D, reloadedCustomer.CurrentARBalance)

                    ' Verify Double-Entry General Ledger:
                    ' Debit 1200 Accounts Receivable: $600.00
                    ' Credit 4000 Sales Revenue: $600.00
                    ' Debit 5000 Cost of Goods Sold: $400.00 (40 units * $10.00)
                    ' Credit 1300 Merchandise Inventory: $400.00 (40 units * $10.00)
                    Dim journal = context.JournalEntries _
                        .Include(Function(j) j.Lines) _
                        .ThenInclude(Function(l) l.Account) _
                        .FirstOrDefault(Function(j) j.SourceDocType = "SalesInvoice" AndAlso j.SourceDocId = invoice.InvoiceId)

                    Assert.IsNotNull(journal, "Expected SalesInvoice journal entry was not found.")
                    Assert.AreEqual(4, journal.Lines.Count)

                    Dim arLine = journal.Lines.First(Function(l) l.Account.Code = "1200")
                    Dim revLine = journal.Lines.First(Function(l) l.Account.Code = "4000")
                    Dim cogsLine = journal.Lines.First(Function(l) l.Account.Code = "5000")
                    Dim invLine = journal.Lines.First(Function(l) l.Account.Code = "1300")

                    Assert.AreEqual(600.0000D, arLine.DebitAmount)
                    Assert.AreEqual(0D, arLine.CreditAmount)
                    Assert.AreEqual(0D, revLine.DebitAmount)
                    Assert.AreEqual(600.0000D, revLine.CreditAmount)
                    Assert.AreEqual(400.0000D, cogsLine.DebitAmount)
                    Assert.AreEqual(0D, cogsLine.CreditAmount)
                    Assert.AreEqual(0D, invLine.DebitAmount)
                    Assert.AreEqual(400.0000D, invLine.CreditAmount)

                    ' Strict double-entry balance: 1000.0000D Debits == 1000.0000D Credits
                    Assert.IsTrue(LedgerPostingEngine.IsBalanced(journal.Lines))

                    ' Audit transaction check
                    Dim invTx = context.InventoryTransactions.FirstOrDefault(Function(t) t.SourceDocType = "SalesInvoice" AndAlso t.SourceDocId = invoice.InvoiceId)
                    Assert.IsNotNull(invTx)
                    Assert.AreEqual(-40, invTx.QtyChange)
                    Assert.AreEqual(60, invTx.RunningQty)
                    Assert.AreEqual(10.0000D, invTx.UnitCost)
                End Using
            End Using
        End Sub

        ' =========================================================================
        ' CUJ 3: Two-Stage Customer Return (quarantine receipt with 0 GL -> inspection 8 restock / 2 scrap -> Credit Note, 5300 shrinkage debit, 1300 restock revaluation)
        ' =========================================================================
        <TestMethod>
        Public Sub CUJ3_TwoStageCustomerReturn_Quarantine_Inspection_CreditNote_And_RestockScrap()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim periodService As New PeriodService(context)
                    Dim o2cService As New OrderToCashService(context, periodService)
                    Dim returnsService As New ReturnsService(context, periodService)

                    ' Given: Customer returns 10 units of SKU-A originally purchased at $15.00/unit (original unit cost $10.00/unit).
                    ' Product on hand has 40 units at $10.00/unit.
                    Dim customer = CreateTestCustomer(context, "CUST-CUJ3-01", "Apex Retail Distributors", creditLimit:=10000.0D)
                    Dim product = CreateTestProduct(context, "SKU-CUJ3-A", "Standard Returnable Widget", baseUnitCost:=10.0000D, initialQty:=50)

                    ' Prior invoice: 10 units fulfilled @ $15.00/unit (leaving 40 on hand, customer AR = $150.00)
                    Dim postingDate As New DateTime(2026, 11, 14)
                    Dim invoiceLines As New List(Of SalesInvoiceLine) From {
                        New SalesInvoiceLine() With {.ProductId = product.ProductId, .FulfilledQty = 10, .UnitPrice = 15.0000D}
                    }
                    Dim invoice = o2cService.FulfillSalesInvoice(0, invoiceLines, postingDate, customer.CustomerId)

                    Dim productAfterSale = context.Products.Find(product.ProductId)
                    Assert.AreEqual(40, productAfterSale.TotalQtyOnHand)
                    Assert.AreEqual(0, productAfterSale.QuarantineQty)
                    Assert.AreEqual(150.0000D, context.Customers.Find(customer.CustomerId).CurrentARBalance)

                    Dim glCountBeforeQuarantine As Integer = context.JournalEntries.Count()

                    ' -----------------------------------------------------------------
                    ' Stage 1: Dock Quarantine Intake (Receive 10 units into quarantine)
                    ' -----------------------------------------------------------------
                    Dim returnLines As New List(Of CustomerReturnLine) From {
                        New CustomerReturnLine() With {.ProductId = product.ProductId, .ReturnedQty = 10}
                    }
                    Dim rma = returnsService.ReceiveReturnToQuarantine(customer.CustomerId, invoice.InvoiceId, returnLines, postingDate, "RMA-2026-CUJ3")

                    Assert.IsNotNull(rma)
                    Assert.AreEqual("Quarantine", rma.Status)
                    Assert.AreEqual(1, rma.Lines.Count)

                    ' Physical inventory on hand remains 40; quarantine stock is 10
                    context.Entry(productAfterSale).Reload()
                    Assert.AreEqual(40, productAfterSale.TotalQtyOnHand)
                    Assert.AreEqual(10, productAfterSale.QuarantineQty)

                    ' CRITICAL INVARIANT: 0 General Ledger entries generated during quarantine intake
                    Dim glCountAfterQuarantine As Integer = context.JournalEntries.Count()
                    Assert.AreEqual(glCountBeforeQuarantine, glCountAfterQuarantine, "Quarantine intake must generate ZERO General Ledger entries.")

                    ' -----------------------------------------------------------------
                    ' Stage 2: Inspection & Disposition (8 restocked, 2 scrapped)
                    ' -----------------------------------------------------------------
                    Dim dispositions As New List(Of ReturnDispositionLine) From {
                        New ReturnDispositionLine(product.ProductId, restockQty:=8, scrapQty:=2)
                    }

                    Dim inspectionDate As New DateTime(2026, 11, 15)
                    Dim inspectedReturn = returnsService.ProcessReturnInspection(rma.ReturnId, dispositions, inspectionDate)

                    Assert.IsNotNull(inspectedReturn)
                    Assert.AreEqual("Posted", inspectedReturn.Status)

                    ' Inventory check:
                    ' Quarantine drops to 0
                    ' TotalQtyOnHand increases by 8 (from 40 to 48)
                    ' Moving average cost revaluation: ((40 * $10.00) + (8 * $10.00)) / (40 + 8) = 480 / 48 = $10.0000
                    context.Entry(productAfterSale).Reload()
                    Assert.AreEqual(0, productAfterSale.QuarantineQty)
                    Assert.AreEqual(48, productAfterSale.TotalQtyOnHand)
                    Assert.AreEqual(10.0000D, productAfterSale.CurrentUnitCost)

                    ' Financial Credit Note Journal Entry Check:
                    ' Debit 4100 Sales Returns & Allowances: $150.00 (10 * $15.00)
                    ' Credit 1200 Accounts Receivable: $150.00
                    ' Debit 1300 Merchandise Inventory: $80.00 (8 restocked * $10.00)
                    ' Debit 5300 Inventory Shrinkage & Damaged Stock Expense: $20.00 (2 scrapped * $10.00)
                    ' Credit 5000 Cost of Goods Sold: $100.00 (10 total * $10.00)
                    Dim creditNoteJournal = context.JournalEntries _
                        .Include(Function(j) j.Lines) _
                        .ThenInclude(Function(l) l.Account) _
                        .FirstOrDefault(Function(j) j.SourceDocType = "CustomerReturn" AndAlso j.SourceDocId = inspectedReturn.ReturnId)

                    Assert.IsNotNull(creditNoteJournal, "Expected CustomerReturn Credit Note journal entry was not found.")
                    Assert.AreEqual(5, creditNoteJournal.Lines.Count)

                    Dim retAllowanceLine = creditNoteJournal.Lines.First(Function(l) l.Account.Code = "4100")
                    Dim arLine = creditNoteJournal.Lines.First(Function(l) l.Account.Code = "1200")
                    Dim invRestockLine = creditNoteJournal.Lines.First(Function(l) l.Account.Code = "1300")
                    Dim shrinkageLine = creditNoteJournal.Lines.First(Function(l) l.Account.Code = "5300")
                    Dim cogsLine = creditNoteJournal.Lines.First(Function(l) l.Account.Code = "5000")

                    Assert.AreEqual(150.0000D, retAllowanceLine.DebitAmount)
                    Assert.AreEqual(0D, retAllowanceLine.CreditAmount)
                    Assert.AreEqual(0D, arLine.DebitAmount)
                    Assert.AreEqual(150.0000D, arLine.CreditAmount)
                    Assert.AreEqual(80.0000D, invRestockLine.DebitAmount)
                    Assert.AreEqual(0D, invRestockLine.CreditAmount)
                    Assert.AreEqual(20.0000D, shrinkageLine.DebitAmount)
                    Assert.AreEqual(0D, shrinkageLine.CreditAmount)
                    Assert.AreEqual(0D, cogsLine.DebitAmount)
                    Assert.AreEqual(100.0000D, cogsLine.CreditAmount)

                    ' Strict balance: Total Debits (150 + 80 + 20 = 250) == Total Credits (150 + 100 = 250)
                    Assert.IsTrue(LedgerPostingEngine.IsBalanced(creditNoteJournal.Lines))

                    ' Subledger check: Customer AR balance cleared from $150.00 to $0.00
                    Dim updatedCustomer = context.Customers.Find(customer.CustomerId)
                    Assert.AreEqual(0D, updatedCustomer.CurrentARBalance)

                    ' Inventory transaction audit check for restocked 8 units
                    Dim invTx = context.InventoryTransactions.FirstOrDefault(Function(t) t.SourceDocType = "CustomerReturn" AndAlso t.SourceDocId = inspectedReturn.ReturnId)
                    Assert.IsNotNull(invTx)
                    Assert.AreEqual(8, invTx.QtyChange)
                    Assert.AreEqual(48, invTx.RunningQty)
                    Assert.AreEqual(10.0000D, invTx.UnitCost)
                End Using
            End Using
        End Sub

        ' =========================================================================
        ' CUJ 4: 3-Way Match with Variances (GRNI clearing, 5100 PPV debit, 5200 Inbound Freight debit, 2000 AP credit)
        ' =========================================================================
        <TestMethod>
        Public Sub CUJ4_ThreeWayMatchWithVariances_GRNI_Clearing_PPV_And_InboundFreight()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim periodService As New PeriodService(context)
                    Dim p2pService As New ProcureToPayService(context, periodService)

                    ' Given: PO exists for 100 units of SKU-A at $10.00/unit ($1,000.00 total)
                    Dim vendor = CreateTestVendor(context, "V-CUJ4-01", "Precision Components Ltd", paymentTermsDays:=30)
                    Dim product = CreateTestProduct(context, "SKU-CUJ4-A", "Precision Bushing", baseUnitCost:=10.0000D, initialQty:=0)

                    Dim poLines As New List(Of POLine) From {
                        New POLine(product.ProductId, 100, 10.0000D)
                    }
                    Dim po = p2pService.CreatePurchaseOrder(vendor.VendorId, poLines, New DateTime(2026, 11, 2), "PO-2026-CUJ4")

                    ' When: Goods Receipt Note confirms 100 units received
                    Dim receiptLines As New List(Of GRNLine) From {
                        New GRNLine(po.Lines.First().POLineId, product.ProductId, 100, 10.0000D)
                    }
                    Dim grn = p2pService.ReceiveGoods(po.POId, receiptLines, New DateTime(2026, 11, 8), "GRN-2026-CUJ4")

                    Assert.AreEqual("Posted", grn.Status)
                    Assert.AreEqual(100, context.Products.Find(product.ProductId).TotalQtyOnHand)

                    ' When: Supplier Bill arrives with:
                    ' - Price variance: $11.00/unit vs $10.00 PO price (PPV = 100 * ($11 - $10) = +$100.00 Unfavorable)
                    ' - Uncapitalized Inbound Freight: $50.00
                    ' - Total Billed: (100 * $11.00) + $50.00 = $1,150.00
                    Dim grnLineId = grn.Lines.First().GRNLineId
                    Dim billLines As New List(Of BillLine) From {
                        New BillLine() With {
                            .GRNLineId = grnLineId,
                            .BilledQty = 100,
                            .BilledUnitPrice = 11.0000D,
                            .FreightAmount = 50.0000D
                        }
                    }

                    Dim billDate As New DateTime(2026, 11, 10)
                    Dim bill = p2pService.MatchSupplierBill(vendor.VendorId, grn.GRNId, billLines, "BILL-2026-CUJ4", billDate)

                    ' Then:
                    ' 1. Supplier bill document posted for $1,150.00
                    Assert.IsNotNull(bill)
                    Assert.AreEqual("Posted", bill.Status)
                    Assert.AreEqual(1150.0000D, bill.TotalAmount)

                    ' 2. Vendor accounts payable subledger incremented by $1,150.00
                    Dim updatedVendor = context.Vendors.Find(vendor.VendorId)
                    Assert.AreEqual(1150.0000D, updatedVendor.CurrentPayableBalance)

                    ' 3. GRN status transitions to Settled
                    Dim updatedGrn = context.GoodsReceiptNotes.Find(grn.GRNId)
                    Assert.AreEqual("Settled", updatedGrn.Status)

                    ' 4. Balanced General Ledger Entry:
                    ' - Debit 2100 GRNI: $1,000.00 (Clearing accrued receipt liability at PO price)
                    ' - Debit 5100 Purchase Price Variance (PPV): $100.00
                    ' - Debit 5200 Inbound Freight Variance: $50.00
                    ' - Credit 2000 Accounts Payable: $1,150.00
                    Dim journal = context.JournalEntries _
                        .Include(Function(j) j.Lines) _
                        .ThenInclude(Function(l) l.Account) _
                        .FirstOrDefault(Function(j) j.SourceDocType = "SupplierBill" AndAlso j.SourceDocId = bill.BillId)

                    Assert.IsNotNull(journal, "Expected SupplierBill journal entry was not found.")
                    Assert.AreEqual(4, journal.Lines.Count)

                    Dim grniLine = journal.Lines.First(Function(l) l.Account.Code = "2100")
                    Dim ppvLine = journal.Lines.First(Function(l) l.Account.Code = "5100")
                    Dim freightLine = journal.Lines.First(Function(l) l.Account.Code = "5200")
                    Dim apLine = journal.Lines.First(Function(l) l.Account.Code = "2000")

                    Assert.AreEqual(1000.0000D, grniLine.DebitAmount)
                    Assert.AreEqual(0D, grniLine.CreditAmount)

                    Assert.AreEqual(100.0000D, ppvLine.DebitAmount)
                    Assert.AreEqual(0D, ppvLine.CreditAmount)

                    Assert.AreEqual(50.0000D, freightLine.DebitAmount)
                    Assert.AreEqual(0D, freightLine.CreditAmount)

                    Assert.AreEqual(0D, apLine.DebitAmount)
                    Assert.AreEqual(1150.0000D, apLine.CreditAmount)

                    ' Strict double-entry balance: 1150.0000D Debits == 1150.0000D Credits
                    Assert.IsTrue(LedgerPostingEngine.IsBalanced(journal.Lines))
                End Using
            End Using
        End Sub

        ' =========================================================================
        ' CUJ 5: Period Lock (posting in sealed period throws ClosedPeriodPostingException)
        ' =========================================================================
        <TestMethod>
        Public Sub CUJ5_PeriodLock_PostingInSealedPeriod_ThrowsClosedPeriodPostingException()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim periodService As New PeriodService(context)
                    Dim o2cService As New OrderToCashService(context, periodService)
                    Dim p2pService As New ProcureToPayService(context, periodService)
                    Dim returnsService As New ReturnsService(context, periodService)

                    Dim customer = CreateTestCustomer(context, "CUST-CUJ5-01", "Period Guard Customer", creditLimit:=10000.0D)
                    Dim vendor = CreateTestVendor(context, "V-CUJ5-01", "Period Guard Vendor", paymentTermsDays:=30)
                    Dim product = CreateTestProduct(context, "SKU-CUJ5-A", "Period Protected Widget", baseUnitCost:=10.0000D, initialQty:=100)

                    ' Get active period (November 2026) and permanently close/seal it
                    Dim activePeriod = periodService.GetActiveFiscalPeriod()
                    Assert.IsNotNull(activePeriod)
                    Assert.IsFalse(activePeriod.IsClosed)

                    periodService.CloseFiscalPeriod(activePeriod.PeriodId)

                    Dim sealedPeriod = context.AccountingPeriods.Find(activePeriod.PeriodId)
                    Assert.IsTrue(sealedPeriod.IsClosed, "Fiscal period must be sealed/closed.")

                    Dim closedPostingDate As New DateTime(2026, 11, 15)

                    ' 1. Outbound Sales Fulfillment in closed period must throw ClosedPeriodPostingException
                    Dim salesInvoiceLines As New List(Of SalesInvoiceLine) From {
                        New SalesInvoiceLine() With {.ProductId = product.ProductId, .FulfilledQty = 10, .UnitPrice = 20.0000D}
                    }

                    Assert.Throws(Of ClosedPeriodPostingException)(Sub()
                        o2cService.FulfillSalesInvoice(0, salesInvoiceLines, closedPostingDate, customer.CustomerId)
                    End Sub)

                    ' Verify stock and customer AR remain untouched
                    Dim reloadedProduct = context.Products.Find(product.ProductId)
                    Assert.AreEqual(100, reloadedProduct.TotalQtyOnHand)
                    Assert.AreEqual(0D, context.Customers.Find(customer.CustomerId).CurrentARBalance)

                    ' 2. Inbound Goods Receipt in closed period must throw ClosedPeriodPostingException
                    Dim poLines As New List(Of POLine) From {
                        New POLine(product.ProductId, 20, 10.0000D)
                    }
                    Dim po = p2pService.CreatePurchaseOrder(vendor.VendorId, poLines, New DateTime(2026, 11, 1), "PO-CUJ5-001")
                    Dim grnLines As New List(Of GRNLine) From {
                        New GRNLine(po.Lines.First().POLineId, product.ProductId, 20, 10.0000D)
                    }

                    Assert.Throws(Of ClosedPeriodPostingException)(Sub()
                        p2pService.ReceiveGoods(po.POId, grnLines, closedPostingDate, postingDate:=closedPostingDate)
                    End Sub)

                    ' Verify stock remains untouched at 100
                    context.Entry(reloadedProduct).Reload()
                    Assert.AreEqual(100, reloadedProduct.TotalQtyOnHand)

                    ' 3. Direct PeriodService posting validation in closed period must throw ClosedPeriodPostingException
                    Assert.Throws(Of ClosedPeriodPostingException)(Sub()
                        periodService.ValidatePostingDate(closedPostingDate)
                    End Sub)
                End Using
            End Using
        End Sub

        ' =========================================================================
        ' Master End-to-End Operational Lifecycle CUJ
        ' Seamlessly links PO -> GRN -> SO -> Invoice -> Payment Receipt -> Bill -> Voucher -> Return -> Credit Note
        ' =========================================================================
        <TestMethod>
        Public Sub FullOperationalLifecycle_ProcureToSalesToReturns_PreservesDoubleEntryIntegrity()
            Using fixture As New TestDatabaseFixture()
                Using context = fixture.CreateContext()
                    Dim periodService As New PeriodService(context)
                    Dim p2pService As New ProcureToPayService(context, periodService)
                    Dim o2cService As New OrderToCashService(context, periodService)
                    Dim returnsService As New ReturnsService(context, periodService)

                    ' Master Chart of Accounts Bank Account (1020 Operating Bank Account)
                    Dim bankAccount = context.Accounts.First(Function(a) a.Code = "1020")

                    ' 1. Setup Vendor, Customer, Product
                    Dim vendor = CreateTestVendor(context, "V-LIFECYCLE", "Primary Merchandiser Ltd", 30)
                    Dim customer = CreateTestCustomer(context, "CUST-LIFECYCLE", "Nationwide Stores Co", 50000.0D)
                    Dim product = CreateTestProduct(context, "SKU-LIFECYCLE-1", "Lifecycle Master Item", 0D, 0)

                    ' 2. PO for 100 units @ $10.00
                    Dim po = p2pService.CreatePurchaseOrder(vendor.VendorId, New List(Of POLine) From {
                        New POLine(product.ProductId, 100, 10.0000D)
                    }, New DateTime(2026, 11, 1), "PO-LIFE-001")

                    ' 3. Receive Goods (GRN) -> Stock = 100, Cost = $10.00, Journal: Dr 1300 $1,000, Cr 2100 $1,000
                    Dim grn = p2pService.ReceiveGoods(po.POId, New List(Of GRNLine) From {
                        New GRNLine(po.Lines.First().POLineId, product.ProductId, 100, 10.0000D)
                    }, New DateTime(2026, 11, 3), "GRN-LIFE-001")

                    Assert.AreEqual(100, context.Products.Find(product.ProductId).TotalQtyOnHand)
                    Assert.AreEqual(10.0000D, context.Products.Find(product.ProductId).CurrentUnitCost)

                    ' 4. Match Supplier Bill -> Dr 2100 $1,000, Cr 2000 $1,000
                    Dim bill = p2pService.MatchSupplierBill(vendor.VendorId, grn.GRNId, New List(Of BillLine) From {
                        New BillLine() With {.GRNLineId = grn.Lines.First().GRNLineId, .BilledQty = 100, .BilledUnitPrice = 10.0000D}
                    }, "BILL-LIFE-001", New DateTime(2026, 11, 5))

                    Assert.AreEqual(1000.0000D, context.Vendors.Find(vendor.VendorId).CurrentPayableBalance)

                    ' 5. Disburse Payment Voucher -> Dr 2000 $1,000, Cr 1020 $1,000
                    Dim voucher = p2pService.DisbursePaymentVoucher(vendor.VendorId, bill.BillId, 1000.0000D, bankAccount.AccountId, New DateTime(2026, 11, 6))
                    Assert.AreEqual(0D, context.Vendors.Find(vendor.VendorId).CurrentPayableBalance)

                    ' 6. Outbound Sales: Customer buys 40 units @ $25.00 ($1,000 revenue, $400 COGS)
                    Dim so = o2cService.CreateSalesOrder(customer.CustomerId, New List(Of SOLine) From {
                        New SOLine(product.ProductId, 40, 25.0000D)
                    }, New DateTime(2026, 11, 7), "SO-LIFE-001")

                    Dim invoice = o2cService.FulfillSalesInvoice(so.SOId, New DateTime(2026, 11, 8))
                    Assert.AreEqual(60, context.Products.Find(product.ProductId).TotalQtyOnHand)
                    Assert.AreEqual(1000.0000D, context.Customers.Find(customer.CustomerId).CurrentARBalance)

                    ' 7. Customer Payment Receipt -> Dr 1020 $1,000, Cr 1200 $1,000
                    Dim receipt = o2cService.RecordPaymentReceipt(customer.CustomerId, invoice.InvoiceId, 1000.0000D, bankAccount.AccountId, New DateTime(2026, 11, 10))
                    Assert.AreEqual(0D, context.Customers.Find(customer.CustomerId).CurrentARBalance)

                    ' 8. Customer Returns 5 units into Quarantine
                    Dim rma = returnsService.ReceiveReturnToQuarantine(customer.CustomerId, invoice.InvoiceId, New List(Of CustomerReturnLine) From {
                        New CustomerReturnLine() With {.ProductId = product.ProductId, .ReturnedQty = 5}
                    }, New DateTime(2026, 11, 12), "RMA-LIFE-001")

                    Assert.AreEqual(60, context.Products.Find(product.ProductId).TotalQtyOnHand)
                    Assert.AreEqual(5, context.Products.Find(product.ProductId).QuarantineQty)

                    ' 9. Process Inspection: 4 restocked, 1 scrapped -> Credit Note
                    Dim inspectedReturn = returnsService.ProcessReturnInspection(rma.ReturnId, New List(Of ReturnDispositionLine) From {
                        New ReturnDispositionLine(product.ProductId, restockQty:=4, scrapQty:=1)
                    }, New DateTime(2026, 11, 14))

                    Assert.AreEqual(64, context.Products.Find(product.ProductId).TotalQtyOnHand)
                    Assert.AreEqual(0, context.Products.Find(product.ProductId).QuarantineQty)

                    ' Assert EVERY Journal Entry across the entire lifecycle is strictly balanced
                    Dim allJournals = context.JournalEntries.Include(Function(j) j.Lines).ToList()
                    Assert.IsTrue(allJournals.Count >= 6, $"Expected at least 6 journal entries, found {allJournals.Count}.")

                    For Each j In allJournals
                        Dim totalDebits = j.Lines.Sum(Function(l) l.DebitAmount)
                        Dim totalCredits = j.Lines.Sum(Function(l) l.CreditAmount)
                        Assert.AreEqual(Math.Round(totalDebits, 4), Math.Round(totalCredits, 4), $"Journal entry {j.EntryId} ({j.SourceDocType}) is out of balance.")
                    Next
                End Using
            End Using
        End Sub

        ' =========================================================================
        ' Private Test Fixture Helpers
        ' =========================================================================

        Private Function CreateTestVendor(context As ErpDbContext, code As String, name As String, paymentTermsDays As Integer, Optional payableBalance As Decimal = 0D) As Vendor
            Dim vendor As New Vendor(code, name, paymentTermsDays, payableBalance)
            context.Vendors.Add(vendor)
            context.SaveChanges()
            Return vendor
        End Function

        Private Function CreateTestCustomer(context As ErpDbContext, code As String, name As String, creditLimit As Decimal, Optional arBalance As Decimal = 0D) As Customer
            Dim customer As New Customer(code, name, creditLimit, arBalance)
            context.Customers.Add(customer)
            context.SaveChanges()
            Return customer
        End Function

        Private Function CreateTestProduct(context As ErpDbContext, sku As String, name As String, baseUnitCost As Decimal, initialQty As Integer) As Product
            Dim product As New Product(sku, name, "Units", baseUnitCost, initialQty)
            context.Products.Add(product)
            context.SaveChanges()
            Return product
        End Function

    End Class

End Namespace
