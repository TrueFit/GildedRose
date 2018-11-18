using GildedRose.Models;
using GildedRose.Persistence.Context;
using GildedRose.Persistence.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GildedRose.Persistence.Actions.Items
{
    public class UpdateInventoryItems : IDataCommand
    {

        private readonly InventoryContext _context;

        private readonly List<InventoryItem> _inventoryItems;

        public UpdateInventoryItems(InventoryContext context, List<InventoryItem> inventoryItems)
        {
            _context = context;
            _inventoryItems = inventoryItems;
        }

        public void Execute()
        {
            _context.InventoryItems.UpdateRange(_inventoryItems);
            _context.SaveChanges();
        }

    }
}
