Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Metadata.Builders
Imports Merchandising_Accounting_App.Domain.Models

Namespace Data.Configurations

    Public Class AccountConfiguration
        Implements IEntityTypeConfiguration(Of Account)

        Public Sub Configure(builder As EntityTypeBuilder(Of Account)) Implements IEntityTypeConfiguration(Of Account).Configure
            builder.ToTable("Accounts")
            builder.HasKey(Function(a) a.AccountId)

            builder.Property(Function(a) a.Code) _
                .HasMaxLength(10) _
                .IsRequired()

            builder.HasIndex(Function(a) a.Code) _
                .IsUnique()

            builder.Property(Function(a) a.Name) _
                .HasMaxLength(100) _
                .IsRequired()

            builder.Property(Function(a) a.AccountType) _
                .HasConversion(Of Integer)() _
                .IsRequired()

            builder.Property(Function(a) a.IsActive) _
                .HasDefaultValue(True)
        End Sub
    End Class

    Public Class AccountingPeriodConfiguration
        Implements IEntityTypeConfiguration(Of AccountingPeriod)

        Public Sub Configure(builder As EntityTypeBuilder(Of AccountingPeriod)) Implements IEntityTypeConfiguration(Of AccountingPeriod).Configure
            builder.ToTable("AccountingPeriods")
            builder.HasKey(Function(p) p.PeriodId)

            builder.Property(Function(p) p.FiscalYear).IsRequired()
            builder.Property(Function(p) p.PeriodNumber).IsRequired()
            builder.Property(Function(p) p.StartDate).IsRequired()
            builder.Property(Function(p) p.EndDate).IsRequired()
            builder.Property(Function(p) p.IsClosed).HasDefaultValue(False)

            builder.HasIndex(Function(p) New With {p.FiscalYear, p.PeriodNumber}) _
                .IsUnique()
        End Sub
    End Class

    Public Class JournalEntryConfiguration
        Implements IEntityTypeConfiguration(Of JournalEntry)

        Public Sub Configure(builder As EntityTypeBuilder(Of JournalEntry)) Implements IEntityTypeConfiguration(Of JournalEntry).Configure
            builder.ToTable("JournalEntries")
            builder.HasKey(Function(j) j.EntryId)

            builder.Property(Function(j) j.SourceDocType) _
                .HasMaxLength(30) _
                .IsRequired()

            builder.Property(Function(j) j.PostingDate).IsRequired()
            builder.Property(Function(j) j.OperationalEventDate).IsRequired()

            builder.Property(Function(j) j.Description) _
                .HasMaxLength(255)

            builder.Property(Function(j) j.CreatedBy) _
                .HasMaxLength(50)

            builder.HasOne(Function(j) j.Period) _
                .WithMany(Function(p) p.JournalEntries) _
                .HasForeignKey(Function(j) j.PeriodId) _
                .OnDelete(DeleteBehavior.Restrict)

            builder.HasMany(Function(j) j.Lines) _
                .WithOne(Function(l) l.JournalEntry) _
                .HasForeignKey(Function(l) l.EntryId) _
                .OnDelete(DeleteBehavior.Cascade)
        End Sub
    End Class

    Public Class JournalEntryLineConfiguration
        Implements IEntityTypeConfiguration(Of JournalEntryLine)

        Public Sub Configure(builder As EntityTypeBuilder(Of JournalEntryLine)) Implements IEntityTypeConfiguration(Of JournalEntryLine).Configure
            builder.ToTable("JournalEntryLines")
            builder.HasKey(Function(l) l.LineId)

            builder.Property(Function(l) l.DebitAmount) _
                .HasPrecision(18, 4) _
                .HasDefaultValue(0D)

            builder.Property(Function(l) l.CreditAmount) _
                .HasPrecision(18, 4) _
                .HasDefaultValue(0D)

            builder.Property(Function(l) l.LineMemo) _
                .HasMaxLength(255)

            builder.HasOne(Function(l) l.Account) _
                .WithMany(Function(a) a.JournalEntryLines) _
                .HasForeignKey(Function(l) l.AccountId) _
                .OnDelete(DeleteBehavior.Restrict)
        End Sub
    End Class

End Namespace
