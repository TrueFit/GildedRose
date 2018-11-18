using GildedRose.Persistence.Context;
using GildedRose.Persistence.Core;
using System.Linq;

namespace GildedRose.Domain.Actions
{
   public class DeleteCategory : IDataCommand
   {

      private readonly InventoryContext _context;

      private readonly int _id;

      public DeleteCategory(InventoryContext context, int id)
      {
         _context = context;
         _id = id;
      }

      public void Execute()
      {
         var category = _context.Categories.FirstOrDefault(i => i.CategoryId == _id);
         _context.Remove(category);
         _context.SaveChanges();
      }
   }
}
