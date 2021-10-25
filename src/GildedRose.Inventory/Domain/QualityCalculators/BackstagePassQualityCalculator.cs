namespace GildedRose.Inventory.Domain.QualityCalculators;

class BackstagePassQualityCalculator : DefaultQualityCalculator
{

    public override bool CanCalculateItem(IInventoryItem item)
    {
        return item.Category == Constants.ItemCategories.BackstagePasses;
    }

    public override int GetQuality(IInventoryItem item)
    {
        switch (item.SellIn)
        {
            case > 10:
                return ValidateResult(item.Quality - DefaultQualityIncrement);
            case > 5:
                return ValidateResult(item.Quality + 2 * DefaultQualityIncrement);
            case >= 0:
                return ValidateResult(item.Quality + 3 * DefaultQualityIncrement);
            default:
                return 0;
        }
    }
}
