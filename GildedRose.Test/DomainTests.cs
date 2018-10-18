using System;
using GildedRose.Domain.Entity;
using GildedRose.Domain.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GildedRose.Test
{
    [TestClass]
    public class DomainTests
    {
        private static ItemFactory _factory;
        private Item _generalItem;
        private Item _brie;
        private Item _notBrieFood;
        private Item _conjured;
        private Item _sulfuras;
        private Item _backstagePass;

        [ClassInitialize]
        public static void Initializae(TestContext testContext)
        {
            _factory = new ConcreteItemFactory();
        }
        
        [TestInitialize]
        public void TestInitialize()
        {
            _generalItem = _factory.GetItem("Sword", "Weapon", 30, 50);
            _brie = _factory.GetItem("Aged Brie", "Food", 50, 10);
            _notBrieFood = _factory.GetItem("Mutton", "Food", 10, 10);
            _conjured = _factory.GetItem("Storm Hammer", "Conjured", 20, 50);
            _sulfuras = _factory.GetItem("Hand of Ragnaros", "Sulfuras", 80, 80);
            _backstagePass = _factory.GetItem("TAFKAL80ETC Concert", "Backstage Passes", 15, 20);
        }

        [TestMethod]
        public void CreateGeneralCategoryItem()
        {
            Assert.IsFalse(_generalItem is AgedBrie);
            Assert.IsFalse(_generalItem is BackstagePasses);
            Assert.IsFalse(_generalItem is Conjured);
            Assert.IsFalse(_generalItem is Sulfuras);
        }
        [TestMethod]
        public void CreateAgedBrieItem()
        {

            // brie should return as AgedBrie
            Assert.IsTrue(_brie is AgedBrie);
            Assert.IsFalse(_brie is BackstagePasses);
            Assert.IsFalse(_brie is Conjured);
            Assert.IsFalse(_brie is Sulfuras);

            // other food items should return as general inventory and not be a derived item child class
            Assert.IsFalse(_notBrieFood is AgedBrie);
            Assert.IsFalse(_notBrieFood is BackstagePasses);
            Assert.IsFalse(_notBrieFood is Conjured);
            Assert.IsFalse(_notBrieFood is Sulfuras);
        }

        [TestMethod]
        public void CreateConjuredCategoryItem()
        {
            Assert.IsFalse(_conjured is AgedBrie);
            Assert.IsFalse(_conjured is BackstagePasses);
            Assert.IsTrue(_conjured is Conjured);
            Assert.IsFalse(_conjured is Sulfuras);
        }

        [TestMethod]
        public void CreateSulfurasCategoryItem()
        {
            Assert.IsFalse(_sulfuras is AgedBrie);
            Assert.IsFalse(_sulfuras is BackstagePasses);
            Assert.IsFalse(_sulfuras is Conjured);
            Assert.IsTrue(_sulfuras is Sulfuras);
        }

        [TestMethod]
        public void CreateBackstagePassesCategoryItem()
        {
            Assert.IsFalse(_backstagePass is AgedBrie);
            Assert.IsTrue(_backstagePass is BackstagePasses);
            Assert.IsFalse(_backstagePass is Conjured);
            Assert.IsFalse(_backstagePass is Sulfuras);
        }

        [TestMethod]
        public void QualityNeverNegative()
        {
            QualityNeverNegative("Almost Negative Quality", "Any Category", 1, 1);
            QualityNeverNegative("Almost Negative Quality", ItemFactory.BACKSTAGEPASSES_CATEGORY, 1, 1);
            QualityNeverNegative("Almost Negative Quality", ItemFactory.SULFURAS_CATEGORY, 1, 1);
            QualityNeverNegative("Almost Negative Quality", ItemFactory.CONJURED_CATEGORY, 1, 1);
            QualityNeverNegative(ItemFactory.AGEDBRIE_NAME, ItemFactory.FOOD_CATEGORY, 1, 1);
        }

        [TestMethod]
        public void SellByDatePassedGeneral()
        {
            int preQuality = _generalItem.Quality;
            int preSellIn = _generalItem.SellIn;
            _generalItem = AgeItem(_generalItem);
            Assert.AreEqual(_generalItem.Quality, preQuality - preSellIn);

            preQuality = _generalItem.Quality;
            _generalItem.IncrementAge();
            Assert.AreEqual(_generalItem.Quality, preQuality - 2);
        }

        [TestMethod]
        public void SellByDatePassedAgedBrie()
        {
            _brie.SellIn = 5;
            int preQuality = _brie.Quality;
            int preSellIn = _brie.SellIn;
            _brie = AgeItem(_brie);
            Assert.AreEqual(_brie.Quality, preQuality + preSellIn);

            preQuality = _brie.Quality;
            _brie.IncrementAge();
            Assert.AreEqual(_brie.Quality, preQuality + 2);
        }

        [TestMethod]
        public void SellByDatePassedConjured()
        {
            // conjured items lose value at twice the rate
            int preQuality = _conjured.Quality;
            int preSellIn = _conjured.SellIn;
            _conjured = AgeItem(_conjured);
            Assert.AreEqual(_conjured.Quality, preQuality - (preSellIn * 2));

            preQuality = _conjured.Quality;
            _conjured.IncrementAge();
            Assert.AreEqual(_conjured.Quality, preQuality - 4);
        }

        [TestMethod]
        public void SellByDatePassedSulfuras()
        {
            // quality never changes on legendary items
            int preQuality = _sulfuras.Quality;
            _sulfuras = AgeItem(_sulfuras);
            Assert.AreEqual(_sulfuras.Quality, preQuality);
            _sulfuras.IncrementAge();
            _sulfuras.IncrementAge();
            _sulfuras.IncrementAge();
            _sulfuras.IncrementAge();
            _sulfuras.IncrementAge();
            _sulfuras.IncrementAge();
            _sulfuras.IncrementAge();
            Assert.AreEqual(_sulfuras.Quality, preQuality);
            // sulfuras ALWAYS worth 80
            Assert.AreEqual(_sulfuras.Quality, 80);
        }

        [TestMethod]
        public void SellByDatePassedBackstagePass()
        {
            // increase in value by 1 for each day up to 10 days remaining
            int preQuality = _backstagePass.Quality;
            int preSellIn = _backstagePass.SellIn;
            _backstagePass = AgeItem(_backstagePass, preSellIn - 10);
            Assert.AreEqual(_backstagePass.Quality, preQuality + (preSellIn - 10));

            // increase in value by 2 for each day between 10 and 5 days remaining
            preQuality = _backstagePass.Quality;
            _backstagePass = AgeItem(_backstagePass, 5);
            Assert.AreEqual(_backstagePass.Quality, preQuality + (5 * 2));

            // increase in value by 3 for each day after 5 days remaining until the day of the concert
            preQuality = _backstagePass.Quality;
            _backstagePass = AgeItem(_backstagePass, 4);
            Assert.AreEqual(_backstagePass.Quality, preQuality + (4 * 3));

            // passes are worthless after the concert
            _backstagePass.IncrementAge();
            _backstagePass.IncrementAge();
            Assert.AreEqual(_backstagePass.Quality, 0);
        }

        private Item AgeItem(Item item, int numDays = Int32.MinValue)
        {
            // default to age the item through its sellin value
            int preSellIn = numDays == Int32.MinValue ? item.SellIn : numDays;
            for (int i = 0; i < preSellIn; i++)
            {
                item.IncrementAge();
            }
            return item;
        }

        private void QualityNeverNegative(string name, string category, int sellIn, int quality)
        {
            Item item = _factory.GetItem(name, category, sellIn, quality);
            Assert.IsTrue(item.Quality > 0);

            for (int i = 0; i < item.SellIn; i++)
            {
                item.IncrementAge();
            }
            Assert.IsTrue(item.Quality >= 0);
            item.IncrementAge();
            item.IncrementAge();
            item.IncrementAge();
            item.IncrementAge();
            item.IncrementAge();
            item.IncrementAge();
            item.IncrementAge();
            item.IncrementAge();
            Assert.IsTrue(item.Quality >= 0);
        }
    }
}
