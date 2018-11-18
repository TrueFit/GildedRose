using System.Collections.Generic;
using GildedRose.Persistence.Context;
using GildedRose.Persistence.Core;
using GildedRose.Domain.Actions;
using GildedRose.Models;
using GildedRose.Persistence.Actions.Items;
using GildedRose.Persistence.Actions;

namespace GildedRose.Persistence.Database
{
    public class ItemData : IDatabase<InventoryItem>
    {
        private readonly IDataExecutor _dataExecutor;

        private readonly InventoryContext _context;

        public ItemData(InventoryContext context)
        {
            _dataExecutor = new DataExecutor();
            _context = context;
        }

        public ItemData(IDataExecutor dataExecutor, InventoryContext context)
        {
            _dataExecutor = dataExecutor;
            _context = context;
        }


        public void AddEntity(InventoryItem inventoryItem)
        {
            _dataExecutor.ExecuteCommand(new AddItemToInventory(_context, inventoryItem));
        }

        public void DeleteEntity(int itemId)
        {
            _dataExecutor.ExecuteCommand(new DeleteInventoryItem(_context, itemId));
        }

        public int GetCount()
        {
            return _dataExecutor.ExecuteQuery(new GetInventoryItemCount(_context));
        }

        public List<InventoryItem> GetEntities(int? inventoryId)
        {
            if (!inventoryId.HasValue)
                throw new System.Exception("To retrieve a list of inventory items, you must supply an inventoryId");

            return _dataExecutor.ExecuteQuery(new GetInventoryItems(_context, inventoryId.Value));
        }

        public List<InventoryItem> SearchEntities(int inventoryId, string itemName)
        {
            return _dataExecutor.ExecuteQuery(new GetInventoryItemsByName(_context, inventoryId, itemName));
        }

        public List<InventoryItem> GetTrash()
        {
            return _dataExecutor.ExecuteQuery(new GetAllTrashItems(_context));
        }

        public InventoryItem GetEntity(int itemId)
        {
            return _dataExecutor.ExecuteQuery(new GetInventoryItem(_context, itemId));
        }

        public void UpdateEntity(InventoryItem inventoryItem)
        {
            _dataExecutor.ExecuteCommand(new UpdateInventoryItem(_context, inventoryItem));
        }

        public void UpdateEntities(List<InventoryItem> inventoryItems)
        {
            _dataExecutor.ExecuteCommand(new UpdateInventoryItems(_context, inventoryItems));
        }
    }
}

