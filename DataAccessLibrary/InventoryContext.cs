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
            //var projDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //SQLiteInstance = projDirectory + @"/GildedRoseInventory.db";

            //-- Locate DB from project path
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine($"1Base Directory == {baseDir}");
            if (baseDir.Contains("/app/bin/Debug")) // in debug container
            {
                var projDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                SQLiteInstance = projDirectory + @"/GildedRoseInventory.db";
            }
            else // in live container
            {
                SQLiteInstance = $"{baseDir}DBFile/GildedRoseInventory.db";
            }

        }

        // Point to local instance of SQLite DB
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine($"2Base Directory == {baseDir}");
            if (baseDir.Contains("/app/bin/Debug"))
            {
                options.UseSqlite($"Data Source={Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/GildedRoseInventory.db"}");
            }
            else options.UseSqlite($"Data Source={baseDir}DBFile/GildedRoseInventory.db");
        }
    }
}
