Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports Merchandising_Accounting_App.Domain
Imports Merchandising_Accounting_App.Domain.Engines
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Namespace Merchandising_Accounting_App.Tests.Domain

    <TestClass>
    Public Class InventoryCostingEngineTests

        <TestMethod>
        Public Sub ValidateSufficientStock_WithVariousQuantities_ReturnsExpectedBooleans()
            ' Current > Required
            Assert.IsTrue(InventoryCostingEngine.ValidateSufficientStock(100, 50))
            ' Current == Required
            Assert.IsTrue(InventoryCostingEngine.ValidateSufficientStock(50, 50))
            ' Current < Required
            Assert.IsFalse(InventoryCostingEngine.ValidateSufficientStock(49, 50))
            ' Zero on hand for zero requested
            Assert.IsTrue(InventoryCostingEngine.ValidateSufficientStock(0, 0))
            ' Zero on hand for positive requested
            Assert.IsFalse(InventoryCostingEngine.ValidateSufficientStock(0, 1))

            ' Negative stock boundaries (physical stock cannot be negative)
            Assert.IsFalse(InventoryCostingEngine.ValidateSufficientStock(-1, 5))
            Assert.IsFalse(InventoryCostingEngine.ValidateSufficientStock(10, -1))
            Assert.IsFalse(InventoryCostingEngine.ValidateSufficientStock(-5, -5))
        End Sub

        <TestMethod>
        Public Sub EnsureSufficientStock_WhenStockSufficient_DoesNotThrow()
            InventoryCostingEngine.EnsureSufficientStock(100, 50, productId:=1)
            InventoryCostingEngine.EnsureSufficientStock(50, 50, productId:=1)
            InventoryCostingEngine.EnsureSufficientStock(0, 0, productId:=1)
        End Sub

        <TestMethod>
        Public Sub EnsureSufficientStock_WhenStockInsufficient_ThrowsInsufficientStockException()
            Dim ex As InsufficientStockException = Assert.Throws(Of InsufficientStockException)(
                Sub()
                    InventoryCostingEngine.EnsureSufficientStock(30, 50, productId:=42)
                End Sub)

            Assert.AreEqual(42, ex.ProductId)
            Assert.AreEqual(50, ex.RequestedQty)
            Assert.AreEqual(30, ex.AvailableQty)
        End Sub

        <TestMethod>
        Public Sub CalculateNewMovingAverageCost_StandardInboundReceiving_CalculatesCorrectWeightedCost()
            ' Given 100 units on hand at $10.00 and 100 units received at $12.00
            ' NewCost = ((100 * 10.00) + (100 * 12.00)) / 200 = 2200 / 200 = 11.0000
            Dim newCost As Decimal = InventoryCostingEngine.CalculateNewMovingAverageCost(100, 10.0000D, 100, 12.0000D)
            Assert.AreEqual(11.0000D, newCost)

            ' Also verify the alias function CalculateNewCost
            Dim aliasCost As Decimal = InventoryCostingEngine.CalculateNewCost(100, 10.0000D, 100, 12.0000D)
            Assert.AreEqual(11.0000D, aliasCost)
        End Sub

        <TestMethod>
        Public Sub CalculateNewMovingAverageCost_AsymmetricQuantitiesAndPrices_CalculatesAccurately()
            ' 200 units @ $10.00 + 100 units @ $13.00 = (2000 + 1300) / 300 = 3300 / 300 = 11.0000
            Dim newCost As Decimal = InventoryCostingEngine.CalculateNewMovingAverageCost(200, 10.0000D, 100, 13.0000D)
            Assert.AreEqual(11.0000D, newCost)

            ' Fractional test with 4 decimal places rounding:
            ' 10 units @ $10.00 + 3 units @ $15.55 = (100 + 46.65) / 13 = 146.65 / 13 = 11.280769... -> 11.2808
            Dim fractionalCost As Decimal = InventoryCostingEngine.CalculateNewMovingAverageCost(10, 10.0000D, 3, 15.5500D)
            Assert.AreEqual(11.2808D, fractionalCost)
        End Sub

        <TestMethod>
        Public Sub CalculateNewMovingAverageCost_InitialIntake_ZeroCurrentStock_AdoptsPOUnitPrice()
            ' When CurrentQty = 0 and ReceivedQty = 50 @ $25.50
            Dim newCost As Decimal = InventoryCostingEngine.CalculateNewMovingAverageCost(0, 0.0000D, 50, 25.5000D)
            Assert.AreEqual(25.5000D, newCost)
        End Sub

        <TestMethod>
        Public Sub CalculateNewMovingAverageCost_ZeroReceived_PreservesCurrentCost()
            ' When CurrentQty = 100 @ $12.50 and ReceivedQty = 0
            Dim newCost As Decimal = InventoryCostingEngine.CalculateNewMovingAverageCost(100, 12.5000D, 0, 0.0000D)
            Assert.AreEqual(12.5000D, newCost)
        End Sub

        <TestMethod>
        Public Sub CalculateNewMovingAverageCost_BothZero_ReturnsCurrentCost()
            Dim newCost As Decimal = InventoryCostingEngine.CalculateNewMovingAverageCost(0, 15.0000D, 0, 0.0000D)
            Assert.AreEqual(15.0000D, newCost)
        End Sub

        <TestMethod>
        Public Sub CalculateNewMovingAverageCost_NegativeInputs_ThrowsArgumentOutOfRangeException()
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() InventoryCostingEngine.CalculateNewMovingAverageCost(-1, 10D, 10, 10D))
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() InventoryCostingEngine.CalculateNewMovingAverageCost(10, 10D, -1, 10D))
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() InventoryCostingEngine.CalculateNewMovingAverageCost(10, -1D, 10, 10D))
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() InventoryCostingEngine.CalculateNewMovingAverageCost(10, 10D, 10, -1D))
        End Sub

        <TestMethod>
        Public Sub CalculateRestockMovingAverageCost_StandardRestock_CalculatesCorrectBlendedCost()
            ' 80 units @ $10.00 + 20 restocked @ $12.00 = (800 + 240) / 100 = 1040 / 100 = 10.4000
            Dim restockCost As Decimal = InventoryCostingEngine.CalculateRestockMovingAverageCost(80, 10.0000D, 20, 12.0000D)
            Assert.AreEqual(10.4000D, restockCost)

            ' Test alias CalculateRestockCost
            Dim aliasRestockCost As Decimal = InventoryCostingEngine.CalculateRestockCost(80, 10.0000D, 20, 12.0000D)
            Assert.AreEqual(10.4000D, aliasRestockCost)
        End Sub

        <TestMethod>
        Public Sub CalculateRestockMovingAverageCost_ZeroCurrentStock_AdoptsOriginalCost()
            Dim restockCost As Decimal = InventoryCostingEngine.CalculateRestockMovingAverageCost(0, 0.0000D, 15, 14.2500D)
            Assert.AreEqual(14.2500D, restockCost)
        End Sub

        <TestMethod>
        Public Sub CalculateRestockMovingAverageCost_ZeroRestocked_PreservesCurrentCost()
            Dim restockCost As Decimal = InventoryCostingEngine.CalculateRestockMovingAverageCost(50, 8.5000D, 0, 10.0000D)
            Assert.AreEqual(8.5000D, restockCost)
        End Sub

        <TestMethod>
        Public Sub CalculateRestockMovingAverageCost_BothZero_ReturnsCurrentCost()
            Dim restockCost As Decimal = InventoryCostingEngine.CalculateRestockMovingAverageCost(0, 9.9900D, 0, 0.0000D)
            Assert.AreEqual(9.9900D, restockCost)
        End Sub

        <TestMethod>
        Public Sub CalculateRestockMovingAverageCost_NegativeInputs_ThrowsArgumentOutOfRangeException()
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() InventoryCostingEngine.CalculateRestockMovingAverageCost(-1, 10D, 10, 10D))
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() InventoryCostingEngine.CalculateRestockMovingAverageCost(10, 10D, -1, 10D))
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() InventoryCostingEngine.CalculateRestockMovingAverageCost(10, -1D, 10, 10D))
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() InventoryCostingEngine.CalculateRestockMovingAverageCost(10, 10D, 10, -1D))
        End Sub

        <TestMethod>
        Public Sub ConvertPackagingToUnits_ValidInputs_ReturnsIntegerProduct()
            ' 10 cartons of 12 units = 120 units
            Dim units As Integer = InventoryCostingEngine.ConvertPackagingToUnits(10, 12)
            Assert.AreEqual(120, units)

            ' 0 cartons = 0 units
            Assert.AreEqual(0, InventoryCostingEngine.ConvertPackagingToUnits(0, 24))

            ' Multiplier of 1
            Assert.AreEqual(15, InventoryCostingEngine.ConvertPackagingToUnits(15, 1))
        End Sub

        <TestMethod>
        Public Sub ConvertPackagingToUnits_InvalidInputs_ThrowsAppropriateExceptions()
            ' Negative packQty
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() InventoryCostingEngine.ConvertPackagingToUnits(-1, 12))

            ' Multiplier <= 0
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() InventoryCostingEngine.ConvertPackagingToUnits(10, 0))
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() InventoryCostingEngine.ConvertPackagingToUnits(10, -5))

            ' Overflow beyond Integer.MaxValue
            Assert.Throws(Of OverflowException)(
                Sub() InventoryCostingEngine.ConvertPackagingToUnits(Integer.MaxValue, 2))
        End Sub

        <TestMethod>
        Public Sub ConvertUnitsToPackaging_ValidInputs_ReturnsPacksAndRemainder()
            Dim remainder As Integer = 0
            ' 125 units with multiplier 12 -> 10 packs, remainder 5
            Dim packs As Integer = InventoryCostingEngine.ConvertUnitsToPackaging(125, 12, remainder)
            Assert.AreEqual(10, packs)
            Assert.AreEqual(5, remainder)

            ' Exact division: 120 units with multiplier 12 -> 10 packs, remainder 0
            packs = InventoryCostingEngine.ConvertUnitsToPackaging(120, 12, remainder)
            Assert.AreEqual(10, packs)
            Assert.AreEqual(0, remainder)

            ' Less than one pack: 7 units with multiplier 12 -> 0 packs, remainder 7
            packs = InventoryCostingEngine.ConvertUnitsToPackaging(7, 12, remainder)
            Assert.AreEqual(0, packs)
            Assert.AreEqual(7, remainder)

            ' Zero units -> 0 packs, remainder 0
            packs = InventoryCostingEngine.ConvertUnitsToPackaging(0, 12, remainder)
            Assert.AreEqual(0, packs)
            Assert.AreEqual(0, remainder)
        End Sub

        <TestMethod>
        Public Sub ConvertUnitsToPackaging_InvalidInputs_ThrowsArgumentOutOfRangeException()
            Dim remainder As Integer = 0
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() InventoryCostingEngine.ConvertUnitsToPackaging(-1, 12, remainder))
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() InventoryCostingEngine.ConvertUnitsToPackaging(10, 0, remainder))
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() InventoryCostingEngine.ConvertUnitsToPackaging(10, -2, remainder))
        End Sub

        <TestMethod>
        Public Sub CalculateInventoryValuation_ValidInputs_ReturnsFourDecimalPlaces()
            Dim val As Decimal = InventoryCostingEngine.CalculateInventoryValuation(50, 12.3456D)
            Assert.AreEqual(617.2800D, val)

            Assert.AreEqual(0D, InventoryCostingEngine.CalculateInventoryValuation(0, 10.0000D))
        End Sub

        <TestMethod>
        Public Sub CalculateInventoryValuation_NegativeInputs_ThrowsArgumentOutOfRangeException()
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() InventoryCostingEngine.CalculateInventoryValuation(-1, 10D))
            Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub() InventoryCostingEngine.CalculateInventoryValuation(10, -1D))
        End Sub

    End Class

End Namespace
