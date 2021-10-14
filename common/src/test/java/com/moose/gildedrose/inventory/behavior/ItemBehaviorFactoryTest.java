package com.moose.gildedrose.inventory.behavior;

import com.moose.gildedrose.inventory.ReflectionUtils;
import com.moose.gildedrose.inventory.model.ItemCategoryType;
import com.moose.gildedrose.inventory.model.RawInventoryItem;
import lombok.SneakyThrows;
import static org.junit.jupiter.api.Assertions.assertEquals;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvFileSource;

class ItemBehaviorFactoryTest {

    @SneakyThrows
    @ParameterizedTest
    @CsvFileSource(resources = "/ItemBehaviorFactoryTest/test_inventory.csv", numLinesToSkip = 1)
    void testItemBehavior(final String name, final String category, final int sellByDays, final int quality, final String expectedBehaviorClass) {
        final ItemBehavior actualItemBehavior = ItemBehaviorFactory.getItemBehavior(new RawInventoryItem(name, ItemCategoryType.getByDescription(category), sellByDays, quality));
        final Class<?> expectedItemBehavior = Class.forName("com.moose.gildedrose.inventory.behavior." + expectedBehaviorClass);
        assertEquals(expectedItemBehavior, actualItemBehavior.getClass());
    }

    @Test
    @SneakyThrows
    void testPrivateConstructor() {
		ReflectionUtils.testPrivateConstructor(ItemBehaviorFactory.class.getDeclaredConstructor());
    }
}
