
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GildedRose_Blazor.Shared;

namespace GildedRose_Blazor.Server.Helpers
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
                //retreive all items and categories
                var allItems = await _context.Items.Where(x => true).ToListAsync();
                var allCategories = await _context.Categories.Where(x => true).ToListAsync();

                //create a list to hold the adjusted item objects
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

                    //keep a copy of the original sellIn before altering
                    var tempSellin = item.SellIn;
                    //always remove day from SellIn
                    item.SellIn -=  daysToIncrement;

                    //Change Quality
                    if (item.QualityAppreciates && item.Quality < 50) //quality increases
                    {
                        //Concert Ticket Quality
                        if (category.CategoryName == "Backstage Passes")
                        {
                            for (int d = 1; d < daysToIncrement+1; d++)
                            {
                                switch (tempSellin - d)
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
                                }
                            }
                        }
                        else // simple increment block
                        {
                                item.Quality += category.DegenerationFactor * daysToIncrement;
                        }
                    }
                    else if (item.Quality > 0) //quality decreases
                    {
                        if (item.SellIn > 0)
                            //sell by hasnt passed, use normal quality degeneration
                            item.Quality -= (category.DegenerationFactor * daysToIncrement);
                        else
                            //sell by passed, degenerate quality x 2
                            item.Quality -= ((category.DegenerationFactor * daysToIncrement) * 2);
                    }
                    //make sure we dont go negative on Quality
                    if (item.Quality < 0) item.Quality = 0;

                    //add to final item list
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

