using System;
using guilded.rose.api.Domain.Models.Interfaces;

namespace guilded.rose.api.Domain.Models.Adjusters
{
    public class BackstageAdjuster : AbstractQualityAdjuster, IQualityAdjuster
    {
        public BackstageAdjuster(int min = 0, int max = 50, int increment = 1) : base(min, max, increment)
        {

        }

        public override IItem CreateAdjustedItem(IItem item, DateTime date)
        {
            // confused by this part of 6)  but Quality drops to 0 after the concert ??? what concert?
            var days = Math.Abs((item.DateCreated - date).Days);

            var adjustedDated = item.DateCreated.AddDays(days).Date;

            var daysLeft = (adjustedDated - date).Days;

            // 6) Ages like fine wine
            Increment = (daysLeft <= 5) ? 3 : (daysLeft <= 10) ? 2 : Increment;

            // 3 & 4) Quality appreciates & can't be better than max 
            item.Quality = item.Quality + (days * Increment) > MaximumQuality ? MaximumQuality : item.Quality + (days * Increment);

            return item;
        }
    }
}