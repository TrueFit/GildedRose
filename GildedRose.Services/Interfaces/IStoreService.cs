using System;
using System.Collections.Generic;
using System.Text;
using GildedRose.Model;

namespace GildedRose.Services.Interfaces
{
    public interface IStoreService
    {
        List<StoreItemDto> GetInventory();
        StoreItemDto GetInventoryItem(string name);
        StoreItemDto GetInventoryItem(int id);
        void AddItem(StoreItemDto item);
        void DeleteItem(StoreItemDto item);
        void UpdateItem(StoreItemDto item);
        void SaveChanges();
    }
}
