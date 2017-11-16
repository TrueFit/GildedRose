using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Repositories.EF.Entities
{
    public class InventoryDbFactory : DbContextFactory<InventoryDb>
    {
        public InventoryDbFactory(string connectionString)
            : base(connectionString, cs => new InventoryDb(cs))
        { }
    }
}
