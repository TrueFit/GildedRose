using AutoMapper;
using GildedRose.Domain.Factories;
using GildedRose.Domain.Models;
using GildedRose.Models;
using GildedRose.Persistence.Context;
using GildedRose.Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GildedRose.Domain
{
    public class InventoryDomain : IDomain<InventoryValue>
    {
        private readonly InventoryData _inventories;
        private readonly IMapper _mapper;
        private readonly InventoryContext _context;

        public InventoryDomain(IDatabase<Inventory> inventories, IMapper mapper, InventoryContext context)
        {
            _mapper = mapper;
            _inventories = (InventoryData)inventories;
            _context = context;
        }

        public void AddInventory(InventoryValue inventoryValue)
        {
            inventoryValue.CurrentDate = DateTime.Now;
            var inventory = _mapper.Map<Inventory>(inventoryValue);
            _inventories.AddEntity(inventory);
        }

        public IEnumerable<InventoryValue> GetInventories()
        {
            return _mapper.Map<List<InventoryValue>>(_inventories.GetEntities(null));
        }

        public InventoryValue GetInventory(int id)
        {
            return _mapper.Map<InventoryValue>(_inventories.GetEntity(id));
        }

        public int GetInventoryCount()
        {
            return _inventories.GetCount();
        }

        public void AdvanceDay(int inventoryId)
        {
            var inventory = _inventories.GetEntity(inventoryId);
            inventory.CurrentDate = inventory.CurrentDate.AddDays(1);
            _inventories.UpdateEntity(inventory);

#warning Normally, I would not new up domains in another domain I'd publish a message to perform this work in other domains to keep things loosely coupled.  Given time contraints, I'm accessing the other domains directly.
            var itemData = new ItemData(_context);
            var itemDomain = new InventoryItemDomain(itemData, _mapper);

            itemDomain.UpdateAllItemQuality(inventoryId, inventory.CurrentDate);
        }


        public Inventory GetLastAddedInventory()
        {
            return _inventories.GetLastAdded();
        }


        public void DeleteInventory(int id)
        {
            _inventories.DeleteEntity(id);
        }


        public void PurchaseDefaultInventory(InventoryValue inventoryValue)
        {
            if (inventoryValue == null)
            {
                throw new ArgumentNullException(nameof(inventoryValue));
            }

            var defaultInventoryFactory = new DefaultInventoryFactory(inventoryValue, _inventories, _mapper, _context);
            defaultInventoryFactory.Scaffold();
        }

    }
}
