package com.moose.gildedrose.inventory.model;

import com.moose.gildedrose.inventory.behavior.DefaultBehavior;
import com.moose.gildedrose.inventory.behavior.ItemBehavior;
import java.util.AbstractMap;
import java.util.stream.Stream;
import static org.junit.jupiter.api.Assertions.assertEquals;
import org.junit.jupiter.api.extension.ExtendWith;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;
import org.junit.jupiter.params.provider.ValueSource;
import static org.mockito.ArgumentMatchers.anyInt;
import org.mockito.Mock;
import static org.mockito.Mockito.times;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;
import org.mockito.junit.jupiter.MockitoExtension;

@ExtendWith(MockitoExtension.class)
class InventoryItemTest {
    @Mock ItemBehavior itemBehavior;

    @ParameterizedTest
    @ValueSource(ints = {Integer.MIN_VALUE, -10, -5, 0, 5, 10, 15, 20, 100, Integer.MAX_VALUE})
    void testAgeItem(final Integer mockedReturnValue) {
        when(this.itemBehavior.calculateQuality(anyInt(), anyInt())).thenReturn(mockedReturnValue);
        when(this.itemBehavior.calculateSellByDays(anyInt())).thenReturn(-mockedReturnValue);

        final InventoryItem testInventoryItem = new InventoryItem("test", ItemCategoryType.ARMOR, 30, 50, this.itemBehavior);
        assertEquals(50, testInventoryItem.getQuality());
        assertEquals(30, testInventoryItem.getSellByDays());

        testInventoryItem.ageItem();
        verify(this.itemBehavior, times(1)).calculateQuality(anyInt(), anyInt());
        verify(this.itemBehavior, times(1)).calculateSellByDays(anyInt());
        assertEquals(mockedReturnValue, testInventoryItem.getQuality());
        assertEquals(-mockedReturnValue, testInventoryItem.getSellByDays());
    }

    @ParameterizedTest
    @MethodSource("getInventoryItemAndExpectedStrings")
    void testToString(final InventoryItem itemUnderTest, final String expectedToString) {
        assertEquals(expectedToString, itemUnderTest.toString());
    }

    private static Stream<Arguments> getInventoryItemAndExpectedStrings() {
        return Stream.of(
                InventoryItemTest.getExpectedString("Example", ItemCategoryType.ARMOR, 5, 4),
                InventoryItemTest.getExpectedString("Test", ItemCategoryType.BACKSTAGE_PASS, 15, 34),
                InventoryItemTest.getExpectedString("Cool", ItemCategoryType.CONJURED, 65, 50),
                InventoryItemTest.getExpectedString("Foo", ItemCategoryType.FOOD, -4, 0),
                InventoryItemTest.getExpectedString("Bar", ItemCategoryType.LEGENDARY, -30, 12),
                InventoryItemTest.getExpectedString("Misc", ItemCategoryType.MISCELLANEOUS, 0, 7),
                InventoryItemTest.getExpectedString("Awesome", ItemCategoryType.POTION, 23, 39),
                InventoryItemTest.getExpectedString("Winner", ItemCategoryType.WEAPON, 1, 6)
        ).map(entry -> Arguments.of(entry.getKey(), entry.getValue()));
    }

    private static AbstractMap.SimpleEntry<InventoryItem, String> getExpectedString(final String name, final ItemCategoryType categoryType, final int sellByDays, final int quality) {
        final InventoryItem inventoryItem = new InventoryItem(name, categoryType, sellByDays, quality, new DefaultBehavior());
        return new AbstractMap.SimpleEntry<>(inventoryItem, String.format("- %s%n  * Category: %s%n  * Sell In: %s Days%n  * Quality: %s", name, categoryType.getDescription(), sellByDays, quality));
    }
}
