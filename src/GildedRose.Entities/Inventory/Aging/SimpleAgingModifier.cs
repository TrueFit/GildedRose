using System.Collections.Generic;

namespace GildedRose.Entities.Inventory.Aging
{
    /// <summary>
    /// Aging modifier for <see cref="Inventory" /> items that have a consistent appreciation/depreciation over time.
    /// </summary>
    public class SimpleAgingModifier : BaseAgingRule
    {
        public BaseQualityModifier QualityModifier { get; set; }

        protected override void SetQuality(Inventory item)
        {
            var qualityModifier = QualityModifier.Modifier;

            int tmpQuality;
            if (qualityModifier < 0)
            {
                tmpQuality = item.Quality - qualityModifier;
                tmpQuality = tmpQuality < 0 ? 0 : tmpQuality;
            }
            else if (qualityModifier > 0)
            {
                tmpQuality = item.Quality + qualityModifier;
                tmpQuality = tmpQuality > MaxQuality ? MaxQuality : tmpQuality;
            }
            else
            {
                tmpQuality = item.Quality;
            }

            item.Quality = tmpQuality;
        }
    }
}
