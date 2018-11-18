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
    public class GetInventoryItem : IDataQuery<InventoryItem>
    {

        private readonly InventoryContext _context;
        private int _itemId { get; }


        public GetInventoryItem(InventoryContext context, int itemId)
        {
            _context = context;
            _itemId = itemId;
        }

        public InventoryItem Execute()
        {
            return _context.InventoryItems
                           .Include(item => item.Category)
                           .Include(item => item.Category.Degradation)
                           .Include(item => item.Quality)
                           .FirstOrDefault(i => i.InventoryItemId == _itemId);
        }

    }
}
