namespace GildedRose.Inventory.Tests.Domain;

public class InventoryItemTests
{
    [Fact]
    public void IventoryItem_EndDay_Reduces_SellInBy1()
    {
        var item = new InventoryItem(new QualityCalculatorFactoryHelper()) { SellIn = 5, Quality = 3 };

        item.EndDay();

        Assert.Equal(4, item.SellIn);
        Assert.Equal(2, item.Quality);
    }

    [Fact]
    public void IventoryItem_EndDay_Reduces_SellInBy1_BecomesNegative()
    {
        var item = new InventoryItem(new QualityCalculatorFactoryHelper()) { SellIn = 0, Quality = 7 };

        item.EndDay();

        Assert.Equal(-1, item.SellIn);
        Assert.Equal(5, item.Quality);
    }

    [Fact]
    public void IventoryItem_GetDetails_ReturnsCSV()
    {
        var item = new InventoryItem(new QualityCalculatorFactoryHelper()) { Category = "my stuff", Name = "jimmies", Quality = 3, SellIn = -5 };

        var details = item.ToString();

        Assert.Equal("jimmies,my stuff,-5,3", details);
    }
}
