Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports Merchandising_Accounting_App.Domain
Imports Merchandising_Accounting_App.Domain.Models

Namespace Domain.Engines

    ''' <summary>
    ''' Standard Chart of Accounts (COA) numeric identifiers matching the authoritative specification (1010-5500).
    ''' </summary>
    Public Module AccountCodes
        Public Const CashOnHand As Integer = 1010
        Public Const OperatingBank As Integer = 1020
        Public Const AccountsReceivable As Integer = 1200
        Public Const MerchandiseInventory As Integer = 1300
        Public Const AccountsPayable As Integer = 2000
        Public Const GRNIClearing As Integer = 2100
        Public Const RetainedEarnings As Integer = 3000
        Public Const OwnersCapital As Integer = 3010
        Public Const SalesRevenue As Integer = 4000
        Public Const SalesReturnsAndAllowances As Integer = 4100
        Public Const CostOfGoodsSold As Integer = 5000
        Public Const PurchasePriceVariance As Integer = 5100
        Public Const InboundFreightVariance As Integer = 5200
        Public Const InventoryShrinkage As Integer = 5300
        Public Const StocktakeVariance As Integer = 5400
        Public Const CashRoundingVariance As Integer = 5500
    End Module

    ''' <summary>
    ''' Pure domain accounting engine enforcing double-entry invariants, penny-rounding allocation,
    ''' and deterministic journal entry generation for all operational documents.
    ''' </summary>
    Public Class LedgerPostingEngine

        ''' <summary>
        ''' Maximum allowable fractional penny rounding difference eligible for automated allocation to Account 5500.
        ''' Discrepancies greater than 0.02 halt with an UnbalancedJournalEntryException.
        ''' </summary>
        Public Const MaxPennyRoundingThreshold As Decimal = 0.02D

        ''' <summary>
        ''' Checks whether debit and credit amounts are balanced to 4 decimal places.
        ''' </summary>
        Public Shared Function IsBalanced(debits As Decimal, credits As Decimal) As Boolean
            Return Math.Round(debits, 4, MidpointRounding.AwayFromZero) = Math.Round(credits, 4, MidpointRounding.AwayFromZero)
        End Function

        ''' <summary>
        ''' Checks whether the collection of journal entry lines is balanced to 4 decimal places.
        ''' </summary>
        Public Shared Function IsBalanced(lines As IEnumerable(Of JournalEntryLine)) As Boolean
            If lines Is Nothing Then
                Return False
            End If

            Dim totalDebits As Decimal = lines.Sum(Function(l) l.DebitAmount)
            Dim totalCredits As Decimal = lines.Sum(Function(l) l.CreditAmount)
            Return IsBalanced(totalDebits, totalCredits)
        End Function

        ''' <summary>
        ''' Validates that the sum of debits strictly equals the sum of credits to 4 decimal places.
        ''' Throws UnbalancedJournalEntryException if the balance invariant is violated.
        ''' </summary>
        Public Shared Function ValidateJournalBalance(lines As IEnumerable(Of JournalEntryLine)) As Boolean
            If lines Is Nothing Then
                Throw New ArgumentNullException(NameOf(lines), "Journal entry lines collection cannot be null.")
            End If

            Dim totalDebits As Decimal = Math.Round(lines.Sum(Function(l) l.DebitAmount), 4, MidpointRounding.AwayFromZero)
            Dim totalCredits As Decimal = Math.Round(lines.Sum(Function(l) l.CreditAmount), 4, MidpointRounding.AwayFromZero)

            If Not IsBalanced(totalDebits, totalCredits) Then
                Throw New UnbalancedJournalEntryException(totalDebits, totalCredits)
            End If

            Return True
        End Function

        ''' <summary>
        ''' Allocates fractional penny-rounding differences generated during unit conversions to Account 5500
        ''' (Cash Rounding &amp; Roundoff Variance) if |variance| &lt;= 0.02.
        ''' Returns True if a rounding line was added; False if already balanced.
        ''' Throws UnbalancedJournalEntryException if |variance| &gt; 0.02.
        ''' </summary>
        Public Shared Function AllocatePennyRounding(lines As IList(Of JournalEntryLine), Optional roundingAccountId As Integer = AccountCodes.CashRoundingVariance) As Boolean
            If lines Is Nothing Then
                Throw New ArgumentNullException(NameOf(lines), "Journal entry lines collection cannot be null.")
            End If

            Dim totalDebits As Decimal = Math.Round(lines.Sum(Function(l) l.DebitAmount), 4, MidpointRounding.AwayFromZero)
            Dim totalCredits As Decimal = Math.Round(lines.Sum(Function(l) l.CreditAmount), 4, MidpointRounding.AwayFromZero)
            Dim variance As Decimal = Math.Round(totalDebits - totalCredits, 4, MidpointRounding.AwayFromZero)

            If variance = 0D Then
                Return False
            End If

            If Math.Abs(variance) > MaxPennyRoundingThreshold Then
                Throw New UnbalancedJournalEntryException(totalDebits, totalCredits,
                    $"Journal debit/credit variance of {Math.Abs(variance):F4} exceeds the maximum penny-rounding threshold of {MaxPennyRoundingThreshold:F2}.")
            End If

            If variance > 0D Then
                ' Debits exceed credits: add a credit line to Account 5500
                lines.Add(New JournalEntryLine(roundingAccountId, 0D, variance, "Fractional penny rounding allocation"))
            Else
                ' Credits exceed debits: add a debit line to Account 5500
                lines.Add(New JournalEntryLine(roundingAccountId, Math.Abs(variance), 0D, "Fractional penny rounding allocation"))
            End If

            Return True
        End Function

        ''' <summary>
        ''' Generates a balanced General Ledger journal entry for a Goods Receipt Note (GRN):
        ''' Debit: 1300 Merchandise Inventory (Received Qty * PO Unit Price)
        ''' Credit: 2100 GRNI Clearing (Received Qty * PO Unit Price)
        ''' </summary>
        Public Shared Function GenerateGRNJournal(grn As GoodsReceiptNote, lines As IEnumerable(Of GRNLine), Optional accountLookup As Func(Of String, Integer) = Nothing) As JournalEntry
            If grn Is Nothing Then
                Throw New ArgumentNullException(NameOf(grn), "GoodsReceiptNote cannot be null.")
            End If
            If lines Is Nothing Then
                Throw New ArgumentNullException(NameOf(lines), "GRN lines cannot be null.")
            End If

            Dim grnLineList As List(Of GRNLine) = lines.ToList()
            If grnLineList.Count = 0 Then
                Throw New ArgumentException("Goods Receipt Note must contain at least one line item.", NameOf(lines))
            End If

            Dim totalAmount As Decimal = 0D
            For Each line As GRNLine In grnLineList
                If line.ReceivedQty < 0 Then
                    Throw New ArgumentOutOfRangeException(NameOf(lines), "Received quantity cannot be negative.")
                End If
                If line.POUnitPrice < 0D Then
                    Throw New ArgumentOutOfRangeException(NameOf(lines), "PO unit price cannot be negative.")
                End If
                totalAmount += Math.Round(CDec(line.ReceivedQty) * line.POUnitPrice, 4, MidpointRounding.AwayFromZero)
            Next

            If totalAmount <= 0D Then
                Throw New ArgumentException("Goods Receipt Note total valuation must be strictly positive.", NameOf(lines))
            End If

            Dim inventoryAccountId As Integer = ResolveAccountId(AccountCodes.MerchandiseInventory, accountLookup)
            Dim grniAccountId As Integer = ResolveAccountId(AccountCodes.GRNIClearing, accountLookup)

            Dim entry As New JournalEntry() With {
                .SourceDocType = "GoodsReceiptNote",
                .SourceDocId = grn.GRNId,
                .PostingDate = grn.PostingDate,
                .OperationalEventDate = grn.ReceiptDate,
                .Description = $"Goods Receipt Note {grn.DocumentNumber}",
                .CreatedBy = "System"
            }

            Dim debitLine As New JournalEntryLine(inventoryAccountId, totalAmount, 0D, $"Inventory dock receipt - {grn.DocumentNumber}") With {
                .JournalEntry = entry
            }
            Dim creditLine As New JournalEntryLine(grniAccountId, 0D, totalAmount, $"GRNI liability accrual - {grn.DocumentNumber}") With {
                .JournalEntry = entry
            }

            entry.Lines.Add(debitLine)
            entry.Lines.Add(creditLine)

            ValidateJournalBalance(entry.Lines)
            Return entry
        End Function

        ''' <summary>
        ''' Generates a balanced General Ledger journal entry for an invoiced Supplier Bill against open GRN lines:
        ''' Debit: 2100 GRNI Clearing (Billed Qty * PO Unit Price)
        ''' Debit/Credit: 5100 Purchase Price Variance ((Billed Price - PO Price) * Billed Qty)
        ''' Debit: 5200 Inbound Freight Variance (Carrier freight charge)
        ''' Credit: 2000 Accounts Payable (Total billed payable amount)
        ''' </summary>
        Public Shared Function GenerateSupplierBillJournal(bill As SupplierBill, lines As IEnumerable(Of BillLine), Optional accountLookup As Func(Of String, Integer) = Nothing) As JournalEntry
            Return GenerateSupplierBillJournal(bill, lines, Nothing, accountLookup)
        End Function

        ''' <summary>
        ''' Generates a balanced General Ledger journal entry for an invoiced Supplier Bill with optional explicit GRN line matching.
        ''' </summary>
        Public Shared Function GenerateSupplierBillJournal(bill As SupplierBill, lines As IEnumerable(Of BillLine), grnLines As IEnumerable(Of GRNLine), Optional accountLookup As Func(Of String, Integer) = Nothing) As JournalEntry
            If bill Is Nothing Then
                Throw New ArgumentNullException(NameOf(bill), "SupplierBill cannot be null.")
            End If
            If lines Is Nothing Then
                Throw New ArgumentNullException(NameOf(lines), "Bill lines cannot be null.")
            End If

            Dim billLineList As List(Of BillLine) = lines.ToList()
            If billLineList.Count = 0 Then
                Throw New ArgumentException("Supplier Bill must contain at least one line item.", NameOf(lines))
            End If

            Dim grnLookup As Dictionary(Of Integer, GRNLine) = Nothing
            If grnLines IsNot Nothing Then
                grnLookup = grnLines.ToDictionary(Function(g) g.GRNLineId, Function(g) g)
            End If

            Dim grniAccountId As Integer = ResolveAccountId(AccountCodes.GRNIClearing, accountLookup)
            Dim ppvAccountId As Integer = ResolveAccountId(AccountCodes.PurchasePriceVariance, accountLookup)
            Dim freightAccountId As Integer = ResolveAccountId(AccountCodes.InboundFreightVariance, accountLookup)
            Dim apAccountId As Integer = ResolveAccountId(AccountCodes.AccountsPayable, accountLookup)
            Dim roundingAccountId As Integer = ResolveAccountId(AccountCodes.CashRoundingVariance, accountLookup)

            Dim entry As New JournalEntry() With {
                .SourceDocType = "SupplierBill",
                .SourceDocId = bill.BillId,
                .PostingDate = bill.PostingDate,
                .OperationalEventDate = bill.BillDate,
                .Description = $"Supplier Bill {bill.DocumentNumber}",
                .CreatedBy = "System"
            }

            Dim totalPayableCredit As Decimal = 0D

            For Each line As BillLine In billLineList
                If line.BilledQty < 0 Then
                    Throw New ArgumentOutOfRangeException(NameOf(lines), "Billed quantity cannot be negative.")
                End If
                If line.BilledUnitPrice < 0D Then
                    Throw New ArgumentOutOfRangeException(NameOf(lines), "Billed unit price cannot be negative.")
                End If
                If line.FreightAmount < 0D Then
                    Throw New ArgumentOutOfRangeException(NameOf(lines), "Freight amount cannot be negative.")
                End If

                If line.BilledQty > 0 Then
                    ' Resolve the baseline PO Unit Price
                    Dim poUnitPrice As Decimal = line.BilledUnitPrice
                    If line.GRNLine IsNot Nothing Then
                        poUnitPrice = line.GRNLine.POUnitPrice
                    ElseIf grnLookup IsNot Nothing AndAlso line.GRNLineId.HasValue AndAlso grnLookup.ContainsKey(line.GRNLineId.Value) Then
                        poUnitPrice = grnLookup(line.GRNLineId.Value).POUnitPrice
                    End If

                    Dim grniClearingAmount As Decimal = Math.Round(CDec(line.BilledQty) * poUnitPrice, 4, MidpointRounding.AwayFromZero)
                    Dim priceVariance As Decimal = Math.Round(CDec(line.BilledQty) * (line.BilledUnitPrice - poUnitPrice), 4, MidpointRounding.AwayFromZero)
                    Dim lineMerchandisePayable As Decimal = Math.Round(CDec(line.BilledQty) * line.BilledUnitPrice, 4, MidpointRounding.AwayFromZero)

                    ' Debit GRNI Clearing at PO Unit Price
                    If grniClearingAmount > 0D Then
                        entry.Lines.Add(New JournalEntryLine(grniAccountId, grniClearingAmount, 0D, $"GRNI clearing for Bill {bill.DocumentNumber}") With {
                            .JournalEntry = entry
                        })
                    End If

                    ' Debit or Credit Purchase Price Variance (PPV)
                    If priceVariance > 0D Then
                        ' Unfavorable price variance (supplier billed more than PO) -> Debit expense
                        entry.Lines.Add(New JournalEntryLine(ppvAccountId, priceVariance, 0D, $"PPV unfavorable variance - Bill {bill.DocumentNumber}") With {
                            .JournalEntry = entry
                        })
                    ElseIf priceVariance < 0D Then
                        ' Favorable price variance (supplier billed less than PO) -> Credit contra-expense
                        entry.Lines.Add(New JournalEntryLine(ppvAccountId, 0D, Math.Abs(priceVariance), $"PPV favorable variance - Bill {bill.DocumentNumber}") With {
                            .JournalEntry = entry
                        })
                    End If

                    totalPayableCredit += lineMerchandisePayable
                End If

                ' Process Freight lines
                If line.FreightAmount > 0D Then
                    entry.Lines.Add(New JournalEntryLine(freightAccountId, line.FreightAmount, 0D, $"Inbound freight charge - Bill {bill.DocumentNumber}") With {
                        .JournalEntry = entry
                    })
                    totalPayableCredit += line.FreightAmount
                ElseIf line.BilledQty = 0 AndAlso line.LineTotal > 0D Then
                    ' Standalone carrier freight charge line
                    Dim targetFreightAccount As Integer = If(line.AccountId.HasValue, line.AccountId.Value, freightAccountId)
                    entry.Lines.Add(New JournalEntryLine(targetFreightAccount, line.LineTotal, 0D, $"Inbound freight bill - {bill.DocumentNumber}") With {
                        .JournalEntry = entry
                    })
                    totalPayableCredit += line.LineTotal
                End If
            Next

            If totalPayableCredit <= 0D Then
                Throw New ArgumentException("Supplier Bill total payable amount must be strictly positive.", NameOf(lines))
            End If

            ' Credit Accounts Payable (2000) for the settled obligation
            entry.Lines.Add(New JournalEntryLine(apAccountId, 0D, totalPayableCredit, $"Accounts Payable - Bill {bill.DocumentNumber}") With {
                .JournalEntry = entry
            })

            ' Allocate penny-rounding if minor fractional unit conversions occurred
            Dim lineList As IList(Of JournalEntryLine) = CType(entry.Lines, IList(Of JournalEntryLine))
            AllocatePennyRounding(lineList, roundingAccountId)

            ValidateJournalBalance(entry.Lines)
            Return entry
        End Function

        ''' <summary>
        ''' Generates a balanced General Ledger journal entry for a Sales Invoice fulfillment:
        ''' Debit: 1200 Accounts Receivable (Fulfilled Qty * Selling Unit Price)
        ''' Credit: 4000 Sales Revenue (Fulfilled Qty * Selling Unit Price)
        ''' Debit: 5000 Cost of Goods Sold (Fulfilled Qty * Invoiced COGS Unit Cost)
        ''' Credit: 1300 Merchandise Inventory (Fulfilled Qty * Invoiced COGS Unit Cost)
        ''' </summary>
        Public Shared Function GenerateSalesInvoiceJournal(invoice As SalesInvoice, lines As IEnumerable(Of SalesInvoiceLine), Optional accountLookup As Func(Of String, Integer) = Nothing) As JournalEntry
            If invoice Is Nothing Then
                Throw New ArgumentNullException(NameOf(invoice), "SalesInvoice cannot be null.")
            End If
            If lines Is Nothing Then
                Throw New ArgumentNullException(NameOf(lines), "Invoice lines cannot be null.")
            End If

            Dim invoiceLineList As List(Of SalesInvoiceLine) = lines.ToList()
            If invoiceLineList.Count = 0 Then
                Throw New ArgumentException("Sales Invoice must contain at least one line item.", NameOf(lines))
            End If

            Dim totalRevenue As Decimal = 0D
            Dim totalCOGS As Decimal = 0D

            For Each line As SalesInvoiceLine In invoiceLineList
                If line.FulfilledQty < 0 Then
                    Throw New ArgumentOutOfRangeException(NameOf(lines), "Fulfilled quantity cannot be negative.")
                End If
                If line.UnitPrice < 0D Then
                    Throw New ArgumentOutOfRangeException(NameOf(lines), "Unit price cannot be negative.")
                End If
                If line.InvoicedCOGSUnitCost < 0D Then
                    Throw New ArgumentOutOfRangeException(NameOf(lines), "Invoiced COGS unit cost cannot be negative.")
                End If

                totalRevenue += Math.Round(CDec(line.FulfilledQty) * line.UnitPrice, 4, MidpointRounding.AwayFromZero)
                totalCOGS += Math.Round(CDec(line.FulfilledQty) * line.InvoicedCOGSUnitCost, 4, MidpointRounding.AwayFromZero)
            Next

            If totalRevenue <= 0D AndAlso totalCOGS <= 0D Then
                Throw New ArgumentException("Sales Invoice must have positive revenue or COGS valuation.", NameOf(lines))
            End If

            Dim arAccountId As Integer = ResolveAccountId(AccountCodes.AccountsReceivable, accountLookup)
            Dim revenueAccountId As Integer = ResolveAccountId(AccountCodes.SalesRevenue, accountLookup)
            Dim cogsAccountId As Integer = ResolveAccountId(AccountCodes.CostOfGoodsSold, accountLookup)
            Dim inventoryAccountId As Integer = ResolveAccountId(AccountCodes.MerchandiseInventory, accountLookup)
            Dim roundingAccountId As Integer = ResolveAccountId(AccountCodes.CashRoundingVariance, accountLookup)

            Dim entry As New JournalEntry() With {
                .SourceDocType = "SalesInvoice",
                .SourceDocId = invoice.InvoiceId,
                .PostingDate = invoice.PostingDate,
                .OperationalEventDate = invoice.InvoiceDate,
                .Description = $"Sales Invoice {invoice.DocumentNumber}",
                .CreatedBy = "System"
            }

            If totalRevenue > 0D Then
                entry.Lines.Add(New JournalEntryLine(arAccountId, totalRevenue, 0D, $"Accounts Receivable - Invoice {invoice.DocumentNumber}") With {
                    .JournalEntry = entry
                })
                entry.Lines.Add(New JournalEntryLine(revenueAccountId, 0D, totalRevenue, $"Sales Revenue - Invoice {invoice.DocumentNumber}") With {
                    .JournalEntry = entry
                })
            End If

            If totalCOGS > 0D Then
                entry.Lines.Add(New JournalEntryLine(cogsAccountId, totalCOGS, 0D, $"Cost of Goods Sold - Invoice {invoice.DocumentNumber}") With {
                    .JournalEntry = entry
                })
                entry.Lines.Add(New JournalEntryLine(inventoryAccountId, 0D, totalCOGS, $"Merchandise Inventory dispatch - Invoice {invoice.DocumentNumber}") With {
                    .JournalEntry = entry
                })
            End If

            Dim lineList As IList(Of JournalEntryLine) = CType(entry.Lines, IList(Of JournalEntryLine))
            AllocatePennyRounding(lineList, roundingAccountId)

            ValidateJournalBalance(entry.Lines)
            Return entry
        End Function

        ''' <summary>
        ''' Generates a balanced General Ledger journal entry for a Credit Note (Customer Return after quarantine inspection):
        ''' Debit: 4100 Sales Returns &amp; Allowances (Invoiced Selling Price)
        ''' Credit: 1200 Accounts Receivable (Invoiced Selling Price)
        ''' Debit: 1300 Merchandise Inventory (Restocked Qty * Original Invoice Cost)
        ''' Debit: 5300 Inventory Shrinkage &amp; Damaged Stock (Scrapped Qty * Original Invoice Cost)
        ''' Credit: 5000 Cost of Goods Sold ((Restocked Qty + Scrapped Qty) * Original Invoice Cost)
        ''' </summary>
        Public Shared Function GenerateCreditNoteJournal(returnDoc As CustomerReturn, lines As IEnumerable(Of CustomerReturnLine), Optional accountLookup As Func(Of String, Integer) = Nothing) As JournalEntry
            If returnDoc Is Nothing Then
                Throw New ArgumentNullException(NameOf(returnDoc), "CustomerReturn cannot be null.")
            End If
            If lines Is Nothing Then
                Throw New ArgumentNullException(NameOf(lines), "Customer return lines cannot be null.")
            End If

            Dim returnLineList As List(Of CustomerReturnLine) = lines.ToList()
            If returnLineList.Count = 0 Then
                Throw New ArgumentException("Customer return must contain at least one line item.", NameOf(lines))
            End If

            Dim totalReturnAllowance As Decimal = 0D
            Dim totalRestockValuation As Decimal = 0D
            Dim totalScrapValuation As Decimal = 0D

            For Each line As CustomerReturnLine In returnLineList
                If line.ReturnedQty < 0 Then
                    Throw New ArgumentOutOfRangeException(NameOf(lines), "Returned quantity cannot be negative.")
                End If
                If line.RestockQty < 0 Then
                    Throw New ArgumentOutOfRangeException(NameOf(lines), "Restocked quantity cannot be negative.")
                End If
                If line.ScrapQty < 0 Then
                    Throw New ArgumentOutOfRangeException(NameOf(lines), "Scrapped quantity cannot be negative.")
                End If
                If line.OriginalUnitCost < 0D Then
                    Throw New ArgumentOutOfRangeException(NameOf(lines), "Original unit cost cannot be negative.")
                End If
                If line.SellingUnitPrice < 0D Then
                    Throw New ArgumentOutOfRangeException(NameOf(lines), "Selling unit price cannot be negative.")
                End If

                Dim accountedQty As Integer = line.ReturnedQty
                If accountedQty = 0 Then
                    accountedQty = line.RestockQty + line.ScrapQty
                End If

                totalReturnAllowance += Math.Round(CDec(accountedQty) * line.SellingUnitPrice, 4, MidpointRounding.AwayFromZero)
                totalRestockValuation += Math.Round(CDec(line.RestockQty) * line.OriginalUnitCost, 4, MidpointRounding.AwayFromZero)
                totalScrapValuation += Math.Round(CDec(line.ScrapQty) * line.OriginalUnitCost, 4, MidpointRounding.AwayFromZero)
            Next

            Dim totalCOGSCredit As Decimal = totalRestockValuation + totalScrapValuation

            If totalReturnAllowance <= 0D AndAlso totalCOGSCredit <= 0D Then
                Throw New ArgumentException("Credit Note must have positive sales allowance or inventory return value.", NameOf(lines))
            End If

            Dim returnsAccountId As Integer = ResolveAccountId(AccountCodes.SalesReturnsAndAllowances, accountLookup)
            Dim arAccountId As Integer = ResolveAccountId(AccountCodes.AccountsReceivable, accountLookup)
            Dim inventoryAccountId As Integer = ResolveAccountId(AccountCodes.MerchandiseInventory, accountLookup)
            Dim shrinkageAccountId As Integer = ResolveAccountId(AccountCodes.InventoryShrinkage, accountLookup)
            Dim cogsAccountId As Integer = ResolveAccountId(AccountCodes.CostOfGoodsSold, accountLookup)
            Dim roundingAccountId As Integer = ResolveAccountId(AccountCodes.CashRoundingVariance, accountLookup)

            Dim entry As New JournalEntry() With {
                .SourceDocType = "CustomerReturn",
                .SourceDocId = returnDoc.ReturnId,
                .PostingDate = returnDoc.PostingDate,
                .OperationalEventDate = returnDoc.ReturnDate,
                .Description = $"Credit Note for Return {returnDoc.DocumentNumber}",
                .CreatedBy = "System"
            }

            If totalReturnAllowance > 0D Then
                entry.Lines.Add(New JournalEntryLine(returnsAccountId, totalReturnAllowance, 0D, $"Sales return allowance - {returnDoc.DocumentNumber}") With {
                    .JournalEntry = entry
                })
                entry.Lines.Add(New JournalEntryLine(arAccountId, 0D, totalReturnAllowance, $"AR subledger credit memo - {returnDoc.DocumentNumber}") With {
                    .JournalEntry = entry
                })
            End If

            If totalRestockValuation > 0D Then
                entry.Lines.Add(New JournalEntryLine(inventoryAccountId, totalRestockValuation, 0D, $"Restocked inventory - {returnDoc.DocumentNumber}") With {
                    .JournalEntry = entry
                })
            End If

            If totalScrapValuation > 0D Then
                entry.Lines.Add(New JournalEntryLine(shrinkageAccountId, totalScrapValuation, 0D, $"Damaged/scrapped return expense - {returnDoc.DocumentNumber}") With {
                    .JournalEntry = entry
                })
            End If

            If totalCOGSCredit > 0D Then
                entry.Lines.Add(New JournalEntryLine(cogsAccountId, 0D, totalCOGSCredit, $"COGS return reversal - {returnDoc.DocumentNumber}") With {
                    .JournalEntry = entry
                })
            End If

            Dim lineList As IList(Of JournalEntryLine) = CType(entry.Lines, IList(Of JournalEntryLine))
            AllocatePennyRounding(lineList, roundingAccountId)

            ValidateJournalBalance(entry.Lines)
            Return entry
        End Function

        ''' <summary>
        ''' Generates a balanced General Ledger journal entry for a Payment Voucher (AP settlement or Customer Refund disbursement):
        ''' Vendor Payment:
        '''   Debit: 2000 Accounts Payable (Disbursed Amount)
        '''   Credit: 1020 Operating Bank Account (Disbursed Amount)
        ''' Customer Refund:
        '''   Debit: 1200 Accounts Receivable (Disbursed Amount)
        '''   Credit: 1020 Operating Bank Account (Disbursed Amount)
        ''' </summary>
        Public Shared Function GeneratePaymentVoucherJournal(voucher As PaymentVoucher, Optional accountLookup As Func(Of String, Integer) = Nothing) As JournalEntry
            If voucher Is Nothing Then
                Throw New ArgumentNullException(NameOf(voucher), "PaymentVoucher cannot be null.")
            End If

            Dim amount As Decimal = Math.Round(voucher.DisbursedAmount, 4, MidpointRounding.AwayFromZero)
            If amount <= 0D Then
                Throw New ArgumentException("Payment Voucher disbursed amount must be strictly positive.", NameOf(voucher))
            End If

            Dim bankAccountId As Integer = If(voucher.BankAccountId <> 0, voucher.BankAccountId, ResolveAccountId(AccountCodes.OperatingBank, accountLookup))
            Dim isCustomerRefund As Boolean = voucher.CustomerId.HasValue OrElse String.Equals(voucher.VoucherType, "CustomerRefund", StringComparison.OrdinalIgnoreCase)
            Dim targetAccountId As Integer = If(isCustomerRefund, ResolveAccountId(AccountCodes.AccountsReceivable, accountLookup), ResolveAccountId(AccountCodes.AccountsPayable, accountLookup))

            Dim entry As New JournalEntry() With {
                .SourceDocType = "PaymentVoucher",
                .SourceDocId = voucher.VoucherId,
                .PostingDate = voucher.PostingDate,
                .OperationalEventDate = voucher.PaymentDate,
                .Description = $"Payment Voucher {voucher.VoucherNumber}",
                .CreatedBy = "System"
            }

            Dim debitMemo As String = If(isCustomerRefund, $"Customer refund disbursement - {voucher.VoucherNumber}", $"Accounts Payable settlement - {voucher.VoucherNumber}")
            entry.Lines.Add(New JournalEntryLine(targetAccountId, amount, 0D, debitMemo) With {
                .JournalEntry = entry
            })
            entry.Lines.Add(New JournalEntryLine(bankAccountId, 0D, amount, $"Bank disbursement - {voucher.VoucherNumber}") With {
                .JournalEntry = entry
            })

            ValidateJournalBalance(entry.Lines)
            Return entry
        End Function

        ''' <summary>
        ''' Generates a balanced General Ledger journal entry for a Customer Payment Receipt:
        ''' Debit: 1020 Operating Bank Account (Amount Received)
        ''' Credit: 1200 Accounts Receivable (Amount Received)
        ''' </summary>
        Public Shared Function GeneratePaymentReceiptJournal(receipt As PaymentReceipt, Optional accountLookup As Func(Of String, Integer) = Nothing) As JournalEntry
            If receipt Is Nothing Then
                Throw New ArgumentNullException(NameOf(receipt), "PaymentReceipt cannot be null.")
            End If

            Dim amount As Decimal = Math.Round(receipt.AmountReceived, 4, MidpointRounding.AwayFromZero)
            If amount <= 0D Then
                Throw New ArgumentException("Payment Receipt amount received must be strictly positive.", NameOf(receipt))
            End If

            Dim bankAccountId As Integer = If(receipt.BankAccountId <> 0, receipt.BankAccountId, ResolveAccountId(AccountCodes.OperatingBank, accountLookup))
            Dim arAccountId As Integer = ResolveAccountId(AccountCodes.AccountsReceivable, accountLookup)

            Dim entry As New JournalEntry() With {
                .SourceDocType = "PaymentReceipt",
                .SourceDocId = receipt.ReceiptId,
                .PostingDate = receipt.PostingDate,
                .OperationalEventDate = receipt.ReceiptDate,
                .Description = $"Payment Receipt {receipt.ReceiptId}",
                .CreatedBy = "System"
            }

            entry.Lines.Add(New JournalEntryLine(bankAccountId, amount, 0D, $"Bank deposit - Receipt #{receipt.ReceiptId}") With {
                .JournalEntry = entry
            })
            entry.Lines.Add(New JournalEntryLine(arAccountId, 0D, amount, $"AR customer receipt - Receipt #{receipt.ReceiptId}") With {
                .JournalEntry = entry
            })

            ValidateJournalBalance(entry.Lines)
            Return entry
        End Function

        ''' <summary>
        ''' Generates a balanced General Ledger journal entry for an Inventory Adjustment (stock count variance, shrinkage write-down):
        ''' Negative QtyChange (Shrinkage/Deficit):
        '''   Debit: TargetAccount (defaults to 5300 Inventory Shrinkage)
        '''   Credit: 1300 Merchandise Inventory
        ''' Positive QtyChange (Surplus):
        '''   Debit: 1300 Merchandise Inventory
        '''   Credit: TargetAccount (defaults to 5400 Stocktake Revaluation Variance)
        ''' </summary>
        Public Shared Function GenerateInventoryAdjustmentJournal(adj As InventoryAdjustment, lines As IEnumerable(Of InventoryAdjustmentLine), Optional accountLookup As Func(Of String, Integer) = Nothing) As JournalEntry
            If adj Is Nothing Then
                Throw New ArgumentNullException(NameOf(adj), "InventoryAdjustment cannot be null.")
            End If
            If lines Is Nothing Then
                Throw New ArgumentNullException(NameOf(lines), "Inventory adjustment lines cannot be null.")
            End If

            Dim adjLineList As List(Of InventoryAdjustmentLine) = lines.ToList()
            If adjLineList.Count = 0 Then
                Throw New ArgumentException("Inventory adjustment must contain at least one line item.", NameOf(lines))
            End If

            Dim inventoryAccountId As Integer = ResolveAccountId(AccountCodes.MerchandiseInventory, accountLookup)
            Dim shrinkageAccountId As Integer = ResolveAccountId(AccountCodes.InventoryShrinkage, accountLookup)
            Dim surplusAccountId As Integer = ResolveAccountId(AccountCodes.StocktakeVariance, accountLookup)
            Dim roundingAccountId As Integer = ResolveAccountId(AccountCodes.CashRoundingVariance, accountLookup)

            Dim entry As New JournalEntry() With {
                .SourceDocType = "InventoryAdjustment",
                .SourceDocId = adj.AdjustmentId,
                .PostingDate = adj.PostingDate,
                .OperationalEventDate = adj.AdjustmentDate,
                .Description = $"Inventory Adjustment {adj.DocumentNumber}",
                .CreatedBy = "System"
            }

            Dim hasActiveChanges As Boolean = False

            For Each line As InventoryAdjustmentLine In adjLineList
                If line.QtyChange = 0 Then
                    Continue For
                End If
                If line.UnitCost < 0D Then
                    Throw New ArgumentOutOfRangeException(NameOf(lines), "Inventory adjustment unit cost cannot be negative.")
                End If

                hasActiveChanges = True
                Dim valuation As Decimal = Math.Round(Math.Abs(CDec(line.QtyChange)) * line.UnitCost, 4, MidpointRounding.AwayFromZero)

                If line.QtyChange < 0 Then
                    ' Shrinkage / write-down
                    Dim targetExpenseAccount As Integer = If(line.TargetAccountId <> 0, line.TargetAccountId, shrinkageAccountId)
                    entry.Lines.Add(New JournalEntryLine(targetExpenseAccount, valuation, 0D, $"Stock deficit write-down - {adj.DocumentNumber}") With {
                        .JournalEntry = entry
                    })
                    entry.Lines.Add(New JournalEntryLine(inventoryAccountId, 0D, valuation, $"Inventory reduction - {adj.DocumentNumber}") With {
                        .JournalEntry = entry
                    })
                Else
                    ' Surplus adjustment
                    Dim targetSurplusAccount As Integer = If(line.TargetAccountId <> 0, line.TargetAccountId, surplusAccountId)
                    entry.Lines.Add(New JournalEntryLine(inventoryAccountId, valuation, 0D, $"Inventory surplus count - {adj.DocumentNumber}") With {
                        .JournalEntry = entry
                    })
                    entry.Lines.Add(New JournalEntryLine(targetSurplusAccount, 0D, valuation, $"Stocktake variance gain - {adj.DocumentNumber}") With {
                        .JournalEntry = entry
                    })
                End If
            Next

            If Not hasActiveChanges Then
                Throw New ArgumentException("Inventory adjustment must contain at least one non-zero quantity line item.", NameOf(lines))
            End If

            Dim lineList As IList(Of JournalEntryLine) = CType(entry.Lines, IList(Of JournalEntryLine))
            AllocatePennyRounding(lineList, roundingAccountId)

            ValidateJournalBalance(entry.Lines)
            Return entry
        End Function

        ''' <summary>
        ''' Generates a balanced General Ledger journal entry for clearing stale unbilled GRNI accrual:
        ''' Debit: 2100 GRNI Clearing (ClearedAmount)
        ''' Credit: 5100 Purchase Price Variance (ClearedAmount)
        ''' </summary>
        Public Shared Function GenerateGRNIAdjustmentJournal(adjustment As GRNIAdjustment, grnLine As GRNLine, Optional accountLookup As Func(Of String, Integer) = Nothing) As JournalEntry
            If adjustment Is Nothing Then
                Throw New ArgumentNullException(NameOf(adjustment), "GRNIAdjustment cannot be null.")
            End If
            If adjustment.ClearedAmount <= 0D Then
                Throw New ArgumentOutOfRangeException(NameOf(adjustment), "Cleared amount must be strictly positive.")
            End If
            If adjustment.ClearedQty <= 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(adjustment), "Cleared quantity must be strictly positive.")
            End If

            Dim grniAccountId As Integer = ResolveAccountId(AccountCodes.GRNIClearing, accountLookup)
            Dim ppvAccountId As Integer = ResolveAccountId(AccountCodes.PurchasePriceVariance, accountLookup)

            Dim entry As New JournalEntry() With {
                .SourceDocType = "GRNIAdjustment",
                .SourceDocId = adjustment.AdjustmentId,
                .PostingDate = adjustment.PostingDate,
                .OperationalEventDate = adjustment.AdjustmentDate,
                .Description = $"GRNI Clearing Adjustment {adjustment.DocumentNumber} - {adjustment.Reason}",
                .CreatedBy = "System"
            }

            Dim memoPrefix As String = If(grnLine IsNot Nothing AndAlso grnLine.GoodsReceiptNote IsNot Nothing,
                                         $"GRN {grnLine.GoodsReceiptNote.DocumentNumber} line {grnLine.GRNLineId}",
                                         $"GRN line {adjustment.GoodsReceiptLineId}")

            entry.Lines.Add(New JournalEntryLine(grniAccountId, adjustment.ClearedAmount, 0D, $"Clear unbilled GRNI liability - {memoPrefix}") With {
                .JournalEntry = entry
            })
            entry.Lines.Add(New JournalEntryLine(ppvAccountId, 0D, adjustment.ClearedAmount, $"Unbilled GRNI variance write-off - {adjustment.Reason}") With {
                .JournalEntry = entry
            })

            ValidateJournalBalance(entry.Lines)
            Return entry
        End Function

        ''' <summary>
        ''' Generates a balanced reversing journal entry for an existing journal entry.
        ''' Directive 1: Operational journal entries (Sales Invoices, GRNs, Supplier Bills, Credit Notes, etc.)
        ''' must NEVER be reversed directly in the GL. Throws InvalidOperationException unless SourceDocType is 'Manual' or 'Adjustment'.
        ''' </summary>
        Public Shared Function GenerateReversingJournal(originalEntry As JournalEntry, reversalPostingDate As DateTime, Optional reason As String = Nothing) As JournalEntry
            If originalEntry Is Nothing Then
                Throw New ArgumentNullException(NameOf(originalEntry), "Original journal entry cannot be null.")
            End If

            Dim docType As String = If(originalEntry.SourceDocType, String.Empty).Trim()
            If Not String.Equals(docType, "Manual", StringComparison.OrdinalIgnoreCase) AndAlso
               Not String.Equals(docType, "Adjustment", StringComparison.OrdinalIgnoreCase) Then
                Throw New InvalidOperationException(
                    $"Direct General Ledger reversal is prohibited for operational document type '{originalEntry.SourceDocType}'. " &
                    "Operational entries (Sales Invoices, GRNs, Supplier Bills, Credit Notes) must be corrected via explicit domain workflow documents to preserve subledger reconciliation.")
            End If

            If originalEntry.Lines Is Nothing OrElse originalEntry.Lines.Count = 0 Then
                Throw New ArgumentException("Original journal entry contains no distribution lines to reverse.", NameOf(originalEntry))
            End If

            Dim memoReason As String = If(String.IsNullOrWhiteSpace(reason), $"Reversal of Entry #{originalEntry.EntryId}", reason.Trim())

            Dim reversalEntry As New JournalEntry() With {
                .SourceDocType = "Reversal",
                .SourceDocId = originalEntry.EntryId,
                .PostingDate = reversalPostingDate,
                .OperationalEventDate = reversalPostingDate,
                .PeriodId = originalEntry.PeriodId,
                .Description = $"Reversal of Entry #{originalEntry.EntryId}: {memoReason}",
                .CreatedBy = "System"
            }

            For Each line In originalEntry.Lines
                reversalEntry.Lines.Add(New JournalEntryLine(
                    line.AccountId,
                    line.CreditAmount,
                    line.DebitAmount,
                    $"Reversal: {line.LineMemo}"
                ) With {
                    .JournalEntry = reversalEntry
                })
            Next

            ValidateJournalBalance(reversalEntry.Lines)
            Return reversalEntry
        End Function

        ''' <summary>
        ''' Resolves the numeric account ID from an optional lookup delegate, defaulting to the authoritative standard code.
        ''' </summary>
        Private Shared Function ResolveAccountId(defaultCode As Integer, accountLookup As Func(Of String, Integer)) As Integer
            If accountLookup IsNot Nothing Then
                Return accountLookup(defaultCode.ToString())
            End If
            Return defaultCode
        End Function

    End Class

End Namespace
