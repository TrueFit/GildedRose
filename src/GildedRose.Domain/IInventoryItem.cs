using System;

namespace GildedRose.Domain
{
    public interface IInventoryItem
    {
        string Category { get; }

        Guid Id { get; }

        string Name { get; }

        int Quality { get; }

        int? SellIn { get; }

        void OnAdvanceToNextDay();
    }
}