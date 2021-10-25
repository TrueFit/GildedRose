using GildedRose.Inventory.Interfaces;

namespace GildedRose.Inventory.Tests.Domain;

public class InventoryListTests
{
    private List<IInventoryItem> DefaultItems => new List<IInventoryItem>
    {
        new InventoryItem(new QualityCalculatorFactoryHelper())
        {
            Name = "item 1",
            Category = "swords",
            SellIn = 4,
            Quality = 0
        },
        new InventoryItem(new QualityCalculatorFactoryHelper())
        {
            Name = "item 2",
            Category = "swords",
            SellIn = 45,
            Quality = 27
        },
        new InventoryItem(new QualityCalculatorFactoryHelper())
        {
            Name = "item 3",
            Category = "bread",
            SellIn = 2,
            Quality = 1
        },
        new InventoryItem(new QualityCalculatorFactoryHelper())
        {
            Name = "item 4",
            Category = "bread",
            SellIn = 3,
            Quality = 3
        }
    };

    [Fact]
    public void InventoryList_GetsDetails()
    {
        var list = new InventoryList(new QualityCalculatorFactoryHelper(), DefaultItems);

        var details = list.GetItem("item 2");

        Assert.Equal("item 2,swords,45,27", details);
    }

    [Fact]
    public void InventoryList_RemoveTrash_Returns0QualityItems()
    {
        var list = new InventoryList(new QualityCalculatorFactoryHelper(), DefaultItems);

        var trash = list.RemoveTrash();

        Assert.Equal("item 1,swords,4,0", trash);
        Assert.Equal("The item item 1 was not found", list.GetItem("item 1"));
    }

    [Fact]
    public void InventoryList_ReturnsAccurateInventoryLists()
    {
        var list = new InventoryList(new QualityCalculatorFactoryHelper(), DefaultItems);

        var fullList = list.ToString();

        Assert.Equal("item 1,swords,4,0" + Environment.NewLine +
            "item 2,swords,45,27" + Environment.NewLine +
            "item 3,bread,2,1" + Environment.NewLine +
            "item 4,bread,3,3", fullList);
    }

    [Fact]
    public void InventoryList_ReturnsAccurateInventoryLists_NextDay()
    {
        var list = new InventoryList(new QualityCalculatorFactoryHelper(), DefaultItems);

        list.EndDay();

        var fullList = list.ToString();

        Assert.Equal("item 1,swords,3,0" + Environment.NewLine +
            "item 2,swords,44,26" + Environment.NewLine +
            "item 3,bread,1,0" + Environment.NewLine +
            "item 4,bread,2,2", fullList);
    }
}
