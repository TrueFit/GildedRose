using System;
using guilded.rose.api.Domain.Models.Interfaces;

namespace guilded.rose.api.Domain.Models.Adjusters
{
    public class DefaultAdjuster : AbstractQualityAdjuster
    {
        public DefaultAdjuster(int min = 0, int max = 50, int increment = 1) : base(min, max, increment)
        {
            
        }

        public override IItem CreateAdjustedItem(IItem item, DateTime date)
        {
            return base.CreateAdjustedItem(item, date);
        }
    }
}