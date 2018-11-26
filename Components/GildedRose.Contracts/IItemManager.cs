using GildedRose.Entities.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GildedRose.Contracts
{
    public interface IItemManager
    {
        Task<IEnumerable<ItemsView>> GetAll();

        Task<IEnumerable<ItemsView>> GetByName(string itemName);

        Task<IEnumerable<ItemsView>> GetByCategory(int categoryId);

        Task Remove(Guid identifer);
    }
}
