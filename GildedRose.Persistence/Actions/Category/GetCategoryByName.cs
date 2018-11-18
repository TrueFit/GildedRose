using GildedRose.Models;
using GildedRose.Persistence.Context;
using GildedRose.Persistence.Core;
using System.Linq;

namespace GildedRose.Domain.Actions
{
    public class GetCategoryByName : IDataQuery<Category>
    {

        private readonly InventoryContext _context;

        private readonly string _name;

        public GetCategoryByName(InventoryContext context, string name)
        {
            _context = context;
            _name = name;
        }

        public Category Execute()
        {
            return _context.Categories.FirstOrDefault(i => i.Name == _name);
        }

    }
}
