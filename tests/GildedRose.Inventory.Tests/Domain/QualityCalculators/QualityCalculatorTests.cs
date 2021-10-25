using GildedRose.Inventory.Domain.QualityCalculators;

namespace GildedRose.Inventory.Tests.Domain.QualityCalculators;

public class QualityCalculatorTests
{
    [Theory]
    [ClassData(typeof(GetQualityTestData))]
    internal void GetQuality_ReturnsAccurateDefault(Type type, InventoryItem item, int expected, string message)
    {
        dynamic calculator = Activator.CreateInstance(type);

        var newQuality = calculator.GetQuality(item);

        Assert.True(expected == newQuality, $"{message + Environment.NewLine}Expected: {expected + Environment.NewLine}Actual: {newQuality}");
    }

    [Theory]
    [ClassData(typeof(GetCanCalculateTestData))]
    internal void CanCalculate_IsValid(Type type, InventoryItem item, bool expected, string message)
    {
        dynamic calculator = Activator.CreateInstance(type);

        var canCalculate = calculator.CanCalculateItem(item);

        Assert.True(expected == canCalculate, $"{message + Environment.NewLine}Expected: {expected + Environment.NewLine}Actual: {canCalculate}");
    }

    [Fact]
    public void ValidateResult_Maximum_Is50()
    {
        var calculator = new DefaultQualityCalculator();

        var newQuality = calculator.GetQuality(new InventoryItem(new QualityCalculatorFactoryHelper()) { Quality = 80, SellIn = 1 });

        Assert.Equal(50, newQuality);
    }
}

class GetQualityTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator() => (new List<object[]>
    {
        new object[] { typeof(DefaultQualityCalculator), new InventoryItem(new QualityCalculatorFactoryHelper())
            { Quality = 14, SellIn = 1 }, 13, "Default Caclulator must de-increment quality by 1 per day."},
        new object[] { typeof(DefaultQualityCalculator), new InventoryItem(new QualityCalculatorFactoryHelper())
            { Quality = 14, SellIn = -1 }, 12, "Default Caclulator must de-increment quality by 2 per day after SellIn."},
        new object[] { typeof(DefaultQualityCalculator), new InventoryItem(new QualityCalculatorFactoryHelper())
            { Quality = 1, SellIn = -1 }, 0, "Default Caclulator cannot return a quality below 0."},
        new object[] { typeof(AgedBrieQualityCalculator), new InventoryItem(new QualityCalculatorFactoryHelper())
            { Quality = 14, SellIn = 1 }, 15, "Default Caclulator must increment quality by 1 per day."},
        new object[] { typeof(AgedBrieQualityCalculator), new InventoryItem(new QualityCalculatorFactoryHelper())
            { Quality = 14, SellIn = -1 }, 16, "Default Caclulator must increment quality by 2 per day after SellIn."},
        new object[] { typeof(AgedBrieQualityCalculator), new InventoryItem(new QualityCalculatorFactoryHelper())
            { Quality = 1433, SellIn = 1 }, 50, "Caclulator must not return more than 50."},
        new object[] { typeof(SulfurasQualityCalculator), new InventoryItem(new QualityCalculatorFactoryHelper())
            { Name = ItemNames.Sulfuras }, 80, "Sulfuras must always have a quality of 80."},
        new object[] { typeof(BackstagePassQualityCalculator), new InventoryItem(new QualityCalculatorFactoryHelper())
            { Quality = 10, SellIn = 11, Category = ItemCategories.BackstagePasses}, 9,
            "Backstage passes must de-increment quality by 1 more than 10 days before SellIn."},
        new object[] { typeof(BackstagePassQualityCalculator), new InventoryItem(new QualityCalculatorFactoryHelper())
            { Quality = 10, SellIn = 9, Category = ItemCategories.BackstagePasses}, 12,
            "Backstage passes must increment quality by 2 between 6 and 10 days before SellIn."},
        new object[] { typeof(BackstagePassQualityCalculator), new InventoryItem(new QualityCalculatorFactoryHelper())
            { Quality = 10, SellIn = 0, Category = ItemCategories.BackstagePasses}, 13,
            "Backstage passes must increment quality by 3 between 0 and 5 days before SellIn."},
        new object[] { typeof(BackstagePassQualityCalculator), new InventoryItem(new QualityCalculatorFactoryHelper())
            { Quality = 10, SellIn = -1, Category = ItemCategories.BackstagePasses}, 0,
            "Backstage passes have a quality of 0 after SellIn."},
        new object[] { typeof(ConjuredQualityCalculator), new InventoryItem(new QualityCalculatorFactoryHelper())
            { Quality = 10, SellIn = 3}, 8, "Conjured items degrade twice as fast."}
    }).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

class GetCanCalculateTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        return (new List<object[]>
    {
        new object[] { typeof(DefaultQualityCalculator), new InventoryItem(new QualityCalculatorFactoryHelper()), true,
            "DefaultQualityCalculator always can calculate quality." },
        new object[] { typeof(AgedBrieQualityCalculator), new InventoryItem(new QualityCalculatorFactoryHelper())
            { Name = ItemNames.AgedBrie }, true, "AgedBrieQualityCalculator always can calculate quality." },
        new object[] { typeof(AgedBrieQualityCalculator), new InventoryItem(new QualityCalculatorFactoryHelper()), false,
            "AgedBrieQualityCalculator always can calculate quality." },
        new object[] { typeof(SulfurasQualityCalculator), new InventoryItem(new QualityCalculatorFactoryHelper())
            { Name = ItemNames.Sulfuras }, true, "SulfurasQualityCalculator always can calculate quality." },
        new object[] { typeof(SulfurasQualityCalculator), new InventoryItem(new QualityCalculatorFactoryHelper())
            { Name = "not sulfuras" }, false, "SulfurasQualityCalculator always can calculate quality." },
        new object[] { typeof(BackstagePassQualityCalculator), new InventoryItem(new QualityCalculatorFactoryHelper())
            { Category = ItemCategories.BackstagePasses }, true, "BackstagePassQualityCalculator always can calculate backstage pass items." },
        new object[] { typeof(BackstagePassQualityCalculator), new InventoryItem(new QualityCalculatorFactoryHelper())
            { Name = "not a backstage pass" }, false, "BackstagePassQualityCalculator always can not calculate quality for others." },
        new object[] { typeof(ConjuredQualityCalculator), new InventoryItem(new QualityCalculatorFactoryHelper())
            { Category = ItemCategories.Conjured }, true, "ConjuredQualityCalculator always can calculate conjured items." },
        new object[] { typeof(ConjuredQualityCalculator), new InventoryItem(new QualityCalculatorFactoryHelper())
            { Name = "not conjured" }, false, "ConjuredQualityCalculator always can not calculate quality for others." }
    }).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
