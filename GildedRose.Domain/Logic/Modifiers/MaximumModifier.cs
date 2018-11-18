using System;
using System.Collections.Generic;
using System.Text;
using GildedRose.Domain.Models;

namespace GildedRose.Domain.Logic.Modifiers
{
    public class MaximumModifier : IItemModifier
    {
        public int Apply(InventoryItemValue item, int qualityValue)
        {
            if (qualityValue > item.Category.MaximumQuality)
            {
                qualityValue = item.Category.MaximumQuality;
            }

            return qualityValue;
        }
    }
}
