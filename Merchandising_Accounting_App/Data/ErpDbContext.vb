Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports Microsoft.EntityFrameworkCore
Imports Merchandising_Accounting_App.Domain.Models

Namespace Data

    ''' <summary>
    ''' Entity Framework Core 10 DbContext managing SQLite connection lifecycle, schema migrations,
    ''' entity mappings, and atomic transactional Unit-of-Work commits for the Merchandising ERP system.
    ''' </summary>
    Public Class ErpDbContext
        Inherits DbContext

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(options As DbContextOptions(Of ErpDbContext))
            MyBase.New(options)
        End Sub

        ' =========================================================================
        ' General Ledger (4 Entities)
        ' =========================================================================
        Public Overridable Property Accounts As DbSet(Of Account)
        Public Overridable Property AccountingPeriods As DbSet(Of AccountingPeriod)
        Public Overridable Property JournalEntries As DbSet(Of JournalEntry)
        Public Overridable Property JournalEntryLines As DbSet(Of JournalEntryLine)

        ' =========================================================================
        ' Inventory (4 Entities)
        ' =========================================================================
        Public Overridable Property Products As DbSet(Of Product)
        Public Overridable Property InventoryTransactions As DbSet(Of InventoryTransaction)
        Public Overridable Property InventoryAdjustments As DbSet(Of InventoryAdjustment)
        Public Overridable Property InventoryAdjustmentLines As DbSet(Of InventoryAdjustmentLine)

        ' =========================================================================
        ' Procurement (9 Entities)
        ' =========================================================================
        Public Overridable Property Vendors As DbSet(Of Vendor)
        Public Overridable Property PurchaseOrders As DbSet(Of PurchaseOrder)
        Public Overridable Property POLines As DbSet(Of POLine)
        Public Overridable Property GoodsReceiptNotes As DbSet(Of GoodsReceiptNote)
        Public Overridable Property GRNLines As DbSet(Of GRNLine)
        Public Overridable Property SupplierBills As DbSet(Of SupplierBill)
        Public Overridable Property BillLines As DbSet(Of BillLine)
        Public Overridable Property PaymentVouchers As DbSet(Of PaymentVoucher)
        Public Overridable Property GRNIAdjustments As DbSet(Of GRNIAdjustment)

        ' =========================================================================
        ' Sales & AR (6 Entities)
        ' =========================================================================
        Public Overridable Property Customers As DbSet(Of Customer)
        Public Overridable Property SalesOrders As DbSet(Of SalesOrder)
        Public Overridable Property SOLines As DbSet(Of SOLine)
        Public Overridable Property SalesInvoices As DbSet(Of SalesInvoice)
        Public Overridable Property SalesInvoiceLines As DbSet(Of SalesInvoiceLine)
        Public Overridable Property PaymentReceipts As DbSet(Of PaymentReceipt)

        ' =========================================================================
        ' Returns (2 Entities)
        ' =========================================================================
        Public Overridable Property CustomerReturns As DbSet(Of CustomerReturn)
        Public Overridable Property CustomerReturnLines As DbSet(Of CustomerReturnLine)

        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            If Not optionsBuilder.IsConfigured Then
                optionsBuilder.UseSqlite(DatabaseInitializer.BuildConnectionString())
            End If
            MyBase.OnConfiguring(optionsBuilder)
        End Sub

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            MyBase.OnModelCreating(modelBuilder)
            ' Scan and register all decoupled IEntityTypeConfiguration implementations
            modelBuilder.ApplyConfigurationsFromAssembly(GetType(ErpDbContext).Assembly)
        End Sub

    End Class

End Namespace
