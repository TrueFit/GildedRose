using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Reflection;

namespace DataAccessLibrary
{
    public class InventoryContext : DbContext
    {

        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }

        public string SQLiteInstance { get; private set; }

        public InventoryContext()
        {
            //-- For DB Initial Creation / Migration
            //var appDataFolder = Environment.SpecialFolder.LocalApplicationData;
            //var filePath = Environment.GetFolderPath(appDataFolder);
            //SQLiteInstance = $"{filePath}{Path.DirectorySeparatorChar}GildedRoseInventory.db";

            //-- DB Mount from project path
            var projDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            SQLiteInstance = projDirectory + @"\GildedRoseInventory.db";
        }

        // Create a local instance of our SQLite DB
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={SQLiteInstance}");
    }
}
