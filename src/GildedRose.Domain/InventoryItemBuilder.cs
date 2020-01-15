using System;

namespace GildedRose.Domain
{
    /// <summary>
    /// Responsible for the creation of <see cref="IInventoryItem"/> instances.
    /// </summary>
    public class InventoryItemBuilder : IInventoryItemBuilder
    {
        public IInventoryItem Build(Guid id, string name, string category, int quality, int sellIn)
        {
            if ("Aged Brie".Equals(name, StringComparison.CurrentCultureIgnoreCase))
            {
                return new BetterWithAgeInventoryItem(id, name, category, quality);
            }

            if ("Sulfuras".Equals(category, StringComparison.CurrentCultureIgnoreCase))
            {
                return new LegendaryInventoryItem(id, name, category, quality);
            }

            // Allow the typo in the category name.
            if ("Backstage Passes".Equals(category, StringComparison.CurrentCultureIgnoreCase) ||
                "Backstage Pasess".Equals(category, StringComparison.CurrentCultureIgnoreCase))
            {
                return new BackstagePassInventoryItem(id, name, category, quality, sellIn);
            }

            if ("Conjured".Equals(category, StringComparison.CurrentCultureIgnoreCase))
            {
                return new ConjuredInventoryItem(id, name, category, quality, sellIn);
            }

            return new StandardInventoryItem(id, name, category, quality, sellIn);
        }
    }
}