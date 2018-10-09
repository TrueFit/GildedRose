using System;
using guilded.rose.api.Domain.Models.Interfaces;

namespace guilded.rose.api.Domain.Models.Adjusters
{
    public class ConjuredAdjuster : AbstractQualityAdjuster, IQualityAdjuster
    {
        public ConjuredAdjuster(int min = 0, int max = 50, int increment = 1) : base(min, max, increment)
        {
            // 7) Conjured depreciates faster
            this.Increment = (increment * 2);
        }

        public override IItem CreateAdjustedItem(IItem item, DateTime date)
        {
            return base.CreateAdjustedItem(item, date);
        }
    }
}