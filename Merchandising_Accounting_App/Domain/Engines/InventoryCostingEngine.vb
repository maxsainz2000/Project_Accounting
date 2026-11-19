Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports Merchandising_Accounting_App.Domain

Namespace Domain.Engines

    ''' <summary>
    ''' Pure domain calculation engine for stock availability, perpetual moving weighted average costing,
    ''' customer return restock costing, and packaging unit conversions.
    ''' </summary>
    Public Class InventoryCostingEngine

        ''' <summary>
        ''' Validates whether sufficient on-hand inventory is physically available to fulfill a requested demand.
        ''' Returns False if stock is insufficient or if quantities are negative.
        ''' </summary>
        Public Shared Function ValidateSufficientStock(currentQty As Integer, requiredQty As Integer) As Boolean
            If currentQty < 0 OrElse requiredQty < 0 Then
                Return False
            End If
            Return currentQty >= requiredQty
        End Function

        ''' <summary>
        ''' Enforces that sufficient on-hand inventory is physically available, throwing InsufficientStockException if not.
        ''' </summary>
        Public Shared Sub EnsureSufficientStock(currentQty As Integer, requiredQty As Integer, Optional productId As Integer = 0)
            If Not ValidateSufficientStock(currentQty, requiredQty) Then
                Throw New InsufficientStockException(productId, requiredQty, currentQty)
            End If
        End Sub

        ''' <summary>
        ''' Computes the perpetual moving weighted average unit cost forward upon physical dock receipt (Goods Receipt Note):
        ''' NewCost = ((CurrentQty * CurrentCost) + (ReceivedQty * UnitPrice)) / (CurrentQty + ReceivedQty)
        ''' Rounded to 4 decimal places.
        ''' </summary>
        Public Shared Function CalculateNewMovingAverageCost(currentQty As Integer, currentCost As Decimal, receivedQty As Integer, poUnitPrice As Decimal) As Decimal
            If currentQty < 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(currentQty), "Current on-hand quantity cannot be negative.")
            End If
            If receivedQty < 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(receivedQty), "Received quantity cannot be negative.")
            End If
            If currentCost < 0D Then
                Throw New ArgumentOutOfRangeException(NameOf(currentCost), "Current unit cost cannot be negative.")
            End If
            If poUnitPrice < 0D Then
                Throw New ArgumentOutOfRangeException(NameOf(poUnitPrice), "Purchase order unit price cannot be negative.")
            End If

            Dim totalQty As Long = CLng(currentQty) + CLng(receivedQty)
            If totalQty = 0L Then
                Return Math.Round(currentCost, 4, MidpointRounding.AwayFromZero)
            End If

            If currentQty = 0 Then
                Return Math.Round(poUnitPrice, 4, MidpointRounding.AwayFromZero)
            End If

            If receivedQty = 0 Then
                Return Math.Round(currentCost, 4, MidpointRounding.AwayFromZero)
            End If

            Dim totalValue As Decimal = (CDec(currentQty) * currentCost) + (CDec(receivedQty) * poUnitPrice)
            Dim newCost As Decimal = totalValue / CDec(totalQty)
            Return Math.Round(newCost, 4, MidpointRounding.AwayFromZero)
        End Function

        ''' <summary>
        ''' Alias for CalculateNewMovingAverageCost conforming to the task brief specification.
        ''' </summary>
        Public Shared Function CalculateNewCost(currentQty As Integer, currentCost As Decimal, receivedQty As Integer, unitPrice As Decimal) As Decimal
            Return CalculateNewMovingAverageCost(currentQty, currentCost, receivedQty, unitPrice)
        End Function

        ''' <summary>
        ''' Computes the blended moving weighted average unit cost upon restocking inspected customer returns:
        ''' RestockCost = ((CurrentQty * CurrentCost) + (RestockedQty * OriginalCost)) / (CurrentQty + RestockedQty)
        ''' Rounded to 4 decimal places.
        ''' </summary>
        Public Shared Function CalculateRestockMovingAverageCost(currentQty As Integer, currentCost As Decimal, restockedQty As Integer, originalInvoiceCost As Decimal) As Decimal
            If currentQty < 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(currentQty), "Current on-hand quantity cannot be negative.")
            End If
            If restockedQty < 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(restockedQty), "Restocked quantity cannot be negative.")
            End If
            If currentCost < 0D Then
                Throw New ArgumentOutOfRangeException(NameOf(currentCost), "Current unit cost cannot be negative.")
            End If
            If originalInvoiceCost < 0D Then
                Throw New ArgumentOutOfRangeException(NameOf(originalInvoiceCost), "Original invoice unit cost cannot be negative.")
            End If

            Dim totalQty As Long = CLng(currentQty) + CLng(restockedQty)
            If totalQty = 0L Then
                Return Math.Round(currentCost, 4, MidpointRounding.AwayFromZero)
            End If

            If currentQty = 0 Then
                Return Math.Round(originalInvoiceCost, 4, MidpointRounding.AwayFromZero)
            End If

            If restockedQty = 0 Then
                Return Math.Round(currentCost, 4, MidpointRounding.AwayFromZero)
            End If

            Dim totalValue As Decimal = (CDec(currentQty) * currentCost) + (CDec(restockedQty) * originalInvoiceCost)
            Dim restockCost As Decimal = totalValue / CDec(totalQty)
            Return Math.Round(restockCost, 4, MidpointRounding.AwayFromZero)
        End Function

        ''' <summary>
        ''' Alias for CalculateRestockMovingAverageCost conforming to the task brief specification.
        ''' </summary>
        Public Shared Function CalculateRestockCost(currentQty As Integer, currentCost As Decimal, restockedQty As Integer, originalCost As Decimal) As Decimal
            Return CalculateRestockMovingAverageCost(currentQty, currentCost, restockedQty, originalCost)
        End Function

        ''' <summary>
        ''' Converts master packaging cartons/packs to atomic integer base units using an integer pack multiplier.
        ''' Fractional or floating-point stock counts are strictly forbidden by architectural invariants.
        ''' </summary>
        Public Shared Function ConvertPackagingToUnits(packQty As Integer, multiplier As Integer) As Integer
            If packQty < 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(packQty), "Packaging pack quantity cannot be negative.")
            End If
            If multiplier <= 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(multiplier), "Packaging multiplier must be strictly greater than zero.")
            End If

            Dim totalUnits As Long = CLng(packQty) * CLng(multiplier)
            If totalUnits > Integer.MaxValue Then
                Throw New OverflowException("Packaging unit conversion exceeded maximum integer value.")
            End If
            Return CInt(totalUnits)
        End Function

        ''' <summary>
        ''' Converts atomic base units to whole master packaging units and calculates remaining loose units.
        ''' </summary>
        Public Shared Function ConvertUnitsToPackaging(units As Integer, multiplier As Integer, ByRef remainderUnits As Integer) As Integer
            If units < 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(units), "Units quantity cannot be negative.")
            End If
            If multiplier <= 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(multiplier), "Packaging multiplier must be strictly greater than zero.")
            End If

            remainderUnits = units Mod multiplier
            Return units \ multiplier
        End Function

        ''' <summary>
        ''' Calculates the total monetary valuation of physical inventory given an integer quantity and unit cost.
        ''' </summary>
        Public Shared Function CalculateInventoryValuation(qty As Integer, unitCost As Decimal) As Decimal
            If qty < 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(qty), "Inventory quantity cannot be negative.")
            End If
            If unitCost < 0D Then
                Throw New ArgumentOutOfRangeException(NameOf(unitCost), "Unit cost cannot be negative.")
            End If

            Return Math.Round(CDec(qty) * unitCost, 4, MidpointRounding.AwayFromZero)
        End Function

    End Class

End Namespace
