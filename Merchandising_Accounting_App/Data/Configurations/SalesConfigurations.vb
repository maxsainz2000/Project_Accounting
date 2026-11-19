Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Metadata.Builders
Imports Merchandising_Accounting_App.Domain.Models

Namespace Data.Configurations

    Public Class CustomerConfiguration
        Implements IEntityTypeConfiguration(Of Customer)

        Public Sub Configure(builder As EntityTypeBuilder(Of Customer)) Implements IEntityTypeConfiguration(Of Customer).Configure
            builder.ToTable("Customers")
            builder.HasKey(Function(c) c.CustomerId)

            builder.Property(Function(c) c.Code) _
                .HasMaxLength(20) _
                .IsRequired()

            builder.HasIndex(Function(c) c.Code) _
                .IsUnique()

            builder.Property(Function(c) c.Name) _
                .HasMaxLength(150) _
                .IsRequired()

            builder.Property(Function(c) c.CreditLimit) _
                .HasPrecision(18, 4) _
                .HasDefaultValue(0D)

            builder.Property(Function(c) c.CurrentARBalance) _
                .HasPrecision(18, 4) _
                .HasDefaultValue(0D)
        End Sub
    End Class

    Public Class SalesOrderConfiguration
        Implements IEntityTypeConfiguration(Of SalesOrder)

        Public Sub Configure(builder As EntityTypeBuilder(Of SalesOrder)) Implements IEntityTypeConfiguration(Of SalesOrder).Configure
            builder.ToTable("SalesOrders")
            builder.HasKey(Function(s) s.SOId)

            builder.Property(Function(s) s.DocumentNumber) _
                .HasMaxLength(30) _
                .IsRequired()

            builder.HasIndex(Function(s) s.DocumentNumber) _
                .IsUnique()

            builder.Property(Function(s) s.Status) _
                .HasMaxLength(20)

            builder.HasOne(Function(s) s.Customer) _
                .WithMany(Function(c) c.SalesOrders) _
                .HasForeignKey(Function(s) s.CustomerId) _
                .OnDelete(DeleteBehavior.Restrict)

            builder.HasMany(Function(s) s.Lines) _
                .WithOne(Function(l) l.SalesOrder) _
                .HasForeignKey(Function(l) l.SOId) _
                .OnDelete(DeleteBehavior.Cascade)
        End Sub
    End Class

    Public Class SOLineConfiguration
        Implements IEntityTypeConfiguration(Of SOLine)

        Public Sub Configure(builder As EntityTypeBuilder(Of SOLine)) Implements IEntityTypeConfiguration(Of SOLine).Configure
            builder.ToTable("SOLines")
            builder.HasKey(Function(l) l.SOLineId)

            builder.Property(Function(l) l.UnitPrice) _
                .HasPrecision(18, 4)

            builder.HasOne(Function(l) l.Product) _
                .WithMany() _
                .HasForeignKey(Function(l) l.ProductId) _
                .OnDelete(DeleteBehavior.Restrict)
        End Sub
    End Class

    Public Class SalesInvoiceConfiguration
        Implements IEntityTypeConfiguration(Of SalesInvoice)

        Public Sub Configure(builder As EntityTypeBuilder(Of SalesInvoice)) Implements IEntityTypeConfiguration(Of SalesInvoice).Configure
            builder.ToTable("SalesInvoices")
            builder.HasKey(Function(i) i.InvoiceId)

            builder.Property(Function(i) i.DocumentNumber) _
                .HasMaxLength(30) _
                .IsRequired()

            builder.HasIndex(Function(i) i.DocumentNumber) _
                .IsUnique()

            builder.Property(Function(i) i.Status) _
                .HasMaxLength(20)

            builder.Property(Function(i) i.TotalAmount) _
                .HasPrecision(18, 4)

            builder.HasOne(Function(i) i.Customer) _
                .WithMany(Function(c) c.SalesInvoices) _
                .HasForeignKey(Function(i) i.CustomerId) _
                .OnDelete(DeleteBehavior.Restrict)

            builder.HasOne(Function(i) i.SalesOrder) _
                .WithMany(Function(s) s.SalesInvoices) _
                .HasForeignKey(Function(i) i.SOId) _
                .IsRequired(False) _
                .OnDelete(DeleteBehavior.Restrict)

            builder.HasMany(Function(i) i.Lines) _
                .WithOne(Function(l) l.SalesInvoice) _
                .HasForeignKey(Function(l) l.InvoiceId) _
                .OnDelete(DeleteBehavior.Cascade)
        End Sub
    End Class

    Public Class SalesInvoiceLineConfiguration
        Implements IEntityTypeConfiguration(Of SalesInvoiceLine)

        Public Sub Configure(builder As EntityTypeBuilder(Of SalesInvoiceLine)) Implements IEntityTypeConfiguration(Of SalesInvoiceLine).Configure
            builder.ToTable("SalesInvoiceLines")
            builder.HasKey(Function(l) l.InvoiceLineId)

            builder.Property(Function(l) l.UnitPrice) _
                .HasPrecision(18, 4)

            builder.Property(Function(l) l.InvoicedCOGSUnitCost) _
                .HasPrecision(18, 4)

            builder.HasOne(Function(l) l.Product) _
                .WithMany() _
                .HasForeignKey(Function(l) l.ProductId) _
                .OnDelete(DeleteBehavior.Restrict)

            builder.HasOne(Function(l) l.SOLine) _
                .WithMany(Function(s) s.InvoiceLines) _
                .HasForeignKey(Function(l) l.SOLineId) _
                .IsRequired(False) _
                .OnDelete(DeleteBehavior.Restrict)
        End Sub
    End Class

    Public Class PaymentReceiptConfiguration
        Implements IEntityTypeConfiguration(Of PaymentReceipt)

        Public Sub Configure(builder As EntityTypeBuilder(Of PaymentReceipt)) Implements IEntityTypeConfiguration(Of PaymentReceipt).Configure
            builder.ToTable("PaymentReceipts")
            builder.HasKey(Function(r) r.ReceiptId)

            builder.Property(Function(r) r.AmountReceived) _
                .HasPrecision(18, 4)

            builder.HasOne(Function(r) r.Customer) _
                .WithMany(Function(c) c.PaymentReceipts) _
                .HasForeignKey(Function(r) r.CustomerId) _
                .OnDelete(DeleteBehavior.Restrict)

            builder.HasOne(Function(r) r.SalesInvoice) _
                .WithMany(Function(i) i.PaymentReceipts) _
                .HasForeignKey(Function(r) r.InvoiceId) _
                .IsRequired(False) _
                .OnDelete(DeleteBehavior.Restrict)

            builder.HasOne(Function(r) r.BankAccount) _
                .WithMany() _
                .HasForeignKey(Function(r) r.BankAccountId) _
                .OnDelete(DeleteBehavior.Restrict)
        End Sub
    End Class

End Namespace
