using GildedRose.Models;
using GildedRose.Persistence.Context;
using GildedRose.Persistence.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GildedRose.Persistence.Actions.Items
{
    public class UpdateInventoryItem : IDataCommand
    {

        private readonly InventoryContext _context;

        private readonly InventoryItem _inventoryItem;

        public UpdateInventoryItem(InventoryContext context, InventoryItem inventoryItem)
        {
            _context = context;
            _inventoryItem = inventoryItem;
        }

        public void Execute()
        {
            _context.InventoryItems.Update(_inventoryItem);
            _context.SaveChanges();
        }

    }
}
