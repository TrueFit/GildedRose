package com.moose.gildedrose.inventory;

import com.moose.gildedrose.inventory.behavior.AgingBehavior;
import com.moose.gildedrose.inventory.behavior.BackstageBehavior;
import com.moose.gildedrose.inventory.behavior.ConjuredBehavior;
import com.moose.gildedrose.inventory.behavior.DefaultBehavior;
import com.moose.gildedrose.inventory.behavior.LegendaryBehavior;
import com.moose.gildedrose.inventory.model.InventoryItem;
import com.moose.gildedrose.inventory.model.ItemCategoryType;
import com.opencsv.exceptions.CsvConstraintViolationException;
import com.opencsv.exceptions.CsvDataTypeMismatchException;
import com.opencsv.exceptions.CsvRequiredFieldEmptyException;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Objects;
import java.util.stream.Stream;
import lombok.SneakyThrows;
import static org.junit.jupiter.api.Assertions.assertAll;
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.assertTrue;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;
import org.junit.jupiter.params.provider.NullAndEmptySource;
import org.junit.jupiter.params.provider.ValueSource;

class InventoryManagerTest {

	private final List<InventoryItem> defaultInventoryItemList = new ArrayList<>(Arrays.asList(
			new InventoryItem("Sword", ItemCategoryType.WEAPON, 30, 50, new DefaultBehavior()),
			new InventoryItem("Axe", ItemCategoryType.WEAPON, 40, 50, new DefaultBehavior()),
			new InventoryItem("Halberd", ItemCategoryType.WEAPON, 60, 40, new DefaultBehavior()),
			new InventoryItem("Aged Brie", ItemCategoryType.FOOD, 50, 10, new AgingBehavior()),
			new InventoryItem("Aged Milk", ItemCategoryType.FOOD, 20, 20, new DefaultBehavior()),
			new InventoryItem("Mutton", ItemCategoryType.FOOD, 10, 10, new DefaultBehavior()),
			new InventoryItem("Hand of Ragnaros", ItemCategoryType.LEGENDARY, 80, 80, new LegendaryBehavior()),
			new InventoryItem("I am Murloc", ItemCategoryType.BACKSTAGE_PASS, 20, 10, new BackstageBehavior()),
			new InventoryItem("Raging Ogre", ItemCategoryType.BACKSTAGE_PASS, 10, 10, new BackstageBehavior()),
			new InventoryItem("Giant Slayer", ItemCategoryType.CONJURED, 15, 50, new ConjuredBehavior()),
			new InventoryItem("Storm Hammer", ItemCategoryType.CONJURED, 20, 50, new ConjuredBehavior()),
			new InventoryItem("Belt of Giant Strength", ItemCategoryType.CONJURED, 20, 40, new ConjuredBehavior()),
			new InventoryItem("Cheese", ItemCategoryType.FOOD, 5, 5, new DefaultBehavior()),
			new InventoryItem("Potion of Healing", ItemCategoryType.POTION, 10, 10, new DefaultBehavior()),
			new InventoryItem("Bag of Holding", ItemCategoryType.MISCELLANEOUS, 10, 50, new DefaultBehavior()),
			new InventoryItem("TAFKAL80ETC Concert", ItemCategoryType.BACKSTAGE_PASS, 15, 20, new BackstageBehavior()),
			new InventoryItem("Elixir of the Mongoose", ItemCategoryType.POTION, 5, 7, new DefaultBehavior()),
			new InventoryItem("+5 Dexterity Vest", ItemCategoryType.ARMOR, 10, 20, new DefaultBehavior()),
			new InventoryItem("Full Plate Mail", ItemCategoryType.ARMOR, 50, 50, new DefaultBehavior()),
			new InventoryItem("Wooden Shield", ItemCategoryType.ARMOR, 10, 30, new DefaultBehavior())
	));

	@SneakyThrows
	@ParameterizedTest
	@ValueSource(strings = {"InventoryLoaderTest/good_inventory_default.txt"})
	void testDefaultFileLoading(final String filePath) {
		final List<InventoryItem> results = InventoryManager.loadInventory(filePath);
		assertNotNull(results);
		assertFalse(results.isEmpty());
		assertEquals(this.defaultInventoryItemList.size(), results.size());
		assertTrue(results.stream()
			.allMatch(actualItem ->
				this.assertInventoryItemEquality(this.defaultInventoryItemList.stream()
					.filter(expected -> Objects.equals(expected.getName(), actualItem.getName()))
					.findFirst()
					.orElse(null),
				actualItem)));
	}

	private boolean assertInventoryItemEquality(final InventoryItem expectedItem, final InventoryItem actualItem) {
		assertAll(
			() -> assertEquals(expectedItem.getCategory(), actualItem.getCategory()),
			() -> assertEquals(expectedItem.getName(), actualItem.getName()),
			() -> assertEquals(expectedItem.getQuality(), actualItem.getQuality()),
			() -> assertEquals(expectedItem.getSellByDays(), actualItem.getSellByDays())
		);
		return true;
	}

    @ParameterizedTest
    @ValueSource(strings = {"/fakefile.txt", "C:\\Users\\Moose\\GildedRose\\PleaseDontDoThis.txt"})
    void testMissingFileLoading(final String filePath) {
        assertThrows(IOException.class, () -> InventoryManager.loadInventory(filePath));
    }

    @ParameterizedTest
    @NullAndEmptySource
    void testNullFileLoading(final String filePath) {
        assertThrows(IllegalArgumentException.class, () -> InventoryManager.loadInventory(filePath));
    }

    @ParameterizedTest
    @MethodSource("getInvalidFilesAndExpectedExceptions")
    void testInvalidFileLoading(final String filePath, final Class<? extends Throwable> expectedException) {
        final Throwable throwable = assertThrows(RuntimeException.class, () -> InventoryManager.loadInventory(filePath));
        assertEquals(expectedException, throwable.getCause().getClass());
    }

    private static Stream<Arguments> getInvalidFilesAndExpectedExceptions() {
        return Stream.of(
            Arguments.of("InventoryLoaderTest/bad_inventory_new_category.txt", CsvConstraintViolationException.class),
            Arguments.of("InventoryLoaderTest/bad_inventory_excessive_quality.txt", CsvConstraintViolationException.class),
            Arguments.of("InventoryLoaderTest/bad_inventory_incorrect_legendary_quality.txt", CsvConstraintViolationException.class),
            Arguments.of("InventoryLoaderTest/bad_inventory_negative_quality.txt", CsvConstraintViolationException.class),
            Arguments.of("InventoryLoaderTest/bad_inventory_wrong_types.txt", CsvDataTypeMismatchException.class),
            Arguments.of("InventoryLoaderTest/bad_inventory_missing_column.txt", CsvRequiredFieldEmptyException.class)
        );
    }

    @SneakyThrows
    @ParameterizedTest
    @ValueSource(strings = {"InventoryLoaderTest/good_inventory_empty_data.txt"})
    void testEmptyFileLoading(final String filePath) {
        final List<InventoryItem> results = InventoryManager.loadInventory(filePath);
        assertNotNull(results);
        assertTrue(results.isEmpty());
    }

    @Test
    @SneakyThrows
    void testPrivateConstructor() {
		ReflectionUtils.testPrivateConstructor(InventoryManager.class.getDeclaredConstructor());
    }

}
