using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GildedRose.BusinessObjects;

namespace GildedRose.Helpers.ItemTypes
{
    public class BackstagePasses : IItemType
    {
        public InventoryItem EndDay(InventoryItem i)
        {
            // "Backstage passes", like aged brie, increases in Quality as it's SellIn value approaches; 
            // Quality increases by 2 when there are 10 days or less 
            // and by 3 when there are 5 days or less 
            // but Quality drops to 0 after the concert

            // Sellin is normal, subtract until it's zero
            i.Sellin = (i.Sellin - 1 > 0) ? i.Sellin - 1 : 0;

            // if the concert has passed, set quality to zero and return
            if (i.Sellin == 0)
            {
                i.Quality = 0;
                return i;
            }

            var qualityFactor = 1;
            // Deal with the different Sellin cases
            if (i.Sellin > 5 && i.Sellin <= 10) qualityFactor = 2;
            if (i.Sellin > 0 && i.Sellin <= 5) qualityFactor = 3;
            i.Quality = (i.Quality + qualityFactor < 50) ? i.Quality + qualityFactor : 50;
            
            return i;
        }
    }
}
