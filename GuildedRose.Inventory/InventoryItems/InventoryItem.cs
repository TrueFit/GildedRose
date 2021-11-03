namespace GuildedRose.Inventory.InventoryItems
{
    /// <summary>
    /// Base class for inventory items.
    /// Has protected virtual properties and methods that can be overridden by
    /// subclasses.
    /// </summary>
    public class InventoryItem
    {
        /// <summary>
        /// ID or primary key for database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Category of the item
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Number of days the item must be sold in
        /// </summary>
        public int SellIn { get; set; }

        /// <summary>
        /// The quality of the item which denotes how valuable the item is
        /// </summary>
        public int Quality { get; set; }

        /// <summary>
        /// Constructor for <see cref="InventoryItem"/> that applies properties to
        /// corresponding arguments.
        /// </summary>
        /// <param name="name">The name of the item to be applied to <see cref="Name"/></param>
        /// <param name="category">The category of the item to be applied to <see cref="Category"/></param>
        /// <param name="sellIn">Number of days the item must be sold in to be applied to <see cref="SellIn"/></param>
        /// <param name="quality">The quality of the item which denotes how valuable the item is to be applied to <see cref="SellIn"/></param>
        public InventoryItem(string name, string category, int sellIn, int quality)
        {
            Name = name;
            Category = category;
            SellIn = sellIn;
            Quality = quality;
        }

        /// <summary>
        /// Constructor for <see cref="InventoryItem"/>. Takes another <see cref="InventoryItem"/> as
        /// an argument. Clones values for <see cref="Name"/>, <see cref="Category"/>, <see cref="SellIn"/>,
        /// and <see cref="Quality"/>.
        /// Also adjusts <see cref="Quality"/> based on minumum and maximum quality values.
        /// </summary>
        /// <param name="inventoryItem">The <see cref="InventoryItem"/> to clone</param>
        public InventoryItem(InventoryItem inventoryItem)
        {
            Name = inventoryItem.Name;
            Category = inventoryItem.Category;
            SellIn = inventoryItem.SellIn;
            Quality = inventoryItem.Quality;
        }

        /// <summary>
        /// Applies changes to properties that are needed when a day passes.
        /// Calls <see cref="UpdateQuality"/> and <see cref="UpdateSellIn"/> in that order
        /// </summary>
        public void IncrementDay()
        {
        }
    }
}
