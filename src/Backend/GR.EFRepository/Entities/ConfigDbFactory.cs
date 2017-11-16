using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Repositories.EF.Entities
{
    public class ConfigDbFactory : DbContextFactory<InventoryDb>
    {
        public ConfigDbFactory(string connectionString)
            : base(connectionString, cs => new InventoryDb(cs))
        { }
    }
}
