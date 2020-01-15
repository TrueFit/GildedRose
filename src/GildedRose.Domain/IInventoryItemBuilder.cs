using System;

namespace GildedRose.Domain
{
    public interface IInventoryItemBuilder
    {
        IInventoryItem Build(Guid id, string name, string category, int quality, int sellIn);
    }
}