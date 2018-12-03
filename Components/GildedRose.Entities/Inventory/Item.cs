using System;
using GildedRose.Core.Contracts;

namespace GildedRose.Entities.Inventory
{
    public class Item : IAuditable
    {
        public Guid Identifier { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int ShelfLife { get; set; }

        public bool IsLegendary { get; set; }

        public DateTime StockDate { get; set; }

        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? Modified { get; set; }

        public int? ModifiedBy { get; set; }
    }
}
