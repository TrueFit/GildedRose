using GildedRose.Models;
using GildedRose.Persistence.Context;
using GildedRose.Persistence.Core;
using System.Linq;

namespace GildedRose.Domain.Actions
{
   public class GetCategory : IDataQuery<Category>
   {

      private readonly InventoryContext _context;

      private readonly int _id;

      public GetCategory(InventoryContext context, int id)
      {
         _context = context;
         _id = id;
      }

      public Category Execute()
      {
         return _context.Categories.FirstOrDefault(i => i.CategoryId == _id);
      }

   }
}
