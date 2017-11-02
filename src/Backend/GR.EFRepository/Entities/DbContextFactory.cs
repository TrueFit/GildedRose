using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Repositories.EF.Entities
{
    public class DbContextFactory<TDbContext> : System.Data.Entity.Infrastructure.IDbContextFactory<TDbContext>
        where TDbContext : System.Data.Entity.DbContext
    {
        public DbContextFactory(string connectionString, Func<string, TDbContext> creatorFunc)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));
            _connectionString = connectionString;
            _creatorFunc = creatorFunc ?? throw new ArgumentNullException(nameof(creatorFunc));
        }

        private string _connectionString;
        private Func<string, TDbContext> _creatorFunc;

        public TDbContext Create() => _creatorFunc(_connectionString);

    }
}
