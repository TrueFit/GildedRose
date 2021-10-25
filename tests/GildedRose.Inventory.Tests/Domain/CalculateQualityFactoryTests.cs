using GildedRose.Inventory.Domain.QualityCalculators;
using GildedRose.Inventory.Interfaces;
using GildedRose.Inventory.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GildedRose.Inventory.Tests.Domain;

public class CalculateQualityFactoryTests
{
    private readonly IEnumerable<ICalculateQuality> _allCalculators;

    public CalculateQualityFactoryTests()
    {
        var services = (new ServiceCollection())
            .RegisterInventoryServices()
            .BuildServiceProvider();

            
        _allCalculators = services.GetServices<ICalculateQuality>();
    }

    [Fact]
    public void Factory_ReturnsDefaultCalculator()
    {
        var factory = new CalculateQualityFactory(_allCalculators);

        var calculator = factory.Create(new InventoryItem(new QualityCalculatorFactoryHelper()));

        Assert.IsType<DefaultQualityCalculator>(calculator);
    }

    [Fact] void Factory_ReturnsCorrectAgedBrieCalculator()
    {
        var factory = new CalculateQualityFactory(_allCalculators);

        var calculator = factory.Create(new InventoryItem(new QualityCalculatorFactoryHelper()) { Name = ItemNames.AgedBrie });

        Assert.IsType<AgedBrieQualityCalculator>(calculator);
    }

    [Fact]
    void Factory_ReturnsCorrectBackstagePassCalculator()
    {
        var factory = new CalculateQualityFactory(_allCalculators);

        var calculator = factory.Create(new InventoryItem(new QualityCalculatorFactoryHelper()) { Category = ItemCategories.BackstagePasses });

        Assert.IsType<BackstagePassQualityCalculator>(calculator);
    }

    [Fact]
    void Factory_ReturnsCorrectConjuredCalculator()
    {
        var factory = new CalculateQualityFactory(_allCalculators);

        var calculator = factory.Create(new InventoryItem(new QualityCalculatorFactoryHelper()) { Category = ItemCategories.Conjured });

        Assert.IsType<ConjuredQualityCalculator>(calculator);
    }

    [Fact]
    void Factory_ReturnsCorrectSulfurasCalculator()
    {
        var factory = new CalculateQualityFactory(_allCalculators);

        var calculator = factory.Create(new InventoryItem(new QualityCalculatorFactoryHelper()) { Name = ItemNames.Sulfuras });

        Assert.IsType<SulfurasQualityCalculator>(calculator);
    }
}
