Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports Merchandising_Accounting_App.Domain
Imports Merchandising_Accounting_App.Domain.Engines
Imports Merchandising_Accounting_App.Domain.Models
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Namespace Merchandising_Accounting_App.Tests.Domain

    <TestClass>
    Public Class LedgerPostingEngineTests

        <TestMethod>
        Public Sub IsBalanced_WithAmounts_ReturnsExpectedBoolean()
            Assert.IsTrue(LedgerPostingEngine.IsBalanced(100.5555D, 100.5555D))
            Assert.IsTrue(LedgerPostingEngine.IsBalanced(100.55554D, 100.55549D)) ' rounds to 100.5555
            Assert.IsFalse(LedgerPostingEngine.IsBalanced(100.5555D, 100.5556D))
            Assert.IsFalse(LedgerPostingEngine.IsBalanced(0D, 10D))
        End Sub

        <TestMethod>
        Public Sub IsBalanced_WithLines_ReturnsExpectedBoolean()
            Dim balancedLines As New List(Of JournalEntryLine) From {
                New JournalEntryLine(1300, 500D, 0D),
                New JournalEntryLine(2100, 0D, 500D)
            }
            Assert.IsTrue(LedgerPostingEngine.IsBalanced(balancedLines))

            Dim unbalancedLines As New List(Of JournalEntryLine) From {
                New JournalEntryLine(1300, 500D, 0D),
                New JournalEntryLine(2100, 0D, 499D)
            }
            Assert.IsFalse(LedgerPostingEngine.IsBalanced(unbalancedLines))
            Assert.IsFalse(LedgerPostingEngine.IsBalanced(CType(Nothing, IEnumerable(Of JournalEntryLine))))
        End Sub

        <TestMethod>
        Public Sub ValidateJournalBalance_WhenBalanced_ReturnsTrue()
            Dim lines As New List(Of JournalEntryLine) From {
                New JournalEntryLine(AccountCodes.MerchandiseInventory, 1250.75D, 0D),
                New JournalEntryLine(AccountCodes.GRNIClearing, 0D, 1250.75D)
            }
            Assert.IsTrue(LedgerPostingEngine.ValidateJournalBalance(lines))
        End Sub

        <TestMethod>
        Public Sub ValidateJournalBalance_WhenUnbalanced_ThrowsUnbalancedJournalEntryException()
            Dim lines As New List(Of JournalEntryLine) From {
                New JournalEntryLine(AccountCodes.MerchandiseInventory, 1000.00D, 0D),
                New JournalEntryLine(AccountCodes.GRNIClearing, 0D, 950.00D)
            }

            Dim ex As UnbalancedJournalEntryException = Assert.Throws(Of UnbalancedJournalEntryException)(
                Sub()
                    LedgerPostingEngine.ValidateJournalBalance(lines)
                End Sub)

            Assert.AreEqual(1000.00D, ex.TotalDebits)
            Assert.AreEqual(950.00D, ex.TotalCredits)
            Assert.AreEqual(50.00D, ex.Imbalance)
        End Sub

        <TestMethod>
        Public Sub ValidateJournalBalance_WhenNull_ThrowsArgumentNullException()
            Assert.Throws(Of ArgumentNullException)(
                Sub()
                    LedgerPostingEngine.ValidateJournalBalance(Nothing)
                End Sub)
        End Sub

        <TestMethod>
        Public Sub AllocatePennyRounding_WhenAlreadyBalanced_ReturnsFalseAndAddsNoLines()
            Dim lines As New List(Of JournalEntryLine) From {
                New JournalEntryLine(1300, 100D, 0D),
                New JournalEntryLine(2100, 0D, 100D)
            }

            Dim modified As Boolean = LedgerPostingEngine.AllocatePennyRounding(lines)
            Assert.IsFalse(modified)
            Assert.AreEqual(2, lines.Count)
        End Sub

        <TestMethod>
        Public Sub AllocatePennyRounding_DebitsExceedCreditsByOneCent_AddsCreditLineTo5500()
            ' Debits: 100.01, Credits: 100.00 -> Variance = +0.01
            Dim lines As New List(Of JournalEntryLine) From {
                New JournalEntryLine(1300, 100.01D, 0D),
                New JournalEntryLine(2100, 0D, 100.00D)
            }

            Dim modified As Boolean = LedgerPostingEngine.AllocatePennyRounding(lines)
            Assert.IsTrue(modified)
            Assert.AreEqual(3, lines.Count)

            Dim roundingLine As JournalEntryLine = lines(2)
            Assert.AreEqual(AccountCodes.CashRoundingVariance, roundingLine.AccountId)
            Assert.AreEqual(0D, roundingLine.DebitAmount)
            Assert.AreEqual(0.01D, roundingLine.CreditAmount)
            Assert.IsTrue(LedgerPostingEngine.IsBalanced(lines))
        End Sub

        <TestMethod>
        Public Sub AllocatePennyRounding_CreditsExceedDebitsByTwoCents_AddsDebitLineTo5500()
            ' Debits: 99.98, Credits: 100.00 -> Variance = -0.02
            Dim lines As New List(Of JournalEntryLine) From {
                New JournalEntryLine(1300, 99.98D, 0D),
                New JournalEntryLine(2100, 0D, 100.00D)
            }

            Dim modified As Boolean = LedgerPostingEngine.AllocatePennyRounding(lines)
            Assert.IsTrue(modified)
            Assert.AreEqual(3, lines.Count)

            Dim roundingLine As JournalEntryLine = lines(2)
            Assert.AreEqual(AccountCodes.CashRoundingVariance, roundingLine.AccountId)
            Assert.AreEqual(0.02D, roundingLine.DebitAmount)
            Assert.AreEqual(0D, roundingLine.CreditAmount)
            Assert.IsTrue(LedgerPostingEngine.IsBalanced(lines))
        End Sub

        <TestMethod>
        Public Sub AllocatePennyRounding_VarianceExceedsThreshold_ThrowsUnbalancedJournalEntryException()
            ' Variance = 0.03 > 0.02
            Dim lines As New List(Of JournalEntryLine) From {
                New JournalEntryLine(1300, 100.03D, 0D),
                New JournalEntryLine(2100, 0D, 100.00D)
            }

            Assert.Throws(Of UnbalancedJournalEntryException)(
                Sub()
                    LedgerPostingEngine.AllocatePennyRounding(lines)
                End Sub)
        End Sub

        <TestMethod>
        Public Sub AllocatePennyRounding_WhenNull_ThrowsArgumentNullException()
            Assert.Throws(Of ArgumentNullException)(
                Sub()
                    LedgerPostingEngine.AllocatePennyRounding(Nothing)
                End Sub)
        End Sub

        <TestMethod>
        Public Sub GenerateGRNJournal_SingleLine_CreatesBalancedDebitsAndCredits()
            ' CUJ 1: Inbound dock receipt: 100 units @ $10.00
            Dim grn As New GoodsReceiptNote() With {
                .GRNId = 1,
                .POId = 10,
                .DocumentNumber = "GRN-2026-001",
                .ReceiptDate = New DateTime(2026, 11, 16),
                .PostingDate = New DateTime(2026, 11, 16),
                .Status = "Posted"
            }
            Dim grnLines As New List(Of GRNLine) From {
                New GRNLine(1, 101, 100, 10.0000D)
            }

            Dim entry As JournalEntry = LedgerPostingEngine.GenerateGRNJournal(grn, grnLines)

            Assert.IsNotNull(entry)
            Assert.AreEqual("GoodsReceiptNote", entry.SourceDocType)
            Assert.AreEqual(1, entry.SourceDocId)
            Assert.AreEqual(grn.PostingDate, entry.PostingDate)
            Assert.AreEqual(grn.ReceiptDate, entry.OperationalEventDate)
            Assert.AreEqual(2, entry.Lines.Count)

            Dim debitLine As JournalEntryLine = entry.Lines.First(Function(l) l.DebitAmount > 0D)
            Dim creditLine As JournalEntryLine = entry.Lines.First(Function(l) l.CreditAmount > 0D)

            Assert.AreEqual(AccountCodes.MerchandiseInventory, debitLine.AccountId)
            Assert.AreEqual(1000.0000D, debitLine.DebitAmount)
            Assert.AreEqual(0D, debitLine.CreditAmount)

            Assert.AreEqual(AccountCodes.GRNIClearing, creditLine.AccountId)
            Assert.AreEqual(0D, creditLine.DebitAmount)
            Assert.AreEqual(1000.0000D, creditLine.CreditAmount)

            Assert.IsTrue(LedgerPostingEngine.IsBalanced(entry.Lines))
        End Sub

        <TestMethod>
        Public Sub GenerateGRNJournal_MultiLineWithCustomAccountLookup_MapsCorrectly()
            Dim grn As New GoodsReceiptNote() With {
                .GRNId = 2,
                .DocumentNumber = "GRN-002",
                .ReceiptDate = DateTime.Today,
                .PostingDate = DateTime.Today
            }
            Dim lines As New List(Of GRNLine) From {
                New GRNLine(1, 101, 50, 12.0000D),
                New GRNLine(2, 102, 20, 25.0000D)
            }

            ' Custom AccountId lookup (e.g. from EF Core database IDs)
            Dim lookup As Func(Of String, Integer) = Function(code)
                                                         If code = "1300" Then Return 91300
                                                         If code = "2100" Then Return 92100
                                                         Return Integer.Parse(code)
                                                     End Function

            Dim entry As JournalEntry = LedgerPostingEngine.GenerateGRNJournal(grn, lines, lookup)

            ' Total = (50 * 12) + (20 * 25) = 600 + 500 = 1100.0000
            Assert.AreEqual(1100.0000D, entry.Lines.Sum(Function(l) l.DebitAmount))
            Assert.AreEqual(1100.0000D, entry.Lines.Sum(Function(l) l.CreditAmount))

            Dim debitLine As JournalEntryLine = entry.Lines.First(Function(l) l.DebitAmount > 0D)
            Dim creditLine As JournalEntryLine = entry.Lines.First(Function(l) l.CreditAmount > 0D)
            Assert.AreEqual(91300, debitLine.AccountId)
            Assert.AreEqual(92100, creditLine.AccountId)
        End Sub

        <TestMethod>
        Public Sub GenerateGRNJournal_InvalidInputs_ThrowsAppropriateExceptions()
            Dim grn As New GoodsReceiptNote() With {.GRNId = 1, .DocumentNumber = "GRN-001"}
            Dim emptyLines As New List(Of GRNLine)()

            Assert.Throws(Of ArgumentNullException)(Sub() LedgerPostingEngine.GenerateGRNJournal(Nothing, New List(Of GRNLine) From {New GRNLine(1, 1, 10, 5D)}))
            Assert.Throws(Of ArgumentNullException)(Sub() LedgerPostingEngine.GenerateGRNJournal(grn, Nothing))
            Assert.Throws(Of ArgumentException)(Sub() LedgerPostingEngine.GenerateGRNJournal(grn, emptyLines))

            ' Negative qty
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() LedgerPostingEngine.GenerateGRNJournal(grn, New List(Of GRNLine) From {New GRNLine(1, 1, -5, 10D)}))

            ' Negative price
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() LedgerPostingEngine.GenerateGRNJournal(grn, New List(Of GRNLine) From {New GRNLine(1, 1, 5, -10D)}))

            ' Zero total
            Assert.Throws(Of ArgumentException)(
                Sub() LedgerPostingEngine.GenerateGRNJournal(grn, New List(Of GRNLine) From {New GRNLine(1, 1, 0, 10D)}))
        End Sub

        <TestMethod>
        Public Sub GenerateSupplierBillJournal_ExactMatchToPO_NoPriceVariance()
            Dim bill As New SupplierBill() With {
                .BillId = 1,
                .DocumentNumber = "BILL-001",
                .BillDate = DateTime.Today,
                .PostingDate = DateTime.Today,
                .TotalAmount = 1000.0000D
            }
            Dim grnLine As New GRNLine(1, 101, 100, 10.0000D) With {.GRNLineId = 5}
            Dim billLines As New List(Of BillLine) From {
                New BillLine() With {
                    .BillId = 1,
                    .GRNLineId = 5,
                    .GRNLine = grnLine,
                    .ProductId = 101,
                    .BilledQty = 100,
                    .BilledUnitPrice = 10.0000D,
                    .LineTotal = 1000.0000D,
                    .FreightAmount = 0D
                }
            }

            Dim entry As JournalEntry = LedgerPostingEngine.GenerateSupplierBillJournal(bill, billLines)

            Assert.IsNotNull(entry)
            Assert.AreEqual("SupplierBill", entry.SourceDocType)
            Assert.AreEqual(2, entry.Lines.Count)

            ' Debit 2100 GRNI $1,000.00, Credit 2000 AP $1,000.00
            Dim debitLine As JournalEntryLine = entry.Lines.First(Function(l) l.DebitAmount > 0D)
            Dim creditLine As JournalEntryLine = entry.Lines.First(Function(l) l.CreditAmount > 0D)

            Assert.AreEqual(AccountCodes.GRNIClearing, debitLine.AccountId)
            Assert.AreEqual(1000.0000D, debitLine.DebitAmount)
            Assert.AreEqual(AccountCodes.AccountsPayable, creditLine.AccountId)
            Assert.AreEqual(1000.0000D, creditLine.CreditAmount)
            Assert.IsTrue(LedgerPostingEngine.IsBalanced(entry.Lines))
        End Sub

        <TestMethod>
        Public Sub GenerateSupplierBillJournal_UnfavorablePriceVariance_DebitsPPV()
            ' PO price: $10.00, Billed price: $12.00 (Unfavorable variance of $2.00/unit across 100 units = $200.00)
            Dim bill As New SupplierBill() With {.BillId = 2, .DocumentNumber = "BILL-002", .BillDate = DateTime.Today, .PostingDate = DateTime.Today}
            Dim grnLine As New GRNLine(1, 101, 100, 10.0000D) With {.GRNLineId = 10}
            Dim billLines As New List(Of BillLine) From {
                New BillLine() With {
                    .BillId = 2,
                    .GRNLineId = 10,
                    .GRNLine = grnLine,
                    .ProductId = 101,
                    .BilledQty = 100,
                    .BilledUnitPrice = 12.0000D,
                    .LineTotal = 1200.0000D
                }
            }

            Dim entry As JournalEntry = LedgerPostingEngine.GenerateSupplierBillJournal(bill, billLines)

            Assert.AreEqual(3, entry.Lines.Count)
            ' Debit 2100 GRNI: $1,000.00
            Dim grniLine As JournalEntryLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.GRNIClearing)
            Assert.AreEqual(1000.0000D, grniLine.DebitAmount)
            ' Debit 5100 PPV: $200.00
            Dim ppvLine As JournalEntryLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.PurchasePriceVariance)
            Assert.AreEqual(200.0000D, ppvLine.DebitAmount)
            Assert.AreEqual(0D, ppvLine.CreditAmount)
            ' Credit 2000 AP: $1,200.00
            Dim apLine As JournalEntryLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.AccountsPayable)
            Assert.AreEqual(1200.0000D, apLine.CreditAmount)

            Assert.IsTrue(LedgerPostingEngine.IsBalanced(entry.Lines))
        End Sub

        <TestMethod>
        Public Sub GenerateSupplierBillJournal_FavorablePriceVariance_CreditsPPV()
            ' PO price: $10.00, Billed price: $9.00 (Favorable variance of $1.00/unit across 100 units = $100.00)
            Dim bill As New SupplierBill() With {.BillId = 3, .DocumentNumber = "BILL-003", .BillDate = DateTime.Today, .PostingDate = DateTime.Today}
            Dim grnLine As New GRNLine(1, 101, 100, 10.0000D) With {.GRNLineId = 11}
            Dim billLines As New List(Of BillLine) From {
                New BillLine() With {
                    .BillId = 3,
                    .GRNLineId = 11,
                    .GRNLine = grnLine,
                    .ProductId = 101,
                    .BilledQty = 100,
                    .BilledUnitPrice = 9.0000D,
                    .LineTotal = 900.0000D
                }
            }

            Dim entry As JournalEntry = LedgerPostingEngine.GenerateSupplierBillJournal(bill, billLines)

            ' Debit 2100 GRNI: $1,000.00
            Dim grniLine As JournalEntryLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.GRNIClearing)
            Assert.AreEqual(1000.0000D, grniLine.DebitAmount)
            ' Credit 5100 PPV: $100.00
            Dim ppvLine As JournalEntryLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.PurchasePriceVariance)
            Assert.AreEqual(0D, ppvLine.DebitAmount)
            Assert.AreEqual(100.0000D, ppvLine.CreditAmount)
            ' Credit 2000 AP: $900.00
            Dim apLine As JournalEntryLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.AccountsPayable)
            Assert.AreEqual(900.0000D, apLine.CreditAmount)

            Assert.IsTrue(LedgerPostingEngine.IsBalanced(entry.Lines))
        End Sub

        <TestMethod>
        Public Sub GenerateSupplierBillJournal_WithFreightAndExplicitGRNLines_RoutesToFreightVariance()
            Dim bill As New SupplierBill() With {.BillId = 4, .DocumentNumber = "BILL-004", .BillDate = DateTime.Today, .PostingDate = DateTime.Today}
            Dim grnLines As New List(Of GRNLine) From {
                New GRNLine(1, 101, 50, 20.0000D) With {.GRNLineId = 21}
            }
            Dim billLines As New List(Of BillLine) From {
                New BillLine() With {
                    .BillId = 4,
                    .GRNLineId = 21,
                    .ProductId = 101,
                    .BilledQty = 50,
                    .BilledUnitPrice = 20.0000D,
                    .FreightAmount = 75.0000D
                }
            }

            Dim entry As JournalEntry = LedgerPostingEngine.GenerateSupplierBillJournal(bill, billLines, grnLines)

            ' Debit 2100 GRNI: $1,000.00
            Dim grniLine As JournalEntryLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.GRNIClearing)
            Assert.AreEqual(1000.0000D, grniLine.DebitAmount)
            ' Debit 5200 Inbound Freight Variance: $75.00
            Dim freightLine As JournalEntryLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.InboundFreightVariance)
            Assert.AreEqual(75.0000D, freightLine.DebitAmount)
            ' Credit 2000 AP: $1,075.00
            Dim apLine As JournalEntryLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.AccountsPayable)
            Assert.AreEqual(1075.0000D, apLine.CreditAmount)

            Assert.IsTrue(LedgerPostingEngine.IsBalanced(entry.Lines))
        End Sub

        <TestMethod>
        Public Sub GenerateSupplierBillJournal_StandaloneCarrierFreightBill_DebitsFreightVariance()
            Dim bill As New SupplierBill() With {.BillId = 5, .DocumentNumber = "BILL-FREIGHT", .BillDate = DateTime.Today, .PostingDate = DateTime.Today}
            Dim billLines As New List(Of BillLine) From {
                New BillLine() With {
                    .BillId = 5,
                    .BilledQty = 0,
                    .LineTotal = 250.0000D,
                    .FreightAmount = 250.0000D
                }
            }

            Dim entry As JournalEntry = LedgerPostingEngine.GenerateSupplierBillJournal(bill, billLines)

            Assert.AreEqual(2, entry.Lines.Count)
            Dim freightLine As JournalEntryLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.InboundFreightVariance)
            Assert.AreEqual(250.0000D, freightLine.DebitAmount)
            Dim apLine As JournalEntryLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.AccountsPayable)
            Assert.AreEqual(250.0000D, apLine.CreditAmount)
            Assert.IsTrue(LedgerPostingEngine.IsBalanced(entry.Lines))
        End Sub

        <TestMethod>
        Public Sub GenerateSupplierBillJournal_InvalidInputs_ThrowsAppropriateExceptions()
            Dim bill As New SupplierBill() With {.BillId = 1, .DocumentNumber = "BILL-001"}
            Assert.Throws(Of ArgumentNullException)(Sub() LedgerPostingEngine.GenerateSupplierBillJournal(Nothing, New List(Of BillLine)()))
            Assert.Throws(Of ArgumentNullException)(Sub() LedgerPostingEngine.GenerateSupplierBillJournal(bill, Nothing))
            Assert.Throws(Of ArgumentException)(Sub() LedgerPostingEngine.GenerateSupplierBillJournal(bill, New List(Of BillLine)()))

            ' Negative qty
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() LedgerPostingEngine.GenerateSupplierBillJournal(bill, New List(Of BillLine) From {New BillLine() With {.BilledQty = -1}}))
            ' Negative price
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() LedgerPostingEngine.GenerateSupplierBillJournal(bill, New List(Of BillLine) From {New BillLine() With {.BilledUnitPrice = -1D}}))
            ' Negative freight
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() LedgerPostingEngine.GenerateSupplierBillJournal(bill, New List(Of BillLine) From {New BillLine() With {.FreightAmount = -1D}}))
            ' Zero total
            Assert.Throws(Of ArgumentException)(
                Sub() LedgerPostingEngine.GenerateSupplierBillJournal(bill, New List(Of BillLine) From {New BillLine() With {.BilledQty = 0, .LineTotal = 0D}}))
        End Sub

        <TestMethod>
        Public Sub GenerateSalesInvoiceJournal_StandardFulfillment_CreatesRevenueAndCOGS()
            ' CUJ 2: Outbound Sales: 50 units @ $15.00 selling price, moving avg cost $10.00
            Dim invoice As New SalesInvoice() With {
                .InvoiceId = 1,
                .CustomerId = 10,
                .DocumentNumber = "INV-2026-001",
                .InvoiceDate = New DateTime(2026, 11, 16),
                .PostingDate = New DateTime(2026, 11, 16),
                .TotalAmount = 750.0000D,
                .Status = "Posted"
            }
            Dim lines As New List(Of SalesInvoiceLine) From {
                New SalesInvoiceLine(101, 50, 15.0000D, 10.0000D)
            }

            Dim entry As JournalEntry = LedgerPostingEngine.GenerateSalesInvoiceJournal(invoice, lines)

            Assert.IsNotNull(entry)
            Assert.AreEqual("SalesInvoice", entry.SourceDocType)
            Assert.AreEqual(1, entry.SourceDocId)
            Assert.AreEqual(invoice.PostingDate, entry.PostingDate)
            Assert.AreEqual(invoice.InvoiceDate, entry.OperationalEventDate)
            Assert.AreEqual(4, entry.Lines.Count)

            ' Revenue lines: Debit 1200 AR $750.00, Credit 4000 Sales Revenue $750.00
            Dim arLine As JournalEntryLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.AccountsReceivable)
            Assert.AreEqual(750.0000D, arLine.DebitAmount)
            Dim revLine As JournalEntryLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.SalesRevenue)
            Assert.AreEqual(750.0000D, revLine.CreditAmount)

            ' COGS lines: Debit 5000 COGS $500.00, Credit 1300 Merchandise Inventory $500.00
            Dim cogsLine As JournalEntryLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.CostOfGoodsSold)
            Assert.AreEqual(500.0000D, cogsLine.DebitAmount)
            Dim invLine As JournalEntryLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.MerchandiseInventory)
            Assert.AreEqual(500.0000D, invLine.CreditAmount)

            Assert.IsTrue(LedgerPostingEngine.IsBalanced(entry.Lines))
        End Sub

        <TestMethod>
        Public Sub GenerateSalesInvoiceJournal_InvalidInputs_ThrowsAppropriateExceptions()
            Dim inv As New SalesInvoice() With {.InvoiceId = 1, .DocumentNumber = "INV-001"}
            Assert.Throws(Of ArgumentNullException)(Sub() LedgerPostingEngine.GenerateSalesInvoiceJournal(Nothing, New List(Of SalesInvoiceLine)()))
            Assert.Throws(Of ArgumentNullException)(Sub() LedgerPostingEngine.GenerateSalesInvoiceJournal(inv, Nothing))
            Assert.Throws(Of ArgumentException)(Sub() LedgerPostingEngine.GenerateSalesInvoiceJournal(inv, New List(Of SalesInvoiceLine)()))

            ' Negative fulfilled qty
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() LedgerPostingEngine.GenerateSalesInvoiceJournal(inv, New List(Of SalesInvoiceLine) From {New SalesInvoiceLine(1, -1, 10D, 5D)}))
            ' Negative price
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() LedgerPostingEngine.GenerateSalesInvoiceJournal(inv, New List(Of SalesInvoiceLine) From {New SalesInvoiceLine(1, 1, -10D, 5D)}))
            ' Negative COGS cost
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() LedgerPostingEngine.GenerateSalesInvoiceJournal(inv, New List(Of SalesInvoiceLine) From {New SalesInvoiceLine(1, 1, 10D, -5D)}))
            ' Zero total
            Assert.Throws(Of ArgumentException)(
                Sub() LedgerPostingEngine.GenerateSalesInvoiceJournal(inv, New List(Of SalesInvoiceLine) From {New SalesInvoiceLine(1, 0, 10D, 5D)}))
        End Sub

        <TestMethod>
        Public Sub GenerateCreditNoteJournal_CUJ3_MixedRestockedAndScrappedUnits_BalancesAccurately()
            ' CUJ 3: 10 units returned @ $15.00 selling price, original cost $10.00.
            ' Inspection: 8 restocked, 2 scrapped.
            ' Allowance = 10 * $15.00 = $150.00 (Debit 4100, Credit 1200)
            ' Restocked = 8 * $10.00 = $80.00 (Debit 1300)
            ' Scrapped = 2 * $10.00 = $20.00 (Debit 5300)
            ' COGS Credit = 10 * $10.00 = $100.00 (Credit 5000)
            Dim retDoc As New CustomerReturn() With {
                .ReturnId = 1,
                .CustomerId = 5,
                .OriginalInvoiceId = 1,
                .DocumentNumber = "CR-2026-001",
                .ReturnDate = New DateTime(2026, 11, 17),
                .PostingDate = New DateTime(2026, 11, 17),
                .Status = "Posted"
            }
            Dim lines As New List(Of CustomerReturnLine) From {
                New CustomerReturnLine(101, 10, 8, 2, 10.0000D, 15.0000D)
            }

            Dim entry As JournalEntry = LedgerPostingEngine.GenerateCreditNoteJournal(retDoc, lines)

            Assert.IsNotNull(entry)
            Assert.AreEqual("CustomerReturn", entry.SourceDocType)
            Assert.AreEqual(1, entry.SourceDocId)
            Assert.AreEqual(5, entry.Lines.Count)

            ' Debit 4100 Sales Returns & Allowances: $150.00
            Dim sraLine As JournalEntryLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.SalesReturnsAndAllowances)
            Assert.AreEqual(150.0000D, sraLine.DebitAmount)

            ' Credit 1200 Accounts Receivable: $150.00
            Dim arLine As JournalEntryLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.AccountsReceivable)
            Assert.AreEqual(150.0000D, arLine.CreditAmount)

            ' Debit 1300 Merchandise Inventory: $80.00
            Dim invLine As JournalEntryLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.MerchandiseInventory)
            Assert.AreEqual(80.0000D, invLine.DebitAmount)

            ' Debit 5300 Inventory Shrinkage: $20.00
            Dim scrapLine As JournalEntryLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.InventoryShrinkage)
            Assert.AreEqual(20.0000D, scrapLine.DebitAmount)

            ' Credit 5000 Cost of Goods Sold: $100.00
            Dim cogsLine As JournalEntryLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.CostOfGoodsSold)
            Assert.AreEqual(100.0000D, cogsLine.CreditAmount)

            Assert.IsTrue(LedgerPostingEngine.IsBalanced(entry.Lines))
        End Sub

        <TestMethod>
        Public Sub GenerateCreditNoteJournal_RestockOnlyAndScrapOnly_GeneratesBalancedLines()
            ' Restock only
            Dim retDoc1 As New CustomerReturn() With {.ReturnId = 2, .DocumentNumber = "CR-002", .ReturnDate = DateTime.Today, .PostingDate = DateTime.Today}
            Dim lines1 As New List(Of CustomerReturnLine) From {
                New CustomerReturnLine(101, 5, 5, 0, 10.0000D, 20.0000D)
            }
            Dim entry1 As JournalEntry = LedgerPostingEngine.GenerateCreditNoteJournal(retDoc1, lines1)
            Assert.AreEqual(4, entry1.Lines.Count)
            Assert.IsTrue(LedgerPostingEngine.IsBalanced(entry1.Lines))

            ' Scrap only
            Dim retDoc2 As New CustomerReturn() With {.ReturnId = 3, .DocumentNumber = "CR-003", .ReturnDate = DateTime.Today, .PostingDate = DateTime.Today}
            Dim lines2 As New List(Of CustomerReturnLine) From {
                New CustomerReturnLine(101, 3, 0, 3, 10.0000D, 20.0000D)
            }
            Dim entry2 As JournalEntry = LedgerPostingEngine.GenerateCreditNoteJournal(retDoc2, lines2)
            Assert.AreEqual(4, entry2.Lines.Count)
            Assert.IsTrue(LedgerPostingEngine.IsBalanced(entry2.Lines))
        End Sub

        <TestMethod>
        Public Sub GenerateCreditNoteJournal_InvalidInputs_ThrowsAppropriateExceptions()
            Dim retDoc As New CustomerReturn() With {.ReturnId = 1, .DocumentNumber = "CR-001"}
            Assert.Throws(Of ArgumentNullException)(Sub() LedgerPostingEngine.GenerateCreditNoteJournal(Nothing, New List(Of CustomerReturnLine)()))
            Assert.Throws(Of ArgumentNullException)(Sub() LedgerPostingEngine.GenerateCreditNoteJournal(retDoc, Nothing))
            Assert.Throws(Of ArgumentException)(Sub() LedgerPostingEngine.GenerateCreditNoteJournal(retDoc, New List(Of CustomerReturnLine)()))

            ' Negative returned qty
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() LedgerPostingEngine.GenerateCreditNoteJournal(retDoc, New List(Of CustomerReturnLine) From {New CustomerReturnLine(1, -1, 0, 0, 10D, 15D)}))
            ' Negative restock qty
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() LedgerPostingEngine.GenerateCreditNoteJournal(retDoc, New List(Of CustomerReturnLine) From {New CustomerReturnLine(1, 1, -1, 0, 10D, 15D)}))
            ' Negative scrap qty
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() LedgerPostingEngine.GenerateCreditNoteJournal(retDoc, New List(Of CustomerReturnLine) From {New CustomerReturnLine(1, 1, 0, -1, 10D, 15D)}))
            ' Negative original cost
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() LedgerPostingEngine.GenerateCreditNoteJournal(retDoc, New List(Of CustomerReturnLine) From {New CustomerReturnLine(1, 1, 1, 0, -10D, 15D)}))
            ' Negative price
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() LedgerPostingEngine.GenerateCreditNoteJournal(retDoc, New List(Of CustomerReturnLine) From {New CustomerReturnLine(1, 1, 1, 0, 10D, -15D)}))
            ' Zero total
            Assert.Throws(Of ArgumentException)(
                Sub() LedgerPostingEngine.GenerateCreditNoteJournal(retDoc, New List(Of CustomerReturnLine) From {New CustomerReturnLine(1, 0, 0, 0, 0D, 0D)}))
        End Sub

        <TestMethod>
        Public Sub GeneratePaymentVoucherJournal_VendorPayment_DebitsAPAndCreditsBank()
            Dim voucher As New PaymentVoucher() With {
                .VoucherId = 1,
                .VoucherNumber = "PV-2026-001",
                .VendorId = 10,
                .PaymentDate = DateTime.Today,
                .PostingDate = DateTime.Today,
                .DisbursedAmount = 1000.0000D,
                .BankAccountId = AccountCodes.OperatingBank
            }

            Dim entry As JournalEntry = LedgerPostingEngine.GeneratePaymentVoucherJournal(voucher)

            Assert.IsNotNull(entry)
            Assert.AreEqual("PaymentVoucher", entry.SourceDocType)
            Assert.AreEqual(2, entry.Lines.Count)

            Dim debitLine As JournalEntryLine = entry.Lines.First(Function(l) l.DebitAmount > 0D)
            Dim creditLine As JournalEntryLine = entry.Lines.First(Function(l) l.CreditAmount > 0D)

            Assert.AreEqual(AccountCodes.AccountsPayable, debitLine.AccountId)
            Assert.AreEqual(1000.0000D, debitLine.DebitAmount)
            Assert.AreEqual(AccountCodes.OperatingBank, creditLine.AccountId)
            Assert.AreEqual(1000.0000D, creditLine.CreditAmount)

            Assert.IsTrue(LedgerPostingEngine.IsBalanced(entry.Lines))
        End Sub

        <TestMethod>
        Public Sub GeneratePaymentVoucherJournal_CustomerRefund_DebitsARAndCreditsBank()
            ' CUJ 3 cash refund: debit 1200 AR, credit 1020 Bank
            Dim voucher As New PaymentVoucher() With {
                .VoucherId = 2,
                .VoucherNumber = "PV-REFUND-001",
                .CustomerId = 5,
                .VoucherType = "CustomerRefund",
                .PaymentDate = DateTime.Today,
                .PostingDate = DateTime.Today,
                .DisbursedAmount = 150.0000D,
                .BankAccountId = AccountCodes.OperatingBank
            }

            Dim entry As JournalEntry = LedgerPostingEngine.GeneratePaymentVoucherJournal(voucher)

            Assert.AreEqual(2, entry.Lines.Count)
            Dim debitLine As JournalEntryLine = entry.Lines.First(Function(l) l.DebitAmount > 0D)
            Dim creditLine As JournalEntryLine = entry.Lines.First(Function(l) l.CreditAmount > 0D)

            Assert.AreEqual(AccountCodes.AccountsReceivable, debitLine.AccountId)
            Assert.AreEqual(150.0000D, debitLine.DebitAmount)
            Assert.AreEqual(AccountCodes.OperatingBank, creditLine.AccountId)
            Assert.AreEqual(150.0000D, creditLine.CreditAmount)

            Assert.IsTrue(LedgerPostingEngine.IsBalanced(entry.Lines))
        End Sub

        <TestMethod>
        Public Sub GeneratePaymentVoucherJournal_InvalidInputs_ThrowsAppropriateExceptions()
            Assert.Throws(Of ArgumentNullException)(Sub() LedgerPostingEngine.GeneratePaymentVoucherJournal(Nothing))

            Dim zeroVoucher As New PaymentVoucher() With {.DisbursedAmount = 0D}
            Assert.Throws(Of ArgumentException)(Sub() LedgerPostingEngine.GeneratePaymentVoucherJournal(zeroVoucher))

            Dim negVoucher As New PaymentVoucher() With {.DisbursedAmount = -50D}
            Assert.Throws(Of ArgumentException)(Sub() LedgerPostingEngine.GeneratePaymentVoucherJournal(negVoucher))
        End Sub

        <TestMethod>
        Public Sub GeneratePaymentReceiptJournal_CustomerPayment_DebitsBankAndCreditsAR()
            Dim receipt As New PaymentReceipt() With {
                .ReceiptId = 1,
                .CustomerId = 15,
                .ReceiptDate = DateTime.Today,
                .PostingDate = DateTime.Today,
                .AmountReceived = 750.0000D,
                .BankAccountId = AccountCodes.OperatingBank
            }

            Dim entry As JournalEntry = LedgerPostingEngine.GeneratePaymentReceiptJournal(receipt)

            Assert.IsNotNull(entry)
            Assert.AreEqual("PaymentReceipt", entry.SourceDocType)
            Assert.AreEqual(2, entry.Lines.Count)

            Dim debitLine As JournalEntryLine = entry.Lines.First(Function(l) l.DebitAmount > 0D)
            Dim creditLine As JournalEntryLine = entry.Lines.First(Function(l) l.CreditAmount > 0D)

            Assert.AreEqual(AccountCodes.OperatingBank, debitLine.AccountId)
            Assert.AreEqual(750.0000D, debitLine.DebitAmount)
            Assert.AreEqual(AccountCodes.AccountsReceivable, creditLine.AccountId)
            Assert.AreEqual(750.0000D, creditLine.CreditAmount)

            Assert.IsTrue(LedgerPostingEngine.IsBalanced(entry.Lines))
        End Sub

        <TestMethod>
        Public Sub GeneratePaymentReceiptJournal_InvalidInputs_ThrowsAppropriateExceptions()
            Assert.Throws(Of ArgumentNullException)(Sub() LedgerPostingEngine.GeneratePaymentReceiptJournal(Nothing))

            Dim zeroReceipt As New PaymentReceipt() With {.AmountReceived = 0D}
            Assert.Throws(Of ArgumentException)(Sub() LedgerPostingEngine.GeneratePaymentReceiptJournal(zeroReceipt))

            Dim negReceipt As New PaymentReceipt() With {.AmountReceived = -100D}
            Assert.Throws(Of ArgumentException)(Sub() LedgerPostingEngine.GeneratePaymentReceiptJournal(negReceipt))
        End Sub

        <TestMethod>
        Public Sub GenerateInventoryAdjustmentJournal_ShrinkageAndSurplus_PostsAccurately()
            Dim adj As New InventoryAdjustment() With {
                .AdjustmentId = 1,
                .DocumentNumber = "ADJ-2026-001",
                .AdjustmentDate = DateTime.Today,
                .PostingDate = DateTime.Today,
                .ReasonCode = "Stocktake"
            }
            ' Deficit/Shrinkage: -5 units @ $10.00 = $50.00 -> Debit 5300, Credit 1300
            ' Surplus: +8 units @ $12.00 = $96.00 -> Debit 1300, Credit 5400
            Dim lines As New List(Of InventoryAdjustmentLine) From {
                New InventoryAdjustmentLine() With {.ProductId = 101, .QtyChange = -5, .UnitCost = 10.0000D},
                New InventoryAdjustmentLine() With {.ProductId = 102, .QtyChange = 8, .UnitCost = 12.0000D}
            }

            Dim entry As JournalEntry = LedgerPostingEngine.GenerateInventoryAdjustmentJournal(adj, lines)

            Assert.IsNotNull(entry)
            Assert.AreEqual("InventoryAdjustment", entry.SourceDocType)
            Assert.AreEqual(4, entry.Lines.Count)

            ' Deficit shrinkage: Debit 5300 $50.00, Credit 1300 $50.00
            Dim shrinkageLine As JournalEntryLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.InventoryShrinkage)
            Assert.AreEqual(50.0000D, shrinkageLine.DebitAmount)

            ' Surplus: Debit 1300 $96.00, Credit 5400 $96.00
            Dim surplusLine As JournalEntryLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.StocktakeVariance)
            Assert.AreEqual(96.0000D, surplusLine.CreditAmount)

            Assert.IsTrue(LedgerPostingEngine.IsBalanced(entry.Lines))
        End Sub

        <TestMethod>
        Public Sub GenerateInventoryAdjustmentJournal_InvalidInputs_ThrowsAppropriateExceptions()
            Dim adj As New InventoryAdjustment() With {.AdjustmentId = 1, .DocumentNumber = "ADJ-001"}
            Assert.Throws(Of ArgumentNullException)(Sub() LedgerPostingEngine.GenerateInventoryAdjustmentJournal(Nothing, New List(Of InventoryAdjustmentLine)()))
            Assert.Throws(Of ArgumentNullException)(Sub() LedgerPostingEngine.GenerateInventoryAdjustmentJournal(adj, Nothing))
            Assert.Throws(Of ArgumentException)(Sub() LedgerPostingEngine.GenerateInventoryAdjustmentJournal(adj, New List(Of InventoryAdjustmentLine)()))

            ' All zero changes
            Assert.Throws(Of ArgumentException)(
                Sub() LedgerPostingEngine.GenerateInventoryAdjustmentJournal(adj, New List(Of InventoryAdjustmentLine) From {
                    New InventoryAdjustmentLine() With {.QtyChange = 0, .UnitCost = 10D}
                }))

            ' Negative unit cost
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() LedgerPostingEngine.GenerateInventoryAdjustmentJournal(adj, New List(Of InventoryAdjustmentLine) From {
                    New InventoryAdjustmentLine() With {.QtyChange = 5, .UnitCost = -10D}
                }))
        End Sub

        ' =========================================================================
        ' GRNI Adjustment & Safe GL Reversal Tests
        ' =========================================================================

        <TestMethod>
        Public Sub GenerateGRNIAdjustmentJournal_ValidInputs_GeneratesBalancedJournalDr2100Cr5100()
            Dim adj As New GRNIAdjustment() With {
                .AdjustmentId = 10,
                .DocumentNumber = "GRNI-ADJ-20261121-001",
                .GoodsReceiptLineId = 5,
                .AdjustmentDate = New DateTime(2026, 11, 21),
                .PostingDate = New DateTime(2026, 11, 21),
                .ClearedQty = 20,
                .ClearedAmount = 300.0000D,
                .Reason = "Vendor went out of business; unbilled accrual write-off",
                .Status = "Posted"
            }
            Dim grnLine As New GRNLine() With {
                .GRNLineId = 5,
                .ReceivedQty = 50,
                .POUnitPrice = 15.0000D,
                .GoodsReceiptNote = New GoodsReceiptNote() With {.DocumentNumber = "GRN-20261101-01"}
            }

            Dim entry As JournalEntry = LedgerPostingEngine.GenerateGRNIAdjustmentJournal(adj, grnLine)

            Assert.IsNotNull(entry)
            Assert.AreEqual("GRNIAdjustment", entry.SourceDocType)
            Assert.AreEqual(10, entry.SourceDocId)
            Assert.AreEqual(2, entry.Lines.Count)

            ' Debit 2100 GRNI Clearing: $300.00
            Dim grniLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.GRNIClearing)
            Assert.AreEqual(300.0000D, grniLine.DebitAmount)
            Assert.AreEqual(0D, grniLine.CreditAmount)

            ' Credit 5100 Purchase Price Variance: $300.00
            Dim ppvLine = entry.Lines.First(Function(l) l.AccountId = AccountCodes.PurchasePriceVariance)
            Assert.AreEqual(0D, ppvLine.DebitAmount)
            Assert.AreEqual(300.0000D, ppvLine.CreditAmount)

            Assert.IsTrue(LedgerPostingEngine.IsBalanced(entry.Lines))
        End Sub

        <TestMethod>
        Public Sub GenerateGRNIAdjustmentJournal_InvalidInputs_ThrowsAppropriateExceptions()
            Dim adj As New GRNIAdjustment() With {
                .AdjustmentId = 1,
                .DocumentNumber = "ADJ-001",
                .ClearedQty = 10,
                .ClearedAmount = 100D
            }
            Dim grnLine As New GRNLine() With {.GRNLineId = 1}

            ' Null adjustment
            Assert.Throws(Of ArgumentNullException)(Sub() LedgerPostingEngine.GenerateGRNIAdjustmentJournal(Nothing, grnLine))

            ' ClearedAmount <= 0
            Dim zeroAmountAdj As New GRNIAdjustment() With {.ClearedQty = 10, .ClearedAmount = 0D}
            Assert.Throws(Of ArgumentOutOfRangeException)(Sub() LedgerPostingEngine.GenerateGRNIAdjustmentJournal(zeroAmountAdj, grnLine))

            ' ClearedQty <= 0
            Dim zeroQtyAdj As New GRNIAdjustment() With {.ClearedQty = 0, .ClearedAmount = 100D}
            Assert.Throws(Of ArgumentOutOfRangeException)(Sub() LedgerPostingEngine.GenerateGRNIAdjustmentJournal(zeroQtyAdj, grnLine))
        End Sub

        <TestMethod>
        Public Sub GenerateReversingJournal_Directive1_OperationalDocTypesThrowInvalidOperationException()
            ' Directive 1: Operational journal entries (Sales Invoices, GRNs, Supplier Bills, Credit Notes, etc.)
            ' must NEVER be reversed directly in the GL UI.
            Dim operationalTypes() As String = {
                "SalesInvoice", "GoodsReceiptNote", "SupplierBill", "CustomerReturn", "PaymentVoucher", "PaymentReceipt"
            }

            For Each docType In operationalTypes
                Dim original As New JournalEntry() With {
                    .EntryId = 100,
                    .SourceDocType = docType,
                    .SourceDocId = 1,
                    .PostingDate = DateTime.Today,
                    .PeriodId = 1,
                    .Description = $"Original operational entry for {docType}"
                }
                original.Lines.Add(New JournalEntryLine(AccountCodes.OperatingBank, 100D, 0D, "Debit"))
                original.Lines.Add(New JournalEntryLine(AccountCodes.AccountsReceivable, 0D, 100D, "Credit"))

                Dim caughtEx = Assert.Throws(Of InvalidOperationException)(
                    Sub() LedgerPostingEngine.GenerateReversingJournal(original, DateTime.Today.AddDays(1)))

                Assert.IsTrue(caughtEx.Message.Contains("prohibited for operational document type"),
                    $"Expected guard message for '{docType}', got: {caughtEx.Message}")
            Next
        End Sub

        <TestMethod>
        Public Sub GenerateReversingJournal_ManualAndAdjustmentDocTypes_GeneratesBalancedReversedLines()
            ' Manual Journal Reversal
            Dim manualEntry As New JournalEntry() With {
                .EntryId = 42,
                .SourceDocType = "Manual",
                .SourceDocId = 42,
                .PostingDate = New DateTime(2026, 11, 1),
                .PeriodId = 1,
                .Description = "Month-end manual adjustment accrual"
            }
            manualEntry.Lines.Add(New JournalEntryLine(AccountCodes.OperatingBank, 500.0000D, 0D, "Initial Dr"))
            manualEntry.Lines.Add(New JournalEntryLine(AccountCodes.RetainedEarnings, 0D, 500.0000D, "Initial Cr"))

            Dim reversalDate As New DateTime(2026, 11, 2)
            Dim reversedEntry = LedgerPostingEngine.GenerateReversingJournal(manualEntry, reversalDate, "Reversing manual accrual")

            Assert.IsNotNull(reversedEntry)
            Assert.AreEqual("Reversal", reversedEntry.SourceDocType)
            Assert.AreEqual(42, reversedEntry.SourceDocId)
            Assert.AreEqual(reversalDate, reversedEntry.PostingDate)
            Assert.AreEqual(2, reversedEntry.Lines.Count)

            ' Original Dr 1020 $500 -> Reversed Cr 1020 $500
            Dim bankLine = reversedEntry.Lines.First(Function(l) l.AccountId = AccountCodes.OperatingBank)
            Assert.AreEqual(0D, bankLine.DebitAmount)
            Assert.AreEqual(500.0000D, bankLine.CreditAmount)

            ' Original Cr 3000 $500 -> Reversed Dr 3000 $500
            Dim reLine = reversedEntry.Lines.First(Function(l) l.AccountId = AccountCodes.RetainedEarnings)
            Assert.AreEqual(500.0000D, reLine.DebitAmount)
            Assert.AreEqual(0D, reLine.CreditAmount)

            Assert.IsTrue(LedgerPostingEngine.IsBalanced(reversedEntry.Lines))

            ' Adjustment Journal Reversal
            Dim adjEntry As New JournalEntry() With {
                .EntryId = 43,
                .SourceDocType = "Adjustment",
                .SourceDocId = 43,
                .PostingDate = New DateTime(2026, 11, 1),
                .PeriodId = 1,
                .Description = "Stock write-off adjustment"
            }
            adjEntry.Lines.Add(New JournalEntryLine(AccountCodes.InventoryShrinkage, 200.0000D, 0D, "Initial Dr"))
            adjEntry.Lines.Add(New JournalEntryLine(AccountCodes.MerchandiseInventory, 0D, 200.0000D, "Initial Cr"))

            Dim reversedAdj = LedgerPostingEngine.GenerateReversingJournal(adjEntry, reversalDate)
            Assert.IsNotNull(reversedAdj)
            Assert.IsTrue(LedgerPostingEngine.IsBalanced(reversedAdj.Lines))
        End Sub

        <TestMethod>
        Public Sub GenerateReversingJournal_InvalidInputs_ThrowsAppropriateExceptions()
            ' Null entry
            Assert.Throws(Of ArgumentNullException)(Sub() LedgerPostingEngine.GenerateReversingJournal(Nothing, DateTime.Today))

            ' Entry with no lines
            Dim emptyEntry As New JournalEntry() With {.EntryId = 1, .SourceDocType = "Manual"}
            Assert.Throws(Of ArgumentException)(Sub() LedgerPostingEngine.GenerateReversingJournal(emptyEntry, DateTime.Today))
        End Sub

    End Class

End Namespace
