Option Strict On
Option Explicit On
Option Compare Binary

Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Threading
Imports System.Windows.Forms
Imports Merchandising_Accounting_App
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Namespace Merchandising_Accounting_App.Tests

    <TestClass>
    Public Class FormInstantiationTests

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
        Public Sub Test_MainShellForm_InstantiatesCleanly()
            RunOnSta(Sub()
                Using form As New UI.MainShellForm()
                    Assert.IsNotNull(form)
                    Assert.AreNotEqual(0, form.Controls.Count)
                    form.CreateControl()
                    Dim handle As IntPtr = form.Handle
                    Assert.AreNotEqual(IntPtr.Zero, handle)
                    Assert.AreEqual(form.CurrentAutoScaleDimensions, form.AutoScaleDimensions)
                    Dim scaleRatio As Single = form.CurrentAutoScaleDimensions.Width / form.AutoScaleDimensions.Width
                    Assert.AreEqual(1.0F, scaleRatio, 0.01F)
                End Using
            End Sub)
        End Sub

        <TestMethod>
        Public Sub Test_LedgerDashboardForm_InstantiatesCleanly()
            RunOnSta(Sub()
                Using form As New UI.Accounting.LedgerDashboardForm()
                    Assert.IsNotNull(form)
                    Assert.AreNotEqual(0, form.Controls.Count)
                    form.CreateControl()
                    Dim handle As IntPtr = form.Handle
                    Assert.AreNotEqual(IntPtr.Zero, handle)
                    Assert.AreEqual(form.CurrentAutoScaleDimensions, form.AutoScaleDimensions)
                    Dim scaleRatio As Single = form.CurrentAutoScaleDimensions.Width / form.AutoScaleDimensions.Width
                    Assert.AreEqual(1.0F, scaleRatio, 0.01F)
                End Using
            End Sub)
        End Sub

        <TestMethod>
        Public Sub Test_PurchasingDashboardForm_InstantiatesCleanly()
            RunOnSta(Sub()
                Using form As New UI.Purchasing.PurchasingDashboardForm()
                    Assert.IsNotNull(form)
                    Assert.AreNotEqual(0, form.Controls.Count)
                    form.CreateControl()
                    Dim handle As IntPtr = form.Handle
                    Assert.AreNotEqual(IntPtr.Zero, handle)
                    Assert.AreEqual(form.CurrentAutoScaleDimensions, form.AutoScaleDimensions)
                    Dim scaleRatio As Single = form.CurrentAutoScaleDimensions.Width / form.AutoScaleDimensions.Width
                    Assert.AreEqual(1.0F, scaleRatio, 0.01F)
                End Using
            End Sub)
        End Sub

        <TestMethod>
        Public Sub Test_SalesDashboardForm_InstantiatesCleanly()
            RunOnSta(Sub()
                Using form As New UI.Sales.SalesDashboardForm()
                    Assert.IsNotNull(form)
                    Assert.AreNotEqual(0, form.Controls.Count)
                    form.CreateControl()
                    Dim handle As IntPtr = form.Handle
                    Assert.AreNotEqual(IntPtr.Zero, handle)
                    Assert.AreEqual(form.CurrentAutoScaleDimensions, form.AutoScaleDimensions)
                    Dim scaleRatio As Single = form.CurrentAutoScaleDimensions.Width / form.AutoScaleDimensions.Width
                    Assert.AreEqual(1.0F, scaleRatio, 0.01F)
                End Using
            End Sub)
        End Sub

        <TestMethod>
        Public Sub Test_WarehouseIntakeForm_InstantiatesCleanly()
            RunOnSta(Sub()
                Using form As New UI.Warehouse.WarehouseIntakeForm()
                    Assert.IsNotNull(form)
                    Assert.AreNotEqual(0, form.Controls.Count)
                    form.CreateControl()
                    Dim handle As IntPtr = form.Handle
                    Assert.AreNotEqual(IntPtr.Zero, handle)
                    Assert.AreEqual(form.CurrentAutoScaleDimensions, form.AutoScaleDimensions)
                    Dim scaleRatio As Single = form.CurrentAutoScaleDimensions.Width / form.AutoScaleDimensions.Width
                    Assert.AreEqual(1.0F, scaleRatio, 0.01F)
                End Using
            End Sub)
        End Sub

        <TestMethod>
        Public Sub Test_FinancialReportsForm_InstantiatesCleanly()
            RunOnSta(Sub()
                Using form As New UI.Reporting.FinancialReportsForm()
                    Assert.IsNotNull(form)
                    Assert.AreNotEqual(0, form.Controls.Count)
                    form.CreateControl()
                    Dim handle As IntPtr = form.Handle
                    Assert.AreNotEqual(IntPtr.Zero, handle)
                    Assert.AreEqual(form.CurrentAutoScaleDimensions, form.AutoScaleDimensions)
                    Dim scaleRatio As Single = form.CurrentAutoScaleDimensions.Width / form.AutoScaleDimensions.Width
                    Assert.AreEqual(1.0F, scaleRatio, 0.01F)
                End Using
            End Sub)
        End Sub

        <TestMethod>
        Public Sub Test_AllFormDesigners_Declare_AutoScaleDimensions_7_17()
            Dim designerFiles() As String = {
                "..\..\..\..\Merchandising_Accounting_App\UI\MainShellForm.Designer.vb",
                "..\..\..\..\Merchandising_Accounting_App\UI\Accounting\LedgerDashboardForm.Designer.vb",
                "..\..\..\..\Merchandising_Accounting_App\UI\Purchasing\PurchasingDashboardForm.Designer.vb",
                "..\..\..\..\Merchandising_Accounting_App\UI\Sales\SalesDashboardForm.Designer.vb",
                "..\..\..\..\Merchandising_Accounting_App\UI\Warehouse\WarehouseIntakeForm.Designer.vb",
                "..\..\..\..\Merchandising_Accounting_App\UI\Reporting\FinancialReportsForm.Designer.vb"
            }

            For Each relPath In designerFiles
                Dim fullPath As String = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, relPath))
                Assert.IsTrue(File.Exists(fullPath), $"Designer file not found at: {fullPath}")
                Dim content As String = File.ReadAllText(fullPath)
                Assert.IsTrue(content.Contains("Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)"),
                    $"Expected 'Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)' in {Path.GetFileName(fullPath)}")
                Assert.IsFalse(content.Contains("Me.Font = New System.Drawing.Font"),
                    $"Form-level Me.Font must not exist in {Path.GetFileName(fullPath)}")
            Next
        End Sub

        <TestMethod>
        Public Sub Test_LedgerDashboardForm_DualResolution_LayoutAllocation()
            RunOnSta(Sub()
                Using shell As New UI.MainShellForm()
                    shell.AutoLoadDefaultModule = False
                    shell.FormBorderStyle = FormBorderStyle.None
                    shell.WindowState = FormWindowState.Normal
                    shell.StartPosition = FormStartPosition.Manual
                    shell.Location = New Point(0, 0)
                    shell.ClientSize = New Size(1280, 720)
                    shell.Visible = True

                    Dim ledger As New UI.Accounting.LedgerDashboardForm()
                    shell.LoadChildForm(ledger, "General Ledger", "Chart of Accounts & Journal Entries")
                    shell.WindowState = FormWindowState.Normal
                    shell.ClientSize = New Size(1280, 720)
                    shell.pnlSidebar.Width = 280
                    shell.PerformLayout()
                    ledger.PerformLayout()

                    ' Standard 1280x720 assertions:
                    Assert.AreEqual(290, ledger.splitMain.SplitterDistance, $"Expected SplitterDistance = 290 at 720p, got {ledger.splitMain.SplitterDistance}")

                    Dim dgv = ledger.dgvJournalEntries
                    Assert.AreEqual(9, dgv.Columns.Count)
                    Assert.AreEqual(38, dgv.Columns("colEntryId").Width)
                    Assert.AreEqual(75, dgv.Columns("colPostingDate").Width)
                    Assert.AreEqual(75, dgv.Columns("colEventDate").Width)
                    Assert.AreEqual(95, dgv.Columns("colDocType").Width)
                    Assert.AreEqual(90, dgv.Columns("colDocNumber").Width)
                    Assert.AreEqual(80, dgv.Columns("colTotalDebits").Width)
                    Assert.AreEqual(80, dgv.Columns("colTotalCredits").Width)
                    Assert.AreEqual(72, dgv.Columns("colBalanceStatus").Width)

                    ' Assert zero horizontal scrollbars and 100% column visibility at 1280x720:
                    Dim hScrollEntries = dgv.Controls.OfType(Of HScrollBar)().FirstOrDefault()
                    Assert.IsNotNull(hScrollEntries, "HScrollBar should exist in dgvJournalEntries controls")
                    Assert.IsFalse(hScrollEntries.Visible, "dgvJournalEntries HorizontalScrollBar must not be visible at 1280x720")
                    Assert.AreEqual(9, dgv.DisplayedColumnCount(False), "All 9 columns must be fully displayed without clipping at 1280x720")

                    Dim dgvLines = ledger.dgvJournalLines
                    Dim hScrollLines = dgvLines.Controls.OfType(Of HScrollBar)().FirstOrDefault()
                    Assert.IsNotNull(hScrollLines, "HScrollBar should exist in dgvJournalLines controls")
                    Assert.IsFalse(hScrollLines.Visible, "dgvJournalLines HorizontalScrollBar must not be visible at 1280x720")

                    ' Maximized 1920x1080 assertions:
                    shell.ClientSize = New Size(1920, 1080)
                    shell.pnlSidebar.Width = 280
                    shell.PerformLayout()
                    ledger.PerformLayout()

                    Assert.IsTrue(ledger.splitMain.SplitterDistance >= 420, $"Expected SplitterDistance >= 420 at 1080p, got {ledger.splitMain.SplitterDistance}")
                    Assert.IsTrue(ledger.splitMain.SplitterDistance <= 480, $"Expected SplitterDistance <= 480 at 1080p, got {ledger.splitMain.SplitterDistance}")
                    Assert.IsFalse(hScrollEntries.Visible, "dgvJournalEntries HorizontalScrollBar must not be visible at 1920x1080")
                    Assert.AreEqual(9, dgv.DisplayedColumnCount(False), "All 9 columns must be fully displayed without clipping at 1920x1080")
                    Assert.IsFalse(hScrollLines.Visible, "dgvJournalLines HorizontalScrollBar must not be visible at 1920x1080")
                End Using
            End Sub)
        End Sub

    End Class

End Namespace
