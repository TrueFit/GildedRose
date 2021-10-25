namespace GildedRose.Inventory.Domain.QualityCalculators;

class SulfurasQualityCalculator : DefaultQualityCalculator
{

    public override bool CanCalculateItem(IInventoryItem item)
    {
        return item.Name == Constants.ItemNames.Sulfuras;
    }

    public override int GetQuality(IInventoryItem item)
    {
        return 80;
    }
}
