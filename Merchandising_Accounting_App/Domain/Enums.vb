Option Strict On
Option Explicit On
Option Compare Binary

Namespace Domain

    ''' <summary>
    ''' High-level categorization of General Ledger accounts according to standard accounting principles.
    ''' </summary>
    Public Enum AccountType
        Asset = 1
        Liability = 2
        Equity = 3
        Revenue = 4
        Expense = 5
    End Enum

    ''' <summary>
    ''' Operational document lifecycle states.
    ''' </summary>
    Public Enum DocumentStatus
        Draft = 1
        Posted = 2
        Settled = 3
        Cancelled = 4
    End Enum

    ''' <summary>
    ''' Physical and financial disposition decision for customer returns.
    ''' </summary>
    Public Enum ReturnDisposition
        Restock = 1
        Scrap = 2
    End Enum

    ''' <summary>
    ''' Categorization reasons for physical inventory adjustments and stocktake variances.
    ''' </summary>
    Public Enum AdjustmentReason
        Shrinkage = 1
        Damage = 2
        CountSurplus = 3
        CountDeficit = 4
    End Enum

End Namespace
