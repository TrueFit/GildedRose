using System;
using guilded.rose.api.Domain.Models.Interfaces;

namespace guilded.rose.api.Domain.Models
{
    public abstract class AbstractQualityAdjuster : IQualityAdjuster
    {
        internal int MinimumQuality = 0;
        internal int MaximumQuality = 50;
        internal int Increment = 1;

        public AbstractQualityAdjuster(int min = 0, int max = 50, int increment = 1)
        {
            MinimumQuality = min;
            MaximumQuality = max;
            Increment = increment;
        }

        public virtual IItem CreateAdjustedItem(IItem item, DateTime date)
        {
            var days = (item.DateCreated - date).Days;

            // 1) SellIn has expired this item's quality is decreasing faster
            Increment = item.SellIn >= days ? Increment : Increment * 2;

            // 2) Adjust Quality or set to minimum
            item.Quality = item.Quality + (days * Increment) < MinimumQuality ? MinimumQuality : item.Quality + (days * Increment);

            return item;
        }
    }
}