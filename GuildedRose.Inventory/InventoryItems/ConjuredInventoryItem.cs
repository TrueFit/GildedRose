namespace GuildedRose.Inventory.InventoryItems
{
    /// <summary>
    /// Represents an <see cref="InventoryItem"/> with special requirements for items with the "Conjured" category.
    /// </summary>
    public class ConjuredInventoryItem : InventoryItem
    {
        public ConjuredInventoryItem(InventoryItem inventoryItem) : base(inventoryItem)
        {
        }
    }
}
