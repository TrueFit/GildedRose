using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GildedRose.BusinessObjects;

namespace GildedRose.Helpers.ItemTypes
{
    public interface IItemType
    {
        InventoryItem EndDay(InventoryItem i);
    }
}
