using System.Collections.Generic;
using GildedRose.Persistence.Context;
using GildedRose.Persistence.Core;
using GildedRose.Domain.Actions;
using GildedRose.Models;
using GildedRose.Persistence.Actions.Items;

namespace GildedRose.Persistence.Database
{
    public class InventoryData : IDatabase<Inventory>
    {
        private readonly IDataExecutor _dataExecutor;

        private readonly InventoryContext _context;

        public InventoryData(InventoryContext context)
        {
            _dataExecutor = new DataExecutor();
            _context = context;
        }

        public InventoryData(IDataExecutor dataExecutor, InventoryContext context)
        {
            _dataExecutor = dataExecutor;
            _context = context;
        }


        public void AddEntity(Inventory inventory)
        {
            _dataExecutor.ExecuteCommand(new AddInventory(_context, inventory));
        }

        public void UpdateEntity(Inventory inventory)
        {
            _dataExecutor.ExecuteCommand(new UpdateInventory(_context, inventory));
        }

        public void DeleteEntity(int inventoryId)
        {
            _dataExecutor.ExecuteCommand(new DeleteInventory(_context, inventoryId));
        }

        public int GetCount()
        {
            return _dataExecutor.ExecuteQuery(new GetInventoryCount(_context));
        }

        public Inventory GetLastAdded()
        {
            return _dataExecutor.ExecuteQuery(new GetLastAddedInventory(_context));
        }

        public List<Inventory> GetEntities(int? inventoryId)
        {
            return _dataExecutor.ExecuteQuery(new GetInventories(_context));
        }
        
        public Inventory GetEntity(int inventoryId)
        {
            return _dataExecutor.ExecuteQuery(new GetInventory(_context, inventoryId));
        }
    }
}

