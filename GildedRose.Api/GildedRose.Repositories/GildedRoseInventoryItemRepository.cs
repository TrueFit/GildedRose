using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GildedRose.Entities;

namespace GildedRose.Repositories
{
    public class GildedRoseInventoryItemRepository : IGildedRoseInventoryItemRepository
    {
        private GildedRoseEntities _context;

        public GildedRoseInventoryItemRepository(GildedRoseEntities context)
        {
            _context = context;
        }
        public IEnumerable<GildedRoseInventoryItem> GetAllItems()
        {
            return _context.GildedRoseInventoryItems;
        }
        public GildedRoseInventoryItem Find(int id)
        {
            return _context.GildedRoseInventoryItems.FirstOrDefault(z => z.id == id);
        }
        public GildedRoseInventoryItem Find(string name)
        {
            return _context.GildedRoseInventoryItems.FirstOrDefault(z => z.name == name);
        }
        public IEnumerable<GildedRoseInventoryItem> GetZeroQualityItems()
        {
            return _context.GildedRoseInventoryItems.Where(z => z.quality == 0);
        }
        public GildedRoseInventoryItem Add(GildedRoseInventoryItem item)
        {
            _context.GildedRoseInventoryItems.Add(item);
            _context.SaveChanges();
            return item;
        }
        public GildedRoseInventoryItem Remove(int id)
        {
            var item = _context.GildedRoseInventoryItems.FirstOrDefault(z => z.id == id);
            if (item == null) return null;
            _context.GildedRoseInventoryItems.Remove(item);
            _context.SaveChanges();
            return item;
        }
        public GildedRoseInventoryItem Update(int id, int quality, int sellin)
        {
            var item = _context.GildedRoseInventoryItems.FirstOrDefault(z => z.id == id);
            if (item == null) return null;
            item.sellin = sellin;
            item.quality = quality;
            _context.Entry(item).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
            return item;
        }
    }
}
