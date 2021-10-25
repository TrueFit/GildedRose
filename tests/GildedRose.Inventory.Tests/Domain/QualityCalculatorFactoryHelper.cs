using GildedRose.Inventory.Domain.QualityCalculators;
using GildedRose.Inventory.Interfaces;

namespace GildedRose.Inventory.Tests.Domain;

class QualityCalculatorFactoryHelper : CalculateQualityFactory
{
    public QualityCalculatorFactoryHelper() : base(null) { }

    public QualityCalculatorFactoryHelper(IEnumerable<ICalculateQuality> calculators) : base(calculators) { }

    public override ICalculateQuality Create(IInventoryItem item)
    {
        return new DefaultQualityCalculator();
    }
}

