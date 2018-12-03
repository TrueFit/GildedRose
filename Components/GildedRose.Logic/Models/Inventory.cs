using System;

namespace GildedRose.Logic.Models
{
    public class Inventory
    {
        public Guid Identifier { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int InitialQuality { get; set; }

        public int CurrentQuality { get; set; }

        public int MaxQuality { get; set; }

        public int SellIn { get; set; }

        public bool IsLegendary { get; set; }

        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? Modified { get; set; }

        public int? ModifiedBy { get; set; }
    }
}
