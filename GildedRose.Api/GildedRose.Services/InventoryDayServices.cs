using System.Collections.Generic;
using GildedRose.BusinessObjects;
using GildedRose.Entities;
using GildedRose.Repositories;
using GildedRose.Translators;
using GildedRose.Helpers;

namespace GildedRose.Services
{
    public class InventoryDayServices
    {
        private readonly GildedRoseEntities _context;

        public InventoryDayServices(GildedRoseEntities context)
        {
            _context = context;
        }
        public IEnumerable<InventoryItem> IncrementDay(int days)
        {
            var updatedItems = new List<InventoryItem>();

            // loop for as many days as we want to end
            for (var j = 0; j < days; j++)
            {
                var repo = new GildedRoseInventoryItemRepository(_context);
                var items = repo.GetAllItems().AsInventoryItems();
                foreach (var i in items)
                {
                    var updatedItem = DayHelpers.EndDay(i);
                    updatedItems.Add(
                        repo.Update(updatedItem.Id, updatedItem.Quality, updatedItem.Sellin).AsInventoryItem()
                    );
                }
            }
            return updatedItems;
        }
    }
}
