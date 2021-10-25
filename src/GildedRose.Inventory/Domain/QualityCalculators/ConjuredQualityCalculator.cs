namespace GildedRose.Inventory.Domain.QualityCalculators;

class ConjuredQualityCalculator : DefaultQualityCalculator
{

    public override bool CanCalculateItem(IInventoryItem item)
    {
        return item.Category == Constants.ItemCategories.Conjured;
    }

    public override int GetQuality(IInventoryItem item)
    {
        var quality = item.Quality - 2 * DefaultQualityIncrement;
        return ValidateResult(quality);
    }
}
