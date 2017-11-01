using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models
{
    public class InventoryItemType
    {
        public short InventoryItemTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public InventoryItemQualityDeltaStrategyId QualityDeltaStrategyId { get; set; }
        public double BaseDelta { get; set; }
        public double MinQuality { get; set; }
        public double MaxQuality { get; set; }
    }
}
