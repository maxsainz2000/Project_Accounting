Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports Microsoft.Data.Sqlite
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Merchandising_Accounting_App.Data
Imports Merchandising_Accounting_App.Data.Repositories
Imports Merchandising_Accounting_App.Domain
Imports Merchandising_Accounting_App.Domain.Models

Namespace Infrastructure

    <TestClass>
    Public Class PersistenceInfrastructureTests

        Private _fixture As TestDatabaseFixture

        <TestInitialize>
        Public Sub Setup()
            _fixture = New TestDatabaseFixture()
        End Sub

        <TestCleanup>
        Public Sub Cleanup()
            _fixture?.Dispose()
        End Sub

        <TestMethod>
        Public Sub Test_DatabaseInitialization_PragmasAndTablesCreated()
            Using context = _fixture.CreateContext()
                ' Verify SQLite foreign keys pragma is active
                Using cmd = context.Database.GetDbConnection().CreateCommand()
                    cmd.CommandText = "PRAGMA foreign_keys;"
                    Dim fkResult = Convert.ToInt32(cmd.ExecuteScalar())
                    Assert.AreEqual(1, fkResult, "Foreign keys should be enabled.")
                End Using

                ' Verify tables exist by querying DbSets
                Assert.IsTrue(context.Accounts.Any(), "Accounts table must be present and seeded.")
                Assert.IsTrue(context.AccountingPeriods.Any(), "AccountingPeriods table must be present and seeded.")
                Assert.AreEqual(0, context.Products.Count(), "Products table should be created and initially empty.")
                Assert.AreEqual(0, context.JournalEntries.Count(), "JournalEntries table should be created and initially empty.")
            End Using
        End Sub

        <TestMethod>
        Public Sub Test_MasterChartOfAccounts_Seeded()
            Using context = _fixture.CreateContext()
                Dim expectedAccounts = New Dictionary(Of String, (Name As String, ExpectedType As AccountType)) From {
                    {"1010", ("Cash on Hand", AccountType.Asset)},
                    {"1020", ("Operating Bank Account", AccountType.Asset)},
                    {"1200", ("Accounts Receivable", AccountType.Asset)},
                    {"1300", ("Merchandise Inventory", AccountType.Asset)},
                    {"2000", ("Accounts Payable", AccountType.Liability)},
                    {"2100", ("Goods Received Not Invoiced (GRNI)", AccountType.Liability)},
                    {"3000", ("Retained Earnings", AccountType.Equity)},
                    {"3010", ("Owner's Capital", AccountType.Equity)},
                    {"4000", ("Sales Revenue", AccountType.Revenue)},
                    {"4100", ("Sales Returns & Allowances", AccountType.Revenue)},
                    {"5000", ("Cost of Goods Sold (COGS)", AccountType.Expense)},
                    {"5100", ("Purchase Price Variance (PPV)", AccountType.Expense)},
                    {"5200", ("Inbound Freight Variance", AccountType.Expense)},
                    {"5300", ("Inventory Shrinkage & Damaged Stock", AccountType.Expense)},
                    {"5400", ("Stocktake Revaluation Variance", AccountType.Expense)},
                    {"5500", ("Cash Rounding & Roundoff Variance", AccountType.Expense)}
                }

                Dim allAccounts = context.Accounts.ToList()
                Assert.AreEqual(16, allAccounts.Count, "Exactly 16 Master Chart of Accounts entries should be seeded.")

                For Each expected In expectedAccounts
                    Dim acc = allAccounts.FirstOrDefault(Function(a) a.Code = expected.Key)
                    Assert.IsNotNull(acc, $"Account code {expected.Key} should be seeded.")
                    Assert.AreEqual(expected.Value.Name, acc.Name, $"Account {expected.Key} name mismatch.")
                    Assert.AreEqual(expected.Value.ExpectedType, acc.AccountType, $"Account {expected.Key} type mismatch.")
                    Assert.IsTrue(acc.IsActive, $"Account {expected.Key} should be active.")
                Next
            End Using
        End Sub

        <TestMethod>
        Public Sub Test_InitialOpenPeriod_Seeded()
            Using context = _fixture.CreateContext()
                Dim periodRepo As New AccountingPeriodRepository(context)
                Dim activePeriod = periodRepo.GetActivePeriod()

                Assert.IsNotNull(activePeriod, "Active accounting period must be present.")
                Assert.AreEqual(2026, activePeriod.FiscalYear)
                Assert.AreEqual(11, activePeriod.PeriodNumber)
                Assert.IsFalse(activePeriod.IsClosed)
                Assert.IsNull(activePeriod.ClosedAtUtc)
                Assert.AreEqual(New DateTime(2026, 11, 1), activePeriod.StartDate.Date)
                Assert.AreEqual(New DateTime(2026, 11, 30), activePeriod.EndDate.Date)

                ' Test lookup by date
                Dim midNov = New DateTime(2026, 11, 15)
                Dim lookedUpPeriod = periodRepo.GetPeriodForDate(midNov)
                Assert.IsNotNull(lookedUpPeriod)
                Assert.AreEqual(activePeriod.PeriodId, lookedUpPeriod.PeriodId)
            End Using
        End Sub

        <TestMethod>
        Public Sub Test_Product_ZeroNegativeStock_CheckConstraint()
            Using context = _fixture.CreateContext()
                ' Attempt to insert a product with negative stock directly
                Dim invalidProduct As New Product("SKU-NEG-1", "Negative Stock Item", "EA", 10.0D, -5)
                context.Products.Add(invalidProduct)

                Dim threwCheckConstraintException As Boolean = False
                Try
                    context.SaveChanges()
                Catch ex As DbUpdateException
                    threwCheckConstraintException = True
                End Try

                Assert.IsTrue(threwCheckConstraintException, "Database check constraint CHECK(TotalQtyOnHand >= 0) must prevent negative stock insertion.")
            End Using
        End Sub

        <TestMethod>
        Public Sub Test_ProductRepository_AtomicDecrement_Success()
            Using context = _fixture.CreateContext()
                Dim repo As New ProductRepository(context)
                Dim prod As New Product("SKU-WIDGET-1", "Standard Widget", "PCS", 25.5D, 10)
                repo.Add(prod)

                ' Decrement 4 units
                Dim success = repo.DecrementStock(prod.ProductId, 4)
                Assert.IsTrue(success, "Decrementing within available stock must succeed.")

                Dim updated = repo.GetById(prod.ProductId)
                Assert.AreEqual(6, updated.TotalQtyOnHand, "Remaining on-hand stock must be exactly 6.")
            End Using
        End Sub

        <TestMethod>
        Public Sub Test_ProductRepository_AtomicDecrement_Shortfall_ReturnsFalse()
            Using context = _fixture.CreateContext()
                Dim repo As New ProductRepository(context)
                Dim prod As New Product("SKU-WIDGET-2", "Limited Widget", "PCS", 12.0D, 3)
                repo.Add(prod)

                ' Attempt to decrement 5 units when only 3 available
                Dim success = repo.DecrementStock(prod.ProductId, 5)
                Assert.IsFalse(success, "Decrementing beyond available stock must return False.")

                Dim unchanged = repo.GetById(prod.ProductId)
                Assert.AreEqual(3, unchanged.TotalQtyOnHand, "Stock must remain unchanged at 3 upon shortfall.")
            End Using
        End Sub

        <TestMethod>
        Public Sub Test_ProductRepository_AtomicDecrementOrThrow_ThrowsInsufficientStockException()
            Using context = _fixture.CreateContext()
                Dim repo As New ProductRepository(context)
                Dim prod As New Product("SKU-WIDGET-3", "Scarce Widget", "PCS", 15.0D, 2)
                repo.Add(prod)

                Dim threwInsufficientStock As Boolean = False
                Try
                    repo.DecrementStockOrThrow(prod.ProductId, 10)
                Catch ex As InsufficientStockException
                    threwInsufficientStock = True
                    Assert.AreEqual(prod.ProductId, ex.ProductId)
                    Assert.AreEqual(10, ex.RequestedQty)
                    Assert.AreEqual(2, ex.AvailableQty)
                End Try

                Assert.IsTrue(threwInsufficientStock, "DecrementStockOrThrow must throw InsufficientStockException on deficit.")
            End Using
        End Sub

        <TestMethod>
        Public Sub Test_LedgerRepository_AppendOnlyInsert_Succeeds()
            Using context = _fixture.CreateContext()
                Dim periodRepo As New AccountingPeriodRepository(context)
                Dim activePeriod = periodRepo.GetActivePeriod()
                Dim ledgerRepo As New LedgerRepository(context)

                Dim entry As New JournalEntry() With {
                    .SourceDocType = "GRN",
                    .SourceDocId = 101,
                    .PostingDate = New DateTime(2026, 11, 5),
                    .OperationalEventDate = New DateTime(2026, 11, 5),
                    .PeriodId = activePeriod.PeriodId,
                    .Description = "GRN receipt inventory capitalization",
                    .CreatedBy = "SystemClerk"
                }

                ' 1300 Inventory Debit 500.0000
                ' 2100 GRNI Credit 500.0000
                Dim accInventory = context.Accounts.First(Function(a) a.Code = "1300")
                Dim accGrni = context.Accounts.First(Function(a) a.Code = "2100")

                entry.Lines.Add(New JournalEntryLine(accInventory.AccountId, 500.0000D, 0D, "Merchandise debit"))
                entry.Lines.Add(New JournalEntryLine(accGrni.AccountId, 0D, 500.0000D, "GRNI clearing credit"))

                ledgerRepo.Add(entry)
                Assert.IsTrue(entry.EntryId > 0, "Journal entry must be assigned an ID upon insert.")

                ' Verify retrieved entry
                Dim retrieved = ledgerRepo.GetById(entry.EntryId)
                Assert.IsNotNull(retrieved)
                Assert.AreEqual(2, retrieved.Lines.Count)
                Assert.AreEqual(500.0000D, retrieved.Lines.Sum(Function(l) l.DebitAmount))
                Assert.AreEqual(500.0000D, retrieved.Lines.Sum(Function(l) l.CreditAmount))
            End Using
        End Sub

        <TestMethod>
        Public Sub Test_LedgerRepository_Update_ThrowsInvalidOperationException()
            Using context = _fixture.CreateContext()
                Dim ledgerRepo As New LedgerRepository(context)
                Dim entry As New JournalEntry() With {.EntryId = 1}

                Dim threw As Boolean = False
                Try
                    ledgerRepo.Update(entry)
                Catch ex As InvalidOperationException
                    threw = True
                End Try
                Assert.IsTrue(threw, "LedgerRepository.Update must throw InvalidOperationException.")
            End Using
        End Sub

        <TestMethod>
        Public Sub Test_LedgerRepository_Delete_ThrowsInvalidOperationException()
            Using context = _fixture.CreateContext()
                Dim ledgerRepo As New LedgerRepository(context)
                Dim entry As New JournalEntry() With {.EntryId = 1}

                Dim threwEntry As Boolean = False
                Try
                    ledgerRepo.Delete(entry)
                Catch ex As InvalidOperationException
                    threwEntry = True
                End Try
                Assert.IsTrue(threwEntry, "LedgerRepository.Delete(entry) must throw InvalidOperationException.")

                Dim threwId As Boolean = False
                Try
                    ledgerRepo.Delete(1)
                Catch ex As InvalidOperationException
                    threwId = True
                End Try
                Assert.IsTrue(threwId, "LedgerRepository.Delete(id) must throw InvalidOperationException.")
            End Using
        End Sub

        <TestMethod>
        Public Sub Test_AccountRepository_QueriesAndAdd()
            Using context = _fixture.CreateContext()
                Dim accountRepo As New AccountRepository(context)

                Dim acc1010 = accountRepo.GetByCode("1010")
                Assert.IsNotNull(acc1010)
                Assert.AreEqual("Cash on Hand", acc1010.Name)

                Dim assets = accountRepo.GetByType(AccountType.Asset)
                Assert.IsTrue(assets.Count >= 4)

                ' Add custom account
                Dim customAcc As New Account("1030", "Petty Cash", AccountType.Asset)
                accountRepo.Add(customAcc)

                Dim retrieved = accountRepo.GetByCode("1030")
                Assert.IsNotNull(retrieved)
                Assert.AreEqual("Petty Cash", retrieved.Name)
            End Using
        End Sub

        <TestMethod>
        Public Sub Test_DocumentRepository_ProcurementAndSalesFlow()
            Using context = _fixture.CreateContext()
                Dim docRepo As New DocumentRepository(context)
                Dim prodRepo As New ProductRepository(context)

                ' Add Product
                Dim product As New Product("SKU-FLOW-1", "Flow Item", "EA", 50.0000D, 100)
                prodRepo.Add(product)

                ' Add Vendor
                Dim vendor As New Vendor("VEND-001", "Global Supplier Inc", 30, 0D)
                docRepo.Add(vendor)
                Assert.IsTrue(vendor.VendorId > 0)

                ' Add Purchase Order
                Dim po As New PurchaseOrder() With {
                    .VendorId = vendor.VendorId,
                    .DocumentNumber = "PO-2026-0001",
                    .OrderDate = DateTime.Now,
                    .Status = DocumentStatus.Draft.ToString(),
                    .TotalAmount = 500.0000D
                }
                po.Lines.Add(New POLine(product.ProductId, 10, 50.0000D))
                docRepo.Add(po)

                Dim retrievedPO = docRepo.GetPurchaseOrderById(po.POId)
                Assert.IsNotNull(retrievedPO)
                Assert.AreEqual("PO-2026-0001", retrievedPO.DocumentNumber)
                Assert.AreEqual(1, retrievedPO.Lines.Count)
                Assert.AreEqual(50.0000D, retrievedPO.Lines.First().UnitPrice)

                ' Add Customer & Sales Order
                Dim customer As New Customer("CUST-001", "Acme Retailers", 5000.0000D, 0D)
                docRepo.Add(customer)

                Dim so As New SalesOrder() With {
                    .CustomerId = customer.CustomerId,
                    .DocumentNumber = "SO-2026-0001",
                    .OrderDate = DateTime.Now,
                    .Status = DocumentStatus.Draft.ToString()
                }
                so.Lines.Add(New SOLine(product.ProductId, 5, 85.5000D))
                docRepo.Add(so)

                Dim retrievedSO = docRepo.GetSalesOrderById(so.SOId)
                Assert.IsNotNull(retrievedSO)
                Assert.AreEqual(1, retrievedSO.Lines.Count)
                Assert.AreEqual(85.5000D, retrievedSO.Lines.First().UnitPrice)

                ' Add Sales Invoice
                Dim invoice As New SalesInvoice() With {
                    .CustomerId = customer.CustomerId,
                    .SOId = so.SOId,
                    .DocumentNumber = "INV-2026-0001",
                    .InvoiceDate = DateTime.Now,
                    .PostingDate = DateTime.Now,
                    .DueDate = DateTime.Now.AddDays(30),
                    .TotalAmount = 427.5000D,
                    .Status = DocumentStatus.Posted.ToString()
                }
                invoice.Lines.Add(New SalesInvoiceLine(product.ProductId, 5, 85.5000D, 50.0000D))
                docRepo.Add(invoice)

                Dim retrievedInv = docRepo.GetSalesInvoiceById(invoice.InvoiceId)
                Assert.IsNotNull(retrievedInv)
                Assert.AreEqual(427.5000D, retrievedInv.TotalAmount)
                Assert.AreEqual(50.0000D, retrievedInv.Lines.First().InvoicedCOGSUnitCost)

                ' Add Customer Return
                Dim retDoc As New CustomerReturn() With {
                    .CustomerId = customer.CustomerId,
                    .OriginalInvoiceId = invoice.InvoiceId,
                    .DocumentNumber = "RET-2026-0001",
                    .ReturnDate = DateTime.Now,
                    .PostingDate = DateTime.Now,
                    .Status = DocumentStatus.Draft.ToString()
                }
                retDoc.Lines.Add(New CustomerReturnLine(product.ProductId, 2, 1, 1, 50.0000D, 85.5000D))
                docRepo.Add(retDoc)

                Dim retrievedReturn = docRepo.GetCustomerReturnById(retDoc.ReturnId)
                Assert.IsNotNull(retrievedReturn)
                Assert.AreEqual(1, retrievedReturn.Lines.Count)
                Assert.AreEqual(1, retrievedReturn.Lines.First().RestockQty)
                Assert.AreEqual(1, retrievedReturn.Lines.First().ScrapQty)
                Assert.AreEqual(50.0000D, retrievedReturn.Lines.First().OriginalUnitCost)
            End Using
        End Sub

    End Class

End Namespace
