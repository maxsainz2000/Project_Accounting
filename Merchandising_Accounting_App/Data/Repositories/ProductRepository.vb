Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports Microsoft.Data.Sqlite
Imports Microsoft.EntityFrameworkCore
Imports Merchandising_Accounting_App.Domain
Imports Merchandising_Accounting_App.Domain.Models

Namespace Data.Repositories

    ''' <summary>
    ''' Repository for Product catalog management and atomic concurrency-safe inventory operations.
    ''' Enforces the zero negative stock invariant via atomic conditional SQL decrement.
    ''' </summary>
    Public Class ProductRepository

        Private ReadOnly _context As ErpDbContext

        Public Sub New(context As ErpDbContext)
            If context Is Nothing Then
                Throw New ArgumentNullException(NameOf(context))
            End If
            _context = context
        End Sub

        Public Function GetById(productId As Integer) As Product
            Return _context.Products.Find(productId)
        End Function

        Public Async Function GetByIdAsync(productId As Integer) As Task(Of Product)
            Return Await _context.Products.FindAsync(productId)
        End Function

        Public Function GetBySku(sku As String) As Product
            If String.IsNullOrWhiteSpace(sku) Then Return Nothing
            Return _context.Products.FirstOrDefault(Function(p) p.SKU = sku)
        End Function

        Public Async Function GetBySkuAsync(sku As String) As Task(Of Product)
            If String.IsNullOrWhiteSpace(sku) Then Return Nothing
            Return Await _context.Products.FirstOrDefaultAsync(Function(p) p.SKU = sku)
        End Function

        Public Function GetAll() As List(Of Product)
            Return _context.Products.OrderBy(Function(p) p.SKU).ToList()
        End Function

        Public Async Function GetAllAsync() As Task(Of List(Of Product))
            Return Await _context.Products.OrderBy(Function(p) p.SKU).ToListAsync()
        End Function

        Public Function GetActiveProducts() As List(Of Product)
            Return _context.Products.Where(Function(p) p.IsActive).OrderBy(Function(p) p.SKU).ToList()
        End Function

        Public Async Function GetActiveProductsAsync() As Task(Of List(Of Product))
            Return Await _context.Products.Where(Function(p) p.IsActive).OrderBy(Function(p) p.SKU).ToListAsync()
        End Function

        Public Sub Add(product As Product)
            If product Is Nothing Then Throw New ArgumentNullException(NameOf(product))
            _context.Products.Add(product)
            _context.SaveChanges()
        End Sub

        Public Async Function AddAsync(product As Product) As Task
            If product Is Nothing Then Throw New ArgumentNullException(NameOf(product))
            Await _context.Products.AddAsync(product)
            Await _context.SaveChangesAsync()
        End Function

        Public Sub Update(product As Product)
            If product Is Nothing Then Throw New ArgumentNullException(NameOf(product))
            _context.Products.Update(product)
            _context.SaveChanges()
        End Sub

        Public Async Function UpdateAsync(product As Product) As Task
            If product Is Nothing Then Throw New ArgumentNullException(NameOf(product))
            _context.Products.Update(product)
            Await _context.SaveChangesAsync()
        End Function

        ''' <summary>
        ''' Executes an atomic conditional update at the SQLite database layer to decrement physical stock.
        ''' If (TotalQtyOnHand &lt; qty), no rows are updated and the function returns False.
        ''' </summary>
        Public Function DecrementStock(productId As Integer, qty As Integer) As Boolean
            If qty <= 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(qty), "Quantity to decrement must be greater than zero.")
            End If

            Dim pQty As New SqliteParameter("@Qty", qty)
            Dim pId As New SqliteParameter("@Id", productId)

            Dim rowsAffected = _context.Database.ExecuteSqlRaw(
                "UPDATE Products SET TotalQtyOnHand = TotalQtyOnHand - @Qty WHERE ProductId = @Id AND TotalQtyOnHand >= @Qty;",
                pQty, pId)

            If rowsAffected > 0 Then
                ' Keep EF Core change tracker in sync if the entity is currently loaded
                Dim tracked = _context.ChangeTracker.Entries(Of Product)().FirstOrDefault(Function(e) e.Entity.ProductId = productId)
                If tracked IsNot Nothing Then
                    tracked.Reload()
                End If
                Return True
            End If

            Return False
        End Function

        ''' <summary>
        ''' Asynchronously executes an atomic conditional update at the SQLite database layer to decrement physical stock.
        ''' </summary>
        Public Async Function DecrementStockAsync(productId As Integer, qty As Integer) As Task(Of Boolean)
            If qty <= 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(qty), "Quantity to decrement must be greater than zero.")
            End If

            Dim pQty As New SqliteParameter("@Qty", qty)
            Dim pId As New SqliteParameter("@Id", productId)

            Dim rowsAffected = Await _context.Database.ExecuteSqlRawAsync(
                "UPDATE Products SET TotalQtyOnHand = TotalQtyOnHand - @Qty WHERE ProductId = @Id AND TotalQtyOnHand >= @Qty;",
                pQty, pId)

            If rowsAffected > 0 Then
                Dim tracked = _context.ChangeTracker.Entries(Of Product)().FirstOrDefault(Function(e) e.Entity.ProductId = productId)
                If tracked IsNot Nothing Then
                    Await tracked.ReloadAsync()
                End If
                Return True
            End If

            Return False
        End Function

        ''' <summary>
        ''' Decrements stock atomically or raises an InsufficientStockException if available inventory is insufficient.
        ''' </summary>
        Public Function DecrementStockOrThrow(productId As Integer, qty As Integer) As Boolean
            If Not DecrementStock(productId, qty) Then
                Dim current = GetById(productId)
                Dim currentQty = If(current IsNot Nothing, current.TotalQtyOnHand, 0)
                Throw New InsufficientStockException(productId, qty, currentQty)
            End If
            Return True
        End Function

        ''' <summary>
        ''' Asynchronously decrements stock atomically or raises an InsufficientStockException if available inventory is insufficient.
        ''' </summary>
        Public Async Function DecrementStockOrThrowAsync(productId As Integer, qty As Integer) As Task(Of Boolean)
            Dim success = Await DecrementStockAsync(productId, qty)
            If Not success Then
                Dim current = Await GetByIdAsync(productId)
                Dim currentQty = If(current IsNot Nothing, current.TotalQtyOnHand, 0)
                Throw New InsufficientStockException(productId, qty, currentQty)
            End If
            Return True
        End Function

        Public Sub AddTransaction(transaction As InventoryTransaction)
            If transaction Is Nothing Then Throw New ArgumentNullException(NameOf(transaction))
            _context.InventoryTransactions.Add(transaction)
            _context.SaveChanges()
        End Sub

        Public Async Function AddTransactionAsync(transaction As InventoryTransaction) As Task
            If transaction Is Nothing Then Throw New ArgumentNullException(NameOf(transaction))
            Await _context.InventoryTransactions.AddAsync(transaction)
            Await _context.SaveChangesAsync()
        End Function

        Public Function GetTransactionsForProduct(productId As Integer) As List(Of InventoryTransaction)
            Return _context.InventoryTransactions _
                .Where(Function(t) t.ProductId = productId) _
                .OrderBy(Function(t) t.PostingDate) _
                .ThenBy(Function(t) t.TransactionId) _
                .ToList()
        End Function

        Public Async Function GetTransactionsForProductAsync(productId As Integer) As Task(Of List(Of InventoryTransaction))
            Return Await _context.InventoryTransactions _
                .Where(Function(t) t.ProductId = productId) _
                .OrderBy(Function(t) t.PostingDate) _
                .ThenBy(Function(t) t.TransactionId) _
                .ToListAsync()
        End Function

        Public Sub SaveChanges()
            _context.SaveChanges()
        End Sub

        Public Async Function SaveChangesAsync() As Task
            Await _context.SaveChangesAsync()
        End Function

    End Class

End Namespace
