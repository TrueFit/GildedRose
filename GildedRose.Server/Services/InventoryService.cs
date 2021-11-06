using GildedRose.Contracts;
using GildedRose.Server.IO;
using GildedRose.Server.Utils;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            response.Items.Add(GetItems());

            return Task.FromResult(response);
        }

        /// <summary>
        /// Add a new item to the store's inventory.
        /// </summary>
        public override Task<AddItemResponse> AddItem(AddItemRequest request, ServerCallContext context)
        {
            if (request == null || request.Item == null)
                return Task.FromResult(new AddItemResponse());

            GetDataSource().AddItem(request.Item);

            return Task.FromResult(new AddItemResponse());
        }

        /// <summary>
        /// Get a single item by its name.
        /// </summary>
        public override Task<GetItemByNameResponse> GetItemByName(GetItemByNameRequest request, ServerCallContext context)
        {
            if (request == null || string.IsNullOrEmpty(request.Name))
                return Task.FromResult(new GetItemByNameResponse());

            var item = GetDataSource().GetItemByName(request.Name);
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
            var progressedItems = new List<ProgressedItem>();

            var items = GetItems();

            // List of items that increase their value over time.
            var increasingQualityItems = new List<string>()
            {
                "Aged Brie"
            };

            var fastDecayingItemCategories = new List<string>()
            {
                "Conjured"
            };

            // Items in this category never have to be sold or decrease in quality.
            var nonDecayingItemCategories = new List<string>()
            {
                "Sulfuras"
            };

            foreach (var item in items)
            {
                // By default, the maximum quality of an item is 50 and the quality and sell-in date for an items decrease by one
                // for each passed day.
                var maxQuality = 50;
                var deltaQuality = -1;
                var deltaSellIn = -1;

                // By default, an item's quality decreases twice as fast once it's expired.
                if (item.SellIn <= 0)
                    deltaQuality *= 2;

                // Certain items might decay faster than others.
                if (fastDecayingItemCategories.Contains(item.Category))
                    deltaQuality *= 2;

                // There are also items that increase their value over time.
                if (increasingQualityItems.Contains(item.Name))
                    deltaQuality *= -1;

                // Non-decaying objects don't change in any way.
                if (nonDecayingItemCategories.Contains(item.Category))
                {
                    deltaQuality = Math.Max(0, deltaQuality);
                    deltaSellIn = 0;

                    // Sulfuras can have a higher quality.
                    if (item.Category.Equals("Sulfuras"))
                        maxQuality = item.Quality;
                }

                // Backstage passes are special.
                if (item.Category.Equals("Backstage Passes"))
                {
                    if (item.SellIn >= 0)
                    {
                        deltaQuality = 1;

                        if (item.SellIn <= 10)
                            deltaQuality++;

                        if (item.SellIn <= 5)
                            deltaQuality++;
                    }
                    else
                    {
                        deltaQuality = int.MinValue;
                    }
                }

                // Update quality and sell-in date.
                var newQuality = Math.Max(0, Math.Min(maxQuality, item.Quality + deltaQuality));

                item.SellIn += deltaSellIn;
                item.Quality = newQuality;

                // Store progress information.
                if (deltaSellIn != 0 || item.Quality != newQuality)
                {
                    progressedItems.Add(new ProgressedItem()
                    {
                        Guid = item.Guid,
                        SellIn = item.SellIn,
                        Quality = item.Quality
                    });
                }
            }

            GetDataSource().UpdateConditions(progressedItems);

            var response = new ProgressToNextDayResponse();
            response.Items.AddRange(progressedItems);

            return Task.FromResult(response);
        }

        /// <summary>
        /// Get all items with such a low quality that they are considered trash.
        /// </summary>
        public override Task<GetTrashResponse> GetTrash(GetTrashRequest request, ServerCallContext context)
        {
            var items = GetItems();

            var response = new GetTrashResponse();
            response.Items.AddRange(items.Where(x => x.Quality <= 0).ToList());

            return Task.FromResult(response);
        }

        /// <summary>
        /// Remove all trash from the inventory.
        /// </summary>
        public override Task<RemoveTrashResponse> RemoveTrash(RemoveTrashRequest request, ServerCallContext context)
        {
            var removedItems = new List<string>();

            var items = GetItems();

            foreach (var item in items)
            {
                if (item.Quality == 0)
                    removedItems.Add(item.Guid);
            }

            GetDataSource().RemoveItems(removedItems);

            var response = new RemoveTrashResponse();
            response.Guids.AddRange(removedItems);

            return Task.FromResult(response);
        }

        public override Task<GetTotalWorthResponse> GetTotalWorth(GetTotalWorthRequest request, ServerCallContext context)
        {
            var totalWorth = 0;

            var items = GetItems();

            foreach (var item in items)
                totalWorth += item.Quality;

            return Task.FromResult(new GetTotalWorthResponse()
            {
                Worth = totalWorth
            });
        }

        private SQLiteDataSource GetDataSource()
        {
            var dataSource = new SQLiteDataSource();

            if (!File.Exists(FileUtils.GetDataBaseFileName()))
            {
                var textFileDataSource = new InventoryListDataSource("inventory.txt");
                var items = textFileDataSource.GetAllItems(out var errors);

                ShowErrors(errors);

                dataSource.CreateNew(items);
            }

            return dataSource;
        }

        private IList<Item> GetItems()
        {
            var items = GetDataSource().GetAllItems(out var errors);

            ShowErrors(errors);

            return items;
        }

        private void ShowErrors(IList<string> errors)
        {
            if (errors.Any())
            {
                Console.WriteLine("Import errors:");
                foreach (var error in errors)
                    Console.WriteLine($"\t{error}");
            }
        }
    }
}
