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
        Task<Models.InventoryItemType> GetItemTypeAsync(short inventoryItemTypeId);
        Task<List<Models.InventoryItemQualityDeltaStrategy>> GetQualityDeltaStrategiesAsync();

        Task<Models.InventoryItem> StoreNewItemAsync(
            short itemTypeId, string name, string description, double initialQuality, double currentQuality,
            DateTime? sellByDate, DateTime inventoryDate);

        Task<Models.InventoryItem> GetItemAsync(int itemId);

        Task<(int TotalItems, List<Models.InventoryItem> Items)> SearchItemsAsync(
                    bool includeAvailable,
                    bool includeExpired,
                    bool includeSold,
                    bool includeDiscarded,
                    IEnumerable<Models.InventoryItemSortOrder> sortOrder,
                    int skip, int take,
                    DateTime now);
        
        Task<Models.InventoryItem> MarkItemDiscardedAsync(int itemId, DateTime now);
        Task<Models.InventoryItem> MarkItemSoldAsync(int itemId, DateTime now);

        Task UpdateItemCurrentQuality(int itemId, double currentQuality);
    }
}
