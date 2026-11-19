Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Metadata.Builders
Imports Merchandising_Accounting_App.Domain.Models

Namespace Data.Configurations

    Public Class CustomerReturnConfiguration
        Implements IEntityTypeConfiguration(Of CustomerReturn)

        Public Sub Configure(builder As EntityTypeBuilder(Of CustomerReturn)) Implements IEntityTypeConfiguration(Of CustomerReturn).Configure
            builder.ToTable("CustomerReturns")
            builder.HasKey(Function(r) r.ReturnId)

            builder.Property(Function(r) r.DocumentNumber) _
                .HasMaxLength(30) _
                .IsRequired()

            builder.HasIndex(Function(r) r.DocumentNumber) _
                .IsUnique()

            builder.Property(Function(r) r.Status) _
                .HasMaxLength(20)

            builder.HasOne(Function(r) r.Customer) _
                .WithMany(Function(c) c.CustomerReturns) _
                .HasForeignKey(Function(r) r.CustomerId) _
                .OnDelete(DeleteBehavior.Restrict)

            builder.HasOne(Function(r) r.OriginalInvoice) _
                .WithMany(Function(i) i.CustomerReturns) _
                .HasForeignKey(Function(r) r.OriginalInvoiceId) _
                .OnDelete(DeleteBehavior.Restrict)

            builder.HasMany(Function(r) r.Lines) _
                .WithOne(Function(l) l.CustomerReturn) _
                .HasForeignKey(Function(l) l.ReturnId) _
                .OnDelete(DeleteBehavior.Cascade)
        End Sub
    End Class

    Public Class CustomerReturnLineConfiguration
        Implements IEntityTypeConfiguration(Of CustomerReturnLine)

        Public Sub Configure(builder As EntityTypeBuilder(Of CustomerReturnLine)) Implements IEntityTypeConfiguration(Of CustomerReturnLine).Configure
            builder.ToTable("CustomerReturnLines")
            builder.HasKey(Function(l) l.ReturnLineId)

            builder.Property(Function(l) l.OriginalUnitCost) _
                .HasPrecision(18, 4)

            builder.Property(Function(l) l.SellingUnitPrice) _
                .HasPrecision(18, 4)

            builder.Property(Function(l) l.DispositionStatus) _
                .HasMaxLength(20)

            builder.HasOne(Function(l) l.Product) _
                .WithMany() _
                .HasForeignKey(Function(l) l.ProductId) _
                .OnDelete(DeleteBehavior.Restrict)
        End Sub
    End Class

End Namespace
