using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.ProcessResults
{
    public abstract class ProcessResultBase : RequestResult
    {
        public DateTime ProcessStartTime { get; set; }
        public TimeSpan ProcessDuration { get; set; }
        public int NumberOfRecordsAffected { get; set; }
    }
}
