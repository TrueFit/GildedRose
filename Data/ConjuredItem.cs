using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GildedRose.Data
{
    public class ConjuredItem : Item
    {
        public ConjuredItem(string name, string category, int sellIn, int quality)
            : base(name, category, sellIn, quality) { }

        public override void UpdateItem()
        {
            LowerDaysLeft();

            LowerQuality();

            ApplyQualityLimit();
        }
        protected override void LowerQuality()
        {
            // ... Conjured items Quality degrades twice as fast
            Quality -= 2;
        }
    }
}
