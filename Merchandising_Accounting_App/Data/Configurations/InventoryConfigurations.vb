Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Metadata.Builders
Imports Merchandising_Accounting_App.Domain.Models

Namespace Data.Configurations

    Public Class ProductConfiguration
        Implements IEntityTypeConfiguration(Of Product)

        Public Sub Configure(builder As EntityTypeBuilder(Of Product)) Implements IEntityTypeConfiguration(Of Product).Configure
            builder.ToTable("Products", Sub(t)
                                           t.HasCheckConstraint("CK_Products_TotalQtyOnHand", "TotalQtyOnHand >= 0")
                                       End Sub)

            builder.HasKey(Function(p) p.ProductId)

            builder.Property(Function(p) p.SKU) _
                .HasMaxLength(50) _
                .IsRequired()

            builder.HasIndex(Function(p) p.SKU) _
                .IsUnique()

            builder.Property(Function(p) p.Name) _
                .HasMaxLength(150) _
                .IsRequired()

            builder.Property(Function(p) p.BaseUnitName) _
                .HasMaxLength(20) _
                .IsRequired()

            builder.Property(Function(p) p.PackUnitName) _
                .HasMaxLength(20)

            builder.Property(Function(p) p.PackMultiplier) _
                .HasDefaultValue(1)

            builder.Property(Function(p) p.CurrentUnitCost) _
                .HasPrecision(18, 4) _
                .HasDefaultValue(0D)

            builder.Property(Function(p) p.TotalQtyOnHand) _
                .HasDefaultValue(0)

            builder.Property(Function(p) p.AllocatedQty) _
                .HasDefaultValue(0)

            builder.Property(Function(p) p.QuarantineQty) _
                .HasDefaultValue(0)

            builder.Property(Function(p) p.IsActive) _
                .HasDefaultValue(True)
        End Sub
    End Class

    Public Class InventoryTransactionConfiguration
        Implements IEntityTypeConfiguration(Of InventoryTransaction)

        Public Sub Configure(builder As EntityTypeBuilder(Of InventoryTransaction)) Implements IEntityTypeConfiguration(Of InventoryTransaction).Configure
            builder.ToTable("InventoryTransactions")
            builder.HasKey(Function(t) t.TransactionId)

            builder.Property(Function(t) t.SourceDocType) _
                .HasMaxLength(30) _
                .IsRequired()

            builder.Property(Function(t) t.OperationalEventDate).IsRequired()
            builder.Property(Function(t) t.PostingDate).IsRequired()

            builder.Property(Function(t) t.UnitCost) _
                .HasPrecision(18, 4)

            builder.Property(Function(t) t.RunningUnitCost) _
                .HasPrecision(18, 4)

            builder.HasOne(Function(t) t.Product) _
                .WithMany(Function(p) p.InventoryTransactions) _
                .HasForeignKey(Function(t) t.ProductId) _
                .OnDelete(DeleteBehavior.Restrict)
        End Sub
    End Class

    Public Class InventoryAdjustmentConfiguration
        Implements IEntityTypeConfiguration(Of InventoryAdjustment)

        Public Sub Configure(builder As EntityTypeBuilder(Of InventoryAdjustment)) Implements IEntityTypeConfiguration(Of InventoryAdjustment).Configure
            builder.ToTable("InventoryAdjustments")
            builder.HasKey(Function(a) a.AdjustmentId)

            builder.Property(Function(a) a.DocumentNumber) _
                .HasMaxLength(30) _
                .IsRequired()

            builder.HasIndex(Function(a) a.DocumentNumber) _
                .IsUnique()

            builder.Property(Function(a) a.ReasonCode) _
                .HasMaxLength(50)

            builder.Property(Function(a) a.Status) _
                .HasMaxLength(20)

            builder.HasMany(Function(a) a.Lines) _
                .WithOne(Function(l) l.Adjustment) _
                .HasForeignKey(Function(l) l.AdjustmentId) _
                .OnDelete(DeleteBehavior.Cascade)
        End Sub
    End Class

    Public Class InventoryAdjustmentLineConfiguration
        Implements IEntityTypeConfiguration(Of InventoryAdjustmentLine)

        Public Sub Configure(builder As EntityTypeBuilder(Of InventoryAdjustmentLine)) Implements IEntityTypeConfiguration(Of InventoryAdjustmentLine).Configure
            builder.ToTable("InventoryAdjustmentLines")
            builder.HasKey(Function(l) l.AdjustmentLineId)

            builder.Property(Function(l) l.UnitCost) _
                .HasPrecision(18, 4)

            builder.Property(Function(l) l.Notes) _
                .HasMaxLength(255)

            builder.HasOne(Function(l) l.Product) _
                .WithMany(Function(p) p.AdjustmentLines) _
                .HasForeignKey(Function(l) l.ProductId) _
                .OnDelete(DeleteBehavior.Restrict)

            builder.HasOne(Function(l) l.TargetAccount) _
                .WithMany() _
                .HasForeignKey(Function(l) l.TargetAccountId) _
                .OnDelete(DeleteBehavior.Restrict)
        End Sub
    End Class

End Namespace
