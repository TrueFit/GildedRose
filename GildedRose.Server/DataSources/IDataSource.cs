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
    }
}
