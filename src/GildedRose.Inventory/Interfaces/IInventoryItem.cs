namespace GildedRose.Inventory.Interfaces;

interface IInventoryItem
{
    public string Category { get; set; }
    public string Name { get; set; }
    public int SellIn { get; set; }
    public int Quality { get; set;  }
    void EndDay();
    public string ToString();   
}
