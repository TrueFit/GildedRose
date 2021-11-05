using GildedRose.Client.Models;
using System;
using System.Collections.Generic;

namespace GildedRose.Client.InventorySystems
{
    /// <summary>
    /// Interface to a shop inventory system.
    /// </summary>
    public interface IInventorySystem
    {
        /// <summary>
        /// Connect to the inventory system.
        /// </summary>
        void Connect();

        /// <summary>
        /// Get all items that are currently available in the store.
        /// </summary>
        IList<IItemModel> GetAllItems();

        /// <summary>
        /// Add a new item to the store's inventory.
        /// </summary>
        void AddItem(IItemModel item);

        /// <summary>
        /// Get a single item by its name.
        /// </summary>
        IItemModel GetItemByName(string name);

        /// <summary>
        /// Progress to the next working day and enjoy the rest of the day. Returns the progressed item information.
        /// </summary>
        IList<IItemModel> ProgressToNextDay();

        /// <summary>
        /// Get all items with such a low quality that they are considered trash.
        /// </summary>
        IList<IItemModel> GetTrash();

        /// <summary>
        /// Remove all trash from the inventory and return the Ids from the deleted items.
        /// </summary>
        IList<Guid> RemoveTrash();
    }
}
