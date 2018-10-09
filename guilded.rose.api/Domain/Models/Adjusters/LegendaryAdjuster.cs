using System;
using guilded.rose.api.Domain.Models.Interfaces;

namespace guilded.rose.api.Domain.Models.Adjusters
{
    public class LegendaryAdjuster : AbstractQualityAdjuster, IQualityAdjuster
    {
        public LegendaryAdjuster(int min = 0, int max = 50, int increment = 1) : base(min, max, increment)
        {

        }

        public override IItem CreateAdjustedItem(IItem item, DateTime date)
        {
            // 5) Legendary never expires or depreciates
            return item;
        }
    }
}