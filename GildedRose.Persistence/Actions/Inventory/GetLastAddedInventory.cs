using GildedRose.Models;
using GildedRose.Persistence.Context;
using GildedRose.Persistence.Core;
using System.Linq;

namespace GildedRose.Domain.Actions
{
    public class GetLastAddedInventory : IDataQuery<Inventory>
    {

        private readonly InventoryContext _context;

        public GetLastAddedInventory(InventoryContext context)
        {
            _context = context;
        }

        public Inventory Execute()
        {
            return _context.Inventories.LastOrDefault();
        }

    }
}
