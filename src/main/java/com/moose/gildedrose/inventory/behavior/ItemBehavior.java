package com.moose.gildedrose.inventory.behavior;

import com.moose.gildedrose.inventory.model.InventoryItem;

/**
 * An {@code interface} for determining a {@link InventoryItem}'s behavior when aging the item.
 * This is intended to be used as a strategy pattern. That is, this interface provides a blueprint for exposing
 * {@link ItemBehavior#calculateQuality} and {@link ItemBehavior#calculateSellByDays} functions that will be used
 * internally to the {@link InventoryItem} itself.
 *
 * These calculations are integral to the validity of the GildedRose's inventory system and should map directly to requirements.
 * @see ItemBehaviorFactory for how these behaviors get assigned.
 */
public interface ItemBehavior {
    int calculateQuality(final int currentQuality, final int currentSellByDays);

    default int calculateSellByDays(final int currentSellByDays) {
        return currentSellByDays - 1;
    }
}
