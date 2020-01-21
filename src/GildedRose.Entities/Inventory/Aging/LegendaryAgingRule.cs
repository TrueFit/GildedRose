namespace GildedRose.Entities.Inventory.Aging
{
    /// <summary>
    /// Aging rule for legendary items (e.g. Sulfuras).
    /// Legendary items always have a quality equal to their max quality value.
    /// </summary>
    public class LegendaryAgingRule : BaseAgingRule
    {
        protected override void SetQuality(Inventory item)
        {
            item.Quality = MaxQuality;
        }

        protected override void SetSellIn(Inventory item)
        {
            return; // SellIn always remains the same
        }
    }
}