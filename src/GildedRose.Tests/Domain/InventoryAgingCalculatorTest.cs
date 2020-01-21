using System;
using System.Collections.Generic;
using Autofac;
using GildedRose.Data;
using GildedRose.Domain.Inventory;
using GildedRose.Entities.Inventory;
using GildedRose.Entities.Inventory.Aging;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace GildedRose.Tests.Domain
{
    public class InventoryAgingCalculatorTest : IClassFixture<BaseTestFixture>
    {
        private BaseTestFixture _fixture;

        public InventoryAgingCalculatorTest(BaseTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory,
         InlineData("+5 Dexterity Vest", "Armor", 10, 20, 9, 19),         // "Normal item"      standard Quality depreciation by 1
         InlineData("+5 Dexterity Vest", "Armor", 1, 8, 0, 6),            //                    starting SellIn of 1 changes to 0 and Quality depreciates by 2
         InlineData("+5 Dexterity Vest", "Armor", 0, 1, 0, 0),            //                    min SellIn and Quality are 0
         InlineData("Belt of Giant Strength", "Conjured", 20, 40, 19, 38),// "Conjured item"    standard Quality depreciation by 2
         InlineData("Belt of Giant Strength", "Conjured", 1, 40, 0, 36),  //                    starting SellIn of 1 changes to 0 and Quality depreciates by 4
         InlineData("Belt of Giant Strength", "Conjured", 1, 3, 0, 0),    //                    min SellIn and Quality are 0
         InlineData("I am Murloc", "Backstage Passes", 20, 10, 19, 11),   // "Backstage passes" SellIn > 10 days Quality appreciates by 1
         InlineData("I am Murloc", "Backstage Passes", 11, 10, 10, 12),   //                    starting SellIn = 11 changes to 10 and Quality appreciates by 2
         InlineData("I am Murloc", "Backstage Passes", 6, 10, 5, 13),     //                    starting SellIn = 6 changes to 5 and Quality appreciates by 3
         InlineData("I am Murloc", "Backstage Passes", 1, 10, 0, 0),      //                    starting SellInn = 1 changes to 0 and Quality depreciates to 0
         InlineData("I am Murloc", "Backstage Passes", 0, 0, 0, 0),       //                    min SellIn is 0
         InlineData("I am Murloc", "Backstage Passes", 10, 50, 9, 50),    //                    max Quality is 50
         InlineData("Aged Brie", "Food", 50, 10, 49, 11),                 // "Aged Brie"        standard Quality appreciation by 1
         InlineData("Aged Brie", "Food", 50, 50, 49, 50),                 //                    max Quality is 50
         InlineData("Hand of Ragnaros", "Sulfuras", 80, 80, 80, 80),      // "Sulfuras item"    stays at the same SellIn and Quality
         InlineData("Hand of Ragnaros", "Sulfuras", 0, 80, 0, 80),        //                    stays at the same SellIn and Quality
         InlineData("Cheese", "Food", 25, 1, 0, 0)                        // Any item           Quality goes to 0 then SellIn is changed to 0
        ]
        public void AgeInventory_NormalItem_SetsCorrectQuality(string name, string category, int startingSellIn, int startingQuality, int expectedSellIn, int expectedQuality)
        {
            SetupAgingRules(false);

            using (var scope = _fixture.ServiceProvider.CreateScope())
            {
                var calc = (InventoryAgingCalculator)scope.ServiceProvider.GetService(typeof(InventoryAgingCalculator));

                var item = new Inventory {Name = name, Category = category, SellIn = startingSellIn, Quality = startingQuality};
                calc.AgeInventory(item);

                Assert.Equal(expectedSellIn, item.SellIn);
                Assert.Equal(expectedQuality, item.Quality);
            }
        }

        [Fact]
        public void AgeInventory_AgingRuleNotFound_ThrowsException()
        {
            SetupAgingRules(true);

            using (var scope = _fixture.ServiceProvider.CreateScope())
            {
                var calc = (InventoryAgingCalculator)scope.ServiceProvider.GetService(typeof(InventoryAgingCalculator));

                var item = new Inventory {Name = "An Item", Category = "Random Item", SellIn = 7, Quality = 25};

                Assert.Throws<ApplicationException>(() => calc.AgeInventory(item));
            }
        }

        #region Private Helpers

        private void SetupAgingRules(bool removeDefaultRule)
        {
            var agingRules = GetAgingRules();
            if (removeDefaultRule)
            {
                agingRules.RemoveAt(0);
            }

            var dataContext = new Mock<IDataContext>(MockBehavior.Loose);
            dataContext.SetupGet(p => p.AgingRules).Returns(agingRules);
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterInstance(dataContext.Object).As<IDataContext>();
            containerBuilder.Update(_fixture.Container);
        }

        private List<BaseAgingRule> GetAgingRules()
        {
            return new List<BaseAgingRule>
            {
                new TieredQualityAgingRule {Name = "Normal Item Rule", ItemCategory = null, ItemName = null, MaxQuality = 50, QualityModifiers = new List<RangedSellInQualityModifier>
                    {
                        new RangedSellInQualityModifier {SellInFrom = 1, SellInTo = int.MaxValue, Modifier = -1},
                        new RangedSellInQualityModifier {SellInFrom = 0, SellInTo = 0, Modifier = -2}
                    }
                },
                new SimpleAgingModifier {Name = "Brie Rule", ItemCategory = "Food", ItemName = "Aged Brie", MaxQuality = 50, QualityModifier = new BaseQualityModifier {Modifier = 1}},
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
        }

        #endregion
    }
}
