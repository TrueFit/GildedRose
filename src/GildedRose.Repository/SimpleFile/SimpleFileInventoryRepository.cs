using GildedRose.Domain;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GildedRose.Repository.SimpleFile
{
    /// <summary>
    /// Handles inventory data by reading from and saving to simple text files. The initial
    /// inventory is read from the provided text file. Subsequent updates and reads are done from a
    /// JSON file.
    /// </summary>
    public sealed class SimpleFileInventoryRepository : IInventoryRepository
    {
        private readonly IInventoryItemBuilder inventoryItemBuilder;
        private readonly IOptions<SimpleFileRepositoryOptions> options;

        public SimpleFileInventoryRepository(IOptions<SimpleFileRepositoryOptions> options, IInventoryItemBuilder inventoryItemBuilder)
        {
            this.options = options;
            this.inventoryItemBuilder = inventoryItemBuilder;
        }

        public async Task AddItemAsync(IInventoryItem item)
        {
            List<IInventoryItem> items = (await this.GetAllAsync()).ToList();
            items.Add(item);
            await this.SaveAsync(items);
        }

        public Task<IEnumerable<IInventoryItem>> GetAllAsync()
        {
            if (File.Exists(this.options.Value.JsonDataFilePath))
            {
                return Task.FromResult(this.ReadFromJsonFile());
            }

            return Task.FromResult(this.ReadFromInitialFile());
        }

        public async Task<bool> RemoveItemAsync(Guid id)
        {
            List<IInventoryItem> items = (await this.GetAllAsync()).ToList();
            int removed = items.RemoveAll(_ => _.Id == id);
            if (removed > 0)
            {
                await this.SaveAsync(items);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Task ResetAsync()
        {
            IEnumerable<IInventoryItem> items = this.ReadFromInitialFile();
            this.SaveAsync(items);
            return Task.CompletedTask;
        }

        public Task SaveAsync(IEnumerable<IInventoryItem> items)
        {
            string json = JsonSerializer.Serialize(items);
            File.WriteAllText(this.options.Value.JsonDataFilePath, json);
            return Task.CompletedTask;
        }

        private IEnumerable<IInventoryItem> ReadFromInitialFile()
        {
            var items = new List<IInventoryItem>();

            if (!File.Exists(this.options.Value.OriginalInventoryTextFilePath))
            {
                throw new FileNotFoundException("The inventory.txt file was not found in the expected location");
            }

            string[] lines = File.ReadAllLines(this.options.Value.OriginalInventoryTextFilePath);
            foreach (string line in lines)
            {
                string[] elements = line.Split(',');
                if (elements.Length == 4)
                {
                    items.Add(this.inventoryItemBuilder.Build(Guid.NewGuid(), elements[0], elements[1], int.Parse(elements[3]), int.Parse(elements[2])));
                }
            }

            return items;
        }

        private IEnumerable<IInventoryItem> ReadFromJsonFile()
        {
            var items = new List<IInventoryItem>();

            var json = File.ReadAllText(this.options.Value.JsonDataFilePath);
            InventoryItemDto[] dtoItems = JsonSerializer.Deserialize<InventoryItemDto[]>(json);

            foreach (InventoryItemDto item in dtoItems)
            {
                items.Add(this.inventoryItemBuilder.Build(item.Id, item.Name, item.Category, item.Quality, item.SellIn.GetValueOrDefault()));
            }

            return items;
        }
    }
}