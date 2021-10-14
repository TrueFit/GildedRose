package com.moose.gildedrose.inventory.behavior;

/**
 * The {@link ItemBehavior} for Back Stage Passes.
 * Uses the default {@link ItemBehavior#calculateSellByDays} which decrements by 1.
 */
public class BackstageBehavior implements ItemBehavior {
	private static final int LEAST_VALUABLE_DATE_CUTOFF = 10;
	private static final int MOST_VALUABLE_DATE_CUTOFF = 5;

    /**
     * Quality is determined by incrementing by how close it is to the sellByDate:
     * <ul>
     *     <li>11+ Days: <b>1</b></li>
     *     <li>6-10 Days: <b>2</b></li>
     *     <li>0-5 Days: <b>3</b></li>
     * </ul>
     * It will drop to <b>0</b> after the sellByDate has passed.
     * Will automatically stop at 50 in cases where quality has exceeded that value.
     * @param currentQuality The current quality of the item before being aged.
     * @param currentSellByDays The current number of days left to sell the item, prior to aging.
     * @return The newly-calculated quality for the item.
     */
    @Override
    public int calculateQuality(final int currentQuality, final int currentSellByDays) {
        final int newQuality;
        if (currentSellByDays > BackstageBehavior.LEAST_VALUABLE_DATE_CUTOFF) {
            newQuality = currentQuality + 1;
        } else if (currentSellByDays <= 0) {
            return 0;
        } else if (currentSellByDays <= BackstageBehavior.MOST_VALUABLE_DATE_CUTOFF) {
            newQuality = currentQuality + 3;
        } else {
            // currentSellByDays <= 10)
            newQuality = currentQuality + 2;
        }
        return Math.min(50, newQuality);
    }
}
