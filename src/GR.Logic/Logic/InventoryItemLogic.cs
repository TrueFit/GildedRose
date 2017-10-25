using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GR.Utilities;
namespace GR.Logic
{
    public class InventoryItemLogic
    {
        public Models.InventoryItemStatus DetermineItemStatus(DateTime? soldDate, DateTime? discardedDate, double itemQuality)
        {
            if (soldDate != null)
                return Models.InventoryItemStatus.Sold;
            if (discardedDate != null)
                return Models.InventoryItemStatus.Discarded;
            if (itemQuality <= 0.0)
                return Models.InventoryItemStatus.Expired;

            return Models.InventoryItemStatus.Available;
        }

        public double DetermineCurrentAvailableItemQuality(
            Models.InventoryItemQualityDeltaStrategy strategy,
            DateTime invoiceDate,
            double initialQuality,
            DateTime? sellByDate,
            double baseDelta = 1.0,
            double minQuality = 0.0,
            double maxQuality = 50.0,
            DateTime? now_ = null)
        {
            //Clean up the dates to make our date math simpler.  invoiceDate is set to the begining 
            //  of the day the item was invoiced.  sellByDate, if specified, is the last second of 
            //  that day.
            invoiceDate = invoiceDate.Date;
            if (sellByDate != null)
                sellByDate = sellByDate.Value.Date.AddDays(1).AddSeconds(-1);

            //For the purpose of this calculation, we only care about the day (time doesn't matter).
            var today = (now_ ?? DateTime.Now).Date;

            if (invoiceDate > today)
                return Math.Max(initialQuality, 0.0);

            switch(strategy)
            {
                case Models.InventoryItemQualityDeltaStrategy.Linear:
                    if(sellByDate == null)
                        throw new ArgumentException($"sellByDate is required when strategy is {nameof(Models.InventoryItemQualityDeltaStrategy.Linear)}.", nameof(strategy));
                    return DetermineCurrentAvailableItemQuality_Linear(
                        invoiceDate, initialQuality, sellByDate.Value, baseDelta, minQuality, maxQuality, today);

                case Models.InventoryItemQualityDeltaStrategy.InverseLinear:
                    return DetermineCurrentAvailableItemQuality_InverseLinear(
                        invoiceDate, initialQuality, baseDelta, minQuality, maxQuality, today);

                case Models.InventoryItemQualityDeltaStrategy.Static:
                    return DetermineCurrentAvailableItemQuality_Static(initialQuality, minQuality, maxQuality);

                case Models.InventoryItemQualityDeltaStrategy.Event:
                    if (sellByDate == null)
                        throw new ArgumentException($"sellByDate is required when strategy is {nameof(Models.InventoryItemQualityDeltaStrategy.Event)}.", nameof(strategy));
                    return DetermineCurrentAvailableItemQuality_Event(
                        invoiceDate, initialQuality, sellByDate.Value, baseDelta, minQuality, maxQuality, today);

                default:
                    throw new ArgumentException($"Unknown {nameof(Models.InventoryItemQualityDeltaStrategy)} value: {strategy}.", nameof(strategy));
            }
        }

        private double DetermineCurrentAvailableItemQuality_Linear(
            DateTime invoiceDate,
            double initialQuality,
            DateTime sellByDate,
            double baseDelta,
            double minQuality,
            double maxQuality,
            DateTime today)
        {
            var normalDecay = Math.Max((int)(DateTimeUtility.Min(today, sellByDate) - invoiceDate).TotalDays, 0) * baseDelta;
            var doubleDecay = Math.Max((int)(today - sellByDate.AddDays(-1)).TotalDays, 0) * baseDelta * 2;

            return Math.Max(Math.Min(initialQuality - normalDecay - doubleDecay, maxQuality), minQuality);
        }

        private double DetermineCurrentAvailableItemQuality_InverseLinear(
            DateTime invoiceDate,
            double initialQuality,
            double baseDelta,
            double minQuality,
            double maxQuality,
            DateTime today)
            => Math.Max(Math.Min(initialQuality + Math.Max((int)(today - invoiceDate).TotalDays, 0) * baseDelta, maxQuality), minQuality);

        private double DetermineCurrentAvailableItemQuality_Static(
            double initialQuality,
            double minQuality,
            double maxQuality)
            => Math.Max(Math.Min(initialQuality, maxQuality), minQuality);

        private double DetermineCurrentAvailableItemQuality_Event(
            DateTime invoiceDate,
            double initialQuality,
            DateTime sellByDate,
            double baseDelta,
            double minQuality,
            double maxQuality,
            DateTime today)
        {
            if (today > sellByDate)
                return 0.0;

            var tripleIncrease = Math.Max((int)(DateTimeUtility.Min(sellByDate             , today) - DateTimeUtility.Max(sellByDate.AddDays(-5) , invoiceDate)).TotalHours, 0) * baseDelta * 3;
            var doubleIncrease = Math.Max((int)(DateTimeUtility.Min(sellByDate.AddDays(-5) , today) - DateTimeUtility.Max(sellByDate.AddDays(-10), invoiceDate)).TotalHours, 0) * baseDelta * 2;
            var singleIncrease = Math.Max((int)(DateTimeUtility.Min(sellByDate.AddDays(-10), today) - DateTimeUtility.Max(sellByDate.AddDays(-10), invoiceDate)).TotalHours, 0) * baseDelta;

            return Math.Max(Math.Min(initialQuality + tripleIncrease + doubleIncrease + singleIncrease, maxQuality), minQuality);
        }
    }
}
