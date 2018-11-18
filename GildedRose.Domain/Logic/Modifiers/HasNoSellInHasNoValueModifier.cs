using System;
using System.Collections.Generic;
using System.Text;
using GildedRose.Domain.Logic.Utility;
using GildedRose.Domain.Models;

namespace GildedRose.Domain.Logic.Modifiers
{
    public class PasNoSellInHasNoValueModifier : IItemModifier
    {
        public int Apply(InventoryItemValue item, int quality)
        {
            if (item.Category.Degradation.HasNoValuePastExpiration && item.IsPastSellInDate)
            {
                quality = 0;
            }

            return quality;
        }
    }
}
