Option Strict On
Option Explicit On
Option Compare Binary

Imports Merchandising_Accounting_App.UI.Accounting
Imports Merchandising_Accounting_App.UI.Reporting
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Namespace Merchandising_Accounting_App.Tests

    <TestClass>
    Public Class DecimalCurrencyFormattingTests

        <TestMethod>
        Public Sub Test_TrialBalanceRow_FormatsToTwoDecimalsWithCurrencySymbol()
            Dim row As New TrialBalanceRowViewModel() With {
                .AccountCode = "1010",
                .AccountName = "Operating Cash",
                .AccountType = "Asset",
                .DebitAmount = 12500.5D,
                .CreditAmount = 0D
            }

            Assert.IsTrue(row.FormattedDebit.Contains("12,500.50"), $"Formatted debit should contain '12,500.50', actual: '{row.FormattedDebit}'")
            Assert.IsTrue(row.FormattedDebit.StartsWith("$"), $"Formatted debit should start with currency symbol '$', actual: '{row.FormattedDebit}'")
            Assert.AreEqual("-", row.FormattedCredit, "Zero credit balance should format as dash '-'")

            Dim creditRow As New TrialBalanceRowViewModel() With {
                .AccountCode = "4000",
                .AccountName = "Sales Revenue",
                .AccountType = "Revenue",
                .DebitAmount = 0D,
                .CreditAmount = 45000D
            }
            Assert.AreEqual("-", creditRow.FormattedDebit)
            Assert.IsTrue(creditRow.FormattedCredit.Contains("45,000.00"), $"Formatted credit should contain '45,000.00', actual: '{creditRow.FormattedCredit}'")
            Assert.IsTrue(creditRow.FormattedCredit.StartsWith("$"), "Formatted credit should start with '$'")
        End Sub

        <TestMethod>
        Public Sub Test_IncomeStatementRow_FormatsPositiveAndNegativeAmounts()
            Dim posRow As New IncomeStatementRowViewModel() With {
                .Category = "Revenue",
                .LineDescription = "Gross Sales",
                .Amount = 150.25D
            }
            Assert.IsTrue(posRow.FormattedAmount.Contains("150.25"), $"Expected '150.25', actual: '{posRow.FormattedAmount}'")
            Assert.IsTrue(posRow.FormattedAmount.StartsWith("$"))

            Dim negRow As New IncomeStatementRowViewModel() With {
                .Category = "Net Income",
                .LineDescription = "Net Operating Income",
                .Amount = -20D
            }
            Assert.IsTrue(negRow.FormattedAmount.Contains("($20.00)") OrElse negRow.FormattedAmount.Contains("-$20.00"),
                $"Negative amount should format with parentheses or negative sign, actual: '{negRow.FormattedAmount}'")
        End Sub

        <TestMethod>
        Public Sub Test_ProductProfitabilityRow_FormatsAllVariancesWithoutZeroTruncation()
            Dim row As New ProductProfitabilityRowViewModel() With {
                .ProductSku = "SKU-001",
                .ProductName = "Test Product",
                .UnitsSold = 10,
                .InvoicedRevenue = 1234.56D,
                .CostOfGoodsSold = 800D,
                .InboundFreightVariance = 12.3D,
                .PurchasePriceVariance = 45D
            }

            Assert.IsTrue(row.FormattedRevenue.Contains("1,234.56"), $"Actual: '{row.FormattedRevenue}'")
            Assert.IsTrue(row.FormattedCogs.Contains("800.00"), $"Cogs zero cents truncated? Actual: '{row.FormattedCogs}'")
            Assert.IsTrue(row.FormattedFreight.Contains("12.30"), $"Freight trailing zero truncated? Actual: '{row.FormattedFreight}'")
            Assert.IsTrue(row.FormattedPpv.Contains("45.00"), $"PPV zero cents truncated? Actual: '{row.FormattedPpv}'")
        End Sub

        <TestMethod>
        Public Sub Test_AccountItem_FormattedBalance_PreservesTwoDecimals()
            Dim item As New AccountItemViewModel() With {
                .Code = "1200",
                .Name = "Accounts Receivable",
                .Balance = 5432.1D
            }
            Assert.IsTrue(item.FormattedBalance.Contains("5,432.10"), $"Expected '5,432.10', actual: '{item.FormattedBalance}'")

            Dim zeroItem As New AccountItemViewModel() With {
                .Code = "1300",
                .Name = "Inventory",
                .Balance = 0D
            }
            Assert.IsTrue(zeroItem.FormattedBalance.Contains("0.00"), $"Expected '0.00', actual: '{zeroItem.FormattedBalance}'")
        End Sub

        <TestMethod>
        Public Sub Test_JournalEntry_BalancesToFourDecimalPlaces()
            Dim entry As New JournalEntryViewModel() With {
                .TotalDebits = 1000.0001D,
                .TotalCredits = 1000.0001D
            }
            Assert.IsTrue(entry.IsBalanced)

            Dim unbalancedEntry As New JournalEntryViewModel() With {
                .TotalDebits = 1000.0001D,
                .TotalCredits = 1000.0005D
            }
            Assert.IsFalse(unbalancedEntry.IsBalanced)
        End Sub

    End Class

End Namespace
