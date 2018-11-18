using System;
using System.Collections.Generic;
using System.Text;
using GildedRose.Domain.Models;
using GildedRose.Domain.Logic.Utility;

namespace GildedRose.Domain.Logic.Modifiers
{
    public class BaseModifier : IItemModifier
    {
        public int Apply(InventoryItemValue item, int quality)
        {
            return item.Quality.Initial - (item.DaysSincePurchased * item.Category.Degradation.Rate);
        }
    }
}
