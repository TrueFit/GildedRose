using GildedRose.Client.Models;
using GildedRose.Contracts;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    Id = Guid.Parse(item.Guid),
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
                    Guid = item.Id.ToString(),
                    Name = item.Name,
                    Category = item.Category,
                    SellIn = item.SellIn,
                    Quality = item.Quality
                }
            });
        }

        /// <inheritdoc />
        public void AddItems(IList<IItemModel> items)
        {
            var request = new AddItemsRequest();

            request.Items.Add(items.Select(x => new Item()
            {
                Guid = x.Id.ToString(),
                Name = x.Name,
                Category = x.Category,
                SellIn = x.SellIn,
                Quality = x.Quality
            }));

            _inventoryClient.AddItems(request);
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
                Id = Guid.Parse(getItemByNameResponse.Item.Guid),
                Name = getItemByNameResponse.Item.Name,
                Category = getItemByNameResponse.Item.Category,
                SellIn = getItemByNameResponse.Item.SellIn,
                Quality = getItemByNameResponse.Item.Quality
            };
        }

        /// <inheritdoc />
        public IList<IItemModel> ProgressToNextDay()
        {
            var progressedItems = new List<IItemModel>();

            var progressToNextDayResponse = _inventoryClient.ProgressToNextDay(new ProgressToNextDayRequest());
            if (progressToNextDayResponse == null || progressToNextDayResponse.Items == null)
                return progressedItems;

            foreach (var item in progressToNextDayResponse.Items)
            {
                progressedItems.Add(new ItemModel()
                {
                    Id = Guid.Parse(item.Guid),
                    SellIn = item.SellIn,
                    Quality = item.Quality
                });
            }

            return progressedItems;
        }

        /// <inheritdoc />
        public IList<IItemModel> GetTrash()
        {
            var items = new List<IItemModel>();

            var getTrashResponse = _inventoryClient.GetTrash(new GetTrashRequest());
            if (getTrashResponse == null || getTrashResponse.Items == null)
                return items;

            foreach (var item in getTrashResponse.Items)
            {
                items.Add(new ItemModel()
                {
                    Id = Guid.Parse(item.Guid),
                    Name = item.Name,
                    Category = item.Category,
                    SellIn = item.SellIn,
                    Quality = item.Quality
                });
            }

            return items;
        }

        /// <inheritdoc />
        public IList<Guid> RemoveTrash()
        {
            var removedItems = new List<Guid>();

            var removeTrashResponse = _inventoryClient.RemoveTrash(new RemoveTrashRequest());
            if (removeTrashResponse == null || removeTrashResponse.Guids == null)
                return removedItems;

            foreach (var removedItem in removeTrashResponse.Guids)
                removedItems.Add(Guid.Parse(removedItem));

            return removedItems;
        }

        /// <inheritdoc />
        public int GetTotalWorth()
        {
            var getTotalWorthResponse = _inventoryClient.GetTotalWorth(new GetTotalWorthRequest());
            if (getTotalWorthResponse == null)
                return 0;

            return getTotalWorthResponse.Worth;
        }
    }
}
