using GildedRose.Models;
using GildedRose.Persistence.Context;
using GildedRose.Persistence.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GildedRose.Persistence.Actions.Items
{
    public class GetInventoryItemsByName : IDataQuery<List<InventoryItem>>
    {

        private readonly InventoryContext _context;
        private int _inventoryId { get; }
        private string _itemName { get; }

        public GetInventoryItemsByName(InventoryContext context, int inventoryId, string itemName)
        {
            _context = context;
            _inventoryId = inventoryId;
            _itemName = itemName;
        }

        public List<InventoryItem> Execute()
        {
#warning In a production system, it is never a good idea to .ToList() an entire collection, but for the purposes of this example I've left it in
            var items = _context.InventoryItems
                           .Include(item => item.Category)
                           .Include(item => item.Category.Degradation)
                           .Include(item => item.Quality)
                           .Where(i => i.InventoryId == _inventoryId && 
                                  (i.Name.Contains(_itemName) || string.IsNullOrEmpty(_itemName))).ToList();
            return items;
        }

    }
}
