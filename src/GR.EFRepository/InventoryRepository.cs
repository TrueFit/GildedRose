using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Repositories.EF
{
    public class InventoryRepository : IInventoryRepository
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

        public async Task<Models.InventoryItemType> GetItemTypeAsync(short inventoryItemTypeId)
        {
            using (var db = new Entities.InventoryDb())
            {
                var entitiy = await db.InventoryItemTypes
                    .FindAsync(inventoryItemTypeId).ConfigureAwait(false);

                return entitiy == null ? null : ToModel(entitiy);
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
            short itemTypeId, string name, string description, double initialQuality, double currentQuality,
            DateTime? sellByDate, DateTime now, DateTime inventoryDate)
        {
            using (var db = new Entities.InventoryDb())
            {
                var newItem = db.InventoryItems.Add(
                    new Entities.InventoryItem
                    {
                        InventoryItemTypeId = itemTypeId,
                        Name = name,
                        Description = description,
                        InitialQuality = initialQuality,
                        CurrentQuality = currentQuality,
                        InventoryDate = inventoryDate,
                        SellByDate = sellByDate,
                        CreatedDate = now,
                    });

                await db.SaveChangesAsync().ConfigureAwait(false);

                newItem.InventoryItemType = await db.InventoryItemTypes.FindAsync(itemTypeId).ConfigureAwait(false);
                return ToModel(newItem, new Logic.InventoryItemLogic());
            }
        }

        public async Task<Models.InventoryItem> GetItem(int itemId, DateTime now)
        {
            using (var db = new Entities.InventoryDb())
            {
                var entity = await db.InventoryItems
                    .Include(ii => ii.InventoryItemType)
                    .Where(ii => ii.InventoryItemId == itemId)
                    .SingleOrDefaultAsync().ConfigureAwait(false);
                return entity == null ? null :
                    ToModel(entity, new Logic.InventoryItemLogic());
            }
        }

        public async Task<(int TotalItems, List<Models.InventoryItem> Items)> SearchItemsAsync(
            bool includeAvailable,
            bool includeExpired,
            bool includeSold,
            bool includeDiscarded,
            IEnumerable<Models.InventoryItemSortOrder> sortOrder,
            int skip, int take,
            DateTime now)
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
                                case Models.InventoryItemSortOrder.Quality: return "CurrentQuality";
                                case Models.InventoryItemSortOrder.QualityDescending: return "CurrentQuality DESC";
                                default: return "ERROR";
                            }
                        }));

            var itemTypes = (await GetItemTypesAsync().ConfigureAwait(false))
                .ToDictionary(iit => iit.InventoryItemTypeId);

            using (var db = new Entities.InventoryDb())
            {
                var items = 
                    //This calls the stored proc dbo.InventoryItems_Search and returns a List of InventoryItem entities.
                    db.InventoryItemSearch(
                        includeAvailable, includeExpired, includeSold, includeDiscarded,
                        orderByClause, skip, take, now, 
                        out int? totalRows)
                    .Select(ii => ToModel(ii, itemTypes, new Logic.InventoryItemLogic()))
                    .ToList();
                return (TotalItems: totalRows.Value, Items: items);
            }
        }

        public async Task<Models.InventoryItem> MarkItemDiscardedAsync(int itemId, DateTime now)
        {
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

        public async Task<Models.InventoryItem> MarkItemSoldAsync(int itemId, DateTime now)
        {
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
                QualityDeltaStrategyId = (Models.InventoryItemQualityDeltaStrategyId)iit.QualityDeltaStrategyId,
                BaseDelta = iit.BaseDelta,
                MinQuality = iit.MinQuality,
                MaxQuality = iit.MaxQuality,
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
