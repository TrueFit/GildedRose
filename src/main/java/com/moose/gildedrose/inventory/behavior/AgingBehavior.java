package com.moose.gildedrose.inventory.behavior;

/**
 * The {@link ItemBehavior} for Aging items such as Aged Brie.
 * Uses the default {@link ItemBehavior#calculateSellByDays} which decrements by 1.
 */
public class AgingBehavior implements ItemBehavior {

    /**
     * Quality is determined by incremented by 1 indefinitely.
     * Will automatically stop at 50 in cases where quality has exceeded that value.
     * @param currentQuality The current quality of the item before being aged.
     * @param currentSellByDays The current number of days left to sell the item, prior to aging.
     * @return The newly-calculated quality for the item.
     */
    @Override
    public int calculateQuality(final int currentQuality, final int currentSellByDays) {
        return Math.min(50, currentQuality + 1);
    }
}
