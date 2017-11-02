using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.ProcessResults
{
    public class NightlyQualityUpdateResult : ProcessResultBase
    {
        public DateTime CurrentDate { get; set; }
        public int NumberOfExpiredRecords { get; set; }
    }
}
