using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GildedRose.Data;
using GildedRose.Entities.Inventory;
using Microsoft.EntityFrameworkCore;

namespace GildedRose.Domain.Inventory
{
    public interface IInventoryRepository
    {
        /// <summary>
        /// Get all Inventory items
        /// </summary>
        Task<IEnumerable<Entities.Inventory.Inventory>> GetList();

        /// <summary>
        /// Get filtered inventory items that match the passed in name
        /// </summary>
        /// <param name="name">Parameter performs a "like" search using case insensitive matching</param>
        /// <returns>List of inventory items matching the name parameter</returns>
        Task<IEnumerable<Entities.Inventory.Inventory>> GetByName(string name);

        /// <summary>
        /// Retrieve "trash" inventory items
        /// </summary>
        /// <returns>A list of inventory items with a Quality of 0</returns>
        Task<IEnumerable<Entities.Inventory.Inventory>> GetTrash();

        /// <summary>
        /// Retrieve audit history for inventory records
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<InventoryHistory>> GetHistory();

        /// <summary>
        /// Update inventory items
        /// </summary>
        /// <param name="inventory"></param>
        /// <returns></returns>
        Task<int> Update(IEnumerable<Entities.Inventory.Inventory> inventory, InventoryHistoryAction action);

        /// <summary>
        /// Age all inventory.
        /// This does not accept parameters or have any control to prevent aging
        /// for future dates. This reason for this is to support easily testing the system.
        /// In the real world, we would provide controls around this to ensure aging
        /// occurs only at logical points (such as overnight when aging has not yet been performed
        /// for the current day --- and not three weeks into the future)
        /// </summary>
        /// <returns></returns>
        Task<int> PerformAging();
    }

    public class InventoryRepository : IInventoryRepository
    {
        private IDataContext _dataContext;
        private InventoryAgingCalculator _agingCalculator;

        public InventoryRepository(IDataContext dataContext, InventoryAgingCalculator agingCalculator)
        {
            _dataContext = dataContext;
            _agingCalculator = agingCalculator;
        }

        public async Task<IEnumerable<Entities.Inventory.Inventory>> GetList()
        {
            return await _dataContext.Inventory
                .OrderBy(i => i.Category)
                .ThenBy(i => i.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Entities.Inventory.Inventory>> GetByName(string name)
        {
            return await _dataContext.Inventory
                .Where(i => i.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }

        public async Task<IEnumerable<Entities.Inventory.Inventory>> GetTrash()
        {
            return await _dataContext.Inventory
                .Where(i => i.Quality == 0)
                .OrderBy(i => i.Category)
                .ThenBy(i => i.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryHistory>> GetHistory()
        {
            return await _dataContext.InventoryHistory
                .OrderBy(i => i.Category)
                .ThenBy(i => i.Name)
                .ThenByDescending(i => i.LastModifiedDate)
                .ToListAsync();
        }

        public async Task<int> PerformAging()
        {
            var inventoryToAge = await _dataContext.Inventory.Where(i => i.Quality > 0).ToListAsync();

            foreach (var item in inventoryToAge)
            {
                _agingCalculator.AgeInventory(item);
            }

            return await Update(inventoryToAge, InventoryHistoryAction.Aged);
        }

        public async Task<int> Update(IEnumerable<Entities.Inventory.Inventory> inventory, InventoryHistoryAction action)
        {
            _dataContext.Inventory.UpdateRange(inventory);

            var modifiedItems = _dataContext.ChangeTracker.Entries()
                .Where(x => x.Entity is Entities.Inventory.Inventory && (x.State == EntityState.Modified || x.State == EntityState.Added))
                .Select(x => x.Entity)
                .ToList();

            foreach (var item in modifiedItems)
            {
                var historyItem = ((Entities.Inventory.Inventory)item).ToInventoryHistory();
                historyItem.LastModifiedDate = DateTime.Now;
                historyItem.Action = action;

                _dataContext.InventoryHistory.Attach(historyItem);
            }

            return await _dataContext.SaveChangesAsync();
        }
    }
}
