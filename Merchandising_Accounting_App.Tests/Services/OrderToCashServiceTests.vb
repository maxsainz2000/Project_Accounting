Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Merchandising_Accounting_App.Data
Imports Merchandising_Accounting_App.Domain
Imports Merchandising_Accounting_App.Domain.Engines
Imports Merchandising_Accounting_App.Domain.Models
Imports Merchandising_Accounting_App.Services
Imports Merchandising_Accounting_App.Tests.Infrastructure

Namespace Services

    <TestClass>
    Public Class OrderToCashServiceTests

        Private _fixture As TestDatabaseFixture

        <TestInitialize>
        Public Sub Initialize()
            _fixture = New TestDatabaseFixture()
        End Sub

        <TestCleanup>
        Public Sub Cleanup()
            _fixture?.Dispose()
        End Sub

        <TestMethod>
        Public Sub Constructor_Parameterless_InitializesCorrectly()
            Dim service As New OrderToCashService()
            Assert.IsNotNull(service)
        End Sub

        ' =========================================================================
        ' CreateSalesOrder Tests
        ' =========================================================================

        <TestMethod>
        Public Sub CreateSalesOrder_ValidLines_CreatesOrderAndPersists()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)

                Dim customer As New Customer("CUST001", "ACME Corp", 5000.0D, 0D)
                context.Customers.Add(customer)
                Dim product As New Product("SKU-100", "Widget A", "Each", 10.0D, 50)
                context.Products.Add(product)
                context.SaveChanges()

                Dim lines As New List(Of SOLine) From {
                    New SOLine(product.ProductId, 10, 25.0D)
                }

                Dim so = service.CreateSalesOrder(customer.CustomerId, lines, New DateTime(2026, 11, 10), "SO-TEST-001")

                Assert.IsNotNull(so)
                Assert.AreEqual("SO-TEST-001", so.DocumentNumber)
                Assert.AreEqual(customer.CustomerId, so.CustomerId)
                Assert.AreEqual("Draft", so.Status)
                Assert.AreEqual(1, so.Lines.Count)
                Assert.AreEqual(10, so.Lines.First().OrderedQty)
                Assert.AreEqual(25.0D, so.Lines.First().UnitPrice)

                ' Verify saved in context
                Dim savedSO = context.SalesOrders.Find(so.SOId)
                Assert.IsNotNull(savedSO)
            End Using
        End Sub

        <TestMethod>
        Public Sub CreateSalesOrder_NullLines_ThrowsArgumentNullException()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)
                Assert.Throws(Of ArgumentNullException)(Sub()
                    service.CreateSalesOrder(1, Nothing)
                End Sub)
            End Using
        End Sub

        <TestMethod>
        Public Sub CreateSalesOrder_EmptyLines_ThrowsArgumentException()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)
                Assert.Throws(Of ArgumentException)(Sub()
                    service.CreateSalesOrder(1, New List(Of SOLine)())
                End Sub)
            End Using
        End Sub

        <TestMethod>
        Public Sub CreateSalesOrder_InvalidCustomerId_ThrowsArgumentException()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)
                Dim lines As New List(Of SOLine) From {New SOLine(1, 5, 20.0D)}
                Assert.Throws(Of ArgumentException)(Sub()
                    service.CreateSalesOrder(0, lines)
                End Sub)
            End Using
        End Sub

        <TestMethod>
        Public Sub CreateSalesOrder_CustomerNotFound_ThrowsArgumentException()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)
                Dim lines As New List(Of SOLine) From {New SOLine(1, 5, 20.0D)}
                Assert.Throws(Of ArgumentException)(Sub()
                    service.CreateSalesOrder(9999, lines)
                End Sub)
            End Using
        End Sub

        <TestMethod>
        Public Sub CreateSalesOrder_InvalidLineProperties_ThrowsExceptions()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)
                Dim customer As New Customer("CUST002", "Beta Ltd", 2000.0D, 0D)
                context.Customers.Add(customer)
                context.SaveChanges()

                ' Zero/Negative ProductId
                Assert.Throws(Of ArgumentException)(Sub()
                    service.CreateSalesOrder(customer.CustomerId, New List(Of SOLine) From {New SOLine(0, 5, 10.0D)})
                End Sub)

                ' Zero/Negative OrderedQty
                Assert.Throws(Of ArgumentOutOfRangeException)(Sub()
                    service.CreateSalesOrder(customer.CustomerId, New List(Of SOLine) From {New SOLine(1, 0, 10.0D)})
                End Sub)

                ' Negative UnitPrice
                Assert.Throws(Of ArgumentOutOfRangeException)(Sub()
                    service.CreateSalesOrder(customer.CustomerId, New List(Of SOLine) From {New SOLine(1, 5, -1.0D)})
                End Sub)

                ' Non-existent product
                Assert.Throws(Of ArgumentException)(Sub()
                    service.CreateSalesOrder(customer.CustomerId, New List(Of SOLine) From {New SOLine(9999, 5, 10.0D)})
                End Sub)
            End Using
        End Sub

        ' =========================================================================
        ' FulfillSalesInvoice Tests
        ' =========================================================================

        <TestMethod>
        Public Sub FulfillSalesInvoice_SufficientStock_FulfillsDecrementsStockAndPostsBalancedJournal()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)

                Dim customer As New Customer("CUST003", "Gamma Inc", 10000.0D, 0D)
                context.Customers.Add(customer)
                Dim product As New Product("SKU-200", "Widget B", "Each", 12.50D, 50)
                context.Products.Add(product)
                context.SaveChanges()

                ' Create Sales Order
                Dim lines As New List(Of SOLine) From {New SOLine(product.ProductId, 20, 30.0D)}
                Dim so = service.CreateSalesOrder(customer.CustomerId, lines)

                Dim postingDate As New DateTime(2026, 11, 15)
                Dim invoice = service.FulfillSalesInvoice(so.SOId, postingDate)

                Assert.IsNotNull(invoice)
                Assert.AreEqual("Posted", invoice.Status)
                Assert.AreEqual(600.0D, invoice.TotalAmount) ' 20 * $30.00
                Assert.AreEqual(1, invoice.Lines.Count)

                Dim invLine = invoice.Lines.First()
                Assert.AreEqual(20, invLine.FulfilledQty)
                Assert.AreEqual(30.0D, invLine.UnitPrice)
                Assert.AreEqual(12.50D, invLine.InvoicedCOGSUnitCost)

                ' Verify physical stock decremented atomically: 50 - 20 = 30
                Dim updatedProduct = context.Products.Find(product.ProductId)
                Assert.AreEqual(30, updatedProduct.TotalQtyOnHand)

                ' Verify InventoryTransaction audit entry
                Dim invTx = context.InventoryTransactions.FirstOrDefault(Function(t) t.SourceDocId = invoice.InvoiceId AndAlso t.SourceDocType = "SalesInvoice")
                Assert.IsNotNull(invTx)
                Assert.AreEqual(-20, invTx.QtyChange)
                Assert.AreEqual(30, invTx.RunningQty)
                Assert.AreEqual(12.50D, invTx.UnitCost)

                ' Verify balanced General Ledger entry
                Dim journalEntry = context.JournalEntries.FirstOrDefault(Function(j) j.SourceDocId = invoice.InvoiceId AndAlso j.SourceDocType = "SalesInvoice")
                Assert.IsNotNull(journalEntry)
                Assert.AreEqual(4, journalEntry.Lines.Count)

                ' 1200 AR: Debit 600.00
                Dim arLine = journalEntry.Lines.First(Function(l) l.Account.Code = "1200")
                Assert.AreEqual(600.0D, arLine.DebitAmount)
                Assert.AreEqual(0D, arLine.CreditAmount)

                ' 4000 Sales Revenue: Credit 600.00
                Dim revLine = journalEntry.Lines.First(Function(l) l.Account.Code = "4000")
                Assert.AreEqual(0D, revLine.DebitAmount)
                Assert.AreEqual(600.0D, revLine.CreditAmount)

                ' 5000 COGS: Debit 250.00 (20 * $12.50)
                Dim cogsLine = journalEntry.Lines.First(Function(l) l.Account.Code = "5000")
                Assert.AreEqual(250.0D, cogsLine.DebitAmount)
                Assert.AreEqual(0D, cogsLine.CreditAmount)

                ' 1300 Inventory: Credit 250.00
                Dim invAcctLine = journalEntry.Lines.First(Function(l) l.Account.Code = "1300")
                Assert.AreEqual(0D, invAcctLine.DebitAmount)
                Assert.AreEqual(250.0D, invAcctLine.CreditAmount)

                ' Assert double-entry balance: 850.00 == 850.00
                Assert.IsTrue(LedgerPostingEngine.IsBalanced(journalEntry.Lines))

                ' Verify customer AR balance incremented
                Dim updatedCustomer = context.Customers.Find(customer.CustomerId)
                Assert.AreEqual(600.0D, updatedCustomer.CurrentARBalance)

                ' Verify sales order status settled
                Dim updatedSO = context.SalesOrders.Find(so.SOId)
                Assert.AreEqual("Settled", updatedSO.Status)
            End Using
        End Sub

        <TestMethod>
        Public Sub FulfillSalesInvoice_InsufficientStock_ThrowsInsufficientStockException_DoesNotMutateStock()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)

                Dim customer As New Customer("CUST004", "Delta Co", 10000.0D, 0D)
                context.Customers.Add(customer)
                Dim product As New Product("SKU-300", "Widget C", "Each", 10.0D, 15) ' Only 15 on hand
                context.Products.Add(product)
                context.SaveChanges()

                Dim invoiceLines As New List(Of SalesInvoiceLine) From {
                    New SalesInvoiceLine() With {.ProductId = product.ProductId, .FulfilledQty = 20, .UnitPrice = 25.0D}
                }

                Dim postingDate As New DateTime(2026, 11, 15)
                Dim caughtEx As InsufficientStockException = Nothing

                Try
                    service.FulfillSalesInvoice(0, invoiceLines, postingDate, customer.CustomerId)
                Catch ex As InsufficientStockException
                    caughtEx = ex
                End Try

                Assert.IsNotNull(caughtEx, "Expected InsufficientStockException was not thrown.")
                Assert.AreEqual(product.ProductId, caughtEx.ProductId.Value)
                Assert.AreEqual(20, caughtEx.RequestedQty.Value)
                Assert.AreEqual(15, caughtEx.AvailableQty.Value)

                ' Verify on-hand inventory was NOT decremented
                Dim reloadedProduct = context.Products.Find(product.ProductId)
                Assert.AreEqual(15, reloadedProduct.TotalQtyOnHand)

                ' Verify no invoices, no transactions, no journal entries were created
                Assert.AreEqual(0, context.SalesInvoices.Count())
                Assert.AreEqual(0, context.InventoryTransactions.Count())
                Assert.AreEqual(0, context.JournalEntries.Count())
            End Using
        End Sub

        <TestMethod>
        Public Sub FulfillSalesInvoice_ClosedPeriod_ThrowsClosedPeriodPostingException()
            Using context = _fixture.CreateContext()
                Dim periodService As New PeriodService(context)
                Dim service As New OrderToCashService(context, periodService)

                Dim customer As New Customer("CUST005", "Epsilon Enterprises", 10000.0D, 0D)
                context.Customers.Add(customer)
                Dim product As New Product("SKU-400", "Widget D", "Each", 10.0D, 50)
                context.Products.Add(product)
                context.SaveChanges()

                ' Close period 11
                Dim activePeriod = periodService.GetActiveFiscalPeriod()
                periodService.CloseFiscalPeriod(activePeriod.PeriodId)

                Dim invoiceLines As New List(Of SalesInvoiceLine) From {
                    New SalesInvoiceLine() With {.ProductId = product.ProductId, .FulfilledQty = 10, .UnitPrice = 20.0D}
                }

                Dim postingDate As New DateTime(2026, 11, 15)
                Assert.Throws(Of ClosedPeriodPostingException)(Sub()
                    service.FulfillSalesInvoice(0, invoiceLines, postingDate, customer.CustomerId)
                End Sub)

                ' Verify stock unchanged
                Dim reloadedProduct = context.Products.Find(product.ProductId)
                Assert.AreEqual(50, reloadedProduct.TotalQtyOnHand)
            End Using
        End Sub

        <TestMethod>
        Public Sub FulfillSalesInvoice_InvalidParameters_ThrowsAppropriateExceptions()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)

                ' Null lines
                Assert.Throws(Of ArgumentNullException)(Sub()
                    service.FulfillSalesInvoice(1, Nothing, DateTime.Today)
                End Sub)

                ' Empty lines
                Assert.Throws(Of ArgumentException)(Sub()
                    service.FulfillSalesInvoice(1, New List(Of SalesInvoiceLine)(), DateTime.Today)
                End Sub)

                ' Invalid SOId
                Dim validLine As New List(Of SalesInvoiceLine) From {New SalesInvoiceLine() With {.ProductId = 1, .FulfilledQty = 1, .UnitPrice = 10.0D}}
                Assert.Throws(Of ArgumentException)(Sub()
                    service.FulfillSalesInvoice(9999, validLine, DateTime.Today)
                End Sub)

                ' No Customer specified with SOId = 0
                Assert.Throws(Of ArgumentException)(Sub()
                    service.FulfillSalesInvoice(0, validLine, DateTime.Today, customerId:=0)
                End Sub)

                ' Customer not found
                Assert.Throws(Of ArgumentException)(Sub()
                    service.FulfillSalesInvoice(0, validLine, DateTime.Today, customerId:=9999)
                End Sub)

                ' SO has no lines
                Dim customer As New Customer("CUST006", "Zeta Corp", 5000.0D, 0D)
                context.Customers.Add(customer)
                context.SaveChanges()

                Dim emptySO As New SalesOrder() With {.CustomerId = customer.CustomerId, .DocumentNumber = "SO-EMPTY", .OrderDate = DateTime.Today}
                context.SalesOrders.Add(emptySO)
                context.SaveChanges()

                Assert.Throws(Of InvalidOperationException)(Sub()
                    service.FulfillSalesInvoice(emptySO.SOId, New DateTime(2026, 11, 15))
                End Sub)
            End Using
        End Sub

        ' =========================================================================
        ' RecordPaymentReceipt Tests
        ' =========================================================================

        <TestMethod>
        Public Sub RecordPaymentReceipt_FullInvoicePayment_ClearsARAndSettlesInvoice()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)

                Dim customer As New Customer("CUST007", "Theta Corp", 10000.0D, 0D)
                context.Customers.Add(customer)
                Dim product As New Product("SKU-500", "Widget E", "Each", 15.0D, 100)
                context.Products.Add(product)
                context.SaveChanges()

                Dim invoiceLines As New List(Of SalesInvoiceLine) From {
                    New SalesInvoiceLine() With {.ProductId = product.ProductId, .FulfilledQty = 10, .UnitPrice = 50.0D}
                }
                Dim postingDate As New DateTime(2026, 11, 15)
                Dim invoice = service.FulfillSalesInvoice(0, invoiceLines, postingDate, customer.CustomerId)

                Assert.AreEqual(500.0D, context.Customers.Find(customer.CustomerId).CurrentARBalance)
                Assert.AreEqual("Posted", invoice.Status)

                ' Record full payment of $500
                Dim bankAccount = context.Accounts.First(Function(a) a.Code = "1020")
                Dim receipt = service.RecordPaymentReceipt(customer.CustomerId, invoice.InvoiceId, 500.0D, bankAccount.AccountId, postingDate, postingDate)

                Assert.IsNotNull(receipt)
                Assert.AreEqual(500.0D, receipt.AmountReceived)
                Assert.AreEqual(invoice.InvoiceId, receipt.InvoiceId.Value)

                ' Verify customer AR balance reduced to 0
                Dim updatedCustomer = context.Customers.Find(customer.CustomerId)
                Assert.AreEqual(0D, updatedCustomer.CurrentARBalance)

                ' Verify invoice status settled
                Dim updatedInvoice = context.SalesInvoices.Find(invoice.InvoiceId)
                Assert.AreEqual("Settled", updatedInvoice.Status)

                ' Verify General Ledger entry: Debit 1020 Operating Bank, Credit 1200 AR
                Dim journalEntry = context.JournalEntries.FirstOrDefault(Function(j) j.SourceDocId = receipt.ReceiptId AndAlso j.SourceDocType = "PaymentReceipt")
                Assert.IsNotNull(journalEntry)
                Assert.AreEqual(2, journalEntry.Lines.Count)

                Dim bankLine = journalEntry.Lines.First(Function(l) l.Account.Code = "1020")
                Assert.AreEqual(500.0D, bankLine.DebitAmount)
                Assert.AreEqual(0D, bankLine.CreditAmount)

                Dim arLine = journalEntry.Lines.First(Function(l) l.Account.Code = "1200")
                Assert.AreEqual(0D, arLine.DebitAmount)
                Assert.AreEqual(500.0D, arLine.CreditAmount)

                Assert.IsTrue(LedgerPostingEngine.IsBalanced(journalEntry.Lines))
            End Using
        End Sub

        <TestMethod>
        Public Sub RecordPaymentReceipt_PartialPayment_LeavesInvoicePosted()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)

                Dim customer As New Customer("CUST008", "Iota LLC", 10000.0D, 0D)
                context.Customers.Add(customer)
                Dim product As New Product("SKU-600", "Widget F", "Each", 20.0D, 100)
                context.Products.Add(product)
                context.SaveChanges()

                Dim invoiceLines As New List(Of SalesInvoiceLine) From {
                    New SalesInvoiceLine() With {.ProductId = product.ProductId, .FulfilledQty = 10, .UnitPrice = 40.0D}
                }
                Dim postingDate As New DateTime(2026, 11, 15)
                Dim invoice = service.FulfillSalesInvoice(0, invoiceLines, postingDate, customer.CustomerId)

                Assert.AreEqual(400.0D, context.Customers.Find(customer.CustomerId).CurrentARBalance)

                ' Record partial payment of $150
                Dim bankAccount = context.Accounts.First(Function(a) a.Code = "1020")
                Dim receipt = service.RecordPaymentReceipt(customer.CustomerId, invoice.InvoiceId, 150.0D, bankAccount.AccountId, postingDate, postingDate)

                Assert.IsNotNull(receipt)
                Dim updatedCustomer = context.Customers.Find(customer.CustomerId)
                Assert.AreEqual(250.0D, updatedCustomer.CurrentARBalance)

                ' Invoice is partially paid, remains "Posted"
                Dim updatedInvoice = context.SalesInvoices.Find(invoice.InvoiceId)
                Assert.AreEqual("Posted", updatedInvoice.Status)
            End Using
        End Sub

        <TestMethod>
        Public Sub RecordPaymentReceipt_UnappliedPayment_WithoutInvoice()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)

                Dim customer As New Customer("CUST009", "Kappa Co", 5000.0D, 300.0D)
                context.Customers.Add(customer)
                context.SaveChanges()

                Dim bankAccount = context.Accounts.First(Function(a) a.Code = "1020")
                Dim postingDate As New DateTime(2026, 11, 15)

                ' Unapplied receipt (Integer overload with 0)
                Dim receipt = service.RecordPaymentReceipt(customer.CustomerId, 0, 100.0D, bankAccount.AccountId, postingDate, postingDate)

                Assert.IsNotNull(receipt)
                Assert.IsFalse(receipt.InvoiceId.HasValue)

                Dim updatedCustomer = context.Customers.Find(customer.CustomerId)
                Assert.AreEqual(200.0D, updatedCustomer.CurrentARBalance)
            End Using
        End Sub

        <TestMethod>
        Public Sub RecordPaymentReceipt_InvalidParameters_ThrowsAppropriateExceptions()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)

                ' Invalid CustomerId
                Assert.Throws(Of ArgumentException)(Sub()
                    service.RecordPaymentReceipt(0, Nothing, 100.0D, 1)
                End Sub)

                ' Customer not found
                Assert.Throws(Of ArgumentException)(Sub()
                    service.RecordPaymentReceipt(9999, Nothing, 100.0D, 1)
                End Sub)

                ' Amount <= 0
                Dim customer As New Customer("CUST010", "Lambda Inc", 5000.0D, 0D)
                context.Customers.Add(customer)
                context.SaveChanges()

                Assert.Throws(Of ArgumentOutOfRangeException)(Sub()
                    service.RecordPaymentReceipt(customer.CustomerId, Nothing, 0D, 1)
                End Sub)
                Assert.Throws(Of ArgumentOutOfRangeException)(Sub()
                    service.RecordPaymentReceipt(customer.CustomerId, Nothing, -10.0D, 1)
                End Sub)

                ' Invoice not found
                Assert.Throws(Of ArgumentException)(Sub()
                    service.RecordPaymentReceipt(customer.CustomerId, 9999, 100.0D, 1)
                End Sub)

                ' Invoice belongs to another customer
                Dim customerOther As New Customer("CUST011", "Mu Ltd", 5000.0D, 0D)
                context.Customers.Add(customerOther)
                Dim product As New Product("SKU-700", "Widget G", "Each", 10.0D, 50)
                context.Products.Add(product)
                context.SaveChanges()

                Dim inv = service.FulfillSalesInvoice(0, New List(Of SalesInvoiceLine) From {
                    New SalesInvoiceLine() With {.ProductId = product.ProductId, .FulfilledQty = 5, .UnitPrice = 20.0D}
                }, New DateTime(2026, 11, 15), customerOther.CustomerId)

                Assert.Throws(Of InvalidOperationException)(Sub()
                    service.RecordPaymentReceipt(customer.CustomerId, inv.InvoiceId, 50.0D, 1)
                End Sub)
            End Using
        End Sub

        <TestMethod>
        Public Sub RecordPaymentReceipt_DefaultBankResolution_WhenBankAccountIdZero()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)

                Dim customer As New Customer("CUST012", "Nu Corp", 5000.0D, 200.0D)
                context.Customers.Add(customer)
                context.SaveChanges()

                Dim postingDate As New DateTime(2026, 11, 15)
                Dim receipt = service.RecordPaymentReceipt(customer.CustomerId, Nothing, 50.0D, 0, postingDate, postingDate)

                Assert.IsNotNull(receipt)
                Dim bankAccount = context.Accounts.First(Function(a) a.Code = "1020")
                Assert.AreEqual(bankAccount.AccountId, receipt.BankAccountId)
            End Using
        End Sub

        ' =========================================================================
        ' Stock Reservation, Credit Limit & Sales Order Lifecycle Tests (Track C)
        ' =========================================================================

        <TestMethod>
        Public Sub CreateSalesOrder_ReservesAllocatedStockAtomically()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)

                Dim customer As New Customer("CUST-RES-01", "Reservation Customer", 10000.0D, 0D)
                context.Customers.Add(customer)
                Dim product As New Product("SKU-RES-01", "Reserved Widget", "Each", 10.0D, 50)
                context.Products.Add(product)
                context.SaveChanges()

                ' Create Sales Order for 15 units
                Dim lines As New List(Of SOLine) From {
                    New SOLine(product.ProductId, 15, 20.0D)
                }

                Dim so = service.CreateSalesOrder(customer.CustomerId, lines)
                Assert.IsNotNull(so)

                ' Verify AllocatedQty updated atomically: 15
                Dim reloaded = context.Products.Find(product.ProductId)
                Assert.AreEqual(15, reloaded.AllocatedQty)
                Assert.AreEqual(50, reloaded.TotalQtyOnHand)
            End Using
        End Sub

        <TestMethod>
        Public Sub CreateSalesOrder_InsufficientUnallocatedStock_ThrowsInsufficientStockException()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)

                Dim customer As New Customer("CUST-RES-02", "Short Customer", 10000.0D, 0D)
                context.Customers.Add(customer)
                ' 20 on hand, 15 already allocated -> only 5 unallocated available
                Dim product As New Product("SKU-RES-02", "Limited Widget", "Each", 10.0D, 20) With {
                    .AllocatedQty = 15
                }
                context.Products.Add(product)
                context.SaveChanges()

                ' Attempt to order 10 units when only 5 are unallocated
                Dim lines As New List(Of SOLine) From {
                    New SOLine(product.ProductId, 10, 25.0D)
                }

                Assert.Throws(Of InsufficientStockException)(Sub()
                    service.CreateSalesOrder(customer.CustomerId, lines)
                End Sub)

                ' Verify AllocatedQty remains 15
                Dim reloaded = context.Products.Find(product.ProductId)
                Assert.AreEqual(15, reloaded.AllocatedQty)
            End Using
        End Sub

        <TestMethod>
        Public Sub CreateSalesOrder_ExceedingCreditLimit_ThrowsCreditLimitExceededException()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)

                ' Customer with credit limit of $1,000, current AR balance $800
                Dim customer As New Customer("CUST-CRED-01", "Tight Credit Customer", 1000.0D, 800.0D)
                context.Customers.Add(customer)
                Dim product As New Product("SKU-CRED-01", "Expensive Widget", "Each", 100.0D, 50)
                context.Products.Add(product)
                context.SaveChanges()

                ' Order total: 3 * $100 = $300. $800 + $300 = $1,100 > $1,000 credit limit
                Dim lines As New List(Of SOLine) From {
                    New SOLine(product.ProductId, 3, 100.0D)
                }

                Assert.Throws(Of CreditLimitExceededException)(Sub()
                    service.CreateSalesOrder(customer.CustomerId, lines, allowCreditOverride:=False)
                End Sub)

                ' Verify no reservation occurred
                Dim reloaded = context.Products.Find(product.ProductId)
                Assert.AreEqual(0, reloaded.AllocatedQty)
            End Using
        End Sub

        <TestMethod>
        Public Sub CreateSalesOrder_CreditLimitOverride_Succeeds()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)

                Dim customer As New Customer("CUST-CRED-02", "VIP Customer", 1000.0D, 800.0D)
                context.Customers.Add(customer)
                Dim product As New Product("SKU-CRED-02", "VIP Widget", "Each", 100.0D, 50)
                context.Products.Add(product)
                context.SaveChanges()

                Dim lines As New List(Of SOLine) From {
                    New SOLine(product.ProductId, 3, 100.0D)
                }

                ' With override = True, order succeeds
                Dim so = service.CreateSalesOrder(customer.CustomerId, lines, allowCreditOverride:=True)
                Assert.IsNotNull(so)

                Dim reloaded = context.Products.Find(product.ProductId)
                Assert.AreEqual(3, reloaded.AllocatedQty)
            End Using
        End Sub

        <TestMethod>
        Public Sub FulfillSalesInvoice_ReleasesAllocatedStockAndDecrementsPhysicalStock()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)

                Dim customer As New Customer("CUST-FUL-01", "Fulfill Customer", 10000.0D, 0D)
                context.Customers.Add(customer)
                Dim product As New Product("SKU-FUL-01", "Fulfill Widget", "Each", 15.0D, 40)
                context.Products.Add(product)
                context.SaveChanges()

                ' 1. Create SO for 10 units -> AllocatedQty = 10, TotalQtyOnHand = 40
                Dim lines As New List(Of SOLine) From {
                    New SOLine(product.ProductId, 10, 30.0D)
                }
                Dim so = service.CreateSalesOrder(customer.CustomerId, lines)

                Dim pAfterSO = context.Products.Find(product.ProductId)
                Assert.AreEqual(10, pAfterSO.AllocatedQty)
                Assert.AreEqual(40, pAfterSO.TotalQtyOnHand)

                ' 2. Fulfill the SO completely (10 units)
                Dim postingDate As New DateTime(2026, 11, 15)
                Dim invoice = service.FulfillSalesInvoice(so.SOId, postingDate)

                Assert.IsNotNull(invoice)
                Assert.AreEqual("Settled", so.Status)

                ' AllocatedQty released back to 0, TotalQtyOnHand decremented to 30
                Dim pAfterInv = context.Products.Find(product.ProductId)
                Assert.AreEqual(0, pAfterInv.AllocatedQty)
                Assert.AreEqual(30, pAfterInv.TotalQtyOnHand)
            End Using
        End Sub

        <TestMethod>
        Public Sub FulfillSalesInvoice_PartialFulfillment_TransitionsToPartiallyInvoiced()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)

                Dim customer As New Customer("CUST-PART-01", "Partial Customer", 10000.0D, 0D)
                context.Customers.Add(customer)
                Dim product As New Product("SKU-PART-01", "Partial Widget", "Each", 10.0D, 50)
                context.Products.Add(product)
                context.SaveChanges()

                ' Create SO for 20 units
                Dim lines As New List(Of SOLine) From {
                    New SOLine(product.ProductId, 20, 25.0D)
                }
                Dim so = service.CreateSalesOrder(customer.CustomerId, lines)
                Dim soLine = so.Lines.First()

                ' Fulfill only 8 of 20 units
                Dim invLines As New List(Of SalesInvoiceLine) From {
                    New SalesInvoiceLine() With {
                        .SOLineId = soLine.SOLineId,
                        .ProductId = product.ProductId,
                        .FulfilledQty = 8,
                        .UnitPrice = 25.0D
                    }
                }

                Dim postingDate As New DateTime(2026, 11, 15)
                Dim invoice = service.FulfillSalesInvoice(so.SOId, invLines, postingDate)

                Assert.IsNotNull(invoice)

                ' Order status must transition to "Partially Invoiced"
                Dim reloadedSO = context.SalesOrders.Find(so.SOId)
                Assert.AreEqual("Partially Invoiced", reloadedSO.Status)

                ' 8 released from AllocatedQty: 20 - 8 = 12 remaining allocated
                ' 8 decremented from TotalQtyOnHand: 50 - 8 = 42 on hand
                Dim reloadedProd = context.Products.Find(product.ProductId)
                Assert.AreEqual(12, reloadedProd.AllocatedQty)
                Assert.AreEqual(42, reloadedProd.TotalQtyOnHand)
            End Using
        End Sub

        <TestMethod>
        Public Sub CancelSalesOrder_ReleasesAllReservedStockAndTransitionsToCancelled()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)

                Dim customer As New Customer("CUST-CNC-01", "Cancel Customer", 10000.0D, 0D)
                context.Customers.Add(customer)
                Dim product As New Product("SKU-CNC-01", "Cancel Widget", "Each", 10.0D, 50)
                context.Products.Add(product)
                context.SaveChanges()

                ' Reserve 15 units via Sales Order
                Dim lines As New List(Of SOLine) From {
                    New SOLine(product.ProductId, 15, 25.0D)
                }
                Dim so = service.CreateSalesOrder(customer.CustomerId, lines)

                Dim pAfterSO = context.Products.Find(product.ProductId)
                Assert.AreEqual(15, pAfterSO.AllocatedQty)

                ' Cancel the Sales Order
                Dim cancelledSO = service.CancelSalesOrder(so.SOId)

                Assert.IsNotNull(cancelledSO)
                Assert.AreEqual("Cancelled", cancelledSO.Status)

                ' Reserved stock must be released back to 0, TotalQtyOnHand remains 50
                Dim pAfterCancel = context.Products.Find(product.ProductId)
                Assert.AreEqual(0, pAfterCancel.AllocatedQty)
                Assert.AreEqual(50, pAfterCancel.TotalQtyOnHand)
            End Using
        End Sub

        <TestMethod>
        Public Sub CancelSalesOrder_AlreadySettledOrCancelled_ThrowsInvalidOperationException()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)

                Dim customer As New Customer("CUST-CNC-02", "Cancel Customer 2", 10000.0D, 0D)
                context.Customers.Add(customer)
                Dim product As New Product("SKU-CNC-02", "Cancel Widget 2", "Each", 10.0D, 50)
                context.Products.Add(product)
                context.SaveChanges()

                Dim so = service.CreateSalesOrder(customer.CustomerId, New List(Of SOLine) From {
                    New SOLine(product.ProductId, 5, 20.0D)
                })

                service.CancelSalesOrder(so.SOId)

                ' Attempting to cancel an already cancelled order
                Assert.Throws(Of InvalidOperationException)(Sub()
                    service.CancelSalesOrder(so.SOId)
                End Sub)
            End Using
        End Sub

        <TestMethod>
        Public Sub CloseSalesOrderShort_ReleasesRemainingReservationsAndTransitionsToClosedShort()
            Using context = _fixture.CreateContext()
                Dim service As New OrderToCashService(context)

                Dim customer As New Customer("CUST-CLS-01", "Close Customer", 10000.0D, 0D)
                context.Customers.Add(customer)
                Dim product As New Product("SKU-CLS-01", "Close Widget", "Each", 10.0D, 50)
                context.Products.Add(product)
                context.SaveChanges()

                ' Create SO for 20 units
                Dim lines As New List(Of SOLine) From {
                    New SOLine(product.ProductId, 20, 25.0D)
                }
                Dim so = service.CreateSalesOrder(customer.CustomerId, lines)
                Dim soLine = so.Lines.First()

                ' Fulfill 12 units partially
                Dim invLines As New List(Of SalesInvoiceLine) From {
                    New SalesInvoiceLine() With {
                        .SOLineId = soLine.SOLineId,
                        .ProductId = product.ProductId,
                        .FulfilledQty = 12,
                        .UnitPrice = 25.0D
                    }
                }
                service.FulfillSalesInvoice(so.SOId, invLines, New DateTime(2026, 11, 15))

                Dim pAfterPartial = context.Products.Find(product.ProductId)
                Assert.AreEqual(8, pAfterPartial.AllocatedQty) ' 20 - 12 = 8 remaining allocated

                ' Customer requests short close (cancel remaining 8)
                Dim closedSO = service.CloseSalesOrderShort(so.SOId)

                Assert.IsNotNull(closedSO)
                Assert.AreEqual("Closed Short", closedSO.Status)

                ' Remaining 8 allocated units released back to 0
                Dim pAfterClose = context.Products.Find(product.ProductId)
                Assert.AreEqual(0, pAfterClose.AllocatedQty)
                Assert.AreEqual(38, pAfterClose.TotalQtyOnHand) ' 50 - 12 = 38
            End Using
        End Sub

    End Class

End Namespace
