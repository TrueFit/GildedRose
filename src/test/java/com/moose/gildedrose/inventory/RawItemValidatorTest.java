package com.moose.gildedrose.inventory;

import com.moose.gildedrose.inventory.model.ItemCategoryType;
import com.moose.gildedrose.inventory.model.RawInventoryItem;
import com.opencsv.exceptions.CsvConstraintViolationException;
import java.util.stream.Stream;
import lombok.SneakyThrows;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.assertTrue;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;

class RawItemValidatorTest {
    private final RawItemValidator rawItemValidator = new RawItemValidator();

    @SneakyThrows
    @ParameterizedTest
    @MethodSource("getRawInventoryItemsAndExpectedResults")
    void testRawInventoryValidation(final RawInventoryItem itemToTest, final Class<? extends Throwable> expectedException) {
        if (expectedException == null) {
            assertTrue(this.rawItemValidator.verifyBean(itemToTest));
        } else {
            assertThrows(expectedException, () -> this.rawItemValidator.verifyBean(itemToTest));
        }
    }

    private static Stream<Arguments> getRawInventoryItemsAndExpectedResults() {
        return Stream.of(
                // Invalid Quality for all but Legendary
                Arguments.of(new RawInventoryItem("Example1", ItemCategoryType.ARMOR, 0, 80), CsvConstraintViolationException.class),
                Arguments.of(new RawInventoryItem("Example2", ItemCategoryType.BACKSTAGE_PASS, 0, 80), CsvConstraintViolationException.class),
                Arguments.of(new RawInventoryItem("Example3", ItemCategoryType.CONJURED, 0, 80), CsvConstraintViolationException.class),
                Arguments.of(new RawInventoryItem("Example4", ItemCategoryType.FOOD, 0, 80), CsvConstraintViolationException.class),
                Arguments.of(new RawInventoryItem("Example5", ItemCategoryType.MISCELLANEOUS, 0, 80), CsvConstraintViolationException.class),
                Arguments.of(new RawInventoryItem("Example6", ItemCategoryType.POTION, 0, 80), CsvConstraintViolationException.class),
                Arguments.of(new RawInventoryItem("Example7", ItemCategoryType.WEAPON, 0, 80), CsvConstraintViolationException.class),
                Arguments.of(new RawInventoryItem("Example8", ItemCategoryType.LEGENDARY, 0, 80), null),
                // Legendary Item Needs 80 Quality
                Arguments.of(new RawInventoryItem("Example9", ItemCategoryType.LEGENDARY, 0, 10), CsvConstraintViolationException.class),
                Arguments.of(new RawInventoryItem("Example10", ItemCategoryType.LEGENDARY, 0, 20), CsvConstraintViolationException.class),
                Arguments.of(new RawInventoryItem("Example11", ItemCategoryType.LEGENDARY, 0, 90), CsvConstraintViolationException.class),
                // Quality must not be negative
                Arguments.of(new RawInventoryItem("Example12", ItemCategoryType.ARMOR, 0, -1), CsvConstraintViolationException.class),
                Arguments.of(new RawInventoryItem("Example13", ItemCategoryType.ARMOR, 0, -20), CsvConstraintViolationException.class),
                // No Issues
                Arguments.of(new RawInventoryItem("Example14", ItemCategoryType.ARMOR, 0, 0), null),
                Arguments.of(new RawInventoryItem("Example15", ItemCategoryType.ARMOR, Integer.MIN_VALUE, 50), null),
                Arguments.of(new RawInventoryItem("Example16", ItemCategoryType.ARMOR, Integer.MAX_VALUE, 50), null)
        );
    }
}
