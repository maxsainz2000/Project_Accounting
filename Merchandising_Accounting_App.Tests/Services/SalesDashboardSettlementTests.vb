Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Threading
Imports System.Windows.Forms
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Merchandising_Accounting_App.Data
Imports Merchandising_Accounting_App.Domain
Imports Merchandising_Accounting_App.Domain.Engines
Imports Merchandising_Accounting_App.Domain.Models
Imports Merchandising_Accounting_App.Services
Imports Merchandising_Accounting_App.Tests.Infrastructure
Imports Merchandising_Accounting_App.UI.Sales

Namespace Services

    <TestClass>
    Public Class SalesDashboardSettlementTests

        Private _fixture As TestDatabaseFixture

        <TestInitialize>
        Public Sub Initialize()
            _fixture = New TestDatabaseFixture()
        End Sub

        <TestCleanup>
        Public Sub Cleanup()
            _fixture?.Dispose()
        End Sub

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
        Public Sub SalesDashboardForm_ContainsRequiredSettlementControls()
            RunOnSta(Sub()
                Using form As New SalesDashboardForm()
                    form.CreateControl()

                    ' Directive 2 & Task 1 controls
                    Assert.IsNotNull(form.btnRecordPayment, "btnRecordPayment must exist")
                    Assert.IsNotNull(form.btnDisburseRefund, "btnDisburseRefund must exist")
                    Assert.IsNotNull(form.pnlCustomerActions, "pnlCustomerActions panel must exist")
                    Assert.IsNotNull(form.btnInvoiceRecordPayment, "btnInvoiceRecordPayment must exist on invoices tab")

                    ' Payment Receipt presentation dialog controls
                    Assert.IsNotNull(form.pnlRecordPaymentDialog, "pnlRecordPaymentDialog must exist")
                    Assert.IsNotNull(form.cmbPaymentCustomer, "cmbPaymentCustomer must exist")
                    Assert.IsNotNull(form.cmbPaymentInvoice, "cmbPaymentInvoice must exist")
                    Assert.IsNotNull(form.numPaymentAmount, "numPaymentAmount must exist")
                    Assert.IsNotNull(form.dtpPaymentDate, "dtpPaymentDate must exist")
                    Assert.IsNotNull(form.cmbPaymentMethod, "cmbPaymentMethod must exist")
                    Assert.IsNotNull(form.btnConfirmPaymentReceipt, "btnConfirmPaymentReceipt must exist")
                    Assert.IsNotNull(form.btnCancelPaymentReceipt, "btnCancelPaymentReceipt must exist")

                    ' Customer Refund presentation dialog controls
                    Assert.IsNotNull(form.pnlRefundDialog, "pnlRefundDialog must exist")
                    Assert.IsNotNull(form.numRefundAmount, "numRefundAmount must exist")
                    Assert.IsNotNull(form.dtpRefundDate, "dtpRefundDate must exist")
                    Assert.IsNotNull(form.cmbRefundMethod, "cmbRefundMethod must exist")
                    Assert.IsNotNull(form.btnConfirmRefund, "btnConfirmRefund must exist")
                    Assert.IsNotNull(form.btnCancelRefund, "btnCancelRefund must exist")

                    ' Verify dialogs are initially hidden
                    Assert.IsFalse(form.pnlRecordPaymentDialog.Visible, "Payment dialog must be initially hidden")
                    Assert.IsFalse(form.pnlRefundDialog.Visible, "Refund dialog must be initially hidden")
                End Using
            End Sub)
        End Sub

        <TestMethod>
        Public Sub CustomerBalancesPanel_RefundButtonState_UpdatesBasedOnArBalance()
            RunOnSta(Sub()
                Using context = _fixture.CreateContext()
                    ' Setup customer with positive AR balance
                    Dim custPositive As New Customer("CUST-POS-01", "Positive AR Customer", 5000.0D, 350.0D)
                    ' Setup customer with negative AR balance (credit from returns)
                    Dim custNegative As New Customer("CUST-NEG-01", "Negative Credit Customer", 5000.0D, -175.0D)
                    context.Customers.Add(custPositive)
                    context.Customers.Add(custNegative)
                    context.SaveChanges()

                    Dim o2cService As New OrderToCashService(context)
                    Dim returnsService As New ReturnsService(context)
                    Dim dataService As New SalesDataService(o2cService, context)

                    Using form As New SalesDashboardForm(dataService, o2cService, returnsService)
                        form.Show()

                        ' Select positive AR customer
                        Dim itemPos = form.cmbCustomer.Items.Cast(Of CustomerViewModel)().FirstOrDefault(Function(c) c.CustomerId = custPositive.CustomerId)
                        form.cmbCustomer.SelectedItem = itemPos
                        Assert.IsFalse(form.btnDisburseRefund.Enabled, "Disburse Refund button must be disabled for positive AR balance")
                        Assert.IsTrue(form.btnRecordPayment.Enabled, "Record Payment button must be enabled")

                        ' Select negative AR customer (credit balance)
                        Dim itemNeg = form.cmbCustomer.Items.Cast(Of CustomerViewModel)().FirstOrDefault(Function(c) c.CustomerId = custNegative.CustomerId)
                        form.cmbCustomer.SelectedItem = itemNeg
                        Assert.IsTrue(form.btnDisburseRefund.Enabled, "Disburse Refund button must be enabled for negative AR credit balance")
                        StringAssert.Contains(form.btnDisburseRefund.Text, "175", "Disburse Refund text should reflect the credit balance")
                        Assert.AreEqual("CREDIT BALANCE", form.lblCustomerBadge.Text)
                    End Using
                End Using
            End Sub)
        End Sub

        <TestMethod>
        Public Sub RecordPaymentDialog_OpensAndPopulatesUnpaidInvoices()
            RunOnSta(Sub()
                Using context = _fixture.CreateContext()
                    Dim customer As New Customer("CUST-PAY-01", "Payment Test Customer", 10000.0D, 0D)
                    Dim product As New Product("SKU-PAY-01", "Payment Widget", "Each", 10.0D, 100)
                    context.Customers.Add(customer)
                    context.Products.Add(product)
                    context.SaveChanges()

                    Dim o2cService As New OrderToCashService(context)
                    Dim returnsService As New ReturnsService(context)

                    ' Fulfill an invoice for $250.00
                    Dim postingDate As New DateTime(2026, 11, 15)
                    Dim invoice = o2cService.FulfillSalesInvoice(0, New List(Of SalesInvoiceLine) From {
                        New SalesInvoiceLine() With {.ProductId = product.ProductId, .FulfilledQty = 10, .UnitPrice = 25.0D}
                    }, postingDate, customer.CustomerId)

                    Dim dataService As New SalesDataService(o2cService, context)

                    Using form As New SalesDashboardForm(dataService, o2cService, returnsService)
                        form.Show()

                        ' Open record payment dialog for this customer
                        form.OpenRecordPaymentDialog(customer.CustomerId, invoice.InvoiceId)

                        Assert.IsTrue(form.pnlRecordPaymentDialog.Visible, "Payment dialog must be visible")
                        Assert.AreEqual(customer.CustomerId, CType(form.cmbPaymentCustomer.SelectedValue, Integer))

                        ' Verify amount was auto-populated from invoice
                        Assert.AreEqual(250.0D, form.numPaymentAmount.Value)

                        ' Close dialog
                        form.CloseRecordPaymentDialog()
                        Assert.IsFalse(form.pnlRecordPaymentDialog.Visible, "Payment dialog must be hidden after close")
                    End Using
                End Using
            End Sub)
        End Sub

        <TestMethod>
        Public Sub RecordPaymentReceipt_FullSettlement_PostsGLAndSettlesInvoice()
            Using context = _fixture.CreateContext()
                Dim customer As New Customer("CUST-SETTLE-01", "Full Settlement Customer", 10000.0D, 0D)
                Dim product As New Product("SKU-SETTLE-01", "Settle Widget", "Each", 15.0D, 50)
                context.Customers.Add(customer)
                context.Products.Add(product)
                context.SaveChanges()

                Dim o2cService As New OrderToCashService(context)
                Dim postingDate As New DateTime(2026, 11, 15)

                ' Fulfill invoice: 10 units @ $40.00 = $400.00
                Dim invoice = o2cService.FulfillSalesInvoice(0, New List(Of SalesInvoiceLine) From {
                    New SalesInvoiceLine() With {.ProductId = product.ProductId, .FulfilledQty = 10, .UnitPrice = 40.0D}
                }, postingDate, customer.CustomerId)

                Assert.AreEqual(400.0D, context.Customers.Find(customer.CustomerId).CurrentARBalance)
                Assert.AreEqual("Posted", invoice.Status)

                ' Record full payment receipt of $400.00 via OrderToCashService
                Dim receipt = o2cService.RecordPaymentReceipt(customer.CustomerId, invoice.InvoiceId, 400.0D, 0, postingDate, postingDate)

                Assert.IsNotNull(receipt)
                Assert.AreEqual(400.0D, receipt.AmountReceived)

                ' Customer AR balance cleared to $0.00
                Dim updatedCustomer = context.Customers.Find(customer.CustomerId)
                Assert.AreEqual(0D, updatedCustomer.CurrentARBalance)

                ' Invoice status updated to Settled
                Dim updatedInvoice = context.SalesInvoices.Find(invoice.InvoiceId)
                Assert.AreEqual("Settled", updatedInvoice.Status)

                ' Verify Double-Entry Journal Entry: Debit 1020 Operating Bank $400, Credit 1200 AR $400
                Dim journal = context.JournalEntries.FirstOrDefault(Function(j) j.SourceDocId = receipt.ReceiptId AndAlso j.SourceDocType = "PaymentReceipt")
                Assert.IsNotNull(journal)
                Assert.AreEqual(2, journal.Lines.Count)

                Dim bankLine = journal.Lines.First(Function(l) l.Account.Code = "1020")
                Assert.AreEqual(400.0D, bankLine.DebitAmount)
                Assert.AreEqual(0D, bankLine.CreditAmount)

                Dim arLine = journal.Lines.First(Function(l) l.Account.Code = "1200")
                Assert.AreEqual(0D, arLine.DebitAmount)
                Assert.AreEqual(400.0D, arLine.CreditAmount)

                Assert.IsTrue(LedgerPostingEngine.IsBalanced(journal.Lines))
            End Using
        End Sub

        <TestMethod>
        Public Sub DisburseCustomerRefund_ClearsNegativeAR_AndPostsGLVoucher()
            Using context = _fixture.CreateContext()
                Dim returnsService As New ReturnsService(context)

                ' Customer with negative AR (credit balance of $300.00 from RMA credit note)
                Dim customer As New Customer("CUST-REF-WORKFLOW", "Refund Workflow Customer", 5000.0D, -300.0D)
                context.Customers.Add(customer)
                context.SaveChanges()

                Dim postingDate As New DateTime(2026, 11, 15)

                ' Disburse customer refund of $300.00
                Dim voucher = returnsService.DisburseCustomerRefund(customer.CustomerId, Nothing, 300.0D, 0, postingDate, postingDate, "VOUCH-REF-001")

                Assert.IsNotNull(voucher)
                Assert.AreEqual("CustomerRefund", voucher.VoucherType)
                Assert.AreEqual(300.0D, voucher.DisbursedAmount)

                ' Customer AR balance incremented from -300 back to 0
                Dim reloadedCustomer = context.Customers.Find(customer.CustomerId)
                Assert.AreEqual(0D, reloadedCustomer.CurrentARBalance)

                ' Verify Double-Entry Journal: Debit 1200 AR $300, Credit 1020 Operating Bank $300
                Dim journal = context.JournalEntries.FirstOrDefault(Function(j) j.SourceDocId = voucher.VoucherId AndAlso j.SourceDocType = "PaymentVoucher")
                Assert.IsNotNull(journal)
                Assert.AreEqual(2, journal.Lines.Count)

                Dim arLine = journal.Lines.First(Function(l) l.Account.Code = "1200")
                Assert.AreEqual(300.0D, arLine.DebitAmount)
                Assert.AreEqual(0D, arLine.CreditAmount)

                Dim bankLine = journal.Lines.First(Function(l) l.Account.Code = "1020")
                Assert.AreEqual(0D, bankLine.DebitAmount)
                Assert.AreEqual(300.0D, bankLine.CreditAmount)

                Assert.IsTrue(LedgerPostingEngine.IsBalanced(journal.Lines))
            End Using
        End Sub

    End Class

End Namespace
