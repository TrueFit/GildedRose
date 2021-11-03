using System.IO;
using System.Threading.Tasks;
using GuildedRose.Inventory.InventoryItems;

namespace GuildedRose.Inventory.Data
{
    public class InventoryContextSeed
    {
        private const string _RootPath = "..";
        private const string _InventorySeedFilePath = "inventory.txt";

        public static async Task SeedAsync(InventoryDbContext inventoryContext)
        {
            using var inventoryFile = File.OpenText(Path.Combine(_RootPath, _InventorySeedFilePath));

            using var streamReader = File.OpenText(Path.Combine(_RootPath, _InventorySeedFilePath));
            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine();
                var inventoryItem = InventoryItemFactory.FactoryFromCsv(line);
                await inventoryContext.Items.AddAsync(inventoryItem);
                await inventoryContext.SaveChangesAsync();
            }
            var items = inventoryContext.Items;
        }
    }
}
