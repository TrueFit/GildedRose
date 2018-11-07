using GildedRose.Core.Contracts;
using System;

namespace GildedRose.Entities.Inventory
{
    public class Item : IAuditable
    {
        public Guid Identifier { get; set; }

        public string Name { get; set; }

        public int Category { get; set; }

        public int ShelfLife { get; set; }

        public int MaxQuality { get; set; }

        public bool IsLegendary { get; set; }

        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? Modified { get; set; }

        public int? ModifiedBy { get; set; }
    }
}
