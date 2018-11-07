using GildedRose.Entities.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GildedRose.Contracts
{
    public interface IItemManager
    {
        Task<IEnumerable<Item>> GetAll();

        Task<IEnumerable<Item>> GetByName(string itemName);

        Task<IEnumerable<Item>> GetByCategory(int categoryId);

        Task Remove(Guid identifer);
    }
}
