using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Managers
{
    public class InventoryManager
    {
        public InventoryManager(Repositories.IInventoryRepository inventoryRepository)
        {
            InventoryRepository = inventoryRepository 
                ?? throw new ArgumentNullException(nameof(inventoryRepository));
        }

        public Repositories.IInventoryRepository InventoryRepository { get; private set; }

        /// <summary>
        /// Get all <see cref="Models.InventoryItemType">InventoryItemTypes</see>.
        /// </summary>
        public Task<List<Models.InventoryItemType>> GetItemTypesAsync()
            => InventoryRepository.GetItemTypesAsync();

        /// <summary>
        /// Get all <see cref="Models.InventoryItemQualityDeltaStrategy">InventoryItemQualityDeltaStrategies</see>.
        /// </summary>
        public Task<List<Models.InventoryItemQualityDeltaStrategy>> GetQualityDeltaStrategiesAsync()
            => InventoryRepository.GetQualityDeltaStrategiesAsync();

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
        public async Task<Models.InventoryItem> AddNewItemAsync(
            short itemTypeId, string name, string description, double quality, DateTime? sellByDate,
            DateTime? now = null, DateTime? inventoryDate = null)
        {
            var itemType = await InventoryRepository.GetItemTypeAsync(itemTypeId).ConfigureAwait(false)
                ?? throw new KeyNotFoundException($"No InventoryItemType with InventoryItemTypeId = {itemTypeId} was found.");

            var iiLogic = new Logic.InventoryItemLogic();
            if (iiLogic.DetermineIsSellByDateRequired(itemType.QualityDeltaStrategyId))
            {
                if (sellByDate == null)
                    throw new ArgumentNullException(nameof(sellByDate), $"sellByDate is required for ItemType {itemType.Name}.");
            }
            else if (sellByDate != null)
                throw new ArgumentException(nameof(sellByDate), $"sellByDate must be null for ItemType {itemType.Name}.");

            now = now ?? DateTime.Now;
            inventoryDate = (inventoryDate ?? now).Value.Date;
            var currentQuality = iiLogic.DetermineCurrentAvailableItemQuality(
                itemType.QualityDeltaStrategyId, inventoryDate.Value, quality, sellByDate,
                itemType.BaseDelta, itemType.MinQuality, itemType.MaxQuality, now);

            return await InventoryRepository.StoreNewItemAsync(
                    itemTypeId, name, description, quality, currentQuality,
                    sellByDate, now.Value, inventoryDate.Value)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Get <see cref="Models.InventoryItem"/>s in the <see cref="Models.InventoryItemStatusId.Available"/> 
        /// state (and optionaly also the <see cref="Models.InventoryItemStatusId.Expired"/> state).
        /// </summary>
        /// <param name="page">The page number to page.  Min value 10. Max value 100.</param>
        /// <param name="pageSize">The page size to page.</param>
        /// <param name="sortOrder">The order to sort the results (prior to paging, defaults to <see cref="Models.InventoryItemSortOrder.Name"/>).</param>
        /// <param name="includeExpired">True to include items in the <see cref="Models.InventoryItemStatusId.Expired"/> state. Defaults to false.</param>
        /// <param name="now">The "current" date and time. Defaults to DateTime.Now.</param>
        public Task<(int TotalItems, List<Models.InventoryItem> Items)> GetAvailableItems(
            int page, int pageSize,
            IEnumerable<Models.InventoryItemSortOrder> sortOrder = null,
            bool includeExpired = false,
            DateTime? now = null)
            => InventoryRepository.SearchItemsAsync(
                true, includeExpired, false, false,
                sortOrder == null || !sortOrder.Any() ? new[] { Models.InventoryItemSortOrder.Name } : sortOrder,
                Math.Max(0, (page - 1) * Math.Min(Math.Max(10, pageSize), 100)), 
                Math.Min(Math.Max(10, pageSize), 100),
                now ?? DateTime.Now);

        /// <summary>
        /// Get <see cref="Models.InventoryItem"/>s in the <see cref="Models.InventoryItemStatusId.Expired"/> 
        /// state.
        /// </summary>
        /// <param name="page">The page number to page.  Min value 10. Max value 100.</param>
        /// <param name="pageSize">The page size to page.</param>
        /// <param name="sortOrder">The order to sort the results (prior to paging, defaults to <see cref="Models.InventoryItemSortOrder.Name"/>).</param>
        /// <param name="now">The "current" date and time. Defaults to DateTime.Now.</param>
        public Task<(int TotalItems, List<Models.InventoryItem> Items)> GetExpiredItems(
            int page, int pageSize,
            IEnumerable<Models.InventoryItemSortOrder> sortOrder = null,
            DateTime? now = null)
            => InventoryRepository.SearchItemsAsync(
                false, true, false, false,
                sortOrder == null || !sortOrder.Any() ? new[] { Models.InventoryItemSortOrder.Name } : sortOrder,
                Math.Max(0, (page - 1) * pageSize), Math.Min(Math.Max(10, pageSize), 100),
                now ?? DateTime.Now);

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
        public async Task<Models.InventoryItem> DiscardItemAsync(int itemId, DateTime? now = null)
        {
            var item = await InventoryRepository.GetItem(itemId, now ?? DateTime.Now).ConfigureAwait(false)
                ?? throw new KeyNotFoundException($"No InventoryItem with InventoryItemID = {itemId} was found.");

            var iiLogic = new Logic.InventoryItemLogic();
            if (!iiLogic.DetermineCanItemBeDiscarded(item.ItemStatusId))
                throw new InvalidOperationException($"InventoryItem with InventoryItemId cannot be discarded because it is in the {item.ItemStatusId} state.");

            return await InventoryRepository.MarkItemDiscardedAsync(itemId, now ?? DateTime.Now).ConfigureAwait(false);
        }

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
        public async Task<Models.InventoryItem> SellItemAsync(int itemId, DateTime? now = null)
        {
            var item = await InventoryRepository.GetItem(itemId, now ?? DateTime.Now).ConfigureAwait(false)
                ?? throw new KeyNotFoundException($"No InventoryItem with InventoryItemID = {itemId} was found.");

            var iiLogic = new Logic.InventoryItemLogic();
            if (!iiLogic.DetermineCanItemBeSold(item.ItemStatusId))
                throw new InvalidOperationException($"InventoryItem with InventoryItemId cannot be sold because it is in the {item.ItemStatusId} state.");

            return await InventoryRepository.MarkItemSoldAsync(itemId, now ?? DateTime.Now).ConfigureAwait(false);
        }

    }
}
