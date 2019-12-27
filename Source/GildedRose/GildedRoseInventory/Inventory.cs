using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace GildedRoseInventory
{
    public class Inventory
    {
        // Internal list of items
        private readonly List<Product> products = new List<Product>();

        /// <summary>
        /// The number of time NextDay() method has been called
        /// </summary>
        public int DaysPassed { get; private set; }

        /// <summary>
        /// Construct an instance of the inventory system.
        /// </summary>
        /// <param name="inventoryFile">file that holds the inventory (inventory.txt)</param>
        public Inventory(string inventoryFile)
        {
            using var reader = new StreamReader(inventoryFile);
            using var csv = new CsvReader(reader);
            csv.Configuration.HasHeaderRecord = false;
            var items = csv.GetRecords<Product>();
            foreach (var item in items)
                products.Add(item);
            SortProducts();
        }

        /// <summary>
        /// Returns a list of items
        /// </summary>
        /// <returns></returns>
        public IList<Product> GetProducts()
        {
            return products;
        }

        /// <summary>
        /// Returns an item by name
        /// </summary>
        /// <param name="name">item name</param>
        /// <returns></returns>
        public Product GetProduct(string name)
        {
            foreach (var item in products)
                if (string.Compare(item.Name, name, true, CultureInfo.InvariantCulture) == 0)
                    return item;
            return null;
        }

        /// <summary>
        /// Returns a list of trash items (Quality = 0))
        /// </summary>
        /// <returns></returns>
        public IList<Product> GetTrashProducts()
        {
            var trash = new List<Product>();
            foreach (var item in products)
                if (item.Quality == 0)
                    trash.Add(item);
            return trash;
        }

        /// <summary>
        /// Sorts the items by category and than by name
        /// </summary>
        public void SortProducts()
        {
            products.Sort();
        }

        /// <summary>
        /// Removes an item from the inventory
        /// </summary>
        /// <param name="item">item to be removed</param>

        public void RemoveProduct(Product item)
        {
            products.Remove(item);
        }

        /// <summary>
        /// Progress to the next day
        /// </summary>
        public void NextDay()
        {
            DaysPassed++;

            foreach (var item in products)
            {
                if (AgedBrieNextDayHandler(item))
                    continue;
                else if (SulfurasNextDayHandler(item))
                    continue;
                else if (BackstagePassesNextDayHandler(item))
                    continue;
                else if (ConjuredNextDayHandler(item))
                    continue;
                else
                    DefaultNextDayHandler(item);
            }
        }

        /// <summary>
        /// Default next day handler
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool DefaultNextDayHandler(Product item)
        {
            item.Quality--;
            if (item.SellIn == 0)
                item.Quality--;

            if (item.Quality < 0)
                item.Quality = 0;

            if (item.SellIn > 0)
                item.SellIn--;

            return true;
        }

        /// <summary>
        /// Sulfuras next day handler
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool SulfurasNextDayHandler(Product item)
        {
            if (item.Category == "Sulfuras")
                return true;
            return false;
        }

        /// <summary>
        /// Backstage Passes next day handler
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool BackstagePassesNextDayHandler(Product item)
        {
            if (item.Category == "Backstage Passes")
            {
                if (item.SellIn > 0)
                    item.SellIn--;

                if (item.SellIn == 0)
                    item.Quality = 0;
                else if (item.SellIn <= 5)
                    item.Quality += 3;
                else if (item.SellIn <= 10)
                    item.Quality += 2;
                else
                    item.Quality += 1;

                if (item.Quality > 50)
                    item.Quality = 50;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Conjured next day handler
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool ConjuredNextDayHandler(Product item)
        {
            if (item.Category == "Conjured")
            {
                item.Quality -= 2;
                if (item.SellIn == 0)
                    item.Quality -= 2;

                if (item.Quality < 0)
                    item.Quality = 0;

                if (item.SellIn > 0)
                    item.SellIn--;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Aged Brie next day handler
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool AgedBrieNextDayHandler(Product item)
        {
            if (item.Name == "Aged Brie")
            {
                if (item.SellIn > 0)
                    item.SellIn--;

                item.Quality += 1;
                if (item.Quality > 50)
                    item.Quality = 50;

                return true;
            }
            return false;
        }
    }
}
