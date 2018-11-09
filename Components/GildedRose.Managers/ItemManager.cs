using GildedRose.Contracts;
using GildedRose.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GildedRose.Managers.Base;
using GildedRose.Store.Base;

namespace GildedRose.Managers
{
    public class ItemManager : Manager, IItemManager
    {
        public ItemManager(DataStore store)
           : base(store)
        {
        }

        public async Task<IEnumerable<Item>> GetAll()
        {
            var query = await this.GetQueryAsync();
            var items = await this.Store.GetListAsync<Item>(query);
            return items;
        }

        public async Task<IEnumerable<Item>> GetByCategory(int categoryId)
        {
            var query = await this.GetQueryAsync();
            var items = await this.Store.GetListAsync<Item>(query, new { categoryId }, this.timeout);
            return items;
        }

        public async Task<IEnumerable<Item>> GetByName(string itemName)
        {
            var query = await this.GetQueryAsync();
            var items = await this.Store.GetListAsync<Item>(query, new { itemName }, this.timeout);
            return items;
        }

        public async Task Remove(Guid identifer)
        {
            var query = await this.GetQueryAsync();
            var items = await this.Store.ExecuteAsync(query, new { itemIdentifier = identifer }, this.timeout);
        }
    }
}
