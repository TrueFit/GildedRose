using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Repositories
{
    public interface IInventoryRepository
    {
        Task<List<Models.InventoryItemType>> GetItemTypesAsync();
        Task<List<Models.InventoryItemQualityDeltaStrategy>> GetQualityDeltaStrategiesAsync();

        Task<Models.InventoryItem> AddNewItemAsync(
            short itemTypeId, string name, string description, double quality, DateTime? sellByDate,
            DateTime? now = null, DateTime? inventoryDate = null);

        Task<(int TotalItems, List<Models.InventoryItem> Items)> GetAvailableItemsAsync(int skip = 0, int take = 100);
        Task<(int TotalItems, List<Models.InventoryItem> Items)> GetExpiredItemsAsync(int skip = 0, int take = 100, DateTime? now = null);

        Task<Models.InventoryItem> MarkItemDiscardedAsync(int itemId, DateTime? now = null);
        Task<Models.InventoryItem> MarkItemSoldAsync(int itemId, DateTime? now = null);
    }
}
