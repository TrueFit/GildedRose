using GuildedRose.Inventory.InventoryItems;
using Xunit;

namespace GuildedRose.Inventory.Test
{
    public class InventoryItemsUnitTest
    {
        public static TheoryData<InventoryItem, int> QualityDegradesTwiceAsFastAfterSellByData = new TheoryData<InventoryItem, int>
        {
            { InventoryItemFactory.Factory(new InventoryItem("Sword", "Weapon", 1, 10)), -2 },
            { InventoryItemFactory.Factory(new InventoryItem("Sword", "Weapon", 0, 10)), -2 },
            { InventoryItemFactory.Factory(new InventoryItem("Giant Slayer", "Conjured", 1, 10)), -4 },
            { InventoryItemFactory.Factory(new InventoryItem("Giant Slayer", "Conjured", 0, 10)), -4 }
        };

        [Theory]
        [MemberData(nameof(QualityDegradesTwiceAsFastAfterSellByData))]
        public void QualityDegradesTwiceAsFastAfterSellBy(InventoryItem inventoryItem, int expectedQualityChange)
        {
            int originalQuality = inventoryItem.Quality;
            inventoryItem.IncrementDay();
            Assert.Equal(originalQuality + expectedQualityChange, inventoryItem.Quality);
        }


        // Intentionally left out Sulfuras as Sellin does not change
        public static TheoryData<InventoryItem> SellinDecreasesByOneData = new TheoryData<InventoryItem>
        {
            { InventoryItemFactory.Factory(new InventoryItem("Sword", "Weapon", 10, 10)) },
            { InventoryItemFactory.Factory(new InventoryItem("Aged Milk", "Food", 3, 20)) },
            { InventoryItemFactory.Factory(new InventoryItem("Aged Brie", "Food", 9, 5)) },
            { InventoryItemFactory.Factory(new InventoryItem("I am Murloc", "Backstage Passes", 2, 50)) }
        };

        [Theory]
        [MemberData(nameof(SellinDecreasesByOneData))]
        public void SellinDecreasesByOne(InventoryItem inventoryItem)
        {
            int originalSellin = inventoryItem.SellIn;
            inventoryItem.IncrementDay();
            Assert.Equal(originalSellin - 1, inventoryItem.SellIn);
        }


        // Intentionally left out Sulfuras as Quality does not change
        // Intentionally left out BackstagePasses, Aged Brie, and Conjured as Quality has special rules
        public static TheoryData<InventoryItem> StandardItemQualityDecreasesByOneData = new TheoryData<InventoryItem>
        {
            { InventoryItemFactory.Factory(new InventoryItem("Sword", "Weapon", 10, 10)) },
            { InventoryItemFactory.Factory(new InventoryItem("Aged Milk", "Food", 3, 20)) }
        };

        [Theory]
        [MemberData(nameof(StandardItemQualityDecreasesByOneData))]
        public void StandardItemQualityDecreasesByOne(InventoryItem inventoryItem)
        {
            int originalQuality = inventoryItem.Quality;
            inventoryItem.IncrementDay();
            Assert.Equal(originalQuality - 1, inventoryItem.Quality);
        }


        public static TheoryData<InventoryItem> QualityIsNeverNegativeData = new TheoryData<InventoryItem>
        {
            { InventoryItemFactory.Factory(new InventoryItem("Sword", "Weapon", 10, 10)) },
            { InventoryItemFactory.Factory(new InventoryItem("Aged Milk", "Food", 3, 20)) },
            { InventoryItemFactory.Factory(new InventoryItem("Aged Brie", "Food", 9, 5)) },
            { InventoryItemFactory.Factory(new InventoryItem("I am Murloc", "Backstage Passes", 2, 50)) },
            { InventoryItemFactory.Factory(new InventoryItem("Hand of Ragnaros", "Sulfuras", 80, 80)) },
            { InventoryItemFactory.Factory(new InventoryItem("Elixir of the Mongoose", "Potion", 5, 7)) },
        };

        [Theory]
        [MemberData(nameof(QualityIsNeverNegativeData))]
        public void QualityIsNeverNegative(InventoryItem inventoryItem)
        {
            for (int i = 0; i < inventoryItem.Quality * 2; i++)
            {
                inventoryItem.IncrementDay();
            }
            Assert.True(inventoryItem.Quality >= 0);
        }


        public static TheoryData<InventoryItem> AgedBrieIncreasesInQualityData = new TheoryData<InventoryItem>
        {
            { InventoryItemFactory.Factory(new InventoryItem("Aged Brie", "Food", 9, 5)) }
        };

        [Theory]
        [MemberData(nameof(AgedBrieIncreasesInQualityData))]
        public void AgedBrieIncreasesInQuality(InventoryItem inventoryItem)
        {
            int originalQuality = inventoryItem.Quality;
            inventoryItem.IncrementDay();
            Assert.Equal(originalQuality + 1, inventoryItem.Quality);
        }


        // Intentionally left out Sulfuras as Quality can be greater than 50
        public static TheoryData<InventoryItem> QualityIsNeverMoreThanFiftyData = new TheoryData<InventoryItem>
        {
            { InventoryItemFactory.Factory(new InventoryItem("Sword", "Weapon", 10, 60)) },
            { InventoryItemFactory.Factory(new InventoryItem("Aged Milk", "Food", 3, 50)) },
            { InventoryItemFactory.Factory(new InventoryItem("Aged Brie", "Food", 9, 49)) },
            { InventoryItemFactory.Factory(new InventoryItem("Aged Brie", "Food", 9, 60)) },
            { InventoryItemFactory.Factory(new InventoryItem("I am Murloc", "Backstage Passes", 2, 50)) }
        };

        [Theory]
        [MemberData(nameof(QualityIsNeverMoreThanFiftyData))]
        public void QualityIsNeverMoreThanFifty(InventoryItem inventoryItem)
        {
            for (int i = 0; i < 10; i++)
            {
                inventoryItem.IncrementDay();
            }
            Assert.True(inventoryItem.Quality <= 50);
        }


        public static TheoryData<InventoryItem> SulfurasSellinDoesntChangeData = new TheoryData<InventoryItem>
        {
            { InventoryItemFactory.Factory(new InventoryItem("Hand of Ragnaros", "Sulfuras", 80, 80)) },
            { InventoryItemFactory.Factory(new InventoryItem("Hand of Ragnaros", "Sulfuras", 90, 80)) },
            { InventoryItemFactory.Factory(new InventoryItem("Hand of Ragnaros", "Sulfuras", 70, 80)) }
        };

        [Theory]
        [MemberData(nameof(SulfurasSellinDoesntChangeData))]
        public void SulfurasSellinDoesntChange(InventoryItem inventoryItem)
        {
            int originalSellIn = inventoryItem.SellIn;
            inventoryItem.IncrementDay();
            Assert.Equal(originalSellIn, inventoryItem.SellIn);
        }


        public static TheoryData<InventoryItem> SulfurasQualityAlwaysEightyData = new TheoryData<InventoryItem>
        {
            { InventoryItemFactory.Factory(new InventoryItem("Hand of Ragnaros", "Sulfuras", 80, 80)) },
            { InventoryItemFactory.Factory(new InventoryItem("Hand of Ragnaros", "Sulfuras", 90, 90)) },
            { InventoryItemFactory.Factory(new InventoryItem("Hand of Ragnaros", "Sulfuras", 70, 70)) }
        };

        [Theory]
        [MemberData(nameof(SulfurasQualityAlwaysEightyData))]
        public void SulfurasQualityIsAlwaysEightyAfterIncrementDay(InventoryItem inventoryItem)
        {
            inventoryItem.IncrementDay();
            Assert.Equal(80, inventoryItem.Quality);
        }

        [Theory]
        [MemberData(nameof(SulfurasQualityAlwaysEightyData))]
        public void SulfurasQualityIsAlwaysEightyBeforeIncrementDay(InventoryItem inventoryItem)
        {
            Assert.Equal(80, inventoryItem.Quality);
        }


        public static TheoryData<InventoryItem, int> BackstagePassesQualityChangeData = new TheoryData<InventoryItem, int>
        {
            { InventoryItemFactory.Factory(new InventoryItem("I am Murloc", "Backstage Passes", 12, 20)), 1 },
            { InventoryItemFactory.Factory(new InventoryItem("I am Murloc", "Backstage Passes", 11, 20)), 2 },
            { InventoryItemFactory.Factory(new InventoryItem("I am Murloc", "Backstage Passes", 10, 20)), 2 },
            { InventoryItemFactory.Factory(new InventoryItem("I am Murloc", "Backstage Passes", 9, 20)), 2 },
            { InventoryItemFactory.Factory(new InventoryItem("I am Murloc", "Backstage Passes", 8, 20)), 2 },
            { InventoryItemFactory.Factory(new InventoryItem("I am Murloc", "Backstage Passes", 7, 20)), 2 },
            { InventoryItemFactory.Factory(new InventoryItem("I am Murloc", "Backstage Passes", 6, 20)), 3 },
            { InventoryItemFactory.Factory(new InventoryItem("I am Murloc", "Backstage Passes", 5, 20)), 3 },
            { InventoryItemFactory.Factory(new InventoryItem("I am Murloc", "Backstage Passes", 4, 20)), 3 },
            { InventoryItemFactory.Factory(new InventoryItem("I am Murloc", "Backstage Passes", 3, 20)), 3 },
            { InventoryItemFactory.Factory(new InventoryItem("I am Murloc", "Backstage Passes", 2, 20)), 3 },
            { InventoryItemFactory.Factory(new InventoryItem("I am Murloc", "Backstage Passes", 1, 20)), -20 },
            { InventoryItemFactory.Factory(new InventoryItem("I am Murloc", "Backstage Passes", 0, 20)), -20 }
        };

        [Theory]
        [MemberData(nameof(BackstagePassesQualityChangeData))]
        public void BackstagePassesQualityChange(InventoryItem inventoryItem, int expectedQualityChange)
        {
            int originalQuality = inventoryItem.Quality;
            inventoryItem.IncrementDay();
            Assert.Equal(originalQuality + expectedQualityChange, inventoryItem.Quality);
        }


        public static TheoryData<InventoryItem, int> ConjuredDegradeTwiceAsFastData = new TheoryData<InventoryItem, int>
        {
            { InventoryItemFactory.Factory(new InventoryItem("Giant Slayer", "Conjured", 10, 10)), -2 },
            { InventoryItemFactory.Factory(new InventoryItem("Giant Slayer", "Conjured", 2, 10)), -2 },
            { InventoryItemFactory.Factory(new InventoryItem("Giant Slayer", "Conjured", 1, 10)), -4 },
            { InventoryItemFactory.Factory(new InventoryItem("Giant Slayer", "Conjured", 0, 10)), -4 }
        };

        [Theory]
        [MemberData(nameof(ConjuredDegradeTwiceAsFastData))]
        public void ConjuredDegradeTwiceAsFast(InventoryItem inventoryItem, int expectedQualityChange)
        {
            int originalQuality = inventoryItem.Quality;
            inventoryItem.IncrementDay();
            Assert.Equal(originalQuality + expectedQualityChange, inventoryItem.Quality);
        }
    }
}
