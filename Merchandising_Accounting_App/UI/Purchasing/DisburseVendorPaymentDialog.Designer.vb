Option Strict On
Option Explicit On
Option Compare Binary

Namespace UI.Purchasing

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class DisburseVendorPaymentDialog
        Inherits System.Windows.Forms.Form

        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        Private components As System.ComponentModel.IContainer

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.pnlHeader = New System.Windows.Forms.Panel()
            Me.lblHeaderSubtitle = New System.Windows.Forms.Label()
            Me.lblHeaderTitle = New System.Windows.Forms.Label()
            Me.pnlFooter = New System.Windows.Forms.Panel()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.btnDisburse = New System.Windows.Forms.Button()
            Me.pnlBody = New System.Windows.Forms.Panel()
            Me.txtVoucherNumber = New System.Windows.Forms.TextBox()
            Me.lblVoucherNumber = New System.Windows.Forms.Label()
            Me.dtpPaymentDate = New System.Windows.Forms.DateTimePicker()
            Me.lblPaymentDate = New System.Windows.Forms.Label()
            Me.cmbBankAccount = New System.Windows.Forms.ComboBox()
            Me.lblBankAccount = New System.Windows.Forms.Label()
            Me.numDisbursedAmount = New System.Windows.Forms.NumericUpDown()
            Me.lblDisbursedAmount = New System.Windows.Forms.Label()
            Me.lblOutstandingBalanceValue = New System.Windows.Forms.Label()
            Me.lblOutstandingBalance = New System.Windows.Forms.Label()
            Me.cmbSupplierBill = New System.Windows.Forms.ComboBox()
            Me.lblSupplierBill = New System.Windows.Forms.Label()
            Me.cmbVendor = New System.Windows.Forms.ComboBox()
            Me.lblVendor = New System.Windows.Forms.Label()
            Me.pnlHeader.SuspendLayout()
            Me.pnlFooter.SuspendLayout()
            Me.pnlBody.SuspendLayout()
            CType(Me.numDisbursedAmount, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'pnlHeader
            '
            Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.pnlHeader.Controls.Add(Me.lblHeaderSubtitle)
            Me.pnlHeader.Controls.Add(Me.lblHeaderTitle)
            Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
            Me.pnlHeader.Name = "pnlHeader"
            Me.pnlHeader.Padding = New System.Windows.Forms.Padding(16, 12, 16, 12)
            Me.pnlHeader.Size = New System.Drawing.Size(564, 64)
            Me.pnlHeader.TabIndex = 0
            '
            'lblHeaderSubtitle
            '
            Me.lblHeaderSubtitle.AutoSize = True
            Me.lblHeaderSubtitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblHeaderSubtitle.Location = New System.Drawing.Point(16, 34)
            Me.lblHeaderSubtitle.Name = "lblHeaderSubtitle"
            Me.lblHeaderSubtitle.Size = New System.Drawing.Size(364, 19)
            Me.lblHeaderSubtitle.TabIndex = 1
            Me.lblHeaderSubtitle.Text = "Accounts Payable Settlement (Dr 2000 AP / Cr 1020 Bank)"
            '
            'lblHeaderTitle
            '
            Me.lblHeaderTitle.AutoSize = True
            Me.lblHeaderTitle.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblHeaderTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblHeaderTitle.Location = New System.Drawing.Point(16, 10)
            Me.lblHeaderTitle.Name = "lblHeaderTitle"
            Me.lblHeaderTitle.Size = New System.Drawing.Size(325, 25)
            Me.lblHeaderTitle.TabIndex = 0
            Me.lblHeaderTitle.Text = "Disburse Vendor Payment Voucher"
            '
            'pnlFooter
            '
            Me.pnlFooter.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlFooter.Controls.Add(Me.btnCancel)
            Me.pnlFooter.Controls.Add(Me.btnDisburse)
            Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlFooter.Location = New System.Drawing.Point(0, 368)
            Me.pnlFooter.Name = "pnlFooter"
            Me.pnlFooter.Padding = New System.Windows.Forms.Padding(16, 10, 16, 10)
            Me.pnlFooter.Size = New System.Drawing.Size(564, 52)
            Me.pnlFooter.TabIndex = 2
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnCancel.Location = New System.Drawing.Point(448, 10)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(100, 32)
            Me.btnCancel.TabIndex = 1
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'btnDisburse
            '
            Me.btnDisburse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnDisburse.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.btnDisburse.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnDisburse.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnDisburse.ForeColor = System.Drawing.Color.White
            Me.btnDisburse.Location = New System.Drawing.Point(288, 10)
            Me.btnDisburse.Name = "btnDisburse"
            Me.btnDisburse.Size = New System.Drawing.Size(150, 32)
            Me.btnDisburse.TabIndex = 0
            Me.btnDisburse.Text = "Disburse Payment"
            Me.btnDisburse.UseVisualStyleBackColor = False
            '
            'pnlBody
            '
            Me.pnlBody.BackColor = System.Drawing.Color.White
            Me.pnlBody.Controls.Add(Me.txtVoucherNumber)
            Me.pnlBody.Controls.Add(Me.lblVoucherNumber)
            Me.pnlBody.Controls.Add(Me.dtpPaymentDate)
            Me.pnlBody.Controls.Add(Me.lblPaymentDate)
            Me.pnlBody.Controls.Add(Me.cmbBankAccount)
            Me.pnlBody.Controls.Add(Me.lblBankAccount)
            Me.pnlBody.Controls.Add(Me.numDisbursedAmount)
            Me.pnlBody.Controls.Add(Me.lblDisbursedAmount)
            Me.pnlBody.Controls.Add(Me.lblOutstandingBalanceValue)
            Me.pnlBody.Controls.Add(Me.lblOutstandingBalance)
            Me.pnlBody.Controls.Add(Me.cmbSupplierBill)
            Me.pnlBody.Controls.Add(Me.lblSupplierBill)
            Me.pnlBody.Controls.Add(Me.cmbVendor)
            Me.pnlBody.Controls.Add(Me.lblVendor)
            Me.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlBody.Location = New System.Drawing.Point(0, 64)
            Me.pnlBody.Name = "pnlBody"
            Me.pnlBody.Padding = New System.Windows.Forms.Padding(20, 16, 20, 16)
            Me.pnlBody.Size = New System.Drawing.Size(564, 304)
            Me.pnlBody.TabIndex = 1
            '
            'txtVoucherNumber
            '
            Me.txtVoucherNumber.Location = New System.Drawing.Point(292, 256)
            Me.txtVoucherNumber.Name = "txtVoucherNumber"
            Me.txtVoucherNumber.Size = New System.Drawing.Size(248, 27)
            Me.txtVoucherNumber.TabIndex = 13
            '
            'lblVoucherNumber
            '
            Me.lblVoucherNumber.AutoSize = True
            Me.lblVoucherNumber.Location = New System.Drawing.Point(292, 234)
            Me.lblVoucherNumber.Name = "lblVoucherNumber"
            Me.lblVoucherNumber.Size = New System.Drawing.Size(189, 19)
            Me.lblVoucherNumber.TabIndex = 12
            Me.lblVoucherNumber.Text = "Voucher # (auto if blank):"
            '
            'dtpPaymentDate
            '
            Me.dtpPaymentDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.dtpPaymentDate.Location = New System.Drawing.Point(24, 256)
            Me.dtpPaymentDate.Name = "dtpPaymentDate"
            Me.dtpPaymentDate.Size = New System.Drawing.Size(248, 27)
            Me.dtpPaymentDate.TabIndex = 11
            '
            'lblPaymentDate
            '
            Me.lblPaymentDate.AutoSize = True
            Me.lblPaymentDate.Location = New System.Drawing.Point(24, 234)
            Me.lblPaymentDate.Name = "lblPaymentDate"
            Me.lblPaymentDate.Size = New System.Drawing.Size(102, 19)
            Me.lblPaymentDate.TabIndex = 10
            Me.lblPaymentDate.Text = "Payment Date:"
            '
            'cmbBankAccount
            '
            Me.cmbBankAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbBankAccount.FormattingEnabled = True
            Me.cmbBankAccount.Location = New System.Drawing.Point(292, 192)
            Me.cmbBankAccount.Name = "cmbBankAccount"
            Me.cmbBankAccount.Size = New System.Drawing.Size(248, 28)
            Me.cmbBankAccount.TabIndex = 9
            '
            'lblBankAccount
            '
            Me.lblBankAccount.AutoSize = True
            Me.lblBankAccount.Location = New System.Drawing.Point(292, 170)
            Me.lblBankAccount.Name = "lblBankAccount"
            Me.lblBankAccount.Size = New System.Drawing.Size(155, 19)
            Me.lblBankAccount.TabIndex = 8
            Me.lblBankAccount.Text = "Funding Bank Account:"
            '
            'numDisbursedAmount
            '
            Me.numDisbursedAmount.DecimalPlaces = 2
            Me.numDisbursedAmount.Location = New System.Drawing.Point(24, 192)
            Me.numDisbursedAmount.Maximum = New Decimal(New Integer() {100000000, 0, 0, 0})
            Me.numDisbursedAmount.Name = "numDisbursedAmount"
            Me.numDisbursedAmount.Size = New System.Drawing.Size(248, 27)
            Me.numDisbursedAmount.TabIndex = 7
            '
            'lblDisbursedAmount
            '
            Me.lblDisbursedAmount.AutoSize = True
            Me.lblDisbursedAmount.Location = New System.Drawing.Point(24, 170)
            Me.lblDisbursedAmount.Name = "lblDisbursedAmount"
            Me.lblDisbursedAmount.Size = New System.Drawing.Size(168, 19)
            Me.lblDisbursedAmount.TabIndex = 6
            Me.lblDisbursedAmount.Text = "Disbursement Amount ($):"
            '
            'lblOutstandingBalanceValue
            '
            Me.lblOutstandingBalanceValue.AutoSize = True
            Me.lblOutstandingBalanceValue.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblOutstandingBalanceValue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
            Me.lblOutstandingBalanceValue.Location = New System.Drawing.Point(180, 134)
            Me.lblOutstandingBalanceValue.Name = "lblOutstandingBalanceValue"
            Me.lblOutstandingBalanceValue.Size = New System.Drawing.Size(51, 21)
            Me.lblOutstandingBalanceValue.TabIndex = 5
            Me.lblOutstandingBalanceValue.Text = "$0.00"
            '
            'lblOutstandingBalance
            '
            Me.lblOutstandingBalance.AutoSize = True
            Me.lblOutstandingBalance.Location = New System.Drawing.Point(24, 134)
            Me.lblOutstandingBalance.Name = "lblOutstandingBalance"
            Me.lblOutstandingBalance.Size = New System.Drawing.Size(150, 19)
            Me.lblOutstandingBalance.TabIndex = 4
            Me.lblOutstandingBalance.Text = "Outstanding Balance:"
            '
            'cmbSupplierBill
            '
            Me.cmbSupplierBill.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbSupplierBill.FormattingEnabled = True
            Me.cmbSupplierBill.Location = New System.Drawing.Point(24, 94)
            Me.cmbSupplierBill.Name = "cmbSupplierBill"
            Me.cmbSupplierBill.Size = New System.Drawing.Size(516, 28)
            Me.cmbSupplierBill.TabIndex = 3
            '
            'lblSupplierBill
            '
            Me.lblSupplierBill.AutoSize = True
            Me.lblSupplierBill.Location = New System.Drawing.Point(24, 72)
            Me.lblSupplierBill.Name = "lblSupplierBill"
            Me.lblSupplierBill.Size = New System.Drawing.Size(176, 19)
            Me.lblSupplierBill.TabIndex = 2
            Me.lblSupplierBill.Text = "Target Supplier Bill / Inv #:"
            '
            'cmbVendor
            '
            Me.cmbVendor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbVendor.FormattingEnabled = True
            Me.cmbVendor.Location = New System.Drawing.Point(24, 34)
            Me.cmbVendor.Name = "cmbVendor"
            Me.cmbVendor.Size = New System.Drawing.Size(516, 28)
            Me.cmbVendor.TabIndex = 1
            '
            'lblVendor
            '
            Me.lblVendor.AutoSize = True
            Me.lblVendor.Location = New System.Drawing.Point(24, 12)
            Me.lblVendor.Name = "lblVendor"
            Me.lblVendor.Size = New System.Drawing.Size(58, 19)
            Me.lblVendor.TabIndex = 0
            Me.lblVendor.Text = "Vendor:"
            '
            'DisburseVendorPaymentDialog
            '
            Me.AcceptButton = Me.btnDisburse
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(564, 420)
            Me.Controls.Add(Me.pnlBody)
            Me.Controls.Add(Me.pnlFooter)
            Me.Controls.Add(Me.pnlHeader)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "DisburseVendorPaymentDialog"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Disburse Vendor Payment"
            Me.pnlHeader.ResumeLayout(False)
            Me.pnlHeader.PerformLayout()
            Me.pnlFooter.ResumeLayout(False)
            Me.pnlBody.ResumeLayout(False)
            Me.pnlBody.PerformLayout()
            CType(Me.numDisbursedAmount, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents pnlHeader As System.Windows.Forms.Panel
        Friend WithEvents lblHeaderTitle As System.Windows.Forms.Label
        Friend WithEvents lblHeaderSubtitle As System.Windows.Forms.Label
        Friend WithEvents pnlFooter As System.Windows.Forms.Panel
        Friend WithEvents btnDisburse As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents pnlBody As System.Windows.Forms.Panel
        Friend WithEvents lblVendor As System.Windows.Forms.Label
        Friend WithEvents cmbVendor As System.Windows.Forms.ComboBox
        Friend WithEvents lblSupplierBill As System.Windows.Forms.Label
        Friend WithEvents cmbSupplierBill As System.Windows.Forms.ComboBox
        Friend WithEvents lblOutstandingBalance As System.Windows.Forms.Label
        Friend WithEvents lblOutstandingBalanceValue As System.Windows.Forms.Label
        Friend WithEvents lblDisbursedAmount As System.Windows.Forms.Label
        Friend WithEvents numDisbursedAmount As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblBankAccount As System.Windows.Forms.Label
        Friend WithEvents cmbBankAccount As System.Windows.Forms.ComboBox
        Friend WithEvents lblPaymentDate As System.Windows.Forms.Label
        Friend WithEvents dtpPaymentDate As System.Windows.Forms.DateTimePicker
        Friend WithEvents lblVoucherNumber As System.Windows.Forms.Label
        Friend WithEvents txtVoucherNumber As System.Windows.Forms.TextBox

    End Class

End Namespace
