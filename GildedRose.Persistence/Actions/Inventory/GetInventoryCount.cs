using GildedRose.Models;
using GildedRose.Persistence.Context;
using GildedRose.Persistence.Core;
using System.Linq;

namespace GildedRose.Domain.Actions
{
    public class GetInventoryCount : IDataQuery<int>
    {

        private readonly InventoryContext _context;

        public GetInventoryCount(InventoryContext context)
        {
            _context = context;
        }

        public int Execute()
        {
            return _context.Inventories.Count();
        }

    }
}
