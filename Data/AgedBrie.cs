using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GildedRose.Data
{
    public class AgedBrie : Item
    {
        public AgedBrie(string name, string category, int sellIn, int quality)
            : base(name, category, sellIn, quality) { }

        public override void UpdateItem()
        {
            LowerDaysLeft();
            IncreaseQuality();
            ApplyQualityLimit();
        }

        //"Aged Brie" actually increases in Quality the older it gets but still limited by the 50 quality limit
        private void IncreaseQuality()
        {
            Quality++;
        }
    }
}
