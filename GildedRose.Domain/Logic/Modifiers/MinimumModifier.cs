using System;
using System.Collections.Generic;
using System.Text;
using GildedRose.Domain.Models;

namespace GildedRose.Domain.Logic.Modifiers
{
    public class MinimumModifier : IItemModifier
    {
        public int Apply(InventoryItemValue item, int qualityValue)
        {
            if (qualityValue < item.Category.MinimumQuality)
            {
                qualityValue = item.Category.MinimumQuality;
            }

            return qualityValue;
        }
    }
}
