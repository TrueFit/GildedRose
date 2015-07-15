package com.moose.gildedrose.inventory;

import com.moose.gildedrose.inventory.model.ItemCategoryType;
import com.opencsv.exceptions.CsvConstraintViolationException;
import lombok.SneakyThrows;
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.ArgumentsSource;

class StringToItemCategoryTypeTest {
    private final StringToItemCategoryType stringToItemCategoryType = new StringToItemCategoryType();

    @SneakyThrows
    @ParameterizedTest
    @ArgumentsSource(ArgumentsProviders.ItemCategoryDescriptionProviders.class)
    void testConvertingStringToItemCategory(final String descriptionToTest, final ItemCategoryType expectedItemCategory) {
        if (expectedItemCategory == null) {
            assertThrows(CsvConstraintViolationException.class, () -> this.stringToItemCategoryType.convert(descriptionToTest));
        } else {
            assertEquals(expectedItemCategory, this.stringToItemCategoryType.convert(descriptionToTest));
        }
    }

}
