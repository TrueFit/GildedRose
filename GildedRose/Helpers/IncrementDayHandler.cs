using DataAccessLibrary;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GildedRose.Helpers
{
    public class IncrementDayHandler
    {
        private readonly InventoryContext _context;

        public IncrementDayHandler(InventoryContext context)
        {
            _context = context;
        }

        public async Task<bool> IncrementOneDay()
        {
            var allItems = await _context.Items.Where(x=> true).ToListAsync();
            var allCategories = await _context.Categories.Where(x=> true).ToListAsync();

            var tempItemList = new List<Item>();

            foreach(var item in allItems)
            {
                //always remove one day from SellIn
                item.SellIn --;

                //store items category for easy access
                var category = allCategories.FirstOrDefault(x => x.CategoryId == item.CategoryId);

                //If Legendary, take no action
                if (category.IsLegendary)
                {
                    tempItemList.Add(item);
                    continue;
                }


                if (item.QualityAppreciates && item.Quality < 50) //quality increases
                {
                    //Concert Ticket Quality
                    if(category.CategoryName == "Backstage Passes")
                    {
                        switch (item.SellIn)
                        {
                            case int i when i > 10:
                                item.Quality += 1;
                                break;
                            case int i when i <= 10 && i > 5:
                                item.Quality += 2;
                                break;
                            case int i when i <= 5 && i > 0:
                                item.Quality += 3;
                                break;
                            case int i when i <= 0:
                                item.Quality = 0;
                                break;
                            default:
                                break;
                        }
                    }
                    else //nested inside for future simple increments
                    {
                        if (item.ItemName == "Aged Brie")
                            item.Quality++;
                    }
                }
                else if (item.Quality != 0) //quality decreases
                {
                    //TODO hanle depreciation
                }

                tempItemList.Add(item);
            }

            //Update all items and save
            _context.Items.UpdateRange(tempItemList);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
