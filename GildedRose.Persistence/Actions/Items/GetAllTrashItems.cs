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
    public class GetAllTrashItems : IDataQuery<List<InventoryItem>>
    {

        private readonly InventoryContext _context;

        public GetAllTrashItems(InventoryContext context)
        {
            _context = context;
        }

        public List<InventoryItem> Execute()
        {
#warning In a production system, it is never a good idea to .ToList() an entire collection with more filters to restrict the quantity of returned data, but for the purposes of this example I've left it in
            var items = _context.InventoryItems
                           .Include(item => item.Category)
                           .Include(item => item.Category.Degradation)
                           .Include(item => item.Quality)
                           .Where(i => i.Quality.Current == 0).AsNoTracking().ToList();
            return items;
        }

    }
}
