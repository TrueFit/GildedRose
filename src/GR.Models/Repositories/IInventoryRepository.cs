using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Repositories
{
    public interface IInventoryRepository
    {
        List<Models.InventoryItemType> GetItemTypes();
        (int TotalItems, List<Models.InventoryItem> Items) GetAvailableItems(int skip = 0, int take = 100);
        Models.InventoryItem GetItemDetials(int itemId);
    }
}
