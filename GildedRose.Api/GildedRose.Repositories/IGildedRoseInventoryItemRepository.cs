using System.Collections.Generic;
using System.Data.Entity;
using GildedRose.Entities;

namespace GildedRose.Repositories
{
    public interface IGildedRoseInventoryItemRepository
    {
        IEnumerable<GildedRoseInventoryItem> GetAllItems();
        GildedRoseInventoryItem Find(int id);
        GildedRoseInventoryItem Find(string name);
        IEnumerable<GildedRoseInventoryItem> GetZeroQualityItems();
        GildedRoseInventoryItem Add(GildedRoseInventoryItem item);
        GildedRoseInventoryItem Remove(int id);
        GildedRoseInventoryItem Update(int id, int quality, int sellin);
    }
}
