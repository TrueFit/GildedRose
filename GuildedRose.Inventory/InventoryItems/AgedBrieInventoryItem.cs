namespace GuildedRose.Inventory.InventoryItems
{
    /// <summary>
    /// Represents an <see cref="InventoryItem"/> with special requirements for the "Aged Brie" name.
    /// Adjusts quality adjustment by day to the negative of the default for <see cref="InventoryItem"/>
    /// </summary>
    public class AgedBrieInventoryItem : InventoryItem
    {
        protected override int QualityAdjustmentByDay => base.QualityAdjustmentByDay * -1;

        public AgedBrieInventoryItem(InventoryItem inventoryItem) : base(inventoryItem)
        {
        }
    }
}
