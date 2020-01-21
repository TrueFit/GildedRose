using System;
using System.ComponentModel.DataAnnotations;

namespace GildedRose.Entities.Inventory
{
    /// <summary>
    /// Represents an item in inventory
    /// </summary>
    public class Inventory
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public int SellIn { get; set; }

        [Required]
        public int Quality { get; set; }

        [Required]
        public DateTime InventoryAddedDate { get; set; }
    }
}
