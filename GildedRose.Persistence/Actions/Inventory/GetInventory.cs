using GildedRose.Models;
using GildedRose.Persistence.Context;
using GildedRose.Persistence.Core;
using System.Linq;

namespace GildedRose.Domain.Actions
{
    public class GetInventory : IDataQuery<Inventory>
    {

        private readonly InventoryContext _context;
        private readonly int _id;

        public GetInventory(InventoryContext context, int id)
        {
            _context = context;
            _id = id;
        }

        public Inventory Execute()
        {
            return _context.Inventories.FirstOrDefault(i => i.InventoryId == _id);
        }

    }
}
