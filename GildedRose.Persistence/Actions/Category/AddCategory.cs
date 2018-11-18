using GildedRose.Models;
using GildedRose.Persistence.Context;
using GildedRose.Persistence.Core;

namespace GildedRose.Domain.Actions
{
    public class AddCategory : IDataCommand
    {

        private readonly InventoryContext _context;

        private readonly Category _category;

        public AddCategory(InventoryContext context, Category category)
        {
            _context = context;
            _category = category;
        }

        public void Execute()
        {
            _context.Categories.Add(_category);
            _context.SaveChanges();
        }

    }
}
