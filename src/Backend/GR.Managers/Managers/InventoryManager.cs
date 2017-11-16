using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Managers
{
    public class InventoryManager : IInventoryManager
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
        public async Task<List<Models.InventoryItemType>> GetItemTypesAsync()
        {
            var result = await InventoryRepository.GetItemTypesAsync().ConfigureAwait(false);
            var iiLogic = new Logic.InventoryItemLogic();
            result.ForEach(iit => iit.IsSellByDateRequired = iiLogic.DetermineIsSellByDateRequired(iit.QualityDeltaStrategyId));
            return result;
        }

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

            var newItem = await InventoryRepository.StoreNewItemAsync(
                    itemTypeId, name, description, quality, currentQuality,
                    sellByDate, inventoryDate.Value)
                .ConfigureAwait(false);

            return SetItemStatus(newItem);
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
        public async Task<Models.Projections.InventoryItemSearchResults> GetAvailableItemsAsync(
            int page, int pageSize,
            IEnumerable<Models.InventoryItemSortOrder> sortOrder = null,
            bool includeExpired = false,
            DateTime? now = null)
        {
            var result = await InventoryRepository.SearchItemsAsync(
                true, includeExpired, false, false,
                sortOrder == null || !sortOrder.Any() ? new[] { Models.InventoryItemSortOrder.Name } : sortOrder,
                Math.Max(0, (page - 1) * Math.Min(Math.Max(10, pageSize), 100)),
                Math.Min(Math.Max(10, pageSize), 100),
                now ?? DateTime.Now)
                .ConfigureAwait(false);

            SetItemStatus(result.Items);

            return
                new Models.Projections.InventoryItemSearchResults
                {
                    TotalItems = result.TotalItems,
                    Items = result.Items,
                };
        }

        /// <summary>
        /// Get <see cref="Models.InventoryItem"/>s in the <see cref="Models.InventoryItemStatusId.Expired"/> 
        /// state.
        /// </summary>
        /// <param name="page">The page number to page.  Min value 10. Max value 100.</param>
        /// <param name="pageSize">The page size to page.</param>
        /// <param name="sortOrder">The order to sort the results (prior to paging, defaults to <see cref="Models.InventoryItemSortOrder.Name"/>).</param>
        /// <param name="now">The "current" date and time. Defaults to DateTime.Now.</param>
        public async Task<Models.Projections.InventoryItemSearchResults> GetExpiredItemsAsync(
            int page, int pageSize,
            IEnumerable<Models.InventoryItemSortOrder> sortOrder = null,
            DateTime? now = null)
        {
            var result = await InventoryRepository.SearchItemsAsync(
                false, true, false, false,
                sortOrder == null || !sortOrder.Any() ? new[] { Models.InventoryItemSortOrder.Name } : sortOrder,
                Math.Max(0, (page - 1) * pageSize), Math.Min(Math.Max(10, pageSize), 100),
                now ?? DateTime.Now)
                .ConfigureAwait(false);

            SetItemStatus(result.Items);

            return
                new Models.Projections.InventoryItemSearchResults
                {
                    TotalItems = result.TotalItems,
                    Items = result.Items,
                };
        }

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
            var item = await InventoryRepository.GetItemAsync(itemId).ConfigureAwait(false)
                ?? throw new KeyNotFoundException($"No InventoryItem with InventoryItemID = {itemId} was found.");

            var iiLogic = new Logic.InventoryItemLogic();
            SetItemStatus(item, iiLogic);
            if (!iiLogic.DetermineCanItemBeDiscarded(item.ItemStatusId))
                throw new InvalidOperationException($"InventoryItem with InventoryItemId cannot be discarded because it is in the {item.ItemStatusId} state.");

            return SetItemStatus(
                await InventoryRepository.MarkItemDiscardedAsync(itemId, now ?? DateTime.Now)
                    .ConfigureAwait(false));
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
            var item = await InventoryRepository.GetItemAsync(itemId).ConfigureAwait(false)
                ?? throw new KeyNotFoundException($"No InventoryItem with InventoryItemID = {itemId} was found.");

            var iiLogic = new Logic.InventoryItemLogic();
            SetItemStatus(item, iiLogic);
            if (!iiLogic.DetermineCanItemBeSold(item.ItemStatusId))
                throw new InvalidOperationException($"InventoryItem with InventoryItemId cannot be sold because it is in the {item.ItemStatusId} state.");

            return SetItemStatus(
                await InventoryRepository.MarkItemSoldAsync(itemId, now ?? DateTime.Now)
                    .ConfigureAwait(false));
        }

        public async Task<Models.ProcessResults.NightlyQualityUpdateResult> UpdateItemQualityAsync(DateTime? now = null)
        {
            var startTime = DateTime.Now;
            var processTimer = System.Diagnostics.Stopwatch.StartNew();

            var itemCount = 0;
            var expiredItemCount = 0;

            try
            {
                now = now ?? DateTime.Now;
                var items =
                    (await InventoryRepository.SearchItemsAsync(
                        true, true, false, false, null, -1, -1, now.Value))
                    .Items;

                var iiLogic = new Logic.InventoryItemLogic();

                foreach (var item in items)
                {
                    var currentQuality = iiLogic.DetermineCurrentAvailableItemQuality(
                        item.ItemType.QualityDeltaStrategyId,
                        item.InventoryDate,
                        item.InitialQuality,
                        item.SellByDate,
                        item.ItemType.BaseDelta,
                        item.ItemType.MinQuality,
                        item.ItemType.MaxQuality,
                        now);

                    await InventoryRepository.UpdateItemCurrentQuality(item.InventoryItemId, currentQuality);

                    var itemStatus = iiLogic.DetermineItemStatus(
                        item.SaleDate, item.DiscardDate, currentQuality);

                    itemCount++;
                    if (itemStatus == Models.InventoryItemStatusId.Expired)
                        expiredItemCount++;
                }
                processTimer.Stop();
            }
            catch (Exception ex)
            {
                return new Models.ProcessResults.NightlyQualityUpdateResult
                {
                    WasSuccessful = false,
                    ProcessStartTime = startTime,
                    ProcessDuration = processTimer.Elapsed,
                    NumberOfRecordsAffected = itemCount,
                    CurrentDate = now.Value,
                    NumberOfExpiredRecords = expiredItemCount,
                    Errors = 
                        new []
                        {
                            "Error updating item quality:",
                            $"{ex.GetType().Name}: {ex.Message}",
                        }

                };
            }

            return new Models.ProcessResults.NightlyQualityUpdateResult
            {
                WasSuccessful = true,
                ProcessStartTime = startTime,
                ProcessDuration = processTimer.Elapsed,
                NumberOfRecordsAffected = itemCount,
                CurrentDate = now.Value,
                NumberOfExpiredRecords = expiredItemCount,
            };
        }

        private void SetItemStatus(IEnumerable<Models.InventoryItem> items, Logic.InventoryItemLogic iiLogic = null)
        {
            iiLogic = iiLogic ?? new Logic.InventoryItemLogic();
            if (items != null)
                foreach (var item in items)
                    SetItemStatus(item, iiLogic);
        }

        private Models.InventoryItem SetItemStatus(Models.InventoryItem item, Logic.InventoryItemLogic iiLogic = null)
        {
            if (item == null)
                return null;
            item.ItemStatusId = (iiLogic ?? new Logic.InventoryItemLogic()).DetermineItemStatus(
                item.SaleDate, item.DiscardDate, item.CurrentQuality);
            return item;
        }

    }
}
