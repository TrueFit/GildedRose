using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GildedRose.BusinessObjects;

namespace GildedRose.Helpers.ItemTypes
{
    public class Conjured : IItemType
    {
        public InventoryItem EndDay(InventoryItem i)
        {
            // "Conjured" items degrade in Quality twice as fast as normal items

            // Sellin is normal, subtract until it's zero
            i.Sellin = (i.Sellin - 1 > 0) ? i.Sellin - 1 : 0;

            var qualityFactor = 2;
            // Once the sell by date has passed, Quality degrades twice as fast 
            if (i.Sellin == 0) qualityFactor = 4;
            i.Quality = (i.Quality - qualityFactor > 0) ? i.Quality - qualityFactor : 0;
            return i;
        }
    }
}
