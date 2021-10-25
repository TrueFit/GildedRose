namespace GildedRose.Inventory.Domain.QualityCalculators;

class DefaultQualityCalculator : ICalculateQuality
{
    public const int DefaultQualityIncrement = 1;
    public const int MaximumQuality = 50;

    public virtual bool CanCalculateItem(IInventoryItem item)
    {
        return true;
    }

    public virtual int GetQuality(IInventoryItem item)
    {
        return ValidateResult(item.Quality - ((item.SellIn >= 0) ?
            DefaultQualityIncrement : DefaultQualityIncrement * 2));
    }

    protected int ValidateResult(int quality)
    {
        return (quality < MaximumQuality) ?
            (quality >= 0) ? quality : 0
            : MaximumQuality;
    }
}
