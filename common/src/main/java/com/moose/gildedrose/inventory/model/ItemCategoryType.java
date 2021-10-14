package com.moose.gildedrose.inventory.model;

import com.fasterxml.jackson.annotation.JsonProperty;
import java.util.Arrays;
import lombok.AllArgsConstructor;
import lombok.Getter;

/**
 * An enumeration of all of the {@link InventoryItem}'s possible categories.
 * This is done to ensure that the categories provided in {@code inventory.txt} are appropriately spelled and not
 * unexpected. That is to say, an addition of a new category requires this file to be updated. In exchange, we have
 * finer-grained control over the uploaded inventory and can safely group all categories in a developer-friendly manner.
 */
@Getter
@AllArgsConstructor
public enum ItemCategoryType {
	@JsonProperty("Weapon") WEAPON("Weapon"),
	@JsonProperty("Food") FOOD("Food"),
	@JsonProperty("Sulfuras") LEGENDARY("Sulfuras"),
	@JsonProperty("Backstage Passes") BACKSTAGE_PASS("Backstage Passes"),
	@JsonProperty("Conjured") CONJURED("Conjured"),
	@JsonProperty("Potion") POTION("Potion"),
	@JsonProperty("Misc") MISCELLANEOUS("Misc"),
	@JsonProperty("Armor") ARMOR("Armor");

    final String description;

    /**
     * Convenience method for returning the appropriate {@link ItemCategoryType} for a given description.
     * @param description The case-insensitive, human-readable {@link String} description that will be used to find a {@link ItemCategoryType}.
     * @return The {@link ItemCategoryType} that matches the provided description, or {@code null} if not found.
     */
    public static ItemCategoryType getByDescription(final String description) {
        return Arrays.stream(ItemCategoryType.values())
                .filter(categoryType -> categoryType.getDescription().equalsIgnoreCase(description))
                .findFirst()
                .orElse(null);
    }

	@Override
	public String toString() {
		return this.description;
	}
}
