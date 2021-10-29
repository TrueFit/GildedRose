using DataAccessLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GildedRose.Helpers
{
    public class DataCreateAndDrop
    {
        public async Task<bool> CreateExampleData()
        {
            using (var db = new InventoryContext())
            {
                Console.WriteLine($"Database path: {db.SQLiteInstance}.");

                // If category data exists, clear it out
                if( await db.Categories.CountAsync(x=> true) > 0 )
                {
                    var categories = await db.Categories.Where(c => true).ToListAsync();
                    db.Categories.RemoveRange(categories);
                    db.SaveChanges();
                }
                // If item data exists, clear it out
                if (await db.Items.CountAsync() > 0)
                {
                    var items = await db.Items.Where(i => true).ToListAsync();
                    db.Items.RemoveRange(items);
                    db.SaveChanges();
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
                db.SaveChanges();

                var allCategories = await db.Categories.Where(c => true).ToListAsync();

                // Create Items
                Console.WriteLine("Inserting a new items");
                db.Add(new Item { ItemName = "Sword", SellIn = 30, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Weapon").CategoryId });
                db.Add(new Item { ItemName = "Axe", SellIn = 40, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Weapon").CategoryId });
                db.Add(new Item { ItemName = "Halberd", SellIn = 60, Quality = 40, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Weapon").CategoryId });
                db.Add(new Item { ItemName = "Aged Brie", SellIn = 50, Quality = 10, QualityAppreciates = true, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Food").CategoryId });
                db.Add(new Item { ItemName = "Aged Milk", SellIn = 20, Quality = 20, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Food").CategoryId });
                db.Add(new Item { ItemName = "Sword", SellIn = 30, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Weapon").CategoryId });
                db.Add(new Item { ItemName = "Sword", SellIn = 30, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Weapon").CategoryId });
                db.Add(new Item { ItemName = "Sword", SellIn = 30, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Weapon").CategoryId });
                db.Add(new Item { ItemName = "Sword", SellIn = 30, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Weapon").CategoryId });
                db.Add(new Item { ItemName = "Sword", SellIn = 30, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Weapon").CategoryId });
                db.Add(new Item { ItemName = "Sword", SellIn = 30, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Weapon").CategoryId });
                db.Add(new Item { ItemName = "Sword", SellIn = 30, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Weapon").CategoryId });
                db.Add(new Item { ItemName = "Sword", SellIn = 30, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Weapon").CategoryId });
                db.Add(new Item { ItemName = "Sword", SellIn = 30, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Weapon").CategoryId });
                db.Add(new Item { ItemName = "Sword", SellIn = 30, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Weapon").CategoryId });
                db.Add(new Item { ItemName = "Sword", SellIn = 30, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Weapon").CategoryId });
                db.Add(new Item { ItemName = "Sword", SellIn = 30, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Weapon").CategoryId });
                db.Add(new Item { ItemName = "Sword", SellIn = 30, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Weapon").CategoryId });
                db.Add(new Item { ItemName = "Sword", SellIn = 30, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Weapon").CategoryId });
                db.Add(new Item { ItemName = "Sword", SellIn = 30, Quality = 50, QualityAppreciates = false, CategoryId = allCategories.FirstOrDefault(c => c.CategoryName == "Weapon").CategoryId });
                db.SaveChanges();

                //// Read
                //Console.WriteLine("Querying for a blog");
                //var blog = db.Blogs
                //    .OrderBy(b => b.BlogId)
                //    .First();

                //// Delete
                //Console.WriteLine("Delete the blog");
                //db.Remove(blog);
                //db.SaveChanges();

                return await Task.FromResult(true);
            }
        }
    }
}
