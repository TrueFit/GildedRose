using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models
{
    public class InventoryItem
    {
        public int InventoryItemId { get; set; }

        public InventoryItemType ItemType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public InventoryItemStatusId ItemStatusId { get; set; }

        public DateTime InventoryDate { get; set; }
        public DateTime? SellByDate { get; set; }
        public DateTime? SaleDate { get; set; }
        public DateTime? DiscardDate { get; set; }

        public double InitialQuality { get; set; }
        public double CurrentQuality { get; set; }
    }
}
