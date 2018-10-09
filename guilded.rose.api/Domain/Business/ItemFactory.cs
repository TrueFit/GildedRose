using System;
using guilded.rose.api.Domain.Models;
using guilded.rose.api.Domain.Models.Adjusters;
using guilded.rose.api.Domain.Models.Interfaces;

namespace guilded.rose.api.Domain.Business
{
    public static class ItemFactory
    {
        public static IItem CreateAdjustedItem(IItem item, DateTime date)
        {

            switch (item.Category.Name)
            {
                case "Backstage Passes":
                    return new BackstageAdjuster().CreateAdjustedItem(item, date);
                case "Sulfuras":
                    return new LegendaryAdjuster().CreateAdjustedItem(item, date);
                case "Conjured":
                    return new ConjuredAdjuster().CreateAdjustedItem(item, date);
                default:
                    return new DefaultAdjuster().CreateAdjustedItem(item, date);
            }

        }
    }
}