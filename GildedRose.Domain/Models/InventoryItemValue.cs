using GildedRose.Domain.Logic.Calulation;
using GildedRose.Domain.Logic.Utility;
using GildedRose.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GildedRose.Domain.Models
{
    public class InventoryItemValue
    {
        public int InventoryItemId { get; set; }
        public int InventoryId { get; set; }
        public string Name { get; set; }
        public DateTime PurchasedOn { get; set; }
        public DateTime? CurrentDate { get; set; }
        public DateTime SellBy
        {
            get
            {
                return PurchasedOn.AddDays(SellIn);
            }
        }
        public int SellIn { get; set; }
        public int DaysSincePurchased
        {
            get
            {
                return DateOperations.GetDaysBetweenDates((CurrentDate ?? DateTime.Now), PurchasedOn);
            }
        }
        public bool IsPastSellInDate
        {
            get
            {
                return DaysSincePurchased > SellIn;
            }
        }
        public bool IsIncreasingDegradation
        {
            get
            {
                return !Category.Degradation.HasNoValuePastExpiration && IsPastSellInDate;
            }
        }
        public bool IsExpiredItem
        {
            get
            {
                return DaysSincePurchased > SellIn;
            }
        }

        public int CategoryId { get; set; }
        public CategoryValue Category { get; set; }
        public int QualityId { get; set; }
        public QualityValue Quality { get; set; }

        public int CalculateCurrentQuality()
        {
            var calculator = new QualityCalculator(this);
            Quality.Current = calculator.Calculate();
            return Quality.Current;
        }

    }
}
