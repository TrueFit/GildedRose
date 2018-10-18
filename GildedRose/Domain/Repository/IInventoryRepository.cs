using GildedRose.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GildedRose.Domain.Repository
{
    public interface IInventoryRepository
    {
        IList<Item> All();
        Item GetByName(string name);
        IList<Item> GetByCondition(Func<Item, bool> whereCondition);
        void Save(IList<Item> items);
    }
}
