using GildedRose.Contracts;
using GildedRose.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GildedRose.Managers.Base;
using GildedRose.Store.Base;
using GildedRose.Store.Contracts;

namespace GildedRose.Managers
{
    public class ItemManager : Manager, IItemManager
    {
        public ItemManager(IDataStore store)
           : base(store)
        {
        }

        public async Task<IEnumerable<ItemsView>> GetAll()
        {
            var query = await this.GetQueryAsync();
            var items = await this.Store.GetListAsync<ItemsView>(query);
            return items;
        }

        public async Task<IEnumerable<ItemsView>> GetByCategory(int categoryId)
        {
            var query = await this.GetQueryAsync();
            var items = await this.Store.GetListAsync<ItemsView>(query, new { categoryId }, this.timeout);
            return items;
        }

        public async Task<IEnumerable<ItemsView>> GetByName(string itemName)
        {
            var query = await this.GetQueryAsync();
            var items = await this.Store.GetListAsync<ItemsView>(query, new { itemName }, this.timeout);
            return items;
        }

        public async Task Remove(Guid identifer)
        {
            var query = await this.GetQueryAsync();
            var items = await this.Store.ExecuteAsync(query, new { itemIdentifier = identifer }, this.timeout);
        }
    }
}
