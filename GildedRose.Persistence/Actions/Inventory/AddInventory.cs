using GildedRose.Models;
using GildedRose.Persistence.Context;
using GildedRose.Persistence.Core;

namespace GildedRose.Domain.Actions
{
   public class AddInventory : IDataCommand
   {

      private readonly InventoryContext _context;

      private readonly Inventory _inventory;

      public AddInventory(InventoryContext context, Inventory inventory)
      {
         _context = context;
         _inventory = inventory;
      }

      public void Execute()
      {
         _context.Inventories.Add(_inventory);
         _context.SaveChanges();
      }

   }
}
