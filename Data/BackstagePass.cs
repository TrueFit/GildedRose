using System;

namespace GildedRose.Data
{
    public class BackstagePass : Item
    {
        public BackstagePass(string name, string category, int sellIn, int quality)
            : base(name, category, sellIn, quality) { }

        public override void UpdateItem()
        {
            LowerDaysLeft();

            // assume that you can no longer sell on Day 0
            if (SellIn < 1)
            {
                // Quality drops to 0 after the concert
                Quality = 0;
            }
            else if (SellIn <= 5)
            // and by 3 when there are 5 days or less
            {
                Quality += 3;
            }

            else if (SellIn <= 10)
            //Quality increases by 2 when there are 10 days or less
            {
                Quality += 2;
            }
            else
            //Backstage passes increase in Quality as it's SellIn value approaches; 
            {
                Quality++;
            }

            ApplyQualityLimit();
        }
    }
}
