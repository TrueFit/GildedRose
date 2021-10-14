package com.moose.gildedrose.inventory.behavior;

/**
 * The {@link ItemBehavior} for Conjured items.
 * Uses the default {@link ItemBehavior#calculateSellByDays} which decrements by 1.
 */
public class ConjuredBehavior implements ItemBehavior {

    /**
     * Quality is determined by decrementing 2 if the sellByDate has not elapsed, otherwise decrement twice as fast.
     * Will automatically stop at 0 in cases where quality has become negative.
     * @param currentQuality The current quality of the item before being aged.
     * @param currentSellByDays The current number of days left to sell the item, prior to aging.
     * @return The newly-calculated quality for the item.
     */
    @Override
    public int calculateQuality(final int currentQuality, final int currentSellByDays) {
        return Math.max(0, currentQuality - (currentSellByDays >= 0 ? 2 : 4));
    }
}
