namespace GildedRose.Entities.Inventory.Aging
{
    /// <summary>
    /// Quality modifier that applies to a specific SellIn range.
    /// This modifier is used by tiered aging rules that change over time
    /// (e.g. Backstage Passes that increase in value as the concert nears and at varying levels)
    /// </summary>
    public class RangedSellInQualityModifier : BaseQualityModifier
    {
        public int SellInFrom { get; set; }
        public int SellInTo { get; set; }
    }
}
