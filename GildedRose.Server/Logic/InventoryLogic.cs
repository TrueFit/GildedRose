using GildedRose.Contracts;
using GildedRose.Server.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GildedRose.Server.Logic
{
    /// <summary>
    /// The actual logic to manage a shop inventory.
    /// </summary>
    public class InventoryLogic
    {
        private readonly static InventoryLogic _inventoryLogic = new InventoryLogic();

        public static InventoryLogic Instance { get { return _inventoryLogic; } }

        private readonly object _itemsLock;

        private readonly List<string> _increasingQualityItems;
        private readonly List<string> _fastDecayingItemCategories;
        private readonly List<string> _nonDecayingItemCategories;

        private IList<Item> _items;

        /// <summary>
        /// Constructor
        /// </summary>
        private InventoryLogic()
        {
            _itemsLock = new object();

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
        /// Initialize the inventory by loading the initial inventory.
        /// </summary>
        public void Initialize()
        {
            _items = InventoryFileImport.ImportItems(@"C:\Projects\Gilded Rose\trunk\inventory.txt", out var errors);

            // List import errors.
            if (errors.Any())
            {
                Console.WriteLine("Import errors:");
                foreach (var error in errors)
                    Console.WriteLine($"\t{error}");
            }
        }

        /// <summary>
        /// Get all items that are currently available in the store.
        /// </summary>
        public IList<Item> GetAllItems()
        {
            lock (_itemsLock)
            {
                return _items;
            }
        }

        /// <summary>
        /// Add a new item to the store's inventory.
        /// </summary>
        public void AddItem(Item item)
        {
            lock (_itemsLock)
            {
                _items.Add(item);
            }
        }

        /// <summary>
        /// Get a single item by its name.
        /// </summary>
        public Item GetItemByName(string name)
        {
            lock (_itemsLock)
            {
                return _items.FirstOrDefault(x => !string.IsNullOrEmpty(x.Name) && x.Name.Equals(name));
            }
        }

        /// <summary>
        /// Progress to the next working day and enjoy the rest of the day.
        /// </summary>
        public void ProgressToNextDay()
        {
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
                    item.SellIn += deltaSellIn;
                    item.Quality = Math.Max(0, Math.Min(maxQuality, item.Quality + deltaQuality));
                }
            }
        }

        /// <summary>
        /// Get all items with such a low quality that they are considered trash.
        /// </summary>
        public IList<Item> GetTrash()
        {
            return _items.Where(x => x.Quality <= 0).ToList();
        }

        /// <summary>
        /// Remove all trash from the inventory.
        /// </summary>
        public void RemoveTrash()
        {
            lock (_itemsLock)
            {
                for (var i = _items.Count - 1; i >= 0; i--)
                {
                    var item = _items[i];

                    if (item.Quality == 0)
                        _items.Remove(item);
                }
            }
        }
    }
}
