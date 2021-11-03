namespace GuildedRose.Inventory.InventoryItems
{
    /// <summary>
    /// Represents an <see cref="InventoryItem"/> with special requirements for items with the "Sulfuras" category.
    /// Adjusts quality and sell in adjustments by day to 0
    /// Adjusts minimum and maximum quality to 80
    /// </summary>
    public class SulfurasInventoryItem : InventoryItem
    {
        protected override int QualityAdjustmentByDay => 0;
        protected override int SellInAdjustmentByDay => 0;
        protected override int MinimumQuality => 80;
        protected override int MaximumQuality => 80;

        public SulfurasInventoryItem(InventoryItem inventoryItem) : base(inventoryItem)
        {
        }
    }
}
