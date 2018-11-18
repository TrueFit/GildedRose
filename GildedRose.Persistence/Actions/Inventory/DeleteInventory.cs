using GildedRose.Persistence.Context;
using GildedRose.Persistence.Core;
using System.Linq;

namespace GildedRose.Domain.Actions
{
   public class DeleteInventory : IDataCommand
   {

      private readonly InventoryContext _context;

      private readonly int _id;

      public DeleteInventory(InventoryContext context, int id)
      {
         _context = context;
         _id = id;
      }

      public void Execute()
      {
         var inventory = _context.Inventories.FirstOrDefault(i => i.InventoryId == _id);
         _context.Remove(inventory);
         _context.SaveChanges();
      }
   }
}
