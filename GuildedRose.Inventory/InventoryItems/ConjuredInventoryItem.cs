namespace GuildedRose.Inventory.InventoryItems
{
    /// <summary>
    /// Represents an <see cref="InventoryItem"/> with special requirements for items with the "Conjured" category.
    /// Adjusts quality adjustment by day to twice the default for <see cref="InventoryItem"/>
    /// </summary>
    public class ConjuredInventoryItem : InventoryItem
    {
        protected override int QualityAdjustmentByDay => base.QualityAdjustmentByDay * 2;

        public ConjuredInventoryItem(InventoryItem inventoryItem) : base(inventoryItem)
        {
        }
    }
}
