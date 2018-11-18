using AutoMapper;
using GildedRose.Domain.Models;
using GildedRose.Models;
using GildedRose.Persistence.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace GildedRose.Domain
{
    public class InventoryItemDomain : IDomain<InventoryItemValue>
    {
        private ItemData _inventoryItems;
        private IMapper _mapper;

        public InventoryItemDomain(IDatabase<InventoryItem> inventoryItems, IMapper mapper)
        {
            _mapper = mapper;
            _inventoryItems = (ItemData)inventoryItems;
        }

        public void AddInventoryItem(InventoryItemValue inventoryValue)
        {
            var InventoryItem = _mapper.Map<InventoryItem>(inventoryValue);
            _inventoryItems.AddEntity(InventoryItem);
        }

        public IEnumerable<InventoryItemValue> GetInventoryItems(int inventoryId)
        {
            return _mapper.Map<List<InventoryItemValue>>(_inventoryItems.GetEntities(inventoryId));
        }

        public IEnumerable<InventoryItemValue> SearchInventoryItems(int inventoryId, string itemName)
        {
            return _mapper.Map<List<InventoryItemValue>>(_inventoryItems.SearchEntities(inventoryId, itemName));
        }

        public IEnumerable<InventoryItemValue> GetTrashItems()
        {
            return _mapper.Map<List<InventoryItemValue>>(_inventoryItems.GetTrash());
        }

        public InventoryItemValue GetInventoryItem(int id)
        {
            return _mapper.Map<InventoryItemValue>(_inventoryItems.GetEntity(id));
        }

        public void UpdateAllItemQuality(int inventoryId, DateTime currentDate)
        {
#warning In a production system, I'd publish a message to a worker host to perform this much work, or configure a job to perform this much work.
            var items = GetInventoryItems(inventoryId);
            foreach (var item in items)
            {
                item.CurrentDate = currentDate;
                item.CalculateCurrentQuality();
            }
            
            _inventoryItems.UpdateEntities(_mapper.Map<List<InventoryItem>>(items));
        }

        public void DeleteInventoryItem(int id)
        {
            _inventoryItems.DeleteEntity(id);
        }

    }
}
