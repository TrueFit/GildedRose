using GildedRose.Domain.Models;
using GildedRose.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GildedRose.Domain.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class InventoryItemValueTests
    {
        public InventoryItemValue ScaffoldInventoryItem(int sellIn = 10, int initialValue = 40, int DegradationRate = 1, 
                                                   int maximum = 50, int DegradationInterval = 0, DateTime? purchasedOn = null, 
                                                   bool hasNoValuePastExpiration = false)
        {
            purchasedOn = purchasedOn ?? DateTime.Now.AddDays(-5);
            return new InventoryItemValue() {
                Name = "Test",
                PurchasedOn = purchasedOn.Value,
                SellIn = sellIn,
                Category = new CategoryValue()
                {
                    Name = "Potion",
                    MaximumQuality = maximum,
                    MinimumQuality = 0,
                    Degradation = new DegradationValue()
                    {
                        Rate = DegradationRate,
                        Interval = DegradationInterval,
                        Threshold = 10,
                        HasNoValuePastExpiration = hasNoValuePastExpiration
                    }
                },
                Quality = new QualityValue()
                {
                    Initial = initialValue,
                    Current = initialValue
                }
            };
        }

        [TestMethod]
        public void InventoryItemMustInstantiate()
        {
            var item = ScaffoldInventoryItem();

            Assert.AreEqual("Test", item.Name);
            Assert.AreEqual(10, item.SellIn);
            Assert.AreEqual("Potion", item.Category.Name);
            Assert.AreEqual(40, item.Quality.Initial);
        }

        /// <summary>
        /// Accounting for:
        ///     At the end of each day our system lowers both values for every item
        /// </summary>
        [TestMethod]
        public void InventoryItemShouldCalculateBasic5DayDegradation()
        {
            var item = ScaffoldInventoryItem();
            var itemQualityValue = item.CalculateCurrentQuality();

            Assert.AreEqual(35, itemQualityValue);
        }

        /// <summary>
        /// Accounting for:
        ///     Once the sell by date has passed, Quality degrades twice as fast
        ///     1 day to sell (ds)
        ///     5 days have past (dp)
        ///     1 Degradation rate (dr)
        ///     1 day of -1 quality
        ///     4 days of -2 quality
        ///     -9 quality total
        ///     31 quality remaining
        /// </summary>
        [TestMethod]
        public void InventoryItemValueShouldDecreaseByDoubleIfSellInPast()
        {
            var item = ScaffoldInventoryItem(1, 40, 1);
            var itemQualityValue = item.CalculateCurrentQuality();

            Assert.AreEqual(31, itemQualityValue);
        }

        /// <summary>
        /// Accounting for: 
        ///     The Quality of an item is never negative
        /// </summary>
        [TestMethod]
        public void InventoryItemValueShouldStopAtMinimumQualityValue()
        {
            var item = ScaffoldInventoryItem(1, 1);
            var itemQualityValue = item.CalculateCurrentQuality();

            Assert.AreEqual(0, itemQualityValue);
        }

        /// <summary>
        /// Accounting for:
        ///     The Quality of an item is never more than 50
        /// </summary>
        [TestMethod]
        public void InventoryItemValueShouldStopAtMaximumQualityValue()
        {
            var item = ScaffoldInventoryItem(100, 100);
            var itemQualityValue = item.CalculateCurrentQuality();

            Assert.AreEqual(50, itemQualityValue);
        }

        /// <summary>
        /// Accounting for:
        ///     "Aged Brie" actually increases in Quality the older it gets
        /// </summary>
        [TestMethod]
        public void InventoryItemValueShouldIncreaseWithAgeIfSetToNegative()
        {
            var item = ScaffoldInventoryItem(10, 40, -1);
            var itemQualityValue = item.CalculateCurrentQuality();

            Assert.AreEqual(45, itemQualityValue);
        }

        /// <summary>
        /// Accounting for:
        ///     "Sulfuras", being a legendary item, never has to be sold or decreases in Quality
        ///     And
        ///     An item can never have its Quality increase above 50, however "Sulfuras" is a legendary item and as such its Quality is 80 and it never alters.
        /// </summary>
        [TestMethod]
        public void InventoryItemValueShouldRemainStaticIfThereIsNoDegradation()
        {
            var item = ScaffoldInventoryItem(10, 80, 0, 80);
            var itemQualityValue = item.CalculateCurrentQuality();

            Assert.AreEqual(80, itemQualityValue);
        }

        /// <summary>
        /// Accounting for:
        ///     "Conjured" items degrade in Quality twice as fast as normal items
        /// </summary>
        [TestMethod]
        public void InventoryItemValueShouldDecreaseFasterWithIncreasedDegradationRate()
        {
            var item = ScaffoldInventoryItem(10, 40, 2);
            var itemQualityValue = item.CalculateCurrentQuality();

            Assert.AreEqual(30, itemQualityValue);
        }

        /// <summary>
        /// Accounting for:
        /// 	6. "Backstage passes", like aged brie, increases in Quality as it's SellIn value approaches;
        /// 	Quality increases by 2 when there are 10 days or less and by 3 when there are 5 days or 
        /// 	less but Quality drops to 0 after the concert
        /// 	Purchased 15 days ago
        /// 	5 standard appreciation = 5
        /// 	5 double appreciation = 10
        /// 	5 triple appreciation = 15
        /// 	30 total appreciation
        /// 	10 base appreciation
        /// 	---
        /// 	40 total
        /// </summary>
        [TestMethod]
        public void InventoryItemValueShouldDecreaseExponentiallyWithDegradationInterval()
        {

            var item = ScaffoldInventoryItem(15, 10, -1, 50, 5, DateTime.Now.AddDays(-15), true);
            var itemQualityValue = item.CalculateCurrentQuality();

            Assert.AreEqual(40, itemQualityValue);
        }

        /// <summary>
        /// Accounting for:
        /// 	6. "Backstage passes", like aged brie, increases in Quality as it's SellIn value approaches;
        /// 	Quality increases by 2 when there are 10 days or less and by 3 when there are 5 days or 
        /// 	less but Quality drops to 0 after the concert
        /// 	Purchased 15 days ago
        /// </summary>
        [TestMethod]
        public void InventoryItemValueShouldDecreaseTo0IfItHasNoValuePastExpiration()
        {
            var item = ScaffoldInventoryItem(5, 10, -1, 50, 5, DateTime.Now.AddDays(-15), true);
            var itemQualityValue = item.CalculateCurrentQuality();

            Assert.AreEqual(0, itemQualityValue);
        }

        /// <summary>
        /// Accounting for:
        /// 	Ragin Ogre
        /// </summary>
        [TestMethod]
        public void RaginOgreShouldIncreaseInValue()
        {
            var item = ScaffoldInventoryItem(10, 10, -1, 50, 5, DateTime.Now.AddDays(-7), true);
            var itemQualityValue = item.CalculateCurrentQuality();

            Assert.AreEqual(26, itemQualityValue);
        }
    }
}
