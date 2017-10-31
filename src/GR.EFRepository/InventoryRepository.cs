using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Repositories.EF
{
    public class InventoryRepository
    {
        #region  IInventoryRepository Methods
        public async Task<List<Models.InventoryItemType>> GetItemTypesAsync()
        {
            using (var db = new Entities.InventoryDb())
            {
                return (await db.InventoryItemTypes
                    .ToListAsync().ConfigureAwait(false))
                    .Select(iit => ToModel(iit))
                    .ToList();
            }
        }

        public async Task<List<Models.InventoryItemQualityDeltaStrategy>> GetQualityDeltaStrategiesAsync()
        {
            using (var db = new Entities.InventoryDb())
            {
                return (await db.QualityDeltaStrategies
                    .ToListAsync().ConfigureAwait(false))
                    .Select(qds =>
                        new Models.InventoryItemQualityDeltaStrategy
                        {
                            StrategyId = (Models.InventoryItemQualityDeltaStrategyId)qds.QualityDeltaStrategyId,
                            Name = qds.Name,
                            Description = qds.Description,
                        })
                    .ToList();
            }
        }

        public async Task<Models.InventoryItem> StoreNewItemAsync(
            short itemTypeId, string name, string description, double quality, DateTime? sellByDate,
            DateTime? now = null, DateTime? inventoryDate = null)
        {
            now = now ?? DateTime.Now;
            inventoryDate = inventoryDate ?? now;
            var iiLogic = new Logic.InventoryItemLogic();

            using (var db = new Entities.InventoryDb())
            {
                var itemType = await db.InventoryItemTypes.FindAsync(itemTypeId).ConfigureAwait(false);
                var deltaStrategyId = (Models.InventoryItemQualityDeltaStrategyId?)itemType?.QualityDeltaStrategyId ?? 0;

                var newItem = db.InventoryItems.Add(
                    new Entities.InventoryItem
                    {
                        InventoryItemTypeId = itemTypeId,
                        Name = name,
                        Description = description,
                        InitialQuality = quality,
                        CurrentQuality = iiLogic.DetermineCurrentAvailableItemQuality(
                            deltaStrategyId, inventoryDate.Value, quality, sellByDate),
                        InventoryDate = inventoryDate.Value,
                        SellByDate = sellByDate,
                        CreatedDate = now.Value,
                    });

                await db.SaveChangesAsync().ConfigureAwait(false);
                newItem.InventoryItemType = itemType;
                return ToModel(newItem, iiLogic);
            }
        }

        public async Task<(int TotalItems, List<Models.InventoryItem> Items)> SearchItemsAsync(
            bool includeAvailable = true,
            bool includeExpired = true,
            bool includeSold = false,
            bool includeDiscarded = false,
            IEnumerable<Models.InventoryItemSortOrder> sortOrder = null,
            int skip = 0, int take = 100,
            DateTime? now = null)
        {
            var orderByClause = string.Join(", ",
                sortOrder
                    ?.Select(
                        iiso =>
                        {
                            switch (iiso)
                            {
                                case Models.InventoryItemSortOrder.Name: return "Name";
                                case Models.InventoryItemSortOrder.NameDescending: return "Name DESC";
                                case Models.InventoryItemSortOrder.InventoryDate: return "InventoryDate";
                                case Models.InventoryItemSortOrder.InventoryDateDescending: return "InventoryDate DESC";
                                case Models.InventoryItemSortOrder.SellByDate: return "SellByDate";
                                case Models.InventoryItemSortOrder.SellByDateDescending: return "SellByDate DESC";
                                case Models.InventoryItemSortOrder.Quality: return "Quality";
                                case Models.InventoryItemSortOrder.QualityDescending: return "Quality DESC";
                                default: return "ERROR";
                            }
                        }));

            var itemTypes = (await GetItemTypesAsync()).ToDictionary(iit => iit.InventoryItemTypeId);
            var iiLogic = new Logic.InventoryItemLogic();

            using (var db = new Entities.InventoryDb())
            {
                var items = 
                    //This calls the stored proc dbo.InventoryItems_Search and returns a List of InventoryItem entities.
                    db.InventoryItemSearch(
                        includeAvailable, includeExpired, includeSold, includeDiscarded,
                        orderByClause, skip, take, now ?? DateTime.Now, 
                        out int? totalRows)
                    .Select(ii => ToModel(ii, itemTypes, iiLogic))
                    .ToList();
                return (TotalItems: totalRows.Value, Items: items);
            }
        }

        public async Task<Models.InventoryItem> MarkItemDiscardedAsync(int itemId, DateTime? now = null)
        {
            now = now ?? DateTime.Now;

            using (var db = new Entities.InventoryDb())
            {
                var item = await db.InventoryItems
                    .Include(ii => ii.InventoryItemType)
                    .SingleOrDefaultAsync(ii => ii.InventoryItemId == itemId).ConfigureAwait(false);
                if (item == null)
                    throw new KeyNotFoundException($"No {nameof(Models.InventoryItem)} with {nameof(Models.InventoryItem.InventoryItemId)} = {itemId} was found.");

                item.DiscardDate = now;
                item.ModifiedDate = DateTime.Now;

                await db.SaveChangesAsync().ConfigureAwait(false);

                return ToModel(item, new Logic.InventoryItemLogic());
            }
        }

        public async Task<Models.InventoryItem> MarkItemSoldAsync(int itemId, DateTime? now = null)
        {
            now = now ?? DateTime.Now;

            using (var db = new Entities.InventoryDb())
            {
                var item = await db.InventoryItems
                    .Include(ii => ii.InventoryItemType)
                    .SingleOrDefaultAsync(ii => ii.InventoryItemId == itemId).ConfigureAwait(false);
                if (item == null)
                    throw new KeyNotFoundException($"No {nameof(Models.InventoryItem)} with {nameof(Models.InventoryItem.InventoryItemId)} = {itemId} was found.");

                item.SaleDate = now;
                item.ModifiedDate = DateTime.Now;

                await db.SaveChangesAsync().ConfigureAwait(false);

                return ToModel(item, new Logic.InventoryItemLogic());
            }
        }
        #endregion

        #region ToModel Methods
        private Models.InventoryItemType ToModel(Entities.InventoryItemType iit)
            => new Models.InventoryItemType
            {
                InventoryItemTypeId = iit.InventoryItemTypeId,
                Name = iit.Name,
                Description = iit.Description,
                MinQuality = iit.MinQuality,
                MaxQuality = iit.MaxQuality,
                QualityDeltaStrategy = (Models.InventoryItemQualityDeltaStrategyId)iit.QualityDeltaStrategyId,
            };

        private Models.InventoryItem ToModel(Entities.InventoryItem ii, Logic.InventoryItemLogic iiLogic)
            => ToModel(ii, ii.InventoryItemType == null ? null : new[] { ToModel(ii.InventoryItemType) }.ToDictionary(iit => iit.InventoryItemTypeId), iiLogic);
                
        private Models.InventoryItem ToModel(Entities.InventoryItem ii, Dictionary<short, Models.InventoryItemType> itemTypes, Logic.InventoryItemLogic iiLogic)
            => new Models.InventoryItem
            {
                InventoryItemId = ii.InventoryItemId,
                ItemType = itemTypes?[ii.InventoryItemTypeId],
                Name = ii.Name,
                Description = ii.Description,
                ItemStatusId = iiLogic.DetermineItemStatus(ii.SaleDate, ii.DiscardDate, ii.CurrentQuality),
                InventoryDate = ii.InventoryDate,
                SellByDate = ii.SellByDate,
                SaleDate = ii.SaleDate,
                DiscardDate = ii.DiscardDate,
                InitialQuality = ii.InitialQuality,
                CurrentQuality = ii.CurrentQuality,
            };
        #endregion
    }
}
