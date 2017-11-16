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

            Assert.AreEqual(GR.Models.InventoryItemStatusId.Available, logic.DetermineItemStatus(soldDate: null    , discardedDate: null    , itemQuality: 1.0));
            Assert.AreEqual(GR.Models.InventoryItemStatusId.Available, logic.DetermineItemStatus(soldDate: null    , discardedDate: null    , itemQuality: 0.1));
            Assert.AreEqual(GR.Models.InventoryItemStatusId.Expired  , logic.DetermineItemStatus(soldDate: null    , discardedDate: null    , itemQuality: 0.0));
            Assert.AreEqual(GR.Models.InventoryItemStatusId.Expired  , logic.DetermineItemStatus(soldDate: null    , discardedDate: null    , itemQuality: -1.0));

            Assert.AreEqual(GR.Models.InventoryItemStatusId.Sold     , logic.DetermineItemStatus(soldDate: someDate, discardedDate: null    , itemQuality: 1.0));
            Assert.AreEqual(GR.Models.InventoryItemStatusId.Sold     , logic.DetermineItemStatus(soldDate: someDate, discardedDate: null    , itemQuality: 0.1));
            Assert.AreEqual(GR.Models.InventoryItemStatusId.Sold     , logic.DetermineItemStatus(soldDate: someDate, discardedDate: null    , itemQuality: 0.0));
            Assert.AreEqual(GR.Models.InventoryItemStatusId.Sold     , logic.DetermineItemStatus(soldDate: someDate, discardedDate: null    , itemQuality: -1.0));

            Assert.AreEqual(GR.Models.InventoryItemStatusId.Sold     , logic.DetermineItemStatus(soldDate: someDate, discardedDate: someDate, itemQuality: 1.0));
            Assert.AreEqual(GR.Models.InventoryItemStatusId.Sold     , logic.DetermineItemStatus(soldDate: someDate, discardedDate: someDate, itemQuality: 0.1));
            Assert.AreEqual(GR.Models.InventoryItemStatusId.Sold     , logic.DetermineItemStatus(soldDate: someDate, discardedDate: someDate, itemQuality: 0.0));
            Assert.AreEqual(GR.Models.InventoryItemStatusId.Sold     , logic.DetermineItemStatus(soldDate: someDate, discardedDate: someDate, itemQuality: -1.0));

            Assert.AreEqual(GR.Models.InventoryItemStatusId.Discarded, logic.DetermineItemStatus(soldDate: null    , discardedDate: someDate, itemQuality: 1.0));
            Assert.AreEqual(GR.Models.InventoryItemStatusId.Discarded, logic.DetermineItemStatus(soldDate: null    , discardedDate: someDate, itemQuality: 0.1));
            Assert.AreEqual(GR.Models.InventoryItemStatusId.Discarded, logic.DetermineItemStatus(soldDate: null    , discardedDate: someDate, itemQuality: 0.0));
            Assert.AreEqual(GR.Models.InventoryItemStatusId.Discarded, logic.DetermineItemStatus(soldDate: null    , discardedDate: someDate, itemQuality: -1.0));
        }

        [Test]
        public void DetermineCurrentAvailableItemQuality_Linear()
        {
            GR.Models.InventoryItemQualityDeltaStrategyId strategy = GR.Models.InventoryItemQualityDeltaStrategyId.Linear;
            DateTime invoiceDate;
            double initialQuality;
            DateTime? sellByDate;
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
            today = new DateTime(2000, 3, 1);
            Assert.AreEqual(0.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));

            initialQuality = 30.0;
            baseDelta = 2.0;
            today = new DateTime(2000, 1, 1);
            Assert.AreEqual(30.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 2);
            Assert.AreEqual(28.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 9);
            Assert.AreEqual(14.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 10);
            Assert.AreEqual(12.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 11);
            Assert.AreEqual(8.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 12);
            Assert.AreEqual(4.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 13);
            Assert.AreEqual(0.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 14);
            Assert.AreEqual(0.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 3, 1);
            Assert.AreEqual(0.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));

            initialQuality = 30.0;
            baseDelta = 1.0;
            minQuality = 4.0;
            maxQuality = 25.0;
            today = new DateTime(2000, 1, 1);
            Assert.AreEqual(25.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 2);
            Assert.AreEqual(24.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 9);
            Assert.AreEqual(17.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 10);
            Assert.AreEqual(16.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 11);
            Assert.AreEqual(14.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 12);
            Assert.AreEqual(12.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 15);
            Assert.AreEqual(6.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 16);
            Assert.AreEqual(4.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 17);
            Assert.AreEqual(4.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 3, 1);
            Assert.AreEqual(4.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));

            initialQuality = 1.0;
            baseDelta = 1.0;
            minQuality = 4.0;
            maxQuality = 25.0;
            today = new DateTime(2000, 1, 1);
            Assert.AreEqual(4.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 2);
            Assert.AreEqual(4.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 3, 1);
            Assert.AreEqual(4.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));

            sellByDate = null;
            Assert.Catch<ArgumentException>(() => logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));

        }

        [Test]
        public void DetermineCurrentAvailableItemQuality_InverseLinear()
        {
            GR.Models.InventoryItemQualityDeltaStrategyId strategy = GR.Models.InventoryItemQualityDeltaStrategyId.InverseLinear;
            DateTime invoiceDate;
            double initialQuality;
            DateTime? sellByDate;
            double baseDelta;
            double minQuality;
            double maxQuality;
            DateTime today;

            var logic = new GR.Logic.InventoryItemLogic();

            invoiceDate = new DateTime(2000, 1, 1);
            initialQuality = 10.0;
            sellByDate = new DateTime(2000, 1, 10);
            baseDelta = 1.0;
            minQuality = 0.0;
            maxQuality = 50.0;

            today = new DateTime(2000, 1, 1);
            Assert.AreEqual(10.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 2);
            Assert.AreEqual(11.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 9);
            Assert.AreEqual(18.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 10);
            Assert.AreEqual(19.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 11);
            Assert.AreEqual(20.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 12);
            Assert.AreEqual(21.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 20);
            Assert.AreEqual(29.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 21);
            Assert.AreEqual(30.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 22);
            Assert.AreEqual(31.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 2, 9);
            Assert.AreEqual(49.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 2, 10);
            Assert.AreEqual(50.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 2, 11);
            Assert.AreEqual(50.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 3, 1);
            Assert.AreEqual(50.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));

            baseDelta = 2.0;
            today = new DateTime(2000, 1, 1);
            Assert.AreEqual(10.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 2);
            Assert.AreEqual(12.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 9);
            Assert.AreEqual(26.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 10);
            Assert.AreEqual(28.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 11);
            Assert.AreEqual(30.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 12);
            Assert.AreEqual(32.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 20);
            Assert.AreEqual(48.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 21);
            Assert.AreEqual(50.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 22);
            Assert.AreEqual(50.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 3, 1);
            Assert.AreEqual(50.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));

            baseDelta = 1.0;
            minQuality = 4.0;
            maxQuality = 25.0;
            today = new DateTime(2000, 1, 1);
            Assert.AreEqual(10.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 2);
            Assert.AreEqual(11.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 9);
            Assert.AreEqual(18.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 10);
            Assert.AreEqual(19.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 11);
            Assert.AreEqual(20.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 12);
            Assert.AreEqual(21.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 20);
            Assert.AreEqual(25.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 21);
            Assert.AreEqual(25.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 22);
            Assert.AreEqual(25.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 3, 1);
            Assert.AreEqual(25, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));

            initialQuality = 50.0;
            minQuality = 4.0;
            maxQuality = 25.0;
            today = new DateTime(2000, 1, 1);
            Assert.AreEqual(25.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 2);
            Assert.AreEqual(25.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 3, 1);
            Assert.AreEqual(25.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));

            initialQuality = 1.0;
            minQuality = 4.0;
            maxQuality = 25.0;
            today = new DateTime(2000, 1, 1);
            Assert.AreEqual(4.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 2);
            Assert.AreEqual(5.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 21);
            Assert.AreEqual(24.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 22);
            Assert.AreEqual(25.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 23);
            Assert.AreEqual(25.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 3, 1);
            Assert.AreEqual(25.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));

            initialQuality = 10.0;
            sellByDate = null;
            minQuality = 0.0;
            maxQuality = 50.0;
            today = new DateTime(2000, 1, 1);
            Assert.AreEqual(10.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 2);
            Assert.AreEqual(11.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 9);
            Assert.AreEqual(18.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 10);
            Assert.AreEqual(19.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 11);
            Assert.AreEqual(20.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 12);
            Assert.AreEqual(21.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 20);
            Assert.AreEqual(29.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 21);
            Assert.AreEqual(30.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 22);
            Assert.AreEqual(31.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 2, 9);
            Assert.AreEqual(49.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 2, 10);
            Assert.AreEqual(50.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 2, 11);
            Assert.AreEqual(50.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 3, 1);
            Assert.AreEqual(50.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));

        }

        [Test]
        public void DetermineCurrentAvailableItemQuality_Static()
        {
            GR.Models.InventoryItemQualityDeltaStrategyId strategy = GR.Models.InventoryItemQualityDeltaStrategyId.Static;
            DateTime invoiceDate;
            double initialQuality;
            DateTime? sellByDate;
            double baseDelta;
            double minQuality;
            double maxQuality;
            DateTime today;

            var logic = new GR.Logic.InventoryItemLogic();

            invoiceDate = new DateTime(2000, 1, 1);
            initialQuality = 10.0;
            sellByDate = new DateTime(2000, 1, 10);
            baseDelta = 1.0;
            minQuality = 0.0;
            maxQuality = 50.0;

            today = new DateTime(2000, 1, 1);
            Assert.AreEqual(10.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 2);
            Assert.AreEqual(10.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 3, 1);
            Assert.AreEqual(10.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));

            initialQuality = 50.0;
            minQuality = 4.0;
            maxQuality = 25.0;
            today = new DateTime(2000, 1, 1);
            Assert.AreEqual(25.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 2);
            Assert.AreEqual(25.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 3, 1);
            Assert.AreEqual(25.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));

            initialQuality = 1.0;
            minQuality = 4.0;
            maxQuality = 25.0;
            today = new DateTime(2000, 1, 1);
            Assert.AreEqual(4.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 2);
            Assert.AreEqual(4.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 3, 1);
            Assert.AreEqual(4.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
        }

        [Test]
        public void DetermineCurrentAvailableItemQuality_Event()
        {
            GR.Models.InventoryItemQualityDeltaStrategyId strategy = GR.Models.InventoryItemQualityDeltaStrategyId.Event;
            DateTime invoiceDate;
            double initialQuality;
            DateTime? sellByDate;
            double baseDelta;
            double minQuality;
            double maxQuality;
            DateTime today;

            var logic = new GR.Logic.InventoryItemLogic();

            invoiceDate = new DateTime(2000, 1, 1);
            initialQuality = 10.0;
            sellByDate = new DateTime(2000, 1, 15);
            baseDelta = 1.0;
            minQuality = 0.0;
            maxQuality = 50.0;

            today = new DateTime(2000, 1, 1);
            Assert.AreEqual(10.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 2);
            Assert.AreEqual(11.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 3);
            Assert.AreEqual(12.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 4);
            Assert.AreEqual(13.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 5);
            Assert.AreEqual(14.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 6);
            Assert.AreEqual(16.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 7);
            Assert.AreEqual(18.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 8);
            Assert.AreEqual(20.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 9);
            Assert.AreEqual(22.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 10);
            Assert.AreEqual(24.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 11);
            Assert.AreEqual(27.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 12);
            Assert.AreEqual(30.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 13);
            Assert.AreEqual(33.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 14);
            Assert.AreEqual(36.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 15);
            Assert.AreEqual(39.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 16);
            Assert.AreEqual(0.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 3, 1);
            Assert.AreEqual(0.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));

            baseDelta = 2.0;
            minQuality = 0.0;
            maxQuality = 50.0;
            today = new DateTime(2000, 1, 1);
            Assert.AreEqual(10.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 2);
            Assert.AreEqual(12.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 3);
            Assert.AreEqual(14.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 4);
            Assert.AreEqual(16.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 5);
            Assert.AreEqual(18.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 6);
            Assert.AreEqual(22.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 7);
            Assert.AreEqual(26.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 8);
            Assert.AreEqual(30.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 9);
            Assert.AreEqual(34.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 10);
            Assert.AreEqual(38.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 11);
            Assert.AreEqual(44.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 12);
            Assert.AreEqual(50.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 13);
            Assert.AreEqual(50.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 14);
            Assert.AreEqual(50.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 15);
            Assert.AreEqual(50.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 1, 16);
            Assert.AreEqual(0.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
            today = new DateTime(2000, 3, 1);
            Assert.AreEqual(0.0, logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));

            sellByDate = null;
            Assert.Catch<ArgumentException>(() => logic.DetermineCurrentAvailableItemQuality(strategy, invoiceDate, initialQuality, sellByDate, baseDelta, minQuality, maxQuality, today));
        }

        [Test]
        public void DetermineIsSellByDateRequired()
        {
            var iiLogic = new GR.Logic.InventoryItemLogic();

            Assert.AreEqual(true , iiLogic.DetermineIsSellByDateRequired(GR.Models.InventoryItemQualityDeltaStrategyId.Linear));
            Assert.AreEqual(false, iiLogic.DetermineIsSellByDateRequired(GR.Models.InventoryItemQualityDeltaStrategyId.InverseLinear));
            Assert.AreEqual(false, iiLogic.DetermineIsSellByDateRequired(GR.Models.InventoryItemQualityDeltaStrategyId.Static));
            Assert.AreEqual(true , iiLogic.DetermineIsSellByDateRequired(GR.Models.InventoryItemQualityDeltaStrategyId.Event));

        }

        [Test]
        public void DetermineCanItemBeDiscarded()
        {
            var iiLogic = new GR.Logic.InventoryItemLogic();
            DateTime now = DateTime.Now;

            Assert.AreEqual(true , iiLogic.DetermineCanItemBeDiscarded(GR.Models.InventoryItemStatusId.Available));
            Assert.AreEqual(true , iiLogic.DetermineCanItemBeDiscarded(GR.Models.InventoryItemStatusId.Expired));
            Assert.AreEqual(false, iiLogic.DetermineCanItemBeDiscarded(GR.Models.InventoryItemStatusId.Discarded));
            Assert.AreEqual(false, iiLogic.DetermineCanItemBeDiscarded(GR.Models.InventoryItemStatusId.Sold));
            Assert.Catch<ArgumentOutOfRangeException>(() => iiLogic.DetermineCanItemBeDiscarded(GR.Models.InventoryItemStatusId.Unknown));
        }

        [Test]
        public void DetermineCanItemBeSold()
        {
            var iiLogic = new GR.Logic.InventoryItemLogic();
            DateTime now = DateTime.Now;

            Assert.AreEqual(true , iiLogic.DetermineCanItemBeSold(GR.Models.InventoryItemStatusId.Available));
            Assert.AreEqual(false, iiLogic.DetermineCanItemBeSold(GR.Models.InventoryItemStatusId.Expired));
            Assert.AreEqual(false, iiLogic.DetermineCanItemBeSold(GR.Models.InventoryItemStatusId.Discarded));
            Assert.AreEqual(false, iiLogic.DetermineCanItemBeSold(GR.Models.InventoryItemStatusId.Sold));
            Assert.Catch<ArgumentOutOfRangeException>(() => iiLogic.DetermineCanItemBeSold(GR.Models.InventoryItemStatusId.Unknown));
        }
    }
}
