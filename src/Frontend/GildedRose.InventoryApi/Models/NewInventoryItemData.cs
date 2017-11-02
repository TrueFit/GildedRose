using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GildedRose.InventoryApi.Models
{
    public class NewInventoryItemData
    {
        public short ItemTypeId { get; set; }
        public string Name{ get; set; }
        public string Description{ get; set; }
        public double Quality{ get; set; }
        public DateTime? SellByDate { get; set; }
    }
}
