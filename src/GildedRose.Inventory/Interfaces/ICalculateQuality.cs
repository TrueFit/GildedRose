namespace GildedRose.Inventory.Interfaces;

interface ICalculateQuality
{
    int GetQuality(IInventoryItem item);
    bool CanCalculateItem(IInventoryItem item);
}
