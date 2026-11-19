Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Linq
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Merchandising_Accounting_App.Data
Imports Merchandising_Accounting_App.Domain
Imports Merchandising_Accounting_App.Domain.Models
Imports Merchandising_Accounting_App.Services
Imports Merchandising_Accounting_App.Tests.Infrastructure

Namespace Services

    <TestClass>
    Public Class PeriodServiceTests

        <TestMethod>
        Public Sub ValidatePostingDate_InActiveOpenPeriod_ReturnsPeriod()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim service As New PeriodService(context)

                    ' Seeded initial period is FY2026-P11: 2026-11-01 to 2026-11-30 (Open)
                    Dim postingDate As New DateTime(2026, 11, 15)
                    Dim period As AccountingPeriod = service.ValidatePostingDate(postingDate)

                    Assert.IsNotNull(period)
                    Assert.AreEqual(2026, period.FiscalYear)
                    Assert.AreEqual(11, period.PeriodNumber)
                    Assert.IsFalse(period.IsClosed)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub ValidatePostingDate_InClosedPeriod_ThrowsClosedPeriodPostingException()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim service As New PeriodService(context)

                    Dim period = context.AccountingPeriods.First(Function(p) p.FiscalYear = 2026 AndAlso p.PeriodNumber = 11)
                    service.CloseFiscalPeriod(period.PeriodId)

                    Dim postingDate As New DateTime(2026, 11, 15)
                    Dim caughtException As ClosedPeriodPostingException = Nothing

                    Try
                        service.ValidatePostingDate(postingDate)
                    Catch ex As ClosedPeriodPostingException
                        caughtException = ex
                    End Try

                    Assert.IsNotNull(caughtException, "Expected ClosedPeriodPostingException was not thrown.")
                    Assert.AreEqual(period.PeriodId, caughtException.PeriodId.Value)
                    Assert.AreEqual(postingDate, caughtException.PostingDate.Value)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub ValidatePostingDate_OutsideConfiguredPeriods_ThrowsClosedPeriodPostingException()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim service As New PeriodService(context)

                    Dim unconfiguredDate As New DateTime(2025, 1, 1)
                    Dim caughtException As ClosedPeriodPostingException = Nothing

                    Try
                        service.ValidatePostingDate(unconfiguredDate)
                    Catch ex As ClosedPeriodPostingException
                        caughtException = ex
                    End Try

                    Assert.IsNotNull(caughtException, "Expected ClosedPeriodPostingException was not thrown.")
                    Assert.AreEqual(unconfiguredDate, caughtException.PostingDate.Value)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub GetActiveFiscalPeriod_ReturnsEarliestOpenPeriod()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim service As New PeriodService(context)

                    ' Add a second open period FY2026-P12
                    service.CreatePeriod(2026, 12, New DateTime(2026, 12, 1), New DateTime(2026, 12, 31, 23, 59, 59))

                    Dim active = service.GetActiveFiscalPeriod()
                    Assert.IsNotNull(active)
                    Assert.AreEqual(2026, active.FiscalYear)
                    Assert.AreEqual(11, active.PeriodNumber)
                    Assert.IsFalse(active.IsClosed)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub GetActiveFiscalPeriod_WhenAllClosed_ThrowsInvalidOperationException()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim service As New PeriodService(context)

                    For Each p In context.AccountingPeriods.ToList()
                        service.CloseFiscalPeriod(p.PeriodId)
                    Next

                    Dim caughtException As InvalidOperationException = Nothing
                    Try
                        service.GetActiveFiscalPeriod()
                    Catch ex As InvalidOperationException
                        caughtException = ex
                    End Try

                    Assert.IsNotNull(caughtException, "Expected InvalidOperationException when all periods are closed.")
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub CloseFiscalPeriod_PermanentlySealsPeriodWithUtcTimestamp()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim service As New PeriodService(context)

                    Dim active = service.GetActiveFiscalPeriod()
                    Dim utcBefore = DateTime.UtcNow.AddSeconds(-1)

                    service.CloseFiscalPeriod(active.PeriodId)

                    Dim refreshed = service.GetPeriodById(active.PeriodId)
                    Assert.IsTrue(refreshed.IsClosed)
                    Assert.IsTrue(refreshed.ClosedAtUtc.HasValue)
                    Assert.IsTrue(refreshed.ClosedAtUtc.Value >= utcBefore)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub CloseFiscalPeriod_IdempotentWhenAlreadyClosed()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim service As New PeriodService(context)

                    Dim active = service.GetActiveFiscalPeriod()
                    service.CloseFiscalPeriod(active.PeriodId)
                    Dim closedTimestamp = service.GetPeriodById(active.PeriodId).ClosedAtUtc

                    ' Second invocation should not fail or alter timestamp
                    service.CloseFiscalPeriod(active.PeriodId)
                    Dim secondCheck = service.GetPeriodById(active.PeriodId)

                    Assert.IsTrue(secondCheck.IsClosed)
                    Assert.AreEqual(closedTimestamp, secondCheck.ClosedAtUtc)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub ResolveStatutoryPostingDate_InOpenPeriod_PreservesOperationalEventDate()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim service As New PeriodService(context)

                    Dim operationalEventDate As New DateTime(2026, 11, 18, 14, 30, 0)
                    Dim resolved = service.ResolveStatutoryPostingDate(operationalEventDate)

                    Assert.AreEqual(operationalEventDate, resolved)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub ResolveStatutoryPostingDate_InClosedPeriod_MapsToFirstOpenDayOfActivePeriod()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim service As New PeriodService(context)

                    ' Create prior closed period FY2026-P10 (October 2026)
                    Dim octPeriod = service.CreatePeriod(2026, 10, New DateTime(2026, 10, 1), New DateTime(2026, 10, 31, 23, 59, 59))
                    service.CloseFiscalPeriod(octPeriod.PeriodId)

                    ' Current active period is FY2026-P11 starting 2026-11-01
                    Dim octEventDate As New DateTime(2026, 10, 25, 10, 0, 0)
                    Dim resolved = service.ResolveStatutoryPostingDate(octEventDate)

                    ' Must resolve to the first open day of P11: 2026-11-01
                    Assert.AreEqual(New DateTime(2026, 11, 1), resolved.Date)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub CreatePeriod_DuplicatePeriod_ThrowsInvalidOperationException()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim service As New PeriodService(context)

                    Dim caughtException As InvalidOperationException = Nothing
                    Try
                        service.CreatePeriod(2026, 11, New DateTime(2026, 11, 1), New DateTime(2026, 11, 30))
                    Catch ex As InvalidOperationException
                        caughtException = ex
                    End Try

                    Assert.IsNotNull(caughtException, "Expected InvalidOperationException on duplicate period creation.")
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub GetAllPeriods_ReturnsChronologicallyOrderedList()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim service As New PeriodService(context)

                    service.CreatePeriod(2026, 12, New DateTime(2026, 12, 1), New DateTime(2026, 12, 31))
                    service.CreatePeriod(2026, 10, New DateTime(2026, 10, 1), New DateTime(2026, 10, 31))

                    Dim all = service.GetAllPeriods()
                    Assert.AreEqual(3, all.Count)
                    Assert.AreEqual(10, all(0).PeriodNumber)
                    Assert.AreEqual(11, all(1).PeriodNumber)
                    Assert.AreEqual(12, all(2).PeriodNumber)
                End Using
            End Using
        End Sub

    End Class

End Namespace
