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

        //instantiate with options
        public InventoryContext(DbContextOptions<InventoryContext> options): base(options)
        {
        }

        public InventoryContext()
        {
            //-- For DB Initial Creation / Migration
            //var appDataFolder = Environment.SpecialFolder.LocalApplicationData;
            //var filePath = Environment.GetFolderPath(appDataFolder);
            //SQLiteInstance = $"{filePath}{Path.DirectorySeparatorChar}GildedRoseInventory.db";

            //-- Locate DB from project path
            var projDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            SQLiteInstance = projDirectory + @"/GildedRoseInventory.db";
        }

        // Point to local instance of SQLite DB
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/GildedRoseInventory.db"}");


    }
}
