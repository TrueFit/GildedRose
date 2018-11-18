using GildedRose.Models;
using GildedRose.Persistence.Context;
using GildedRose.Persistence.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GildedRose.Persistence.Actions.Items
{
    public class DeleteInventoryItem : IDataCommand
    {

        private readonly InventoryContext _context;

        private readonly int _itemId;

        public DeleteInventoryItem(InventoryContext context, int itemId)
        {
            _context = context;
            _itemId = itemId;
        }

        public void Execute()
        {
            var inventory = _context.InventoryItems.FirstOrDefault(i => i.InventoryItemId == _itemId);
            _context.Remove(inventory);
            _context.SaveChanges();
        }

    }
}
