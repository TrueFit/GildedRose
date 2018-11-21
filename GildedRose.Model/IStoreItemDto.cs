namespace GildedRose.Model
{
    public interface IStoreItemDto
    {
        string Name { get; set; }
        string Category { get; set; }
        int SellIn { get; set; }
        int Quality { get; set; }
    }
}
