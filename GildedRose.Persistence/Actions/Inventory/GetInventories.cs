using GildedRose.Models;
using GildedRose.Persistence.Context;
using GildedRose.Persistence.Core;
using System.Collections.Generic;
using System.Linq;

namespace GildedRose.Domain.Actions
{
   public class GetInventories : IDataQuery<List<Inventory>>
   {

      private readonly InventoryContext _context;

      public GetInventories(InventoryContext context)
      {
         _context = context;
      }

      public List<Inventory> Execute()
      {
#warning In a production system, it is never a good idea to .ToList() an entire collection, but for the purposes of this example I've left it in
            return _context.Inventories.ToList();
      }

   }
}
