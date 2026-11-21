Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Metadata.Builders
Imports Merchandising_Accounting_App.Domain.Models

Namespace Data.Configurations

    Public Class VendorConfiguration
        Implements IEntityTypeConfiguration(Of Vendor)

        Public Sub Configure(builder As EntityTypeBuilder(Of Vendor)) Implements IEntityTypeConfiguration(Of Vendor).Configure
            builder.ToTable("Vendors")
            builder.HasKey(Function(v) v.VendorId)

            builder.Property(Function(v) v.Code) _
                .HasMaxLength(20) _
                .IsRequired()

            builder.HasIndex(Function(v) v.Code) _
                .IsUnique()

            builder.Property(Function(v) v.Name) _
                .HasMaxLength(150) _
                .IsRequired()

            builder.Property(Function(v) v.CurrentPayableBalance) _
                .HasPrecision(18, 4) _
                .HasDefaultValue(0D)
        End Sub
    End Class

    Public Class PurchaseOrderConfiguration
        Implements IEntityTypeConfiguration(Of PurchaseOrder)

        Public Sub Configure(builder As EntityTypeBuilder(Of PurchaseOrder)) Implements IEntityTypeConfiguration(Of PurchaseOrder).Configure
            builder.ToTable("PurchaseOrders")
            builder.HasKey(Function(p) p.POId)

            builder.Property(Function(p) p.DocumentNumber) _
                .HasMaxLength(30) _
                .IsRequired()

            builder.HasIndex(Function(p) p.DocumentNumber) _
                .IsUnique()

            builder.Property(Function(p) p.Status) _
                .HasMaxLength(20)

            builder.Property(Function(p) p.TotalAmount) _
                .HasPrecision(18, 4) _
                .HasDefaultValue(0D)

            builder.HasOne(Function(p) p.Vendor) _
                .WithMany(Function(v) v.PurchaseOrders) _
                .HasForeignKey(Function(p) p.VendorId) _
                .OnDelete(DeleteBehavior.Restrict)

            builder.HasMany(Function(p) p.Lines) _
                .WithOne(Function(l) l.PurchaseOrder) _
                .HasForeignKey(Function(l) l.POId) _
                .OnDelete(DeleteBehavior.Cascade)
        End Sub
    End Class

    Public Class POLineConfiguration
        Implements IEntityTypeConfiguration(Of POLine)

        Public Sub Configure(builder As EntityTypeBuilder(Of POLine)) Implements IEntityTypeConfiguration(Of POLine).Configure
            builder.ToTable("POLines")
            builder.HasKey(Function(l) l.POLineId)

            builder.Property(Function(l) l.UnitPrice) _
                .HasPrecision(18, 4)

            builder.HasOne(Function(l) l.Product) _
                .WithMany() _
                .HasForeignKey(Function(l) l.ProductId) _
                .OnDelete(DeleteBehavior.Restrict)
        End Sub
    End Class

    Public Class GoodsReceiptNoteConfiguration
        Implements IEntityTypeConfiguration(Of GoodsReceiptNote)

        Public Sub Configure(builder As EntityTypeBuilder(Of GoodsReceiptNote)) Implements IEntityTypeConfiguration(Of GoodsReceiptNote).Configure
            builder.ToTable("GoodsReceiptNotes")
            builder.HasKey(Function(g) g.GRNId)

            builder.Property(Function(g) g.DocumentNumber) _
                .HasMaxLength(30) _
                .IsRequired()

            builder.HasIndex(Function(g) g.DocumentNumber) _
                .IsUnique()

            builder.Property(Function(g) g.Status) _
                .HasMaxLength(20)

            builder.HasOne(Function(g) g.PurchaseOrder) _
                .WithMany(Function(p) p.GoodsReceiptNotes) _
                .HasForeignKey(Function(g) g.POId) _
                .OnDelete(DeleteBehavior.Restrict)

            builder.HasMany(Function(g) g.Lines) _
                .WithOne(Function(l) l.GoodsReceiptNote) _
                .HasForeignKey(Function(l) l.GRNId) _
                .OnDelete(DeleteBehavior.Cascade)
        End Sub
    End Class

    Public Class GRNLineConfiguration
        Implements IEntityTypeConfiguration(Of GRNLine)

        Public Sub Configure(builder As EntityTypeBuilder(Of GRNLine)) Implements IEntityTypeConfiguration(Of GRNLine).Configure
            builder.ToTable("GRNLines")
            builder.HasKey(Function(l) l.GRNLineId)

            builder.Property(Function(l) l.POUnitPrice) _
                .HasPrecision(18, 4)

            builder.HasOne(Function(l) l.POLine) _
                .WithMany(Function(p) p.GRNLines) _
                .HasForeignKey(Function(l) l.POLineId) _
                .OnDelete(DeleteBehavior.Restrict)

            builder.HasOne(Function(l) l.Product) _
                .WithMany() _
                .HasForeignKey(Function(l) l.ProductId) _
                .OnDelete(DeleteBehavior.Restrict)
        End Sub
    End Class

    Public Class SupplierBillConfiguration
        Implements IEntityTypeConfiguration(Of SupplierBill)

        Public Sub Configure(builder As EntityTypeBuilder(Of SupplierBill)) Implements IEntityTypeConfiguration(Of SupplierBill).Configure
            builder.ToTable("SupplierBills")
            builder.HasKey(Function(b) b.BillId)

            builder.Property(Function(b) b.DocumentNumber) _
                .HasMaxLength(30) _
                .IsRequired()

            builder.HasIndex(Function(b) b.DocumentNumber) _
                .IsUnique()

            builder.Property(Function(b) b.Status) _
                .HasMaxLength(20)

            builder.Property(Function(b) b.TotalAmount) _
                .HasPrecision(18, 4)

            builder.HasOne(Function(b) b.Vendor) _
                .WithMany(Function(v) v.SupplierBills) _
                .HasForeignKey(Function(b) b.VendorId) _
                .OnDelete(DeleteBehavior.Restrict)

            builder.HasMany(Function(b) b.Lines) _
                .WithOne(Function(l) l.SupplierBill) _
                .HasForeignKey(Function(l) l.BillId) _
                .OnDelete(DeleteBehavior.Cascade)
        End Sub
    End Class

    Public Class BillLineConfiguration
        Implements IEntityTypeConfiguration(Of BillLine)

        Public Sub Configure(builder As EntityTypeBuilder(Of BillLine)) Implements IEntityTypeConfiguration(Of BillLine).Configure
            builder.ToTable("BillLines")
            builder.HasKey(Function(l) l.BillLineId)

            builder.Property(Function(l) l.BilledUnitPrice) _
                .HasPrecision(18, 4)

            builder.Property(Function(l) l.LineTotal) _
                .HasPrecision(18, 4)

            builder.Property(Function(l) l.FreightAmount) _
                .HasPrecision(18, 4) _
                .HasDefaultValue(0D)

            builder.HasOne(Function(l) l.GRNLine) _
                .WithMany(Function(g) g.BillLines) _
                .HasForeignKey(Function(l) l.GRNLineId) _
                .IsRequired(False) _
                .OnDelete(DeleteBehavior.Restrict)

            builder.HasOne(Function(l) l.Product) _
                .WithMany() _
                .HasForeignKey(Function(l) l.ProductId) _
                .IsRequired(False) _
                .OnDelete(DeleteBehavior.Restrict)

            builder.HasOne(Function(l) l.Account) _
                .WithMany() _
                .HasForeignKey(Function(l) l.AccountId) _
                .IsRequired(False) _
                .OnDelete(DeleteBehavior.Restrict)
        End Sub
    End Class

    Public Class PaymentVoucherConfiguration
        Implements IEntityTypeConfiguration(Of PaymentVoucher)

        Public Sub Configure(builder As EntityTypeBuilder(Of PaymentVoucher)) Implements IEntityTypeConfiguration(Of PaymentVoucher).Configure
            builder.ToTable("PaymentVouchers")
            builder.HasKey(Function(v) v.VoucherId)

            builder.Property(Function(v) v.VoucherNumber) _
                .HasMaxLength(30) _
                .IsRequired()

            builder.HasIndex(Function(v) v.VoucherNumber) _
                .IsUnique()

            builder.Property(Function(v) v.VoucherType) _
                .HasMaxLength(20)

            builder.Property(Function(v) v.DisbursedAmount) _
                .HasPrecision(18, 4)

            builder.HasOne(Function(v) v.Vendor) _
                .WithMany(Function(vnd) vnd.PaymentVouchers) _
                .HasForeignKey(Function(v) v.VendorId) _
                .IsRequired(False) _
                .OnDelete(DeleteBehavior.Restrict)

            builder.HasOne(Function(v) v.Customer) _
                .WithMany() _
                .HasForeignKey(Function(v) v.CustomerId) _
                .IsRequired(False) _
                .OnDelete(DeleteBehavior.Restrict)

            builder.HasOne(Function(v) v.BankAccount) _
                .WithMany() _
                .HasForeignKey(Function(v) v.BankAccountId) _
                .OnDelete(DeleteBehavior.Restrict)
        End Sub
    End Class

    Public Class GRNIAdjustmentConfiguration
        Implements IEntityTypeConfiguration(Of GRNIAdjustment)

        Public Sub Configure(builder As EntityTypeBuilder(Of GRNIAdjustment)) Implements IEntityTypeConfiguration(Of GRNIAdjustment).Configure
            builder.ToTable("GRNIAdjustments")
            builder.HasKey(Function(a) a.AdjustmentId)

            builder.Property(Function(a) a.DocumentNumber) _
                .HasMaxLength(30) _
                .IsRequired()

            builder.HasIndex(Function(a) a.DocumentNumber) _
                .IsUnique()

            builder.Property(Function(a) a.Status) _
                .HasMaxLength(20)

            builder.Property(Function(a) a.Reason) _
                .HasMaxLength(255) _
                .IsRequired()

            builder.Property(Function(a) a.ClearedAmount) _
                .HasPrecision(18, 4)

            builder.HasOne(Function(a) a.GoodsReceiptLine) _
                .WithMany(Function(g) g.GRNIAdjustments) _
                .HasForeignKey(Function(a) a.GoodsReceiptLineId) _
                .OnDelete(DeleteBehavior.Restrict)
        End Sub
    End Class

End Namespace
