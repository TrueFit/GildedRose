namespace GildedRose.Inventory.Interfaces;

interface ICalculateQualityFactory
{
    ICalculateQuality Create(IInventoryItem item);
}
