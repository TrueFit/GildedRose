namespace GuildedRose.Inventory.InventoryItems
{
    /// <summary>
    /// Creates an <see cref="InventoryItem"/> of specific types based on internal configurations.
    /// This is the main class to edit when adding new inventory items with special requirements.
    /// </summary>
    public class InventoryItemFactory
    {
        private const string AgedBrieName = "Aged Brie";
        private const string SulfurasCategory = "Sulfuras";
        private const string BackstagePassesCategory = "Backstage Passes";
        private const string ConjuredCategory = "Conjured";

        /// <summary>
        /// Creates an <see cref="InventoryItem"/> of specific types from another
        /// <see cref="InventoryItem"/> object.
        /// </summary>
        /// <param name="inventoryItem">The <see cref="InventoryItem"/> object to create from</param>
        /// <returns>An <see cref="InventoryItem"/> that may be of a specific type
        /// based on internal configurations</returns>
        public static InventoryItem Factory(InventoryItem inventoryItem)
        {
            if (inventoryItem.Name == AgedBrieName)
            {
                return new AgedBrieInventoryItem(inventoryItem);
            }
            else if (inventoryItem.Category == SulfurasCategory)
            {
                return new SulfurasInventoryItem(inventoryItem);
            }
            else if (inventoryItem.Category == BackstagePassesCategory)
            {
                return new BackStagePassesInventoryItem(inventoryItem);
            }
            else if (inventoryItem.Category == ConjuredCategory)
            {
                return new ConjuredInventoryItem(inventoryItem);
            }
            else
            {
                return new InventoryItem(inventoryItem);
            }
        }
    }
}
