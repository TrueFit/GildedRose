using System;
using System.Collections.Generic;
using System.Text;
using GildedRose.Domain.Models;

namespace GildedRose.Domain.Logic.Modifiers
{
    public class LinearStagedInceaseModifier : IItemModifier
    {
        private InventoryItemValue _item;

        public int Apply(InventoryItemValue item, int quality)
        {
            _item = item;

            if (!item.IsPastSellInDate && item.Category.Degradation.Interval > 1)
            {
                quality = quality -= CalculateDegradationIntervalValue();
            }

            return quality;
        }

        /// <summary>
        /// 	6. "Backstage passes", like aged brie, increases in Quality as it's SellIn value approaches;
        /// 	Quality increases by 2 when there are 10 days or less and by 3 when there are 5 days or 
        /// 	less but Quality drops to 0 after the concert
        /// </summary>
        /// <param name="daysSincePurchased"></param>
        /// <param name="sellIn"></param>
        /// <returns></returns>
        private int CalculateDegradationIntervalValue()
        {
            int sellIn = _item.SellIn;
            int DegradationValue = 0;
            int totalIntervalDegradationDays = 0;

            if (_item.Category.Degradation.Threshold > 1)
            {
                var daysBeforeSellIn = sellIn - _item.DaysSincePurchased;
                var daysWithinIntervalThreshold = _item.Category.Degradation.Threshold - daysBeforeSellIn;

                var DegradationIntervals = Math.Round((decimal)(daysBeforeSellIn / _item.Category.Degradation.Interval));

                for (var i = 0; i < daysWithinIntervalThreshold; i++)
                {
                    // Calculating the rate modifier for intervals, at minimum, it will be 2, increasing by 1 for each interval reached
                    var DegradationRateModifier = (int)Math.Round((decimal)(totalIntervalDegradationDays / _item.Category.Degradation.Interval)) + 1;

                    // Calculating the additional rate multiplier, accounting for the base rate that has already been applied
                    DegradationValue += (_item.Category.Degradation.Rate * DegradationRateModifier);

                    totalIntervalDegradationDays++;
                }
            }

            return DegradationValue;
        }

    }
}
