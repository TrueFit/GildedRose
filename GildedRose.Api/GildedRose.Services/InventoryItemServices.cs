using GildedRose.Entities;
using System.Collections.Generic;
using GildedRose.Repositories;
using GildedRose.BusinessObjects;
using GildedRose.Translators;

namespace GildedRose.Services
{
    public class InventoryItemServices
    {
        private readonly GildedRoseEntities _context;

        public InventoryItemServices(GildedRoseEntities context)
        {
            _context = context;
        }

        public IEnumerable<InventoryItem> ResetInitialInventory()
        {
            // this cheeseball method resets db to original state for Sellin and Quality
            var repo = new GildedRoseInventoryItemRepository(_context);
            repo.Update(1, 30, 50);
            repo.Update(2, 40, 50);
            repo.Update(3, 60, 40);
            repo.Update(4, 50, 10);
            repo.Update(5, 20, 20);
            repo.Update(6, 10, 10);
            repo.Update(7, 80, 80);
            repo.Update(8, 20, 10);
            repo.Update(9, 10, 10);
            repo.Update(10, 15, 50);
            repo.Update(11, 20, 50);
            repo.Update(12, 20, 40);
            repo.Update(13, 5, 5);
            repo.Update(14, 10, 10);
            repo.Update(15, 10, 50);
            repo.Update(16, 15, 20);
            repo.Update(17, 5, 7);
            repo.Update(18, 10, 20);
            repo.Update(19, 50, 50);
            repo.Update(20, 10, 30);

            return GetAllItems();
        }

        public IEnumerable<InventoryItem> GetAllItems()
        {
            var repo = new GildedRoseInventoryItemRepository(_context);
            return repo.GetAllItems().AsInventoryItems();
        }

        public IEnumerable<InventoryItem> GetZeroQualityItems()
        {
            var repo = new GildedRoseInventoryItemRepository(_context);
            return repo.GetZeroQualityItems().AsInventoryItems();
        }

        public InventoryItem Find(string name)
        {
            var repo = new GildedRoseInventoryItemRepository(_context);
            return repo.Find(name).AsInventoryItem();
        }
    }
}
