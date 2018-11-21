using GildedRose.Model;
using Microsoft.EntityFrameworkCore;

namespace GildedRose.Data.Interfaces
{
    public interface IStoreContext
    {
        DbSet<StoreItemDto> StoreItems { get; set; }

        void Commit();
    }
}