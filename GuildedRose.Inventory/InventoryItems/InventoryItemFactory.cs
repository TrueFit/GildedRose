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

        /// <summary>
        /// Creates an <see cref="InventoryItem"/> from a string of comma separated values representing
        /// the properties of the item.
        /// </summary>
        /// <param name="csvLine">A string of comma separated values representing the properties of the item.
        /// Where the first value = Name, the second value = Category, the third value = SellIn, and the fourth value = Quality.
        /// (e.g. "Sword,Weapon,30,50")</param>
        /// <returns></returns>
        public static InventoryItem FactoryFromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');

            var inventoryItem = Factory(new InventoryItem(values[0],
                                                          values[1],
                                                          int.Parse(values[2]),
                                                          int.Parse(values[3])));

            return inventoryItem;
        }
    }
}
