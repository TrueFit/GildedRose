using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Repositories
{
    public interface IConfigRepository
    {
        Models.Config GetConfiguration();
        Task SetSimulationDateOffset(TimeSpan offset);
    }
}
