using GildedRose.Models;
using GildedRose.Persistence.Context;
using GildedRose.Persistence.Core;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GildedRose.Domain.Actions
{
   public class GetCategories : IDataQuery<List<Category>>
   {

      private readonly InventoryContext _context;

      public GetCategories(InventoryContext context)
      {
         _context = context;
      }

      public List<Category> Execute()
      {
#warning In a production system, it is never a good idea to .ToList() an entire collection, but for the purposes of this example I've left it in
            return _context.Categories
                        .Include(category => category.Degradation)
                        .ToList();
      }

   }
}
