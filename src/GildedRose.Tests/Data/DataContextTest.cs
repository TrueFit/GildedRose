using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GildedRose.Data;
using GildedRose.Entities.Inventory;
using GildedRose.Entities.Inventory.Aging;
using Xunit;

namespace GildedRose.Tests.Data
{
    /// <summary>
    /// This integration test was created primarily to facility the setup and configuration of the Entity Framework
    /// and the data seeding in the "Initialize" migration.
    /// </summary>
    public class DataContextTest : IClassFixture<BaseTestFixture>
    {
        public BaseTestFixture _fixture;

        public DataContextTest(BaseTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void InitializeMigration_OnEmptyDatabase_SeedsInventoryAndHistoryRecords()
        {
            DataContextHelper.SetupCleanDatabase();

            using (var dc = new DataContext())
            {
                var inventory = dc.Inventory.ToList();
                var history = dc.InventoryHistory.ToList();

                // Ensure we loaded the expected total records
                // Hard-coding 20 here isn't a typical real world approach 
                // In this case, I'm treating the file as static so this is confirming that expectation
                var expectedCount = 20;
                Assert.Equal(expectedCount, inventory.Count);
                Assert.Equal(expectedCount, history.Count);

                // Test that the column-to-property mapping worked by checking the first item (if it worked, others will as well)
                // We will also compare the first history item as we go property-by-property
                var firstInventoryItem = inventory.First();
                var firstHistoryItem = history.First();
                Assert.Equal("Sword", firstInventoryItem.Name);
                Assert.Equal(firstInventoryItem.Name, firstHistoryItem.Name);
                Assert.Equal("Weapon", firstInventoryItem.Category);
                Assert.Equal(firstInventoryItem.Category, firstHistoryItem.Category);
                Assert.Equal(30, firstInventoryItem.SellIn);
                Assert.Equal(firstInventoryItem.SellIn, firstHistoryItem.SellIn);
                Assert.Equal(50, firstInventoryItem.Quality);
                Assert.Equal(firstInventoryItem.Quality, firstHistoryItem.Quality);
                Assert.True(firstInventoryItem.InventoryAddedDate != DateTime.MinValue); // here we make sure the date is set and doesn't get the default DateTime value
                Assert.Equal(firstInventoryItem.InventoryAddedDate, firstHistoryItem.InventoryAddedDate);

                // Now for the unique history item properties
                Assert.True(firstHistoryItem.LastModifiedDate != DateTime.MinValue); // here we make sure the date is set and doesn't get the default DateTime value
                Assert.Equal(InventoryHistoryAction.Created, firstHistoryItem.Action);
            }
        }

        [Fact]
        public void AgingRules_MissingFile_ThrowsException()
        {
            SetupAgingRules(null);

            using (var dc = new DataContext())
            {
                Assert.Throws<FileNotFoundException>(() => dc.AgingRules.ToList());
            }
        }

        [Fact]
        public void AgingRules_ExistingFile_ReturnsRules()
        {
            var agingRules = new List<BaseAgingRule>
            {
                new TieredQualityAgingRule {Name = "Normal Item Rule", ItemCategory = null, ItemName = null, MaxQuality = 50, QualityModifiers = new List<RangedSellInQualityModifier>
                    {
                        new RangedSellInQualityModifier {SellInFrom = 1, SellInTo = int.MaxValue, Modifier = -1},
                        new RangedSellInQualityModifier {SellInFrom = 0, SellInTo = 0, Modifier = -2}
                    }
                },
                new SimpleAgingModifier {Name = "Brie Rule", ItemCategory = "", MaxQuality = 50, QualityModifier = new BaseQualityModifier {Modifier = 1}},
                new TieredQualityAgingRule {Name = "Conjured Item Rule", ItemCategory = "Conjured", ItemName = null, MaxQuality = 50, QualityModifiers = new List<RangedSellInQualityModifier>
                    {
                        new RangedSellInQualityModifier {SellInFrom = 1, SellInTo = int.MaxValue, Modifier = -2},
                        new RangedSellInQualityModifier {SellInFrom = 0, SellInTo = 0, Modifier = -4}
                    }
                },
                new LegendaryAgingRule {Name = "Sulfuras Rule", ItemCategory = "Sulfuras", ItemName = null, MaxQuality = 80},
                new TieredQualityAgingRule {Name = "Backstage Passes Rule", ItemCategory = "Backstage Passes", ItemName = null, MaxQuality = 50, QualityModifiers = new List<RangedSellInQualityModifier>
                {
                    new RangedSellInQualityModifier {SellInFrom = 11, SellInTo = int.MaxValue, Modifier = 1},
                    new RangedSellInQualityModifier {SellInFrom = 6, SellInTo = 10, Modifier = 2},
                    new RangedSellInQualityModifier {SellInFrom = 1, SellInTo = 5, Modifier = 3},
                    new RangedSellInQualityModifier {SellInFrom = 0, SellInTo = 0, Modifier = int.MinValue}
                }}
            };

            SetupAgingRules(agingRules);

            using (var dc = new DataContext())
            {
                var rules = dc.AgingRules.ToList();

                Assert.Equal(5, rules.Count);
            }
        }

        #region Private Helper Methods

        private void SetupAgingRules(List<BaseAgingRule> rules)
        {
            var agingFilePath = DataContextHelper.GetAgingRulesPath();
            if (File.Exists(agingFilePath))
            {
                File.Delete(agingFilePath);
            }

            if (rules != null)
            {
                DataContextHelper.SaveAgingRules(rules);
            }
        }

        #endregion
    }
}
