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
    public class GetInventoryItems : IDataQuery<List<InventoryItem>>
    {

        private readonly InventoryContext _context;
        private int _inventoryId { get; }


        public GetInventoryItems(InventoryContext context, int inventoryId)
        {
            _context = context;
            _inventoryId = inventoryId;
        }

        public List<InventoryItem> Execute()
        {
#warning In a production system, it is never a good idea to .ToList() an entire collection, but for the purposes of this example I've left it in
            var items = _context.InventoryItems
                           .Include(item => item.Category)
                           .Include(item => item.Category.Degradation)
                           .Include(item => item.Quality)
                           .Where(i => i.InventoryId == _inventoryId).AsNoTracking().ToList();
            return items;
        }

    }
}
