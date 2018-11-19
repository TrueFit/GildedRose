using gild_model;
using gild_repo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gild_repo_tests
{
    [TestClass]
    public class InventorySnapshotProcessorTests
    {
        private InventorySnapshotProcessor _snapshotProcessor;
        private List<InventoryItem> _initialInventory;

        [TestInitialize]
        public void Setup()
        {
            _initialInventory = new List<InventoryItem>
            {
                new InventoryItem
                {
                    Name = "Sword",
                    Category = "Weapon",                    
                    Quality = 25,
                    SellIn = 15
                },
                new InventoryItem
                {
                    Name = "Aged Brie",
                    Category = "Doesn't matter",                    
                    Quality = 10,
                    SellIn = 5
                },
                new InventoryItem
                {
                    Name = "Does't matter",
                    Category = "Sulfuras",
                    
                    // quality of 10 should be overwritten by 80.
                    Quality = 10,
                    SellIn = 5
                },
                new InventoryItem
                {
                    Name = "Limozeen",
                    Category = "Backstage Passes",                    
                    Quality = 3,
                    SellIn = 15
                },
                new InventoryItem
                {
                    Name = "Doesn't matter",
                    Category = "Conjured",
                    Quality = 20,
                    SellIn = 5
                },
            };

            _snapshotProcessor = new InventorySnapshotProcessor();
        }

        [TestMethod]
        public void A_normal_items_quality_should_decrease_by_1_each_day()
        {
            var snapshot = new InventorySnapshot();
            var setInitialInventory = JsonConvert.SerializeObject(new SetInitialInventoryDataEvent { InventoryItems = _initialInventory });

            var currentTime = new DateTime(2018, 1, 1, 10, 0, 0);
            _snapshotProcessor.ProcessEvent(snapshot, setInitialInventory, currentTime);

            for (var i = 0; i < 6; i++)
            {
                currentTime = currentTime.AddMinutes(1);

                var advanceDay = JsonConvert.SerializeObject(new AdvanceDayDataEvent());
                _snapshotProcessor.ProcessEvent(snapshot, advanceDay, currentTime);
            }

            var sword = snapshot.InventoryItems.Single(item => string.Equals(item.Name, "Sword", StringComparison.Ordinal));

            // starts at 25
            // -1 x 6
            sword.Quality.ShouldBe(19);
        }

        [TestMethod]
        public void A_normal_items_quality_should_decrease_by_2_after_the_sell_by_date()
        {
            var snapshot = new InventorySnapshot();
            var setInitialInventory = JsonConvert.SerializeObject(new SetInitialInventoryDataEvent { InventoryItems = _initialInventory });

            var currentTime = new DateTime(2018, 1, 1, 10, 0, 0);
            _snapshotProcessor.ProcessEvent(snapshot, setInitialInventory, currentTime);

            for (var i = 0; i < 17; i++)
            {
                currentTime = currentTime.AddMinutes(1);

                var advanceDay = JsonConvert.SerializeObject(new AdvanceDayDataEvent());
                _snapshotProcessor.ProcessEvent(snapshot, advanceDay, currentTime);
            }

            var sword = snapshot.InventoryItems.Single(item => string.Equals(item.Name, "Sword", StringComparison.Ordinal));

            // starts at 25
            // -1 x 15
            // -2 x 2
            sword.Quality.ShouldBe(6);
        }

        [TestMethod]
        public void Aged_bries_quality_should_increase_with_age()
        {
            var snapshot = new InventorySnapshot();
            var setInitialInventory = JsonConvert.SerializeObject(new SetInitialInventoryDataEvent { InventoryItems = _initialInventory });

            _snapshotProcessor.ProcessEvent(snapshot, setInitialInventory, new DateTime(2018, 1, 1, 10, 0, 0));

            var advanceDay = JsonConvert.SerializeObject(new AdvanceDayDataEvent());

            _snapshotProcessor.ProcessEvent(snapshot, advanceDay, new DateTime(2018, 1, 1, 10, 1, 0));

            var agedBrie = snapshot.InventoryItems.Single(item => string.Equals(item.Name, "Aged Brie", StringComparison.Ordinal));

            agedBrie.Quality.ShouldBe(11);            
        }

        [TestMethod]
        public void Aged_bries_quality_should_increase_twice_as_fast_after_the_sell_by_date()
        {
            var snapshot = new InventorySnapshot();
            var setInitialInventory = JsonConvert.SerializeObject(new SetInitialInventoryDataEvent { InventoryItems = _initialInventory });

            var currentTime = new DateTime(2018, 1, 1, 10, 0, 0);
            _snapshotProcessor.ProcessEvent(snapshot, setInitialInventory, currentTime);

            for (var i = 0; i < 6; i++)
            {
                currentTime = currentTime.AddMinutes(1);

                var advanceDay = JsonConvert.SerializeObject(new AdvanceDayDataEvent());
                _snapshotProcessor.ProcessEvent(snapshot, advanceDay, currentTime);
            }

            var agedBrie = snapshot.InventoryItems.Single(item => string.Equals(item.Name, "Aged Brie", StringComparison.Ordinal));

            // starts at 10
            // +1 x 5
            // +2 x 1
            agedBrie.Quality.ShouldBe(17);
        }

        [TestMethod]
        public void Aged_bries_sell_in_should_not_go_below_zero()
        {
            var snapshot = new InventorySnapshot();
            var setInitialInventory = JsonConvert.SerializeObject(new SetInitialInventoryDataEvent { InventoryItems = _initialInventory });

            var currentTime = new DateTime(2018, 1, 1, 10, 0, 0);
            _snapshotProcessor.ProcessEvent(snapshot, setInitialInventory, currentTime);

            for (var i = 0; i < 100; i++)
            {
                currentTime = currentTime.AddMinutes(1);

                var advanceDay = JsonConvert.SerializeObject(new AdvanceDayDataEvent());
                _snapshotProcessor.ProcessEvent(snapshot, advanceDay, currentTime);
            }

            var agedBrie = snapshot.InventoryItems.Single(item => string.Equals(item.Name, "Aged Brie", StringComparison.Ordinal));

            agedBrie.SellIn.ShouldBe(0);
        }

        [TestMethod]
        public void Aged_bries_quality_should_not_go_above_50()
        {
            var snapshot = new InventorySnapshot();
            var setInitialInventory = JsonConvert.SerializeObject(new SetInitialInventoryDataEvent { InventoryItems = _initialInventory });

            var currentTime = new DateTime(2018, 1, 1, 10, 0, 0);
            _snapshotProcessor.ProcessEvent(snapshot, setInitialInventory, currentTime);

            for (var i = 0; i < 100; i++)
            {
                currentTime = currentTime.AddMinutes(1);

                var advanceDay = JsonConvert.SerializeObject(new AdvanceDayDataEvent());
                _snapshotProcessor.ProcessEvent(snapshot, advanceDay, currentTime);
            }

            var agedBrie = snapshot.InventoryItems.Single(item => string.Equals(item.Name, "Aged Brie", StringComparison.Ordinal));

            agedBrie.Quality.ShouldBe(50);
        }

        [TestMethod]
        public void Sulfuras_quality_should_always_be_80()
        {
            var snapshot = new InventorySnapshot();
            var setInitialInventory = JsonConvert.SerializeObject(new SetInitialInventoryDataEvent { InventoryItems = _initialInventory });

            var currentTime = new DateTime(2018, 1, 1, 10, 0, 0);
            _snapshotProcessor.ProcessEvent(snapshot, setInitialInventory, currentTime);

            snapshot.InventoryItems.Single(item => string.Equals(item.Category, "Sulfuras", StringComparison.Ordinal)).Quality
                .ShouldBe(80);

            for (var i = 0; i < 100; i++)
            {
                currentTime = currentTime.AddMinutes(1);

                var advanceDay = JsonConvert.SerializeObject(new AdvanceDayDataEvent());
                _snapshotProcessor.ProcessEvent(snapshot, advanceDay, currentTime);

                snapshot.InventoryItems.Single(item => string.Equals(item.Category, "Sulfuras", StringComparison.Ordinal)).Quality
                    .ShouldBe(80);
            }
        }

        // Backstage passes", like aged brie, increases in Quality as it's SellIn value approaches;
        // Quality increases by 2 when there are 10 days or less and by 3 when there are 5 days or less
        // but Quality drops to 0 after the concert
        //
        // Assuming this means that before 10 days, that it goes up by 1 per day?
        [TestMethod]
        public void Backstage_passes_quality_increases_by_one_per_day_when_there_are_more_than_10_days_left()
        {
            var snapshot = new InventorySnapshot();
            var setInitialInventory = JsonConvert.SerializeObject(new SetInitialInventoryDataEvent { InventoryItems = _initialInventory });

            var currentTime = new DateTime(2018, 1, 1, 10, 0, 0);
            _snapshotProcessor.ProcessEvent(snapshot, setInitialInventory, currentTime);

            currentTime = currentTime.AddMinutes(1);

            var advanceDay = JsonConvert.SerializeObject(new AdvanceDayDataEvent());
            _snapshotProcessor.ProcessEvent(snapshot, advanceDay, currentTime);

            // Starts at 3
            // +1 x 1
            snapshot.InventoryItems.Single(item => string.Equals(item.Category, "Backstage Passes", StringComparison.Ordinal)).Quality
                .ShouldBe(4);
        }

        [TestMethod]
        public void Backstage_passes_quality_increases_by_two_per_day_when_there_are_less_than_10_days_left_but_more_than_5_days_left()
        {
            var snapshot = new InventorySnapshot();
            var setInitialInventory = JsonConvert.SerializeObject(new SetInitialInventoryDataEvent { InventoryItems = _initialInventory });

            var currentTime = new DateTime(2018, 1, 1, 10, 0, 0);
            _snapshotProcessor.ProcessEvent(snapshot, setInitialInventory, currentTime);

            for (var i = 0; i < 8; i++)
            {
                currentTime = currentTime.AddMinutes(1);

                var advanceDay = JsonConvert.SerializeObject(new AdvanceDayDataEvent());
                _snapshotProcessor.ProcessEvent(snapshot, advanceDay, currentTime);
            }

            // SellIn starts at 15
            // Qualtity starts at 3
            // +1 x 5
            // +2 x 3
            snapshot.InventoryItems.Single(item => string.Equals(item.Category, "Backstage Passes", StringComparison.Ordinal)).Quality
                .ShouldBe(14);
        }

        [TestMethod]
        public void Backstage_passes_quality_increases_by_three_per_day_when_there_are_less_than_5_days_left()
        {
            var snapshot = new InventorySnapshot();
            var setInitialInventory = JsonConvert.SerializeObject(new SetInitialInventoryDataEvent { InventoryItems = _initialInventory });

            var currentTime = new DateTime(2018, 1, 1, 10, 0, 0);
            _snapshotProcessor.ProcessEvent(snapshot, setInitialInventory, currentTime);

            for (var i = 0; i < 12; i++)
            {
                currentTime = currentTime.AddMinutes(1);

                var advanceDay = JsonConvert.SerializeObject(new AdvanceDayDataEvent());
                _snapshotProcessor.ProcessEvent(snapshot, advanceDay, currentTime);
            }

            // SellIn starts at 15
            // Qualtity starts at 3
            // +1 x 5
            // +2 x 5
            // +3 x 2
            snapshot.InventoryItems.Single(item => string.Equals(item.Category, "Backstage Passes", StringComparison.Ordinal)).Quality
                .ShouldBe(24);
        }

        [TestMethod]
        public void Backstage_passes_quality_should_drop_to_zero_after_the_concert()
        {
            var snapshot = new InventorySnapshot();
            var setInitialInventory = JsonConvert.SerializeObject(new SetInitialInventoryDataEvent { InventoryItems = _initialInventory });

            var currentTime = new DateTime(2018, 1, 1, 10, 0, 0);
            _snapshotProcessor.ProcessEvent(snapshot, setInitialInventory, currentTime);

            for (var i = 0; i < 16; i++)
            {
                currentTime = currentTime.AddMinutes(1);

                var advanceDay = JsonConvert.SerializeObject(new AdvanceDayDataEvent());
                _snapshotProcessor.ProcessEvent(snapshot, advanceDay, currentTime);
            }

            snapshot.InventoryItems.Single(item => string.Equals(item.Category, "Backstage Passes", StringComparison.Ordinal)).Quality
                .ShouldBe(0);
        }

        [TestMethod]
        public void Conjured_items_should_degrade_twice_as_fast_as_normal_items()
        {
            var snapshot = new InventorySnapshot();
            var setInitialInventory = JsonConvert.SerializeObject(new SetInitialInventoryDataEvent { InventoryItems = _initialInventory });

            var currentTime = new DateTime(2018, 1, 1, 10, 0, 0);
            _snapshotProcessor.ProcessEvent(snapshot, setInitialInventory, currentTime);

            currentTime = currentTime.AddMinutes(1);

            var advanceDay = JsonConvert.SerializeObject(new AdvanceDayDataEvent());
            _snapshotProcessor.ProcessEvent(snapshot, advanceDay, currentTime);

            // 20 - 2 = 18
            snapshot.InventoryItems.Single(item => string.Equals(item.Category, "Conjured", StringComparison.Ordinal)).Quality
                .ShouldBe(18);
        }
    }
}
