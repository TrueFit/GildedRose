using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GildedRose.Data.Interfaces;
using GildedRose.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace GildedRose.Data.Repos
{
    public class StoreItemRepository : RepositoryBase<StoreItemDto>, IStoreItemRepository
    { 
        private readonly StoreContext _dbContext;
 
        public StoreItemRepository(StoreContext dbContext) :base(dbContext)
        {
            _dbContext = dbContext;
        }

        #region Implementation
        public IEnumerable<StoreItemDto> GetListofTrash()
        {
            return _dbContext.StoreItems.Where(x => x.Quality == 0);
        }
        #endregion

        public StoreItemDto GetById(int id)
        {
            return _dbContext.StoreItems.Single(x => x.Id == id);
        }

        public StoreItemDto GetByName(string name)
        {
            return _dbContext.StoreItems.SingleOrDefault(x => x.Name == name);
        }
    }
}
