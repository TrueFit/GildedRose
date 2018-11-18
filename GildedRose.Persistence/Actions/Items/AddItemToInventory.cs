using GildedRose.Models;
using GildedRose.Persistence.Context;
using GildedRose.Persistence.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GildedRose.Persistence.Actions.Items
{
    public class AddItemToInventory : IDataCommand
    {

        private readonly InventoryContext _context;

        private readonly InventoryItem _inventoryItem;

        public AddItemToInventory(InventoryContext context, InventoryItem inventoryItem)
        {
            _context = context;
            _inventoryItem = inventoryItem;
        }

        public void Execute()
        {
            _context.InventoryItems.Add(_inventoryItem);
            _context.SaveChanges();
        }

    }
}
