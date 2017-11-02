using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GR.Models;
using GR.Repositories;

namespace GR.Managers
{
    public interface IInventoryManager
    {
        /// <summary>
        /// Get all <see cref="Models.InventoryItemType">InventoryItemTypes</see>.
        /// </summary>
        Task<List<InventoryItemType>> GetItemTypesAsync();

        /// <summary>
        /// Get all <see cref="Models.InventoryItemQualityDeltaStrategy">InventoryItemQualityDeltaStrategies</see>.
        /// </summary>
        Task<List<InventoryItemQualityDeltaStrategy>> GetQualityDeltaStrategiesAsync();

        /// <summary>
        /// Add a new <see cref="Models.InventoryItem"/> to the repository.
        /// </summary>
        /// <param name="itemTypeId">The InventoryItemTypeId of the <see cref="Models.InventoryItemType"/> for this new item.</param>
        /// <param name="name">The name of this new InventoryItem.</param>
        /// <param name="description">An optional description for this new inventory item.</param>
        /// <param name="quality">The initial Quality value of this new item (as of <paramref name="inventoryDate"/>).</param>
        /// <param name="sellByDate">The Sell By Date of this new item.  May or may not be required depending on <see cref="Models.InventoryItemQualityDeltaStrategy"/>.</param>
        /// <param name="now">The "current" date and time. Defaults to DateTime.Now.</param>
        /// <param name="inventoryDate">The date to use as the Inventory Date of the new item.  Defaults to <paramref name="now"/>.</param>
        /// <returns>The new item that was created and added to the repository.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no <paramref name="itemTypeId"/> does 
        /// not match any InventoryItemType in the <see cref="Repositories.IInventoryRepository">Repository</see>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the specified InventoryItemType
        /// requires that <paramref name="sellByDate"/> be specified but it was null.</exception>
        /// <exception cref="ArgumentException">Thrown when the specified InventoryItemType
        /// requires that <paramref name="sellByDate"/> be null but it was specified.</exception>
        Task<InventoryItem> AddNewItemAsync(short itemTypeId, string name, string description, double quality, DateTime? sellByDate, DateTime? now = null, DateTime? inventoryDate = null);

        /// <summary>
        /// Get <see cref="Models.InventoryItem"/>s in the <see cref="Models.InventoryItemStatusId.Available"/> 
        /// state (and optionaly also the <see cref="Models.InventoryItemStatusId.Expired"/> state).
        /// </summary>
        /// <param name="page">The page number to page.  Min value 10. Max value 100.</param>
        /// <param name="pageSize">The page size to page.</param>
        /// <param name="sortOrder">The order to sort the results (prior to paging, defaults to <see cref="Models.InventoryItemSortOrder.Name"/>).</param>
        /// <param name="includeExpired">True to include items in the <see cref="Models.InventoryItemStatusId.Expired"/> state. Defaults to false.</param>
        /// <param name="now">The "current" date and time. Defaults to DateTime.Now.</param>
        Task<Models.Projections.InventoryItemSearchResults> GetAvailableItemsAsync(int page, int pageSize, IEnumerable<InventoryItemSortOrder> sortOrder = null, bool includeExpired = false, DateTime? now = null);

        /// <summary>
        /// Get <see cref="Models.InventoryItem"/>s in the <see cref="Models.InventoryItemStatusId.Expired"/> 
        /// state.
        /// </summary>
        /// <param name="page">The page number to page.  Min value 10. Max value 100.</param>
        /// <param name="pageSize">The page size to page.</param>
        /// <param name="sortOrder">The order to sort the results (prior to paging, defaults to <see cref="Models.InventoryItemSortOrder.Name"/>).</param>
        /// <param name="now">The "current" date and time. Defaults to DateTime.Now.</param>
        Task<Models.Projections.InventoryItemSearchResults> GetExpiredItemsAsync(int page, int pageSize, IEnumerable<InventoryItemSortOrder> sortOrder = null, DateTime? now = null);

        /// <summary>
        /// Discard an <see cref="Models.InventoryItem"/>.
        /// </summary>
        /// <param name="itemId">The InventoryItemId of the <see cref="Models.InventoryItem"/> to be discarded.</param>
        /// <param name="now">The "current" date and time. Defaults to DateTime.Now.</param>
        /// <returns>The discarded InventoryItem.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no <paramref name="itemId"/> does not 
        /// match any InventoryItem in the <see cref="Repositories.IInventoryRepository">Repository</see>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the specified InventoryItem is
        /// not in a valid state to be discarded.</exception>
        Task<InventoryItem> DiscardItemAsync(int itemId, DateTime? now = null);

        /// <summary>
        /// Sell an <see cref="Models.InventoryItem"/>.
        /// </summary>
        /// <param name="itemId">The InventoryItemId of the <see cref="Models.InventoryItem"/> to be sold.</param>
        /// <param name="now">The "current" date and time. Defaults to DateTime.Now.</param>
        /// <returns>The sold InventoryItem.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no <paramref name="itemId"/> does not 
        /// match any InventoryItem in the <see cref="Repositories.IInventoryRepository">Repository</see>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the specified InventoryItem is
        /// not in a valid state to be sold.</exception>
        Task<InventoryItem> SellItemAsync(int itemId, DateTime? now = null);

        Task<Models.ProcessResults.NightlyQualityUpdateResult> UpdateItemQualityAsync(DateTime? now = null);
    }
}