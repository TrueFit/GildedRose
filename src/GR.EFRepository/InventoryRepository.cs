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

        public async Task<Models.InventoryItem> AddNewItemAsync(
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

        public Task<(int TotalItems, List<Models.InventoryItem> Items)> GetAvailableItemsAsync(int skip = 0, int take = 100)
            => QueryItems(db => db.InventoryItems.Where(ii => ii.SaleDate == null && ii.DiscardDate == null), skip, take);

        public Task<(int TotalItems, List<Models.InventoryItem> Items)> GetExpiredItemsAsync(int skip = 0, int take = 100, DateTime? now = null)
        {
            now = now ?? DateTime.Now;
            return QueryItems(
                db => db.InventoryItems.Where(ii => ii.SaleDate == null && ii.DiscardDate == null && ii.SellByDate < now),
                skip, take);
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

        private async Task<(int TotalItems, List<Models.InventoryItem> Items)> QueryItems(
            Func<Entities.InventoryDb, IQueryable<Entities.InventoryItem>> query, 
            int skip = 0, int take = 100)
        {
            var itemTypes = (await GetItemTypesAsync()).ToDictionary(iit => iit.InventoryItemTypeId);
            using (var db = new Entities.InventoryDb())
            {
                var iiLogic = new Logic.InventoryItemLogic();
                var q = query(db);
                return
                (
                    TotalItems: q.Count(),
                    Items: (await q.OrderBy(ii => ii.Name)
                        .Skip(skip).Take(take)
                        .ToListAsync().ConfigureAwait(false))
                        .Select(ii => ToModel(ii, itemTypes, iiLogic))
                        .ToList()
                );
            }
        }

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
    }
}
