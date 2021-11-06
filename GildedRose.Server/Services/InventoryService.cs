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

        private IList<Item> _items;
        private readonly object _itemsLock;

        private readonly List<string> _increasingQualityItems;
        private readonly List<string> _fastDecayingItemCategories;
        private readonly List<string> _nonDecayingItemCategories;

        private bool _hasBeenInitialized;

        /// <summary>
        /// Constructor
        /// </summary>
        public InventoryService(ILogger<InventoryService> logger)
        {
            _logger = logger;

            _items = new List<Item>();
            _itemsLock = new object();

            _hasBeenInitialized = false;

            // List of items that increase their value over time.
            _increasingQualityItems = new List<string>()
            {
                "Aged Brie"
            };

            _fastDecayingItemCategories = new List<string>()
            {
                "Conjured"
            };

            // Items in this category never have to be sold or decrease in quality.
            _nonDecayingItemCategories = new List<string>()
            {
                "Sulfuras"
            };
        }

        /// <summary>
        /// Get all items that are currently available in the store.
        /// </summary>
        public override Task<GetAllItemsResponse> GetAllItems(GetAllItemsRequest request, ServerCallContext context)
        {
            Initialize();

            var response = new GetAllItemsResponse();

            lock (_itemsLock)
            {
                response.Items.Add(_items);
            }

            return Task.FromResult(response);
        }

        /// <summary>
        /// Add a new item to the store's inventory.
        /// </summary>
        public override Task<AddItemResponse> AddItem(AddItemRequest request, ServerCallContext context)
        {
            Initialize();

            if (request == null || request.Item == null)
                return Task.FromResult(new AddItemResponse());

            lock (_itemsLock)
            {
                _items.Add(request.Item);

                return Task.FromResult(new AddItemResponse());
            }
        }

        /// <summary>
        /// Get a single item by its name.
        /// </summary>
        public override Task<GetItemByNameResponse> GetItemByName(GetItemByNameRequest request, ServerCallContext context)
        {
            Initialize();

            if (request == null || string.IsNullOrEmpty(request.Name))
                return Task.FromResult(new GetItemByNameResponse());

            lock (_itemsLock)
            {
                var item = _items.FirstOrDefault(x => !string.IsNullOrEmpty(x.Name) && x.Name.Equals(request.Name));
                if (item == null)
                    return Task.FromResult(new GetItemByNameResponse());

                return Task.FromResult(new GetItemByNameResponse()
                {
                    Item = item
                });
            }
        }

        /// <summary>
        /// Progress to the next working day and enjoy the rest of the day.
        /// </summary>
        public override Task<ProgressToNextDayResponse> ProgressToNextDay(ProgressToNextDayRequest request, ServerCallContext context)
        {
            Initialize();

            var progressedItems = new List<ProgressedItem>();

            lock (_itemsLock)
            {
                foreach (var item in _items)
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
                    if (_fastDecayingItemCategories.Contains(item.Category))
                        deltaQuality *= 2;

                    // There are also items that increase their value over time.
                    if (_increasingQualityItems.Contains(item.Name))
                        deltaQuality *= -1;

                    // Non-decaying objects don't change in any way.
                    if (_nonDecayingItemCategories.Contains(item.Category))
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
            }

            var response = new ProgressToNextDayResponse();
            response.Items.AddRange(progressedItems);

            return Task.FromResult(response);
        }

        /// <summary>
        /// Get all items with such a low quality that they are considered trash.
        /// </summary>
        public override Task<GetTrashResponse> GetTrash(GetTrashRequest request, ServerCallContext context)
        {
            Initialize();

            var response = new GetTrashResponse();

            lock (_itemsLock)
            {
                response.Items.AddRange(_items.Where(x => x.Quality <= 0).ToList());
            }

            return Task.FromResult(response);
        }

        /// <summary>
        /// Remove all trash from the inventory.
        /// </summary>
        public override Task<RemoveTrashResponse> RemoveTrash(RemoveTrashRequest request, ServerCallContext context)
        {
            Initialize();

            var removedItems = new List<string>();

            lock (_itemsLock)
            {
                for (var i = _items.Count - 1; i >= 0; i--)
                {
                    var item = _items[i];

                    if (item.Quality == 0)
                    {
                        _items.Remove(item);

                        removedItems.Add(item.Guid);
                    }
                }
            }

            var response = new RemoveTrashResponse();
            response.Guids.AddRange(removedItems);

            return Task.FromResult(response);
        }

        public override Task<GetTotalWorthResponse> GetTotalWorth(GetTotalWorthRequest request, ServerCallContext context)
        {
            Initialize();

            var totalWorth = 0;

            lock (_itemsLock)
            {
                foreach (var item in _items)
                {
                    totalWorth += item.Quality;
                }
            }

            return Task.FromResult(new GetTotalWorthResponse()
            {
                Worth = totalWorth
            });
        }


        /// <summary>
        /// Initialize the inventory by loading the initial inventory.
        /// </summary>
        private void Initialize()
        {
            lock (_itemsLock)
            {
                if (_hasBeenInitialized)
                    return;

                // Load inventory list from data base if one exists. If there is no data base, load the inventory text file.
                IDataSource dataSource = new InventoryListDataSource("inventory.txt");

                if (File.Exists(FileUtils.GetDataBaseFileName()))
                    dataSource = new SQLiteDataSource();

                _items = dataSource.GetAllItems(out var errors);

                // List import errors.
                if (errors.Any())
                {
                    Console.WriteLine("Import errors:");
                    foreach (var error in errors)
                        Console.WriteLine($"\t{error}");
                }

                _hasBeenInitialized = true;
            }
        }
    }
}
