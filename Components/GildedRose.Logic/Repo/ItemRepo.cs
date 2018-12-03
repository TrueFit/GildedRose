using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using GildedRose.Logic.Models;
using GildedRose.Contracts;

namespace GildedRose.Logic.Repo
{
    public class ItemRepo
    {
        private readonly IItemManager itemManager;

        public ItemRepo(IItemManager itemManager)
        {
            this.itemManager = itemManager;
        }

        public async Task<IEnumerable<Inventory>> GetAll(DateTime dateViewed)
        {
            var allItems = await this.itemManager.GetAll();
            var items =
                allItems.Select(
                x => new
                Inventory()
                {
                    Identifier = x.Identifier,
                    Name = x.Name,
                    CurrentQuality =
                        this.CalculateQuality(
                            x.Name,
                            x.CategoryName,
                            (int)(dateViewed.Date - x.StockDate.Date).TotalDays,
                            x.InitialQuality,
                            x.ShelfLife),
                    MaxQuality = x.CategoryName != "Sulfuras" ? 50 : 80,
                    InitialQuality = x.InitialQuality,
                    SellIn = x.ShelfLife - (int)(dateViewed.Date - x.StockDate.Date).TotalDays,
                    CategoryId = x.CategoryId,
                    CategoryName = x.CategoryName,
                    IsLegendary = x.IsLegendary,
                    Created = x.Created,
                    CreatedBy = x.CreatedBy,
                    Modified = x.Modified,
                    ModifiedBy = x.ModifiedBy,
                });

            return items;
        }

        private int CalculateQuality(string name, string category, int daysInStock, int initialQuality, int shelfLife)
        {
            int quality = 0;
            switch (category.ToUpper().Trim())
            {
                case "SULFURAS":
                        return 80;

                case "BACKSTAGE PASSES":
                        if (daysInStock == shelfLife)
                        {
                            return 50;
                        }

                        if (daysInStock < shelfLife)
                        {
                            var remainingDaysTillConcert = shelfLife - daysInStock;
                            if (remainingDaysTillConcert <= 10)
                            {
                                quality = initialQuality + (daysInStock + ((10 - remainingDaysTillConcert + 1) * 2));
                            }

                            if (remainingDaysTillConcert <= 5)
                            {
                            quality = initialQuality + (daysInStock + ((5 - remainingDaysTillConcert + 1) * 3));
                        }

                            return remainingDaysTillConcert < 0 ? 0
                                        : quality <= 50
                                                ? quality : 50;
                        }

                        return 0;

                case "CONJURED":

                        quality = initialQuality - (daysInStock * 2);
                        return quality > 0 ? quality : 0;

                default:
                        if (name.ToUpper().Trim() == "AGED BRIE")
                        {
                            if (daysInStock < shelfLife)
                            {
                                quality = daysInStock + initialQuality;
                                return quality < 50 ? quality : 50;
                            }
                        }

                        if (daysInStock <= shelfLife)
                        {
                            quality = initialQuality - daysInStock;
                        }

                        if (daysInStock > shelfLife)
                        {
                            quality = initialQuality - ((initialQuality + shelfLife + (daysInStock - shelfLife)) * 2);
                        }

                        return quality > 0 ? quality : 0;
            }
        }
    }
}
