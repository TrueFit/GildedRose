using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GildedRose.InventoryApi
{
    public class InventoryController : Controller
    {
        #region Constructor and Dependencies
        public InventoryController(GR.Managers.IInventoryManager inventoryManager)
        {
            InventoryManager = inventoryManager
                ?? throw new ArgumentNullException(nameof(inventoryManager));
        }

        private GR.Managers.IInventoryManager InventoryManager { get; set; }
        #endregion

        [HttpGet]
        public Task<GR.Models.RequestResult<List<GR.Models.InventoryItemType>>> GetItemTypes()
            => WrapWithRequestResult(InventoryManager.GetItemTypesAsync);

        [HttpGet]
        public Task<GR.Models.RequestResult<List<GR.Models.InventoryItemQualityDeltaStrategy>>> GetQualityDeltaStrategies()
            => WrapWithRequestResult(InventoryManager.GetQualityDeltaStrategiesAsync);

        [HttpPost]
        public Task<GR.Models.RequestResult<GR.Models.InventoryItem>> AddNewItem([FromBody]Models.NewInventoryItemData newItemData)
            => WrapWithRequestResult(() => InventoryManager.AddNewItemAsync(
                newItemData.ItemTypeId, newItemData.Name, newItemData.Description, newItemData.Quality, newItemData.SellByDate));

        [HttpGet]
        public Task<GR.Models.Projections.InventoryItemSearchResults> GetAvailableItems(
            string[] sortOrder = null, int page = 1, int pageSize = 20)
            => InventoryManager.GetAvailableItemsAsync(page, pageSize, ParseItemSortOrder(sortOrder));

        [HttpGet]
        public Task<GR.Models.Projections.InventoryItemSearchResults> GetExpiredItems(
            string[] sortOrder = null, int page = 1, int pageSize = 20)
            => InventoryManager.GetExpiredItemsAsync(page, pageSize, ParseItemSortOrder(sortOrder));

        [HttpPost]
        public Task<GR.Models.RequestResult<GR.Models.InventoryItem>> DiscardItem(int itemId)
            => WrapWithRequestResult(() => InventoryManager.DiscardItemAsync(itemId));

        [HttpPost]
        public Task<GR.Models.RequestResult<GR.Models.InventoryItem>> SellItem(int itemId)
            => WrapWithRequestResult(() => InventoryManager.SellItemAsync(itemId));

        #region Private Methods
        private IEnumerable<GR.Models.InventoryItemSortOrder> ParseItemSortOrder(string[] sortOrderStrings)
            => sortOrderStrings == null || !sortOrderStrings.Any() ? null :
                sortOrderStrings
                    .Select(sos => ParseItemSortOrder(sos))
                    .Where(so => so != null)
                    .Cast<GR.Models.InventoryItemSortOrder>();

        private GR.Models.InventoryItemSortOrder? ParseItemSortOrder(string sortOrderString)
        {
            if (string.IsNullOrWhiteSpace(sortOrderString))
                return null;

            sortOrderString = sortOrderString.ToLower();
            switch (sortOrderString)
            {
                case "name": return GR.Models.InventoryItemSortOrder.Name;
                case "name desc": return GR.Models.InventoryItemSortOrder.NameDescending;
                case "itemtype": return GR.Models.InventoryItemSortOrder.ItemType;
                case "itemtype desc": return GR.Models.InventoryItemSortOrder.ItemTypeDescending;
                case "inventorydate": return GR.Models.InventoryItemSortOrder.InventoryDate;
                case "inventorydate desc": return GR.Models.InventoryItemSortOrder.InventoryDateDescending;
                case "sellbydate": return GR.Models.InventoryItemSortOrder.SellByDate;
                case "sellbydate desc": return GR.Models.InventoryItemSortOrder.SellByDateDescending;
                case "quality": return GR.Models.InventoryItemSortOrder.Quality;
                case "quality desc": return GR.Models.InventoryItemSortOrder.QualityDescending;
                default: return null;
            }
        }

        private async Task<GR.Models.RequestResult<T>> WrapWithRequestResult<T>(Func<Task<T>> request)
        {
            try
            {
                return GR.Models.RequestResult.Success(await request());
            }
            catch (Exception ex)
            {
                return GR.Models.RequestResult.Failure<T>(ex);
            }
        }
        #endregion
    }
}
