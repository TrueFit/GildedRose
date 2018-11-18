using GildedRose.Models;
using System.Collections.Generic;

namespace GildedRose.Persistence.Database
{
    public interface IDatabase<T>
    {
        T GetEntity(int id);

#warning In a production system I'd likely use a consistent dynamic filter object that would be used for the "Get Entities" call
        List<T> GetEntities(int? id);

        void DeleteEntity(int id);

        void AddEntity(T item);

        void UpdateEntity(T item);

        int GetCount();

    }
}
