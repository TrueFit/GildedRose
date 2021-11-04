using GildedRose.Client.Models;
using GildedRose.Contracts;
using Grpc.Net.Client;
using System.Collections.Generic;
using static GildedRose.Contracts.Inventory;

namespace GildedRose.Client.InventorySystems
{
    /// <summary>
    /// An inventory system that is run by a gRPC server.
    /// </summary>
    public class GrpcInventorySystem : IInventorySystem
    {
        private GrpcChannel _grpcChannel;
        private InventoryClient _inventoryClient;

        /// <inheritdoc />
        public void Connect()
        {
            _grpcChannel = GrpcChannel.ForAddress("https://localhost:5001/");

            _inventoryClient = new InventoryClient(_grpcChannel);
        }

        /// <inheritdoc />
        public IList<IItemModel> GetAllItems()
        {
            var items = new List<IItemModel>();

            var getAllItemsResponse = _inventoryClient.GetAllItems(new GetAllItemsRequest());
            if (getAllItemsResponse == null)
                return items;

            foreach (var item in getAllItemsResponse.Items)
            {
                items.Add(new ItemModel()
                {
                    Name = item.Name,
                    Category = item.Category,
                    SellIn = item.SellIn,
                    Quality = item.Quality
                });
            }

            return items;
        }

        /// <inheritdoc />
        public void AddItem(IItemModel item)
        {
            _inventoryClient.AddItem(new AddItemRequest()
            {
                Item = new Item()
                {
                    Name = item.Name,
                    Category = item.Category,
                    SellIn = item.SellIn,
                    Quality = item.Quality
                }
            });
        }

        /// <inheritdoc />
        public IItemModel GetItemByName(string name)
        {
            var getItemByNameResponse = _inventoryClient.GetItemByName(new GetItemByNameRequest()
            {
                Name = name
            });

            if (getItemByNameResponse == null || getItemByNameResponse.Item == null)
                return new ItemModel();

            return new ItemModel()
            {
                Name = getItemByNameResponse.Item.Name,
                Category = getItemByNameResponse.Item.Category,
                SellIn = getItemByNameResponse.Item.SellIn,
                Quality = getItemByNameResponse.Item.Quality
            };
        }

        /// <inheritdoc />
        public void ProgressToNextDay()
        {
            _inventoryClient.ProgressToNextDay(new ProgressToNextDayRequest());
        }

        /// <inheritdoc />
        public IList<IItemModel> GetTrash()
        {
            var items = new List<IItemModel>();

            var getTrashResponse = _inventoryClient.GetTrash(new GetTrashRequest());
            if (getTrashResponse == null)
                return items;

            foreach (var item in getTrashResponse.Items)
            {
                items.Add(new ItemModel()
                {
                    Name = item.Name,
                    Category = item.Category,
                    SellIn = item.SellIn,
                    Quality = item.Quality
                });
            }

            return items;
        }

        /// <inheritdoc />
        public void RemoveTrash()
        {
            _inventoryClient.RemoveTrash(new RemoveTrashRequest());
        }
    }
}
