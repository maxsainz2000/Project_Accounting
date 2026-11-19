Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports Microsoft.EntityFrameworkCore
Imports Merchandising_Accounting_App.Domain.Models

Namespace Data.Repositories

    ''' <summary>
    ''' Repository coordinating operational procurement, sales, return, and inventory adjustment documents.
    ''' </summary>
    Public Class DocumentRepository

        Private ReadOnly _context As ErpDbContext

        Public Sub New(context As ErpDbContext)
            If context Is Nothing Then
                Throw New ArgumentNullException(NameOf(context))
            End If
            _context = context
        End Sub

        ' =========================================================================
        ' Master Parties (Vendors & Customers)
        ' =========================================================================

        Public Function GetVendorById(id As Integer) As Vendor
            Return _context.Vendors.Find(id)
        End Function

        Public Async Function GetVendorByIdAsync(id As Integer) As Task(Of Vendor)
            Return Await _context.Vendors.FindAsync(id)
        End Function

        Public Function GetVendorByCode(code As String) As Vendor
            If String.IsNullOrWhiteSpace(code) Then Return Nothing
            Return _context.Vendors.FirstOrDefault(Function(v) v.Code = code)
        End Function

        Public Function GetAllVendors() As List(Of Vendor)
            Return _context.Vendors.OrderBy(Function(v) v.Name).ToList()
        End Function

        Public Function GetCustomerById(id As Integer) As Customer
            Return _context.Customers.Find(id)
        End Function

        Public Async Function GetCustomerByIdAsync(id As Integer) As Task(Of Customer)
            Return Await _context.Customers.FindAsync(id)
        End Function

        Public Function GetCustomerByCode(code As String) As Customer
            If String.IsNullOrWhiteSpace(code) Then Return Nothing
            Return _context.Customers.FirstOrDefault(Function(c) c.Code = code)
        End Function

        Public Function GetAllCustomers() As List(Of Customer)
            Return _context.Customers.OrderBy(Function(c) c.Name).ToList()
        End Function

        ' =========================================================================
        ' Procurement Documents
        ' =========================================================================

        Public Function GetPurchaseOrderById(id As Integer) As PurchaseOrder
            Return _context.PurchaseOrders _
                .Include(Function(p) p.Vendor) _
                .Include(Function(p) p.Lines) _
                    .ThenInclude(Function(l) l.Product) _
                .FirstOrDefault(Function(p) p.POId = id)
        End Function

        Public Async Function GetPurchaseOrderByIdAsync(id As Integer) As Task(Of PurchaseOrder)
            Return Await _context.PurchaseOrders _
                .Include(Function(p) p.Vendor) _
                .Include(Function(p) p.Lines) _
                    .ThenInclude(Function(l) l.Product) _
                .FirstOrDefaultAsync(Function(p) p.POId = id)
        End Function

        Public Function GetPurchaseOrderByNumber(docNo As String) As PurchaseOrder
            Return _context.PurchaseOrders _
                .Include(Function(p) p.Vendor) _
                .Include(Function(p) p.Lines) _
                    .ThenInclude(Function(l) l.Product) _
                .FirstOrDefault(Function(p) p.DocumentNumber = docNo)
        End Function

        Public Function GetGoodsReceiptNoteById(id As Integer) As GoodsReceiptNote
            Return _context.GoodsReceiptNotes _
                .Include(Function(g) g.PurchaseOrder) _
                .Include(Function(g) g.Lines) _
                    .ThenInclude(Function(l) l.Product) _
                .FirstOrDefault(Function(g) g.GRNId = id)
        End Function

        Public Async Function GetGoodsReceiptNoteByIdAsync(id As Integer) As Task(Of GoodsReceiptNote)
            Return Await _context.GoodsReceiptNotes _
                .Include(Function(g) g.PurchaseOrder) _
                .Include(Function(g) g.Lines) _
                    .ThenInclude(Function(l) l.Product) _
                .FirstOrDefaultAsync(Function(g) g.GRNId = id)
        End Function

        Public Function GetSupplierBillById(id As Integer) As SupplierBill
            Return _context.SupplierBills _
                .Include(Function(b) b.Vendor) _
                .Include(Function(b) b.Lines) _
                    .ThenInclude(Function(l) l.Product) _
                .FirstOrDefault(Function(b) b.BillId = id)
        End Function

        Public Async Function GetSupplierBillByIdAsync(id As Integer) As Task(Of SupplierBill)
            Return Await _context.SupplierBills _
                .Include(Function(b) b.Vendor) _
                .Include(Function(b) b.Lines) _
                    .ThenInclude(Function(l) l.Product) _
                .FirstOrDefaultAsync(Function(b) b.BillId = id)
        End Function

        Public Function GetPaymentVoucherById(id As Integer) As PaymentVoucher
            Return _context.PaymentVouchers _
                .Include(Function(v) v.Vendor) _
                .Include(Function(v) v.BankAccount) _
                .FirstOrDefault(Function(v) v.VoucherId = id)
        End Function

        ' =========================================================================
        ' Sales & AR Documents
        ' =========================================================================

        Public Function GetSalesOrderById(id As Integer) As SalesOrder
            Return _context.SalesOrders _
                .Include(Function(s) s.Customer) _
                .Include(Function(s) s.Lines) _
                    .ThenInclude(Function(l) l.Product) _
                .FirstOrDefault(Function(s) s.SOId = id)
        End Function

        Public Async Function GetSalesOrderByIdAsync(id As Integer) As Task(Of SalesOrder)
            Return Await _context.SalesOrders _
                .Include(Function(s) s.Customer) _
                .Include(Function(s) s.Lines) _
                    .ThenInclude(Function(l) l.Product) _
                .FirstOrDefaultAsync(Function(s) s.SOId = id)
        End Function

        Public Function GetSalesInvoiceById(id As Integer) As SalesInvoice
            Return _context.SalesInvoices _
                .Include(Function(i) i.Customer) _
                .Include(Function(i) i.Lines) _
                    .ThenInclude(Function(l) l.Product) _
                .FirstOrDefault(Function(i) i.InvoiceId = id)
        End Function

        Public Async Function GetSalesInvoiceByIdAsync(id As Integer) As Task(Of SalesInvoice)
            Return Await _context.SalesInvoices _
                .Include(Function(i) i.Customer) _
                .Include(Function(i) i.Lines) _
                    .ThenInclude(Function(l) l.Product) _
                .FirstOrDefaultAsync(Function(i) i.InvoiceId = id)
        End Function

        Public Function GetSalesInvoiceByNumber(docNo As String) As SalesInvoice
            Return _context.SalesInvoices _
                .Include(Function(i) i.Customer) _
                .Include(Function(i) i.Lines) _
                    .ThenInclude(Function(l) l.Product) _
                .FirstOrDefault(Function(i) i.DocumentNumber = docNo)
        End Function

        Public Function GetPaymentReceiptById(id As Integer) As PaymentReceipt
            Return _context.PaymentReceipts _
                .Include(Function(r) r.Customer) _
                .Include(Function(r) r.BankAccount) _
                .FirstOrDefault(Function(r) r.ReceiptId = id)
        End Function

        ' =========================================================================
        ' Returns Documents
        ' =========================================================================

        Public Function GetCustomerReturnById(id As Integer) As CustomerReturn
            Return _context.CustomerReturns _
                .Include(Function(r) r.Customer) _
                .Include(Function(r) r.OriginalInvoice) _
                .Include(Function(r) r.Lines) _
                    .ThenInclude(Function(l) l.Product) _
                .FirstOrDefault(Function(r) r.ReturnId = id)
        End Function

        Public Async Function GetCustomerReturnByIdAsync(id As Integer) As Task(Of CustomerReturn)
            Return Await _context.CustomerReturns _
                .Include(Function(r) r.Customer) _
                .Include(Function(r) r.OriginalInvoice) _
                .Include(Function(r) r.Lines) _
                    .ThenInclude(Function(l) l.Product) _
                .FirstOrDefaultAsync(Function(r) r.ReturnId = id)
        End Function

        ' =========================================================================
        ' Inventory Adjustments
        ' =========================================================================

        Public Function GetAdjustmentById(id As Integer) As InventoryAdjustment
            Return _context.InventoryAdjustments _
                .Include(Function(a) a.Lines) _
                    .ThenInclude(Function(l) l.Product) _
                .FirstOrDefault(Function(a) a.AdjustmentId = id)
        End Function

        ' =========================================================================
        ' Generic Persistence Operations
        ' =========================================================================

        Public Sub Add(Of T As Class)(entity As T)
            If entity Is Nothing Then Throw New ArgumentNullException(NameOf(entity))
            _context.Set(Of T)().Add(entity)
            _context.SaveChanges()
        End Sub

        Public Async Function AddAsync(Of T As Class)(entity As T) As Task
            If entity Is Nothing Then Throw New ArgumentNullException(NameOf(entity))
            Await _context.Set(Of T)().AddAsync(entity)
            Await _context.SaveChangesAsync()
        End Function

        Public Sub Update(Of T As Class)(entity As T)
            If entity Is Nothing Then Throw New ArgumentNullException(NameOf(entity))
            _context.Set(Of T)().Update(entity)
            _context.SaveChanges()
        End Sub

        Public Async Function UpdateAsync(Of T As Class)(entity As T) As Task
            If entity Is Nothing Then Throw New ArgumentNullException(NameOf(entity))
            _context.Set(Of T)().Update(entity)
            Await _context.SaveChangesAsync()
        End Function

        Public Sub SaveChanges()
            _context.SaveChanges()
        End Sub

        Public Async Function SaveChangesAsync() As Task
            Await _context.SaveChangesAsync()
        End Function

    End Class

End Namespace
