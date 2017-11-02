using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Repositories.EF
{
    public abstract class EfRepositoryBase<TDbContext>
        where TDbContext : System.Data.Entity.DbContext
    {
        #region Constructor and Dependencies
        public EfRepositoryBase(System.Data.Entity.Infrastructure.IDbContextFactory<TDbContext> dbFactory)
        {
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        private System.Data.Entity.Infrastructure.IDbContextFactory<TDbContext> _dbFactory;

        protected TDbContext NewDbContext() => _dbFactory.Create();
        #endregion

        #region QueryDb Overloads
        protected Task<T> QueryDbAsync<T>(TDbContext db, Func<TDbContext, Task<T>> query)
            => query(db ?? throw new ArgumentNullException(nameof(db)));

        protected Task QueryDbAsync(TDbContext db, Func<TDbContext, Task> query)
            => query(db ?? throw new ArgumentNullException(nameof(db)));

        protected async Task<T> QueryDbAsync<T>(Func<TDbContext, Task<T>> query)
        { using (var db = NewDbContext()) return await QueryDbAsync(db, query).ConfigureAwait(false); }

        protected async Task QueryDbAsync(Func<TDbContext, Task> query)
        { using (var db = NewDbContext()) await QueryDbAsync(db, query).ConfigureAwait(false); }
        #endregion
    }
}
