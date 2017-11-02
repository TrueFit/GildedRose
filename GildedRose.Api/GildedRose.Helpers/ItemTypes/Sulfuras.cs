using System;
using GildedRose.BusinessObjects;

namespace GildedRose.Helpers.ItemTypes
{
    public class Sulfuras : IItemType
    {
        public InventoryItem EndDay(InventoryItem i)
        {
            // "Sulfuras", being a legendary item, never has to be sold or decreases in Quality
            
            // set Sellin and Quality to 80, just in case some other process messes with it
            i.Sellin = (i.Sellin != 80) ? 80 : i.Sellin;
            i.Quality = (i.Quality != 80) ? 80 : i.Quality;
            return i;
        }
    }
}
