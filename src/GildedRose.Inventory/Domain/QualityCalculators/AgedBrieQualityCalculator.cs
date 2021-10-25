namespace GildedRose.Inventory.Domain.QualityCalculators;

class AgedBrieQualityCalculator : DefaultQualityCalculator
{

    public override bool CanCalculateItem(IInventoryItem item)
    {
        return item.Name == Constants.ItemNames.AgedBrie;
    }

    public override int GetQuality(IInventoryItem item)
    {
        var newQuality = item.Quality + ((item.SellIn >= 0) ? DefaultQualityIncrement : DefaultQualityIncrement * 2);
        return ValidateResult(newQuality);
    }
}
