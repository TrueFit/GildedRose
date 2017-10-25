using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit.InventoryTests
{
    [TestFixture]
    public class InventoryItemTests
    {
        [Test]
        public void DetermineItemStatus()
        {
            var logic = new GR.Logic.InventoryItemLogic();
            var someDate = new DateTime(2000, 1, 1);

            Assert.AreEqual(GR.Models.InventoryItemStatus.Available, logic.DetermineItemStatus(soldDate: null    , discardedDate: null    , itemQuality: 1.0));
            Assert.AreEqual(GR.Models.InventoryItemStatus.Available, logic.DetermineItemStatus(soldDate: null    , discardedDate: null    , itemQuality: 0.1));
            Assert.AreEqual(GR.Models.InventoryItemStatus.Expired  , logic.DetermineItemStatus(soldDate: null    , discardedDate: null    , itemQuality: 0.0));
            Assert.AreEqual(GR.Models.InventoryItemStatus.Expired  , logic.DetermineItemStatus(soldDate: null    , discardedDate: null    , itemQuality: -1.0));

            Assert.AreEqual(GR.Models.InventoryItemStatus.Sold     , logic.DetermineItemStatus(soldDate: someDate, discardedDate: null    , itemQuality: 1.0));
            Assert.AreEqual(GR.Models.InventoryItemStatus.Sold     , logic.DetermineItemStatus(soldDate: someDate, discardedDate: null    , itemQuality: 0.1));
            Assert.AreEqual(GR.Models.InventoryItemStatus.Sold     , logic.DetermineItemStatus(soldDate: someDate, discardedDate: null    , itemQuality: 0.0));
            Assert.AreEqual(GR.Models.InventoryItemStatus.Sold     , logic.DetermineItemStatus(soldDate: someDate, discardedDate: null    , itemQuality: -1.0));

            Assert.AreEqual(GR.Models.InventoryItemStatus.Sold     , logic.DetermineItemStatus(soldDate: someDate, discardedDate: someDate, itemQuality: 1.0));
            Assert.AreEqual(GR.Models.InventoryItemStatus.Sold     , logic.DetermineItemStatus(soldDate: someDate, discardedDate: someDate, itemQuality: 0.1));
            Assert.AreEqual(GR.Models.InventoryItemStatus.Sold     , logic.DetermineItemStatus(soldDate: someDate, discardedDate: someDate, itemQuality: 0.0));
            Assert.AreEqual(GR.Models.InventoryItemStatus.Sold     , logic.DetermineItemStatus(soldDate: someDate, discardedDate: someDate, itemQuality: -1.0));

            Assert.AreEqual(GR.Models.InventoryItemStatus.Discarded, logic.DetermineItemStatus(soldDate: null    , discardedDate: someDate, itemQuality: 1.0));
            Assert.AreEqual(GR.Models.InventoryItemStatus.Discarded, logic.DetermineItemStatus(soldDate: null    , discardedDate: someDate, itemQuality: 0.1));
            Assert.AreEqual(GR.Models.InventoryItemStatus.Discarded, logic.DetermineItemStatus(soldDate: null    , discardedDate: someDate, itemQuality: 0.0));
            Assert.AreEqual(GR.Models.InventoryItemStatus.Discarded, logic.DetermineItemStatus(soldDate: null    , discardedDate: someDate, itemQuality: -1.0));
        }

        [Test]
        public void DetermineCurrentAvailableItemQuality_Linear()
        {
            GR.Models.InventoryItemQualityDeltaStrategy strategy = GR.Models.InventoryItemQualityDeltaStrategy.Linear;
            DateTime invoiceDate;
            double initialQuality;
            DateTime sellByDate;
            double baseDelta;
            double minQuality;
            double maxQuality;
            DateTime today;

            var logic = new GR.Logic.InventoryItemLogic();

            invoiceDate = new DateTime(2000, 1, 1);
            initialQuality = 30.0;
            sellByDate = new DateTime(2000, 1, 10);
            baseDelta = 1.0;
            minQuality = 0.0;
            maxQuality = 50.0;

            today = new DateTime(2000, 1, 1);
            Assert.AreEqual(30.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 2);
            Assert.AreEqual(29.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 9);
            Assert.AreEqual(22.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 10);
            Assert.AreEqual(21.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 11);
            Assert.AreEqual(19.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 12);
            Assert.AreEqual(17.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 20);
            Assert.AreEqual(1.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 21);
            Assert.AreEqual(0.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 22);
            Assert.AreEqual(0.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
        }
    }
}
