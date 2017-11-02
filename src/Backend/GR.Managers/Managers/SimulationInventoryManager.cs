using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Managers
{
    /// <summary>
    /// Tracks and provides a simulated current date and time to a wrapped <see cref="InventoryManager"/>.
    /// </summary>
    public class SimulationInventoryManager : IInventoryManager
    {
        #region Constructor and Dependencies
        public SimulationInventoryManager(InventoryManager inventoryManager, Repositories.IConfigRepository configRepository)
        {
            _inventoryManager = inventoryManager ?? throw new ArgumentNullException(nameof(inventoryManager));
            _configRepository = configRepository ?? throw new ArgumentNullException(nameof(configRepository));

            var config = _configRepository.GetConfiguration();
            Offset = config.SimulationDateOffset;
        }
        
        private InventoryManager _inventoryManager;
        private Repositories.IConfigRepository _configRepository;

        private TimeSpan Offset { get; set; }
        #endregion

        public DateTime Now => DateTime.Now + Offset;

        public async Task<Models.ProcessResults.NightlyQualityUpdateResult> AdvanceDateByOneDay()
        {
            Offset = Offset.Add(TimeSpan.FromDays(1));
            await _configRepository.SetSimulationDateOffset(Offset).ConfigureAwait(false);
            return await UpdateItemQualityAsync(Now).ConfigureAwait(false);
        }

        public async Task<Models.ProcessResults.NightlyQualityUpdateResult> SetDate(DateTime newNow)
        {
            Offset = newNow - DateTime.Now;
            await _configRepository.SetSimulationDateOffset(Offset).ConfigureAwait(false);
            return await UpdateItemQualityAsync(Now);
        }

        #region IInventoryManager Wrapper Methods
        public Task<List<Models.InventoryItemType>> GetItemTypesAsync() 
            => _inventoryManager.GetItemTypesAsync();

        public Task<List<Models.InventoryItemQualityDeltaStrategy>> GetQualityDeltaStrategiesAsync() 
            => _inventoryManager.GetQualityDeltaStrategiesAsync();

        public Task<Models.InventoryItem> AddNewItemAsync(
            short itemTypeId, string name, string description, double quality,
            DateTime? sellByDate, DateTime? now = null, DateTime? inventoryDate = null)
            => _inventoryManager.AddNewItemAsync(
                itemTypeId, name, description, quality,
                sellByDate, now ?? Now, inventoryDate);

        public Task<Models.Projections.InventoryItemSearchResults> GetAvailableItemsAsync(
            int page, int pageSize,
            IEnumerable<Models.InventoryItemSortOrder> sortOrder = null,
            bool includeExpired = false,
            DateTime? now = null)
            => _inventoryManager.GetAvailableItemsAsync(page, pageSize, sortOrder, includeExpired, now ?? Now);

        public Task<Models.Projections.InventoryItemSearchResults> GetExpiredItemsAsync(
            int page, int pageSize,
            IEnumerable<Models.InventoryItemSortOrder> sortOrder = null,
            DateTime? now = null)
            => _inventoryManager.GetExpiredItemsAsync(page, pageSize, sortOrder, now ?? Now);

        public Task<Models.InventoryItem> DiscardItemAsync(int itemId, DateTime? now = null)
            => _inventoryManager.DiscardItemAsync(itemId, now ?? Now);

        public Task<Models.InventoryItem> SellItemAsync(int itemId, DateTime? now = null)
            => _inventoryManager.SellItemAsync(itemId, now ?? Now);

        public Task<Models.ProcessResults.NightlyQualityUpdateResult> UpdateItemQualityAsync(DateTime? now = null)
            => _inventoryManager.UpdateItemQualityAsync(now ?? Now);
        #endregion
    }
}
