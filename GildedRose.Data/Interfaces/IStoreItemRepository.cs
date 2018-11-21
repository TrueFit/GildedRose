using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GildedRose.Model;

namespace GildedRose.Data.Interfaces
{
    public interface IStoreItemRepository:IRepository<StoreItemDto>
    {
        IEnumerable<StoreItemDto> GetListofTrash();
    }
}