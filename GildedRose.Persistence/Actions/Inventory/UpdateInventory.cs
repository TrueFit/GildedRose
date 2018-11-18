using GildedRose.Models;
using GildedRose.Persistence.Context;
using GildedRose.Persistence.Core;

namespace GildedRose.Domain.Actions
{
    public class UpdateInventory : IDataCommand
    {

        private readonly InventoryContext _context;

        private readonly Inventory _inventory;

        public UpdateInventory(InventoryContext context, Inventory inventory)
        {
            _context = context;
            _inventory = inventory;
        }

        public void Execute()
        {
            _context.Inventories.Update(_inventory);
            _context.SaveChanges();
        }

    }
}
