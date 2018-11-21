using System.Collections.Generic;
using System.Linq;
using GildedRose.Data.Interfaces;
using GildedRose.Model;
using GildedRose.Services.Interfaces;

namespace GildedRose.Services
{
    public class StoreService : IStoreService
    {
        private readonly IStoreItemRepository _itemRepository;

        public StoreService(IStoreItemRepository itemsRepository)
        {
            this._itemRepository = itemsRepository;
        }

        List<StoreItemDto> IStoreService.GetInventory()
        {
            List<StoreItemDto> list = _itemRepository.GetAll().ToList();
             return list;
        }

        StoreItemDto IStoreService.GetInventoryItem(int id)
        {
            var item = _itemRepository.GetById(id);
            return item;
        }

        StoreItemDto IStoreService.GetInventoryItem(string name)
        {
            var item = _itemRepository.GetByName(name);
            return item;
        }

        void IStoreService.AddItem(StoreItemDto item)
        {
            _itemRepository.Add(item);
        }

        void IStoreService.DeleteItem(StoreItemDto item)
        {
            _itemRepository.Delete(item);
        }

        void IStoreService.UpdateItem(StoreItemDto item)
        {
            var tmp = _itemRepository.GetByName(item.Name);
            item.Id = tmp.Id;
            _itemRepository.Update(item);
        }

        void IStoreService.SaveChanges()
        {
            _itemRepository.Save();
        }
    }
}
