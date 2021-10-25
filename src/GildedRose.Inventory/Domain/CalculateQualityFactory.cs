namespace GildedRose.Inventory.Domain;

class CalculateQualityFactory : ICalculateQualityFactory
{
    IEnumerable<ICalculateQuality> _calculators;

    public CalculateQualityFactory(IEnumerable<ICalculateQuality> calculators) => _calculators = calculators;

    public virtual ICalculateQuality Create(IInventoryItem item) => _calculators
        .OrderBy(c => typeof(DefaultQualityCalculator) == c.GetType())
        .FirstOrDefault<ICalculateQuality>(c => c.CanCalculateItem(item)) ??
        new DefaultQualityCalculator();
}
