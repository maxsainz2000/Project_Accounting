Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows.Forms
Imports Merchandising_Accounting_App.Domain.Models

Namespace UI.Purchasing

    Public Class DisburseVendorPaymentDialog

        Private ReadOnly _dataService As PurchasingDataService
        Private ReadOnly _targetVendorId As Integer?
        Private ReadOnly _targetBillId As Integer?
        Private _allBills As List(Of OpenSupplierBillItem) = New List(Of OpenSupplierBillItem)()
        Private _selectedBill As OpenSupplierBillItem

        <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
        Public Property CreatedVoucher As PaymentVoucher

        Public Sub New(dataService As PurchasingDataService, Optional targetVendorId As Integer? = Nothing, Optional targetBillId As Integer? = Nothing)
            InitializeComponent()
            _dataService = dataService
            _targetVendorId = targetVendorId
            _targetBillId = targetBillId
        End Sub

        Private Sub DisburseVendorPaymentDialog_Load(sender As Object, e As EventArgs) Handles Me.Load
            PopulateData()
        End Sub

        Private Sub PopulateData()
            If _dataService Is Nothing Then Return

            ' 1. Load Bank Accounts
            cmbBankAccount.Items.Clear()
            Dim banks = _dataService.GetBankAccounts()
            For Each b In banks
                cmbBankAccount.Items.Add(b)
            Next
            cmbBankAccount.DisplayMember = "Name"
            cmbBankAccount.ValueMember = "AccountId"
            ' Default to Account 1020 Operating Bank
            Dim operatingBank = banks.FirstOrDefault(Function(b) b.Code = "1020")
            If operatingBank IsNot Nothing Then
                cmbBankAccount.SelectedItem = operatingBank
            ElseIf cmbBankAccount.Items.Count > 0 Then
                cmbBankAccount.SelectedIndex = 0
            End If

            ' 2. Load Vendors
            cmbVendor.Items.Clear()
            For Each v In _dataService.Vendors
                cmbVendor.Items.Add(v)
            Next
            cmbVendor.DisplayMember = "DisplayText"

            ' 3. Load Open Supplier Bills
            _allBills = _dataService.GetOpenSupplierBills()

            ' Pre-selection logic
            If _targetVendorId.HasValue AndAlso _targetVendorId.Value > 0 Then
                Dim matchedVendor = _dataService.Vendors.FirstOrDefault(Function(v) v.VendorId = _targetVendorId.Value)
                If matchedVendor IsNot Nothing Then
                    cmbVendor.SelectedItem = matchedVendor
                ElseIf cmbVendor.Items.Count > 0 Then
                    cmbVendor.SelectedIndex = 0
                End If
            ElseIf cmbVendor.Items.Count > 0 Then
                cmbVendor.SelectedIndex = 0
            End If

            RefreshBillsForSelectedVendor()

            If _targetBillId.HasValue AndAlso _targetBillId.Value > 0 Then
                For i As Integer = 0 To cmbSupplierBill.Items.Count - 1
                    Dim item = TryCast(cmbSupplierBill.Items(i), OpenSupplierBillItem)
                    If item IsNot Nothing AndAlso item.BillId = _targetBillId.Value Then
                        cmbSupplierBill.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            dtpPaymentDate.Value = DateTime.Today
        End Sub

        Private Sub cmbVendor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbVendor.SelectedIndexChanged
            RefreshBillsForSelectedVendor()
        End Sub

        Private Sub RefreshBillsForSelectedVendor()
            Dim selectedVendor = TryCast(cmbVendor.SelectedItem, PurchasingVendorModel)
            cmbSupplierBill.Items.Clear()

            If selectedVendor Is Nothing Then
                _selectedBill = Nothing
                UpdateBillDetails()
                Return
            End If

            Dim vendorBills = _allBills.Where(Function(b) b.VendorId = selectedVendor.VendorId).ToList()
            For Each b In vendorBills
                cmbSupplierBill.Items.Add(b)
            Next
            cmbSupplierBill.DisplayMember = "DisplayText"

            If cmbSupplierBill.Items.Count > 0 Then
                cmbSupplierBill.SelectedIndex = 0
            Else
                _selectedBill = Nothing
                UpdateBillDetails()
            End If
        End Sub

        Private Sub cmbSupplierBill_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSupplierBill.SelectedIndexChanged
            _selectedBill = TryCast(cmbSupplierBill.SelectedItem, OpenSupplierBillItem)
            UpdateBillDetails()
        End Sub

        Private Sub UpdateBillDetails()
            If _selectedBill IsNot Nothing Then
                lblOutstandingBalanceValue.Text = $"${_selectedBill.RemainingBalance:N2}"
                numDisbursedAmount.Maximum = _selectedBill.RemainingBalance
                numDisbursedAmount.Value = _selectedBill.RemainingBalance
                btnDisburse.Enabled = True
            Else
                lblOutstandingBalanceValue.Text = "$0.00"
                numDisbursedAmount.Maximum = 0D
                numDisbursedAmount.Value = 0D
                btnDisburse.Enabled = False
            End If
        End Sub

        Private Sub btnDisburse_Click(sender As Object, e As EventArgs) Handles btnDisburse.Click
            If _selectedBill Is Nothing Then
                MessageBox.Show("Please select an outstanding supplier bill to settle.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim amount As Decimal = numDisbursedAmount.Value
            If amount <= 0D Then
                MessageBox.Show("Disbursement amount must be greater than zero.", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim selectedBank = TryCast(cmbBankAccount.SelectedItem, Account)
            If selectedBank Is Nothing Then
                MessageBox.Show("Please select a funding bank account.", "Bank Account Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim paymentDate As DateTime = dtpPaymentDate.Value

            Try
                Dim voucher = _dataService.DisburseVendorPayment(_selectedBill.VendorId, _selectedBill.BillId, amount, selectedBank.AccountId, paymentDate)
                CreatedVoucher = voucher

                MessageBox.Show($"Vendor Payment Voucher disbursed successfully!{Environment.NewLine}" &
                                $"Voucher #: {voucher.VoucherNumber}{Environment.NewLine}" &
                                $"Settlement Amount: ${voucher.DisbursedAmount:N2}{Environment.NewLine}" &
                                $"General Ledger Posting:{Environment.NewLine}" &
                                $"  Debit  Account 2000 (Accounts Payable): ${voucher.DisbursedAmount:N2}{Environment.NewLine}" &
                                $"  Credit Account {selectedBank.Code} ({selectedBank.Name}): ${voucher.DisbursedAmount:N2}",
                                "Payment Disbursed & Posted", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As Exception
                MessageBox.Show($"Error disbursing payment: {ex.Message}", "Disbursement Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

    End Class

End Namespace
