using System;
using GuildedRose.Inventory.InventoryItems;
using Xunit;

namespace GuildedRose.Inventory.Test
{
    public class InventoryItemFactoryUnitTest
    {
        public static TheoryData<InventoryItem, Type> FactoryCreatesCorrectTypeData = new TheoryData<InventoryItem, Type>
        {
            { InventoryItemFactory.Factory(new InventoryItem("Sword", "Weapon", 30, 50)), typeof(InventoryItem) },
            { InventoryItemFactory.Factory(new InventoryItem("Aged Brie", "Food", 50, 10)), typeof(AgedBrieInventoryItem) },
            { InventoryItemFactory.Factory(new InventoryItem("Hand of Ragnaros", "Sulfuras", 80, 80)), typeof(SulfurasInventoryItem) },
            { InventoryItemFactory.Factory(new InventoryItem("I am Murloc", "Backstage Passes", 20, 10)), typeof(BackStagePassesInventoryItem) },
            { InventoryItemFactory.Factory(new InventoryItem("Giant Slayer", "Conjured", 15, 50)), typeof(ConjuredInventoryItem) }
        };

        [Theory]
        [MemberData(nameof(FactoryCreatesCorrectTypeData))]
        public void FactoryCreatesCorrectType(InventoryItem inventoryItem, Type expectedType)
        {
            var factoryItem = InventoryItemFactory.Factory(inventoryItem);
            Assert.Equal(expectedType, factoryItem.GetType());
        }
    }
}
