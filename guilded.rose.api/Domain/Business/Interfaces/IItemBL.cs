using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using guilded.rose.api.Domain.Models;
using guilded.rose.api.Domain.Models.Interfaces;

namespace guilded.rose.api.Domain.Business.Interfaces
{
    public interface IItemBL
    {
        Task<List<Item>> GetItems();
        Task<IItem> GetItemByName(string name);
        Task<List<IItem>> GetDailyItems(DateTime date);

        Task<List<IItem>> GetDailyTrash(DateTime date);
    }
}