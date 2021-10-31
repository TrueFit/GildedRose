using DataAccessLibrary;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<bool> IncrementDays(int daysToIncrement)
        {
            try
            {
                var allItems = await _context.Items.Where(x => true).ToListAsync();
                var allCategories = await _context.Categories.Where(x => true).ToListAsync();

                var tempItemList = new List<Item>();

                foreach (var item in allItems)
                {

                    //store the items category for easy access
                    var category = allCategories.FirstOrDefault(x => x.CategoryId == item.CategoryId);

                    //If Legendary, take no further action
                    if (category.IsLegendary)
                    {
                        tempItemList.Add(item);
                        continue;
                    }

                    //always remove day from SellIn
                    item.SellIn =  item.SellIn - daysToIncrement;

                    if (item.QualityAppreciates && item.Quality < 50) //quality increases
                    {
                        //Concert Ticket Quality
                        if (category.CategoryName == "Backstage Passes")
                        {
                            for (int d = daysToIncrement; d > 0; d--)
                            {
                                switch (d)
                                {
                                    //case -> d
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
                                }
                            }
                        }
                        else
                        {
                            if (item.ItemName == "Aged Brie") //nested inside for future simple increments
                                item.Quality += category.DegenerationFactor * daysToIncrement;
                        }
                    }
                    else if (item.Quality > 0) //quality decreases
                    {
                        if (item.SellIn > 0)
                            //sell by hasnt passed, use normal quality degeneration
                            item.Quality = item.Quality - (category.DegenerationFactor * daysToIncrement);
                        else
                            //sell by passed, degenerate quality x 2
                            item.Quality = item.Quality - ((category.DegenerationFactor * daysToIncrement) * 2);
                    }
                    //make sure we dont go negative on Quality
                    if (item.Quality < 0) item.Quality = 0;

                    tempItemList.Add(item);
                }
                //Update all items and save
                _context.Items.UpdateRange(tempItemList);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: Cannot Increment days");
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}

