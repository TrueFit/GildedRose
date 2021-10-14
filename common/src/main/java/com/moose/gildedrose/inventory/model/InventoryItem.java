package com.moose.gildedrose.inventory.model;

import com.moose.gildedrose.inventory.behavior.ItemBehavior;
import lombok.AccessLevel;
import lombok.AllArgsConstructor;
import lombok.Getter;

/**
 * Wrapper class of {@link RawInventoryItem}.
 * The {@link InventoryItem} is intentionally immutable and solely provides a mechanism for which to "age an item".
 * When constructed, the {@link RawInventoryItem} that this class wraps will provide all applicable
 * data as to not require explicit pass-through functionality. Additionally, it requires an {@link ItemBehavior} to do the aging
 * calculations. This is simply a strategy pattern.
 */
@Getter
@AllArgsConstructor
public class InventoryItem {
    private String name;

    private ItemCategoryType category;
    private int sellByDays;
    private int quality;
    @Getter(AccessLevel.NONE)
    private final ItemBehavior itemBehavior;

    /**
     * Ages the item. Intended to be invoked at the close of business for the GildedRose. This will do the appropriate
     * calculations for quality as well as sell by days using the provided {@link ItemBehavior}.
     * Results are obtained using the {@link InventoryItem#getQuality()} and {@link InventoryItem#getSellByDays()}.
     */
    public void ageItem() {
        this.quality = this.itemBehavior.calculateQuality(this.quality, this.sellByDays);
        this.sellByDays = this.itemBehavior.calculateSellByDays(this.sellByDays);
    }

    @Override
    public String toString() {
        return "- " + this.getName() + System.lineSeparator() +
                "  * Category: " + this.getCategory().getDescription() + System.lineSeparator() +
                "  * Sell In: " + this.getSellByDays() + " Days" + System.lineSeparator() +
                "  * Quality: " + this.getQuality();
    }

}
