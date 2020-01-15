using GildedRose.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GildedRose.Repository
{
    public interface IInventoryRepository
    {
        Task AddItemAsync(IInventoryItem item);

        Task<IEnumerable<IInventoryItem>> GetAllAsync();

        Task<bool> RemoveItemAsync(Guid id);

        Task ResetAsync();

        Task SaveAsync(IEnumerable<IInventoryItem> items);
    }
}