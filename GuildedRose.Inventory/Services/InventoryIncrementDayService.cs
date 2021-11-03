using GuildedRose.Inventory.Data;
using GuildedRose.Inventory.InventoryItems;

namespace GuildedRose.Inventory.Services
{
    /// <summary>
    /// Service that handles incrementing day for all <see cref="InventoryItem"/> objects
    /// in the database.
    /// </summary>
    public class InventoryIncrementDayService
    {
        private readonly InventoryDbContext _InventoryDbContext;

        /// <summary>
        /// Constructor for <see cref="InventoryIncrementDayService"/>
        /// </summary>
        /// <param name="inventoryDbContext">The inventory item database context</param>
        public InventoryIncrementDayService(InventoryDbContext inventoryDbContext)
        {
            _InventoryDbContext = inventoryDbContext;
        }

        /// <summary>
        /// Increments the day for all items in the database.
        /// Creates inventory items from <see cref="InventoryItemFactory.Factory(InventoryItem)"/>
        /// and applies <see cref="InventoryItem.IncrementDay"/>
        /// </summary>
        public void IncrementDayForAllItems()
        {
            foreach (var item in _InventoryDbContext.Items)
            {
                var typedItem = InventoryItemFactory.Factory(item);
                typedItem.IncrementDay();
                _InventoryDbContext.Items.Remove(item);
                _InventoryDbContext.Items.Add(typedItem);
            }
            _InventoryDbContext.SaveChanges();
        }
    }
}