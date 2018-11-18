using GildedRose.Persistence.Context;
using GildedRose.Persistence.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GildedRose.Persistence.Actions
{
    public class GetCategoryCount : IDataQuery<int>
    {
        private readonly InventoryContext _context;

        public GetCategoryCount(InventoryContext context)
        {
            _context = context;
        }

        public int Execute()
        {
            return _context.Categories.Count();
        }
    }
}
