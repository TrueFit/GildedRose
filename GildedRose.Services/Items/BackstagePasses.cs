using System.ComponentModel.Design;
using GildedRose.Data.Interfaces;
using GildedRose.Model;

namespace GildedRose.Services.Items
{
    public class BackstagePasses : StandardBusinessObject
    {
        public BackstagePasses(IStoreItemRepository itemsRepository) : base(itemsRepository)
        {
        }
        public override void AdjustQuality(StoreItemDto item)
        {
            // Once the sell by date has passed, Quality degrades twice as fast
            if (item.SellIn == 0)
                item.Quality = 0;
            else if (item.SellIn < 6)
                item.Quality += 3;
            else if (item.SellIn < 11)
                item.Quality += 2;
            else
                item.Quality += 1;
        }

    }
}
