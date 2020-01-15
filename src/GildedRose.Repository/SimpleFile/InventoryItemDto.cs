using System;

namespace GildedRose.Repository.SimpleFile
{
    public sealed class InventoryItemDto
    {
        public string Category { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Quality { get; set; }

        public int? SellIn { get; set; }
    }
}