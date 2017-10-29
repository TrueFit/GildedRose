using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models
{
    public enum InventoryItemStatusId
    {
        Unknown = 0,
        Available,
        Sold,
        Expired,
        Discarded,
    }
}
