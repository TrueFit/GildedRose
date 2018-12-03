using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GildedRose.Contracts;
using GildedRose.Core.Contracts;
using GildedRose.Entities.Inventory;
using GildedRose.Managers.Base;
using GildedRose.Store.Contracts;

namespace GildedRose.Managers
{
    public class ItemManager : Manager, IItemManager
    {
        public ItemManager(
            IDataStore store,
            IConfigurationStore config)
           : base(store)
        {
            this.Timeout = config.GetConfiguration<int>("SQLTimeout");
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
            var items = await this.Store.GetListAsync<ItemsView>(query, new { categoryId }, this.Timeout);
            return items;
        }

        public async Task<IEnumerable<ItemsView>> GetByName(string itemName)
        {
            var query = await this.GetQueryAsync();
            var items = await this.Store.GetListAsync<ItemsView>(query, new { itemName }, this.Timeout);
            return items;
        }

        public async Task Remove(Guid identifer)
        {
            var query = await this.GetQueryAsync();
            var items = await this.Store.ExecuteAsync(query, new { itemIdentifier = identifer }, this.Timeout);
        }
    }
}
