Option Strict On
Option Explicit On
Option Compare Binary

Imports System.Collections.Generic

Namespace UI.Accounting

    Public Class AccountItemViewModel
        Public Property AccountId As Integer
        Public Property Code As String = String.Empty
        Public Property Name As String = String.Empty
        Public Property AccountType As String = String.Empty
        Public Property NormalBalance As String = String.Empty
        Public Property Balance As Decimal
        Public Property IsActive As Boolean = True

        Public ReadOnly Property DisplayCodeAndName As String
            Get
                Return $"{Code} - {Name}"
            End Get
        End Property

        Public ReadOnly Property FormattedBalance As String
            Get
                Return Balance.ToString("C2")
            End Get
        End Property
    End Class

    Public Class JournalEntryLineViewModel
        Public Property LineId As Integer
        Public Property EntryId As Integer
        Public Property AccountId As Integer
        Public Property AccountCode As String = String.Empty
        Public Property AccountName As String = String.Empty
        Public Property DebitAmount As Decimal
        Public Property CreditAmount As Decimal
        Public Property LineMemo As String = String.Empty

        Public ReadOnly Property FormattedDebit As String
            Get
                If DebitAmount > 0D Then
                    Return DebitAmount.ToString("C2")
                Else
                    Return "-"
                End If
            End Get
        End Property

        Public ReadOnly Property FormattedCredit As String
            Get
                If CreditAmount > 0D Then
                    Return CreditAmount.ToString("C2")
                Else
                    Return "-"
                End If
            End Get
        End Property
    End Class

    Public Class JournalEntryViewModel
        Public Property EntryId As Integer
        Public Property PostingDate As DateTime
        Public Property OperationalEventDate As DateTime
        Public Property SourceDocType As String = String.Empty
        Public Property SourceDocNumber As String = String.Empty
        Public Property PeriodLabel As String = String.Empty
        Public Property Description As String = String.Empty
        Public Property CreatedBy As String = String.Empty
        Public Property TotalDebits As Decimal
        Public Property TotalCredits As Decimal
        Public Property Lines As New List(Of JournalEntryLineViewModel)()

        Public ReadOnly Property IsBalanced As Boolean
            Get
                Return Math.Round(TotalDebits, 4) = Math.Round(TotalCredits, 4)
            End Get
        End Property

        Public ReadOnly Property FormattedPostingDate As String
            Get
                Return PostingDate.ToString("yyyy-MM-dd")
            End Get
        End Property

        Public ReadOnly Property FormattedEventDate As String
            Get
                Return OperationalEventDate.ToString("yyyy-MM-dd")
            End Get
        End Property

        Public ReadOnly Property BalanceStatusText As String
            Get
                If IsBalanced Then
                    Return "Balanced (Debits = Credits)"
                Else
                    Dim diff As Decimal = Math.Abs(TotalDebits - TotalCredits)
                    Return $"UNBALANCED (Variance: {diff:C2})"
                End If
            End Get
        End Property
    End Class

    Public Class FiscalPeriodViewModel
        Public Property PeriodId As Integer
        Public Property FiscalYear As Integer
        Public Property PeriodNumber As Integer
        Public Property StartDate As DateTime
        Public Property EndDate As DateTime
        Public Property IsClosed As Boolean
        Public Property ClosedAtUtc As DateTime?

        Public ReadOnly Property PeriodLabel As String
            Get
                Return $"FY{FiscalYear}-P{PeriodNumber:D2}"
            End Get
        End Property

        Public ReadOnly Property StatusText As String
            Get
                If IsClosed Then
                    Return "CLOSED (Permanently Immutable)"
                Else
                    Return "OPEN (Statutory Postings Active)"
                End If
            End Get
        End Property

        Public ReadOnly Property DateRangeText As String
            Get
                Return $"{StartDate:yyyy-MM-dd} to {EndDate:yyyy-MM-dd}"
            End Get
        End Property
    End Class

    Public Class LedgerDashboardViewModel
        Public Property Accounts As New List(Of AccountItemViewModel)()
        Public Property JournalEntries As New List(Of JournalEntryViewModel)()
        Public Property CurrentPeriod As FiscalPeriodViewModel

        Public Sub New()
            LoadMockData()
        End Sub

        Private Sub LoadMockData()
            CurrentPeriod = New FiscalPeriodViewModel() With {
                .PeriodId = 11,
                .FiscalYear = 2026,
                .PeriodNumber = 11,
                .StartDate = New DateTime(2026, 11, 1),
                .EndDate = New DateTime(2026, 11, 30),
                .IsClosed = False,
                .ClosedAtUtc = Nothing
            }

            Accounts = New List(Of AccountItemViewModel) From {
                New AccountItemViewModel() With {.AccountId = 1, .Code = "1010", .Name = "Cash on Hand", .AccountType = "Asset", .NormalBalance = "Debit", .Balance = 2500.0D, .IsActive = True},
                New AccountItemViewModel() With {.AccountId = 2, .Code = "1020", .Name = "Operating Bank Account", .AccountType = "Asset", .NormalBalance = "Debit", .Balance = 43300.0D, .IsActive = True},
                New AccountItemViewModel() With {.AccountId = 3, .Code = "1200", .Name = "Accounts Receivable", .AccountType = "Asset", .NormalBalance = "Debit", .Balance = 0.0D, .IsActive = True},
                New AccountItemViewModel() With {.AccountId = 4, .Code = "1300", .Name = "Merchandise Inventory", .AccountType = "Asset", .NormalBalance = "Debit", .Balance = 680.0D, .IsActive = True},
                New AccountItemViewModel() With {.AccountId = 5, .Code = "2000", .Name = "Accounts Payable", .AccountType = "Liability", .NormalBalance = "Credit", .Balance = 0.0D, .IsActive = True},
                New AccountItemViewModel() With {.AccountId = 6, .Code = "2100", .Name = "Goods Received Not Invoiced (GRNI)", .AccountType = "Liability", .NormalBalance = "Credit", .Balance = 0.0D, .IsActive = True},
                New AccountItemViewModel() With {.AccountId = 7, .Code = "3000", .Name = "Retained Earnings", .AccountType = "Equity", .NormalBalance = "Credit", .Balance = 42000.0D, .IsActive = True},
                New AccountItemViewModel() With {.AccountId = 8, .Code = "3010", .Name = "Owner's Capital", .AccountType = "Equity", .NormalBalance = "Credit", .Balance = 4000.0D, .IsActive = True},
                New AccountItemViewModel() With {.AccountId = 9, .Code = "4000", .Name = "Sales Revenue", .AccountType = "Revenue", .NormalBalance = "Credit", .Balance = 600.0D, .IsActive = True},
                New AccountItemViewModel() With {.AccountId = 10, .Code = "4100", .Name = "Sales Returns & Allowances", .AccountType = "Revenue (Contra)", .NormalBalance = "Debit", .Balance = 150.0D, .IsActive = True},
                New AccountItemViewModel() With {.AccountId = 11, .Code = "5000", .Name = "Cost of Goods Sold (COGS)", .AccountType = "Expense", .NormalBalance = "Debit", .Balance = 300.0D, .IsActive = True},
                New AccountItemViewModel() With {.AccountId = 12, .Code = "5100", .Name = "Purchase Price Variance (PPV)", .AccountType = "Expense", .NormalBalance = "Debit", .Balance = 100.0D, .IsActive = True},
                New AccountItemViewModel() With {.AccountId = 13, .Code = "5200", .Name = "Inbound Freight Variance", .AccountType = "Expense", .NormalBalance = "Debit", .Balance = 50.0D, .IsActive = True},
                New AccountItemViewModel() With {.AccountId = 14, .Code = "5300", .Name = "Inventory Shrinkage & Damaged Stock", .AccountType = "Expense", .NormalBalance = "Debit", .Balance = 20.0D, .IsActive = True},
                New AccountItemViewModel() With {.AccountId = 15, .Code = "5400", .Name = "Stocktake Revaluation Variance", .AccountType = "Expense", .NormalBalance = "Debit", .Balance = 0.0D, .IsActive = True},
                New AccountItemViewModel() With {.AccountId = 16, .Code = "5500", .Name = "Cash Rounding & Roundoff Variance", .AccountType = "Expense", .NormalBalance = "Debit", .Balance = 0.0D, .IsActive = True}
            }

            JournalEntries = New List(Of JournalEntryViewModel)()

            ' CUJ 1: Inbound Receiving Dock GRN
            Dim je1 As New JournalEntryViewModel() With {
                .EntryId = 101,
                .PostingDate = New DateTime(2026, 11, 3),
                .OperationalEventDate = New DateTime(2026, 11, 3),
                .SourceDocType = "Goods Receipt Note",
                .SourceDocNumber = "GRN-2026-0001",
                .PeriodLabel = "FY2026-P11",
                .Description = "Receipt of 100 units SKU-A @ $10.00/unit into Inventory",
                .CreatedBy = "dock_clerk",
                .TotalDebits = 1000.0D,
                .TotalCredits = 1000.0D
            }
            je1.Lines.Add(New JournalEntryLineViewModel() With {.LineId = 1, .EntryId = 101, .AccountId = 4, .AccountCode = "1300", .AccountName = "Merchandise Inventory", .DebitAmount = 1000.0D, .CreditAmount = 0.0D, .LineMemo = "Inbound stock receipt 100 units SKU-A"})
            je1.Lines.Add(New JournalEntryLineViewModel() With {.LineId = 2, .EntryId = 101, .AccountId = 6, .AccountCode = "2100", .AccountName = "Goods Received Not Invoiced (GRNI)", .DebitAmount = 0.0D, .CreditAmount = 1000.0D, .LineMemo = "GRNI clearing liability accrual"})
            JournalEntries.Add(je1)

            ' Supplier Bill with PPV and Freight matching
            Dim je2 As New JournalEntryViewModel() With {
                .EntryId = 102,
                .PostingDate = New DateTime(2026, 11, 5),
                .OperationalEventDate = New DateTime(2026, 11, 5),
                .SourceDocType = "Supplier Bill",
                .SourceDocNumber = "BILL-2026-0042",
                .PeriodLabel = "FY2026-P11",
                .Description = "Supplier Bill for GRN-2026-0001 with $1/unit PPV and $50 Freight",
                .CreatedBy = "ap_accountant",
                .TotalDebits = 1150.0D,
                .TotalCredits = 1150.0D
            }
            je2.Lines.Add(New JournalEntryLineViewModel() With {.LineId = 3, .EntryId = 102, .AccountId = 6, .AccountCode = "2100", .AccountName = "Goods Received Not Invoiced (GRNI)", .DebitAmount = 1000.0D, .CreditAmount = 0.0D, .LineMemo = "Clear GRNI accrual for 100 units"})
            je2.Lines.Add(New JournalEntryLineViewModel() With {.LineId = 4, .EntryId = 102, .AccountId = 12, .AccountCode = "5100", .AccountName = "Purchase Price Variance (PPV)", .DebitAmount = 100.0D, .CreditAmount = 0.0D, .LineMemo = "Price variance: $11 bill vs $10 PO price"})
            je2.Lines.Add(New JournalEntryLineViewModel() With {.LineId = 5, .EntryId = 102, .AccountId = 13, .AccountCode = "5200", .AccountName = "Inbound Freight Variance", .DebitAmount = 50.0D, .CreditAmount = 0.0D, .LineMemo = "Third-party carrier freight uncapitalized"})
            je2.Lines.Add(New JournalEntryLineViewModel() With {.LineId = 6, .EntryId = 102, .AccountId = 5, .AccountCode = "2000", .AccountName = "Accounts Payable", .DebitAmount = 0.0D, .CreditAmount = 1150.0D, .LineMemo = "Trade payable obligation to vendor"})
            JournalEntries.Add(je2)

            ' CUJ 2: Outbound Sales Invoice Fulfillment (40 units @ $15 selling price, cost $10)
            Dim je3 As New JournalEntryViewModel() With {
                .EntryId = 103,
                .PostingDate = New DateTime(2026, 11, 8),
                .OperationalEventDate = New DateTime(2026, 11, 8),
                .SourceDocType = "Sales Invoice",
                .SourceDocNumber = "INV-2026-0101",
                .PeriodLabel = "FY2026-P11",
                .Description = "Sales Invoice fulfillment: 40 units SKU-A dispatched",
                .CreatedBy = "sales_rep",
                .TotalDebits = 1000.0D,
                .TotalCredits = 1000.0D
            }
            je3.Lines.Add(New JournalEntryLineViewModel() With {.LineId = 7, .EntryId = 103, .AccountId = 3, .AccountCode = "1200", .AccountName = "Accounts Receivable", .DebitAmount = 600.0D, .CreditAmount = 0.0D, .LineMemo = "Trade receivable: 40 units @ $15.00"})
            je3.Lines.Add(New JournalEntryLineViewModel() With {.LineId = 8, .EntryId = 103, .AccountId = 9, .AccountCode = "4000", .AccountName = "Sales Revenue", .DebitAmount = 0.0D, .CreditAmount = 600.0D, .LineMemo = "Sales revenue recognition"})
            je3.Lines.Add(New JournalEntryLineViewModel() With {.LineId = 9, .EntryId = 103, .AccountId = 11, .AccountCode = "5000", .AccountName = "Cost of Goods Sold (COGS)", .DebitAmount = 400.0D, .CreditAmount = 0.0D, .LineMemo = "COGS for 40 units dispatched @ $10.00"})
            je3.Lines.Add(New JournalEntryLineViewModel() With {.LineId = 10, .EntryId = 103, .AccountId = 4, .AccountCode = "1300", .AccountName = "Merchandise Inventory", .DebitAmount = 0.0D, .CreditAmount = 400.0D, .LineMemo = "Perpetual inventory decrement 40 units"})
            JournalEntries.Add(je3)

            ' CUJ 3: Customer Return with Quarantine Inspection (10 returned: 8 restocked, 2 scrapped)
            Dim je4 As New JournalEntryViewModel() With {
                .EntryId = 104,
                .PostingDate = New DateTime(2026, 11, 10),
                .OperationalEventDate = New DateTime(2026, 11, 10),
                .SourceDocType = "Credit Note",
                .SourceDocNumber = "CN-2026-0005",
                .PeriodLabel = "FY2026-P11",
                .Description = "Credit Note for Return RMA-2026-0012: 8 Restocked, 2 Damaged/Scrap",
                .CreatedBy = "finance_manager",
                .TotalDebits = 250.0D,
                .TotalCredits = 250.0D
            }
            je4.Lines.Add(New JournalEntryLineViewModel() With {.LineId = 11, .EntryId = 104, .AccountId = 10, .AccountCode = "4100", .AccountName = "Sales Returns & Allowances", .DebitAmount = 150.0D, .CreditAmount = 0.0D, .LineMemo = "Customer credit memo: 10 units @ $15.00"})
            je4.Lines.Add(New JournalEntryLineViewModel() With {.LineId = 12, .EntryId = 104, .AccountId = 3, .AccountCode = "1200", .AccountName = "Accounts Receivable", .DebitAmount = 0.0D, .CreditAmount = 150.0D, .LineMemo = "AR subledger credit to customer"})
            je4.Lines.Add(New JournalEntryLineViewModel() With {.LineId = 13, .EntryId = 104, .AccountId = 4, .AccountCode = "1300", .AccountName = "Merchandise Inventory", .DebitAmount = 80.0D, .CreditAmount = 0.0D, .LineMemo = "Restock 8 units @ $10.00 into active inventory"})
            je4.Lines.Add(New JournalEntryLineViewModel() With {.LineId = 14, .EntryId = 104, .AccountId = 14, .AccountCode = "5300", .AccountName = "Inventory Shrinkage & Damaged Stock", .DebitAmount = 20.0D, .CreditAmount = 0.0D, .LineMemo = "Damaged return scrap write-off: 2 units @ $10.00"})
            je4.Lines.Add(New JournalEntryLineViewModel() With {.LineId = 15, .EntryId = 104, .AccountId = 11, .AccountCode = "5000", .AccountName = "Cost of Goods Sold (COGS)", .DebitAmount = 0.0D, .CreditAmount = 100.0D, .LineMemo = "Reversal of COGS for 10 returned units"})
            JournalEntries.Add(je4)

            ' Customer Payment Receipt
            Dim je5 As New JournalEntryViewModel() With {
                .EntryId = 105,
                .PostingDate = New DateTime(2026, 11, 12),
                .OperationalEventDate = New DateTime(2026, 11, 12),
                .SourceDocType = "Payment Receipt",
                .SourceDocNumber = "RCPT-2026-0089",
                .PeriodLabel = "FY2026-P11",
                .Description = "Customer payment settling net invoice balance ($600 - $150 = $450)",
                .CreatedBy = "ar_clerk",
                .TotalDebits = 450.0D,
                .TotalCredits = 450.0D
            }
            je5.Lines.Add(New JournalEntryLineViewModel() With {.LineId = 16, .EntryId = 105, .AccountId = 2, .AccountCode = "1020", .AccountName = "Operating Bank Account", .DebitAmount = 450.0D, .CreditAmount = 0.0D, .LineMemo = "Deposit into Operating Checking Account"})
            je5.Lines.Add(New JournalEntryLineViewModel() With {.LineId = 17, .EntryId = 105, .AccountId = 3, .AccountCode = "1200", .AccountName = "Accounts Receivable", .DebitAmount = 0.0D, .CreditAmount = 450.0D, .LineMemo = "Clear customer trade receivable"})
            JournalEntries.Add(je5)

            ' Vendor Payment Voucher
            Dim je6 As New JournalEntryViewModel() With {
                .EntryId = 106,
                .PostingDate = New DateTime(2026, 11, 14),
                .OperationalEventDate = New DateTime(2026, 11, 14),
                .SourceDocType = "Payment Voucher",
                .SourceDocNumber = "PV-2026-0033",
                .PeriodLabel = "FY2026-P11",
                .Description = "Vendor settlement for BILL-2026-0042",
                .CreatedBy = "treasury_clerk",
                .TotalDebits = 1150.0D,
                .TotalCredits = 1150.0D
            }
            je6.Lines.Add(New JournalEntryLineViewModel() With {.LineId = 18, .EntryId = 106, .AccountId = 5, .AccountCode = "2000", .AccountName = "Accounts Payable", .DebitAmount = 1150.0D, .CreditAmount = 0.0D, .LineMemo = "Settle vendor trade payable"})
            je6.Lines.Add(New JournalEntryLineViewModel() With {.LineId = 19, .EntryId = 106, .AccountId = 2, .AccountCode = "1020", .AccountName = "Operating Bank Account", .DebitAmount = 0.0D, .CreditAmount = 1150.0D, .LineMemo = "Electronic bank transfer disbursement"})
            JournalEntries.Add(je6)
        End Sub
    End Class

End Namespace
