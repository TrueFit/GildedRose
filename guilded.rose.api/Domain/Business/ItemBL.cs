using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using guilded.rose.api.Domain.DataAccess;
using guilded.rose.api.Domain.Models;
using guilded.rose.api.Domain.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using guilded.rose.api.Domain.Business.Interfaces;
using guilded.rose.api.Domain.Models.Interfaces;

namespace guilded.rose.api.Domain.Business
{
    public class ItemBL : IItemBL
    {
        private readonly IRepository<Item> _itemRepository;

        public ItemBL(IRepository<Item> itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<List<Item>> GetItems() => await _itemRepository.Get();

        public async Task<IItem> GetItemByName(string name) => await _itemRepository.GetByName(name);

        public async Task<List<IItem>> GetDailyItems(DateTime date)
        {
            var items = await _itemRepository.Get();
            return items.ToAdjustedItems(date).ToList();
        }

        public async Task<List<IItem>> GetDailyTrash(DateTime date)
        {
            var items = await _itemRepository.Get();
            return items.ToAdjustedItems(date).Where(item => item.Quality == 0).ToList();
        }

    }
}