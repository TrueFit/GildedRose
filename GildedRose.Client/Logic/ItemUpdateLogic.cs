using GildedRose.Client.Models;
using GildedRose.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GildedRose.Client.Logic
{
    public class ItemUpdateLogic
    {
        private readonly List<string> _increasingQualityItems;
        private readonly List<string> _fastDecayingItemCategories;
        private readonly List<string> _nonDecayingItemCategories;

        public ItemUpdateLogic()
        {
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

        public void AddItem(ObservableCollection<ItemCategoryViewModel> categories, ItemModel itemModel)
        {
            var category = categories.FirstOrDefault(x => !string.IsNullOrEmpty(x.Name) && x.Name.Equals(itemModel.Category));
            if (category == null)
            {
                category = new ItemCategoryViewModel(new ItemCategoryModel()
                {
                    Name = itemModel.Category
                });
                categories.Add(category);
            }

            category.Items.Add(new ItemViewModel(itemModel));
        }

        public void UpdateItems(ObservableCollection<ItemCategoryViewModel> categories)
        {
            // Assumptions:
            // - The requirements do not specify how fast items decay each day. It is assumed the decay is 1 per day.
            // - The item "Sulfuras, Hand of Ragnaros" is stored in a format in the inventory file that is valid with "Hand of Ragnaros"
            //   being the name and Sulfuras being the category. It is therefore being processed as such.
            // - It is not specified how items in different categories would behave. In my generalized approach, a conjured aged brie would
            //   get a high quality very quickly.

            foreach (var category in categories)
            {
                foreach (var item in category.Items)
                {
                    // By default, the maximum quality of an item is 50 and the quality and sell-in date for an items decrease by one
                    // for each passed day.
                    var maxQuality = 50;
                    var deltaQuality = -1;
                    var deltaSellIn = -1;

                    // By default, an item's quality decreases twice as fast once it's expired.
                    if (item.Model.SellIn <= 0)
                        deltaQuality *= 2;

                    // Certain items might decay faster than others.
                    if (_fastDecayingItemCategories.Contains(item.Model.Category))
                        deltaQuality *= 2;

                    // There are also items that increase their value over time.
                    if (_increasingQualityItems.Contains(item.Model.Name))
                        deltaQuality *= -1;

                    // Non-decaying objects don't change in any way.
                    if (_nonDecayingItemCategories.Contains(item.Model.Category))
                    {
                        deltaQuality = Math.Max(0, deltaQuality);
                        deltaSellIn = 0;

                        // Sulfuras can have a higher quality.
                        if (item.Model.Category.Equals("Sulfuras"))
                            maxQuality = item.Model.Quality;
                    }

                    // Backstage passes are special.
                    if (item.Model.Category.Equals("Backstage Passes"))
                    {
                        if (item.Model.SellIn >= 0)
                        {
                            deltaQuality = 1;

                            if (item.Model.SellIn <= 10)
                                deltaQuality++;

                            if (item.Model.SellIn <= 5)
                                deltaQuality++;
                        }
                        else
                        {
                            deltaQuality = int.MinValue;
                        }
                    }

                    // Update quality and sell-in date.
                    item.Model.SellIn += deltaSellIn;
                    item.Model.Quality = Math.Max(0, Math.Min(maxQuality, item.Model.Quality + deltaQuality));
                }
            }
        }

        public void RemoveTrash(ObservableCollection<ItemCategoryViewModel> categories)
        {
            for (var j = categories.Count - 1; j >= 0; j--)
            {
                var category = categories[j];

                for (var i = category.Items.Count - 1; i >= 0; i--)
                {
                    var item = category.Items[i];

                    if (item.Quality == 0)
                        category.Items.Remove(item);
                }

                if (category.Items.Count == 0)
                    categories.Remove(category);
            }
        }
    }
}
