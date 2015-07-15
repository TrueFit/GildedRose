package com.moose.gildedrose.inventory;

import com.moose.gildedrose.inventory.model.ItemCategoryType;
import java.util.stream.IntStream;
import java.util.stream.Stream;
import lombok.AccessLevel;
import lombok.NoArgsConstructor;
import org.apache.commons.lang3.StringUtils;
import org.junit.jupiter.api.extension.ExtensionContext;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.ArgumentsProvider;

/**
 * Collection of {@link ArgumentsProvider}s to be re-used across test classes.
 */
@NoArgsConstructor(access = AccessLevel.PRIVATE)
public class ArgumentsProviders {

    /**
     * {@link ArgumentsProvider} for {@link String} descriptions that map to an {@link ItemCategoryType} (or null if invalid).
     */
    public static class ItemCategoryDescriptionProviders implements ArgumentsProvider {
        @Override
        public Stream<? extends Arguments> provideArguments(final ExtensionContext context) {
            return Stream.of(
                    // Standard Descriptions
                    Arguments.of("Weapon", ItemCategoryType.WEAPON),
                    Arguments.of("Food", ItemCategoryType.FOOD),
                    Arguments.of("Sulfuras", ItemCategoryType.LEGENDARY),
                    Arguments.of("Backstage Passes", ItemCategoryType.BACKSTAGE_PASS),
                    Arguments.of("Conjured", ItemCategoryType.CONJURED),
                    Arguments.of("Potion", ItemCategoryType.POTION),
                    Arguments.of("Misc", ItemCategoryType.MISCELLANEOUS),
                    Arguments.of("Armor", ItemCategoryType.ARMOR),
                    // Case Insensitivity
                    Arguments.of("BaCkStAgE pAsSeS", ItemCategoryType.BACKSTAGE_PASS),
                    Arguments.of("armOR", ItemCategoryType.ARMOR),
                    // Invalid Values
                    Arguments.of("armour", null),
                    Arguments.of("mystical", null),
                    Arguments.of("miscellaneous", null),
                    Arguments.of("random", null),
                    Arguments.of(null, null),
                    Arguments.of(StringUtils.EMPTY, null),
                    Arguments.of(StringUtils.SPACE, null)
            );
        }
    }

    /**
     * {@link ArgumentsProvider} for starting {@link Integer} sellByDays and the expected result after 1 day of aging.
     */
    public static class SellByDaysCalculationProvider implements ArgumentsProvider {
        @Override
        public Stream<? extends Arguments> provideArguments(final ExtensionContext context) {
            return IntStream.range(-50, 51).mapToObj(integer -> Arguments.of(integer, integer - 1));
        }
    }
}
