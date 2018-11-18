using GildedRose.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GildedRose.Domain.Logic.Modifiers
{
    public interface IItemModifier
    {
        int Apply(InventoryItemValue itemValue, int quality);
    }
}
