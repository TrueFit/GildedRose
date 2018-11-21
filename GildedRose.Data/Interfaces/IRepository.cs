using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GildedRose.Model;

namespace GildedRose.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Delete(Expression<Func<T, bool>> where);
        void Delete(T entity);
        T Get(Expression<Func<T, bool>> where);
        IEnumerable<T> GetAll();
        T GetById(int id);
        T GetByName(string name);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        void Save();
        void Update(T entity);
    }
}
