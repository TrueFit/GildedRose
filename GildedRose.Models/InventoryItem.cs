using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GildedRose.Models
{
    public class InventoryItem
    {

        public InventoryItem()
        {

        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InventoryItemId { get; set; }
        public int InventoryId { get; set; }
        public string Name { get; set; }
        public DateTime PurchasedOn { get; set; }
        public int SellIn { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int QualityId { get; set; }
        public Quality Quality { get; set; }
    }
}
