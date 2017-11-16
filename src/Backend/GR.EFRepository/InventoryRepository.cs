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
        #region Constructor and Dependencies
        public InventoryRepository(Entities.InventoryDbFactory dbFactory)
        {
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        private Entities.InventoryDbFactory _dbFactory;

        private Entities.IInventoryDb NewDbContext() => _dbFactory.Create();
        #endregion

        #region QueryDb Overloads
        protected Task<T> QueryDbAsync<T>(Entities.IInventoryDb db, Func<Entities.IInventoryDb, Task<T>> query)
            => query(db ?? throw new ArgumentNullException(nameof(db)));

        protected Task QueryDbAsync(Entities.IInventoryDb db, Func<Entities.IInventoryDb, Task> query)
            => query(db ?? throw new ArgumentNullException(nameof(db)));

        protected async Task<T> QueryDbAsync<T>(Func<Entities.IInventoryDb, Task<T>> query)
        { using (var db = NewDbContext()) return await QueryDbAsync(db, query).ConfigureAwait(false); }

        protected async Task QueryDbAsync(Func<Entities.IInventoryDb, Task> query)
        { using (var db = NewDbContext()) await QueryDbAsync(db, query).ConfigureAwait(false); }
        #endregion

        #region  IInventoryRepository Methods
        public Task<List<Models.InventoryItemType>> GetItemTypesAsync()
            => QueryDbAsync(async db =>
                (await db.InventoryItemTypes
                    .ToListAsync().ConfigureAwait(false))
                    .Select(iit => ToModel(iit))
                    .ToList());

        public Task<Models.InventoryItemType> GetItemTypeAsync(short inventoryItemTypeId)
            => QueryDbAsync(
                async db =>
                {
                    var entitiy = await db.InventoryItemTypes
                        .FindAsync(inventoryItemTypeId).ConfigureAwait(false);
                    return entitiy == null ? null : ToModel(entitiy);
                });

        public Task<List<Models.InventoryItemQualityDeltaStrategy>> GetQualityDeltaStrategiesAsync()
            => QueryDbAsync(async db =>
                (await db.QualityDeltaStrategies
                    .ToListAsync().ConfigureAwait(false))
                    .Select(qds =>
                        new Models.InventoryItemQualityDeltaStrategy
                        {
                            StrategyId = (Models.InventoryItemQualityDeltaStrategyId)qds.QualityDeltaStrategyId,
                            Name = qds.Name,
                            Description = qds.Description,
                        })
                    .ToList());

        public Task<Models.InventoryItem> StoreNewItemAsync(
            short itemTypeId, string name, string description, double initialQuality, double currentQuality,
            DateTime? sellByDate, DateTime inventoryDate)
            => QueryDbAsync(
                async db =>
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
                            CreatedDate = DateTime.Now,
                        });

                    await db.SaveChangesAsync().ConfigureAwait(false);

                    newItem.InventoryItemType = await db.InventoryItemTypes.FindAsync(itemTypeId).ConfigureAwait(false);
                    return ToModel(newItem);
                });

        public Task<Models.InventoryItem> GetItemAsync(int itemId)
            => QueryDbAsync(
                async db =>
                {
                    var entity = await db.InventoryItems
                        .Include(ii => ii.InventoryItemType)
                        .Where(ii => ii.InventoryItemId == itemId)
                        .SingleOrDefaultAsync().ConfigureAwait(false);
                    return entity == null ? null : ToModel(entity);
                });

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
                                case Models.InventoryItemSortOrder.Name: return "II.Name";
                                case Models.InventoryItemSortOrder.NameDescending: return "II.Name DESC";
                                case Models.InventoryItemSortOrder.ItemType: return "IIT.Name";
                                case Models.InventoryItemSortOrder.ItemTypeDescending: return "IIT.Name DESC";
                                case Models.InventoryItemSortOrder.InventoryDate: return "II.InventoryDate";
                                case Models.InventoryItemSortOrder.InventoryDateDescending: return "II.InventoryDate DESC";
                                case Models.InventoryItemSortOrder.SellByDate: return "II.SellByDate";
                                case Models.InventoryItemSortOrder.SellByDateDescending: return "II.SellByDate DESC";
                                case Models.InventoryItemSortOrder.Quality: return "II.CurrentQuality";
                                case Models.InventoryItemSortOrder.QualityDescending: return "II.CurrentQuality DESC";
                                default: return "[ERROR]";
                            }
                        })
                    ?? new string[] { });

            var itemTypes = (await GetItemTypesAsync().ConfigureAwait(false))
                .ToDictionary(iit => iit.InventoryItemTypeId);

            return await QueryDbAsync(
                async db =>
                {
                    int? totalRows = null;
                    var items =
                        //This calls the stored proc dbo.InventoryItems_Search and returns a List of InventoryItem entities.
                        (await Task.Factory.StartNew(() =>
                            db.InventoryItemSearch(
                                includeAvailable, includeExpired, includeSold, includeDiscarded,
                                orderByClause, skip, take, now,
                                out totalRows)).ConfigureAwait(false))
                        .Select(ii => ToModel(ii, itemTypes))
                        .ToList();
                    return (TotalItems: totalRows.Value, Items: items);
                });
        }

        public Task<Models.InventoryItem> MarkItemDiscardedAsync(int itemId, DateTime now)
            => QueryDbAsync(
                async db =>
                {
                    var item = await db.InventoryItems
                    .Include(ii => ii.InventoryItemType)
                    .SingleOrDefaultAsync(ii => ii.InventoryItemId == itemId).ConfigureAwait(false);
                    if (item == null)
                        throw new KeyNotFoundException($"No {nameof(Models.InventoryItem)} with {nameof(Models.InventoryItem.InventoryItemId)} = {itemId} was found.");

                    item.DiscardDate = now;
                    item.ModifiedDate = DateTime.Now;

                    await db.SaveChangesAsync().ConfigureAwait(false);

                    return ToModel(item);
                });

        public Task<Models.InventoryItem> MarkItemSoldAsync(int itemId, DateTime now)
            => QueryDbAsync(
                async db =>
                {
                    var item = await db.InventoryItems
                    .Include(ii => ii.InventoryItemType)
                    .SingleOrDefaultAsync(ii => ii.InventoryItemId == itemId).ConfigureAwait(false);
                    if (item == null)
                        throw new KeyNotFoundException($"No {nameof(Models.InventoryItem)} with {nameof(Models.InventoryItem.InventoryItemId)} = {itemId} was found.");

                    item.SaleDate = now;
                    item.ModifiedDate = DateTime.Now;

                    await db.SaveChangesAsync().ConfigureAwait(false);

                    return ToModel(item);
                });

        public Task UpdateItemCurrentQuality(int itemId, double currentQuality)
            => QueryDbAsync(
                async db =>
                {
                    var item = (await db.InventoryItems.FindAsync(itemId).ConfigureAwait(false))
                    ?? throw new KeyNotFoundException($"No InventoryItem with InventoryItemId = {itemId} was found.");
                    item.CurrentQuality = currentQuality;
                    item.ModifiedDate = DateTime.Now;

                    await db.SaveChangesAsync().ConfigureAwait(false);
                    return;
                });
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

        private Models.InventoryItem ToModel(Entities.InventoryItem ii)
            => ToModel(ii, ii.InventoryItemType == null ? null : new[] { ToModel(ii.InventoryItemType) }.ToDictionary(iit => iit.InventoryItemTypeId));
                
        private Models.InventoryItem ToModel(Entities.InventoryItem ii, Dictionary<short, Models.InventoryItemType> itemTypes)
            => new Models.InventoryItem
            {
                InventoryItemId = ii.InventoryItemId,
                ItemType = itemTypes?[ii.InventoryItemTypeId],
                Name = ii.Name,
                Description = ii.Description,
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
