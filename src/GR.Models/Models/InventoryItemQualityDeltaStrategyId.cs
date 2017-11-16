using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models
{
    public enum InventoryItemQualityDeltaStrategyId : byte
    { 
        Linear = 1,
        InverseLinear = 2,
        Static = 3,
        Event = 4,
    }
}
