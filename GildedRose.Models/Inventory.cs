using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GildedRose.Models
{
    public class Inventory
    {
        public Inventory()
        {

        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InventoryId { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public DateTime CurrentDate { get; set; }
        public List<InventoryItem> InventoryItems { get; set; }
    }
}
