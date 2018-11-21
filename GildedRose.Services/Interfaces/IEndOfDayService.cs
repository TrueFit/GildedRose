using System;
using System.Collections.Generic;
using System.Text;
using GildedRose.Model;

namespace GildedRose.Services.Interfaces
{
    public interface IEndOfDayService
    {
        List<StoreItemDto> GetListofTrash();
        void ProcessEndOfDay();
        void SaveChanges();

    }
}
