namespace GildedRose.Inventory.Domain;

class InventoryItem : IInventoryItem
{
    private readonly ICalculateQualityFactory _qualityFactory;

    public InventoryItem(ICalculateQualityFactory qualityFactory)
    {
        _qualityFactory = qualityFactory;
    }

    public string Category { get; set; }
    public string Name { get; set; }
    public int SellIn { get; set; }
    public int Quality { get; set; }

    public void EndDay()
    {
        SellIn -= 1;

        var calculator = _qualityFactory.Create(this);
        Quality = calculator.GetQuality(this);
    }

    public override string ToString()
    {
        return String.Join(",", new string[] { Name, Category, SellIn.ToString(), Quality.ToString() });
    }
}
