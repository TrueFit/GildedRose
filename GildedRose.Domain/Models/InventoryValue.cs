using System;
using System.Collections.Generic;
using System.Text;

namespace GildedRose.Domain.Models
{
    public class InventoryValue
    {
        public InventoryValue()
        {
        }

        public int InventoryId { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public DateTime CurrentDate { get; set; }
        public List<InventoryItemValue> InventoryItems { get; set; }
    }
}
