using System;
using System.Collections.Generic;
using System.Text;
using GildedRose.Data.Interfaces;
using GildedRose.Model;

namespace GildedRose.Data.Repos
{
    class StoreItemRepositoryBase : RepositoryBase<IStoreItem>
    {
        private readonly IStoreContext _dbContext;

        public StoreItemRepositoryBase(StoreContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
