package com.moose.gildedrose.inventory.behavior;

/**
 * The {@link ItemBehavior} for Legendary items like the Hand of Ragnaros.
 * Legendary items are unaffected by aging.
 */
public class LegendaryBehavior implements ItemBehavior {

    /**
     * Quality is not affected for legendary items.
     * @param currentQuality The current quality of the item before being aged.
     * @param currentSellByDays The current number of days left to sell the item, prior to aging.
     * @return The unchanged quality for the item.
     */
    @Override
    public int calculateQuality(final int currentQuality, final int currentSellByDays) {
        // Legendary Item's Quality does not decrease, so do not decrement.
        return currentQuality;
    }

    /**
     * SellByDays is not affected for legendary items.
     * @param currentSellByDays The current number of days left to sell the item, prior to aging.
     * @return The unchanged sellByDays for the item.
     */
    @Override
    public int calculateSellByDays(final int currentSellByDays) {
        // Legendary Item has no intrinsic SellByDate, so do not decrement.
        return currentSellByDays;
    }
}
