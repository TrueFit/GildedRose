using GildedRose.Contracts;
using System.Collections.Generic;

namespace GildedRose.Server.IO
{
    /// <summary>
    /// Generic interface for any kind of data source that can store inventory.
    /// </summary>
    public interface IDataSource
    {
        /// <summary>
        /// Create a new data source and store the given items in it.
        /// </summary>
        void CreateNew(IList<Item> items);

        /// <summary>
        /// Get all items stored in the data source
        /// </summary>
        /// <param name="errors">Any error that might have occured during the import.</param>
        IList<Item> GetAllItems(out IList<string> errors);

        /// <summary>
        /// Add a new item to the shop's inventory.
        /// </summary>
        /// <param name="item">The new item</param>
        void AddItem(Item item);

        /// <summary>
        /// Get an item from the data source by its name.
        /// </summary>
        Item GetItemByName(string name);

        /// <summary>
        /// Update the conditions of a set of items.
        /// </summary>
        void UpdateConditions(IList<ProgressedItem> progressedItems);

        /// <summary>
        /// Remove a set of items from the data source.
        /// </summary>
        /// <param name="guids">The GUIDs from the items that shall be removed.</param>
        void RemoveItems(IList<string> guids);
    }
}
