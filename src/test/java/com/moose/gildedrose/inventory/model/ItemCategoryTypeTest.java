package com.moose.gildedrose.inventory.model;

import com.moose.gildedrose.inventory.ArgumentsProviders;
import lombok.SneakyThrows;
import static org.junit.jupiter.api.Assertions.assertEquals;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.ArgumentsSource;

class ItemCategoryTypeTest {

    @SneakyThrows
    @ParameterizedTest
    @ArgumentsSource(ArgumentsProviders.ItemCategoryDescriptionProviders.class)
    void testGettingItemCategoryByDescription(final String descriptionToTest, final ItemCategoryType expectedItemCategory) {
        assertEquals(expectedItemCategory, ItemCategoryType.getByDescription(descriptionToTest));
    }

}
