using System;
using System.Collections.Generic;
using System.Text;
using GildedRose.Domain.Logic.Utility;
using GildedRose.Domain.Models;

namespace GildedRose.Domain.Logic.Modifiers
{
    public class DoubleDegradation : IItemModifier
    {
        public int Apply(InventoryItemValue item, int quality)
        {
            if (item.IsIncreasingDegradation && item.IsPastSellInDate)
            {
                var doubleDegradationDuration = item.DaysSincePurchased - item.SellIn;
                quality -= doubleDegradationDuration * item.Category.Degradation.Rate;
            }

            return quality;
        }
    }
}
