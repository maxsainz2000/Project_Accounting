Option Strict On
Option Explicit On
Option Compare Binary

Imports System.Threading
Imports Merchandising_Accounting_App
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Namespace Merchandising_Accounting_App.Tests

    <TestClass>
    Public Class BrandingAndBrandingStringsTests

        Private Sub RunOnSta(action As Action)
            Dim caughtEx As Exception = Nothing
            Dim thread As New Thread(Sub()
                Try
                    action.Invoke()
                Catch ex As Exception
                    caughtEx = ex
                End Try
            End Sub)
            thread.SetApartmentState(ApartmentState.STA)
            thread.Start()
            thread.Join()

            If caughtEx IsNot Nothing Then
                Throw New AssertFailedException($"Failed on STA thread: {caughtEx.Message}", caughtEx)
            End If
        End Sub

        <TestMethod>
        Public Sub Test_MainShell_BrandingAndSidebar_ConformToRequirements()
            RunOnSta(Sub()
                Using shell As New UI.MainShellForm()
                    shell.CreateControl()
                    Dim handle As IntPtr = shell.Handle
                    Assert.AreNotEqual(IntPtr.Zero, handle)

                    Assert.AreEqual("Merchandising ERP", shell.lblAppTitle.Text)
                    Assert.IsTrue(shell.lblAppTitle.AutoSize)
                    Assert.IsTrue(shell.lblAppTitle.Left >= 20, $"Expected lblAppTitle.Left >= 20, actual: {shell.lblAppTitle.Left}")
                    Assert.IsTrue(shell.lblAppTitle.Top >= 24, $"Expected lblAppTitle.Top >= 24, actual: {shell.lblAppTitle.Top}")
                    Assert.IsTrue(shell.pnlSidebar.Width >= 280, $"Expected pnlSidebar.Width >= 280, actual: {shell.pnlSidebar.Width}")

                    ' Navigation buttons properties
                    Assert.AreEqual("General Ledger & COA", shell.btnLedger.Text)
                    Assert.IsFalse(shell.btnLedger.UseMnemonic)
                    Assert.IsTrue(shell.btnLedger.Width >= 280)
                    Assert.IsTrue(shell.btnLedger.Height >= 52)

                    Assert.AreEqual("Purchasing & 3-Way Match", shell.btnPurchasing.Text)
                    Assert.IsFalse(shell.btnPurchasing.UseMnemonic)
                    Assert.IsTrue(shell.btnPurchasing.Width >= 280)
                    Assert.IsTrue(shell.btnPurchasing.Height >= 52)

                    Assert.AreEqual("Sales & Fulfillment", shell.btnSales.Text)
                    Assert.IsFalse(shell.btnSales.UseMnemonic)
                    Assert.IsTrue(shell.btnSales.Width >= 280)
                    Assert.IsTrue(shell.btnSales.Height >= 52)

                    Assert.AreEqual("Warehouse & Intake", shell.btnWarehouse.Text)
                    Assert.IsFalse(shell.btnWarehouse.UseMnemonic)
                    Assert.IsTrue(shell.btnWarehouse.Width >= 280)
                    Assert.IsTrue(shell.btnWarehouse.Height >= 52)

                    Assert.AreEqual("Financial Reports", shell.btnFinancialReports.Text)
                    Assert.IsFalse(shell.btnFinancialReports.UseMnemonic)
                    Assert.IsTrue(shell.btnFinancialReports.Width >= 280)
                    Assert.IsTrue(shell.btnFinancialReports.Height >= 52)
                End Using
            End Sub)
        End Sub

        <TestMethod>
        Public Sub Test_MainShell_HeaderDynamicHeight_EnsuresBreathingRoom()
            RunOnSta(Sub()
                Using shell As New UI.MainShellForm()
                    Using child As New UI.Accounting.LedgerDashboardForm()
                        shell.LoadChildForm(child, "Test Module Title", "Test Module Subtitle with detailed descriptive text")
                        Assert.IsFalse(shell.pnlHeader.Height < 88, $"Expected pnlHeader.Height >= 88, actual: {shell.pnlHeader.Height}")
                    End Using
                End Using
            End Sub)
        End Sub

    End Class

End Namespace
