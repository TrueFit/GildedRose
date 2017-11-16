using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.Projections
{
    public class InventoryItemSearchResults
    {
        public int TotalItems { get; set; }
        public List<InventoryItem> Items { get; set; }
    }
}
