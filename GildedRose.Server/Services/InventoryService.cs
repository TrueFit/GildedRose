using GildedRose.Contracts;
using GildedRose.Server.Logic;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GildedRose.Server
{
    /// <summary>
    /// A service to manage a shop's inventory.
    /// </summary>
    public class InventoryService : Inventory.InventoryBase
    {
        private readonly ILogger<InventoryService> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        public InventoryService(ILogger<InventoryService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get all items that are currently available in the store.
        /// </summary>
        public override Task<GetAllItemsResponse> GetAllItems(GetAllItemsRequest request, ServerCallContext context)
        {
            var response = new GetAllItemsResponse();
            response.Items.Add(InventoryLogic.Instance.GetAllItems());

            return Task.FromResult(response);
        }

        /// <summary>
        /// Add a new item to the store's inventory.
        /// </summary>
        public override Task<AddItemResponse> AddItem(AddItemRequest request, ServerCallContext context)
        {
            if (request == null || request.Item == null)
                return Task.FromResult(new AddItemResponse());

            InventoryLogic.Instance.AddItem(request.Item);

            return Task.FromResult(new AddItemResponse());
        }

        /// <summary>
        /// Get a single item by its name.
        /// </summary>
        public override Task<GetItemByNameResponse> GetItemByName(GetItemByNameRequest request, ServerCallContext context)
        {
            if (request == null || string.IsNullOrEmpty(request.Name))
                return Task.FromResult(new GetItemByNameResponse());

            var item = InventoryLogic.Instance.GetItemByName(request.Name);
            if (item == null)
                return Task.FromResult(new GetItemByNameResponse());

            return Task.FromResult(new GetItemByNameResponse()
            {
                Item = item
            });
        }

        /// <summary>
        /// Progress to the next working day and enjoy the rest of the day.
        /// </summary>
        public override Task<ProgressToNextDayResponse> ProgressToNextDay(ProgressToNextDayRequest request, ServerCallContext context)
        {
            var progressedItems = InventoryLogic.Instance.ProgressToNextDay();

            var response = new ProgressToNextDayResponse();
            response.Items.AddRange(progressedItems);

            return Task.FromResult(response);
        }

        /// <summary>
        /// Get all items with such a low quality that they are considered trash.
        /// </summary>
        public override Task<GetTrashResponse> GetTrash(GetTrashRequest request, ServerCallContext context)
        {
            var response = new GetTrashResponse();
            response.Items.AddRange(InventoryLogic.Instance.GetTrash());

            return Task.FromResult(response);
        }

        /// <summary>
        /// Remove all trash from the inventory.
        /// </summary>
        public override Task<RemoveTrashResponse> RemoveTrash(RemoveTrashRequest request, ServerCallContext context)
        {
            var removedItems = InventoryLogic.Instance.RemoveTrash();

            var response = new RemoveTrashResponse();
            response.Guids.AddRange(removedItems);

            return Task.FromResult(response);
        }
    }
}
