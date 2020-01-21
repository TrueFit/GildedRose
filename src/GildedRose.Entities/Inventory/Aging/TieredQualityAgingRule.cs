using System.Collections.Generic;
using System.Linq;

namespace GildedRose.Entities.Inventory.Aging
{
    /// <summary>
    /// Aging rule for <see cref="Inventory"/> items that appreciate/depreciate at different rates over time.
    /// </summary>
    public class TieredQualityAgingRule : BaseAgingRule
    {
        public List<RangedSellInQualityModifier> QualityModifiers { get; set; }

        protected override void SetQuality(Inventory item)
        {
            var qualityModifer = GetQualityModifer(item);
            var modifier = qualityModifer.Modifier;
            var tmpQuality = item.Quality + modifier;
            
            if (modifier < 0)
            {
                tmpQuality = tmpQuality < 0 ? 0 : tmpQuality;
            }
            else if (modifier > 0)
            {
                tmpQuality = tmpQuality > MaxQuality ? MaxQuality : tmpQuality;
            }

            item.Quality = tmpQuality;
        }

        private RangedSellInQualityModifier GetQualityModifer(Inventory item)
        {
            return QualityModifiers.First(i => item.SellIn >= i.SellInFrom && item.SellIn <= i.SellInTo);
        }
    }
}
