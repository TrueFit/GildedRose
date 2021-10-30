using DataAccessLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GildedRose.Helpers
{
    public class DataCreateAndDrop
    {
        public static async Task<string> CreateExampleData()
        {
            using (var db = new InventoryContext())
            {
                Console.WriteLine($"Database path: {db.SQLiteInstance}.");

                // If category data exists, clear it out
                if( await db.Categories.CountAsync(x=> true) > 0 )
                {
                    var categories = await db.Categories.Where(c => true).ToListAsync();
                    db.Categories.RemoveRange(categories);
                    await db.SaveChangesAsync();
                }
                // If item data exists, clear it out
                if (await db.Items.CountAsync() > 0)
                {
                    var items = await db.Items.Where(i => true).ToListAsync();
                    db.Items.RemoveRange(items);
                    await db.SaveChangesAsync();
                }

                // --Create Categories
                Console.WriteLine("Inserting a new categories");
                db.Add(new Category { CategoryName = "Weapon", IsLegendary = false, DegenerationFactor = 1 });
                db.Add(new Category { CategoryName = "Food", IsLegendary = false, DegenerationFactor = 1 });
                db.Add(new Category { CategoryName = "Sulfuras", IsLegendary = true, DegenerationFactor = 0 });
                db.Add(new Category { CategoryName = "Backstage Passes", IsLegendary = false, DegenerationFactor = 1 });
                db.Add(new Category { CategoryName = "Conjured", IsLegendary = false, DegenerationFactor = 2 });
                db.Add(new Category { CategoryName = "Potion", IsLegendary = false, DegenerationFactor = 1 });
                db.Add(new Category { CategoryName = "Armor", IsLegendary = false, DegenerationFactor = 1 });
                db.Add(new Category { CategoryName = "Misc", IsLegendary = false, DegenerationFactor = 1 });
                await db.SaveChangesAsync();

                var allCategories = await db.Categories.AsNoTracking().Where(c => true).ToListAsync();

                // Create Items
                Console.WriteLine("Inserting a new items");
                db.Add(new Item { ItemName = "Sword", SellIn = 30, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Weapon").CategoryId });
                db.Add(new Item { ItemName = "Axe", SellIn = 40, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Weapon").CategoryId });
                db.Add(new Item { ItemName = "Halberd", SellIn = 60, Quality = 40, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Weapon").CategoryId });

                db.Add(new Item { ItemName = "Aged Brie", SellIn = 50, Quality = 10, QualityAppreciates = true, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Food").CategoryId });
                db.Add(new Item { ItemName = "Aged Milk", SellIn = 20, Quality = 20, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Food").CategoryId });
                db.Add(new Item { ItemName = "Mutton", SellIn = 10, Quality = 10, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Food").CategoryId });
                db.Add(new Item { ItemName = "Cheese", SellIn = 5, Quality = 5, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Food").CategoryId });

                db.Add(new Item { ItemName = "Hand of Ragnaros", SellIn = 80, Quality = 80, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Sulfuras").CategoryId });

                db.Add(new Item { ItemName = "I am Murloc", SellIn = 20, Quality = 10, QualityAppreciates = true, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Backstage Passes").CategoryId });
                db.Add(new Item { ItemName = "Raging Ogre", SellIn = 10, Quality = 10, QualityAppreciates = true, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Backstage Passes").CategoryId });
                db.Add(new Item { ItemName = "TAFKAL80ETC Concert", SellIn = 15, Quality = 20, QualityAppreciates = true, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Backstage Passes").CategoryId });

                db.Add(new Item { ItemName = "Giant Slayer", SellIn = 15, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Conjured").CategoryId });
                db.Add(new Item { ItemName = "Storm Hammer", SellIn = 20, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Conjured").CategoryId });
                db.Add(new Item { ItemName = "Belt of Giant Strength", SellIn = 20, Quality = 40, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Conjured").CategoryId });

                db.Add(new Item { ItemName = "Potion of Healing", SellIn = 10, Quality = 10, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Potion").CategoryId });
                db.Add(new Item { ItemName = "Elixir of the Mongoose", SellIn = 5, Quality = 7, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Potion").CategoryId });

                db.Add(new Item { ItemName = "+5 Dexterity Vest", SellIn = 10, Quality = 20, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Armor").CategoryId });
                db.Add(new Item { ItemName = "Full Plate Mail", SellIn = 50, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Armor").CategoryId });
                db.Add(new Item { ItemName = "Wooden Shield", SellIn = 10, Quality = 10, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Armor").CategoryId });

                db.Add(new Item { ItemName = "Bag of Holding", SellIn = 10, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Misc").CategoryId });
                await db.SaveChangesAsync();

                await db.DisposeAsync();

                return await Task.FromResult("Success");
            }
        }
    }
}
