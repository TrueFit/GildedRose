package com.gildedrose.service;

import static org.junit.jupiter.api.Assertions.assertArrayEquals;

import java.util.ArrayList;
import java.util.List;
import java.util.StringTokenizer;

import org.junit.jupiter.api.Test;

import com.gildedrose.model.Item;
import com.gildedrose.model.ItemCategory;
import com.gildedrose.model.ItemDefinition;

public class InventoryServiceImplTests {

	/* -- PUBLIC METHODS -- */

	@Test
	public void updateItem_qualityDegradesByOneByDefault() {

		// Arrange
		Item item = createItem("name", "Food", 3, 5);
		ValuePair[] expected = parseValuePairs("2,4 | 1,3");

		// Act
		ValuePair[] actual = progressItem(item, expected.length);

		// Assert
		assertArrayEquals(expected, actual);
	}

	@Test
	public void updateItem_qualityDegradesTwiceAfterSellBy() {

		// Arrange
		Item item = createItem("name", "Food", 3, 11);
		ValuePair[] expected = parseValuePairs("2,10 | 1,9 | 0,7 | -1,5 | -2,3 | -3,1 | -4,0");

		// Act
		ValuePair[] actual = progressItem(item, expected.length);

		// Assert
		assertArrayEquals(expected, actual);
	}

	@Test
	public void updateItem_qualityIsNeverNegative() {

		// Arrange
		Item item = createItem("name", "Food", 2, 1);
		ValuePair[] expected = parseValuePairs("1,0 | 0,0 | -1,0");

		// Act
		ValuePair[] actual = progressItem(item, expected.length);

		// Assert
		assertArrayEquals(expected, actual);
	}

	@Test
	public void updateItem_qualityIsNeverMoreThanFifty() {

		// Arrange
		Item item = createItem("name", "Backstage Passes", 5, 46);
		ValuePair[] expected = parseValuePairs("4,49 | 3,50 | 2,50");

		// Act
		ValuePair[] actual = progressItem(item, expected.length);

		// Assert
		assertArrayEquals(expected, actual);
	}

	@Test
	public void updateItem_sulfurasNeverChanges() {

		// Arrange
		Item item = createItem("name", "Sulfuras", 80, 80);
		ValuePair[] expected = parseValuePairs("80,80 | 80,80 | 80,80");

		// Act
		ValuePair[] actual = progressItem(item, expected.length);

		// Assert
		assertArrayEquals(expected, actual);
	}

	@Test
	public void updateItem_backstagePassesIncreaseInQuality() {

		// Arrange
		Item item = createItem("name", "Backstage Passes", 12, 25);
		ValuePair[] expected = parseValuePairs(
				"11,26 | 10,28 | 9,30 | 8,32 | 7,34 | 6,36 | 5,39 | 4,42 | 3,45 | 2,48 | 1,50 | 0,0");

		// Act
		ValuePair[] actual = progressItem(item, expected.length);

		// Assert
		assertArrayEquals(expected, actual);
	}

	@Test
	public void updateItem_conjuredItemsDegradeTwiceAsFast() {

		// Arrange
		Item item = createItem("name", "Conjured", 3, 11);
		ValuePair[] expected = parseValuePairs("2,9 | 1,7 | 0,3 | -1,0");

		// Act
		ValuePair[] actual = progressItem(item, expected.length);

		// Assert
		assertArrayEquals(expected, actual);
	}

	/* -- PRIVATE METHODS -- */

	private Item createItem(String name, String categoryName, int sellIn, int quality) {
		ItemCategory category = new ItemCategory();
		category.setName(categoryName);

		ItemDefinition definition = new ItemDefinition();
		definition.setName(name);
		definition.setCategory(category);
		category.getDefinitions().add(definition);

		Item item = new Item();
		item.setDefinition(definition);
		item.setSellIn(sellIn);
		item.setQuality(quality);
		definition.getItems().add(item);

		return item;
	}

	private ValuePair[] progressItem(Item item, int days) {
		InventoryServiceImpl service = new InventoryServiceImpl();
		ValuePair[] results = new ValuePair[days];

		for (int i = 0; i < days; i++) {
			service.updateItem(item);
			results[i] = new ValuePair(item.getSellIn(), item.getQuality());
		}

		return results;
	}

	private ValuePair[] parseValuePairs(String s) {
		List<ValuePair> pairs = new ArrayList<>();

		for (StringTokenizer st1 = new StringTokenizer(s, "|"); st1.hasMoreTokens();) {
			StringTokenizer st2 = new StringTokenizer(st1.nextToken(), ",");
			int sellIn = Integer.parseInt(st2.nextToken().trim());
			int quality = Integer.parseInt(st2.nextToken().trim());
			pairs.add(new ValuePair(sellIn, quality));
		}

		return pairs.toArray(new ValuePair[pairs.size()]);
	}

	/* -- CLASSES -- */

	private static class ValuePair {

		private int sellIn;

		private int quality;

		public ValuePair(int sellIn, int quality) {
			this.sellIn = sellIn;
			this.quality = quality;
		}

		@Override
		public int hashCode() {
			final int prime = 31;
			int result = 1;
			result = prime * result + quality;
			result = prime * result + sellIn;
			return result;
		}

		@Override
		public boolean equals(Object obj) {
			if (this == obj)
				return true;
			if (obj == null)
				return false;
			if (getClass() != obj.getClass())
				return false;
			ValuePair other = (ValuePair) obj;
			if (quality != other.quality)
				return false;
			if (sellIn != other.sellIn)
				return false;
			return true;
		}

		@Override
		public String toString() {
			return "{sellIn=" + sellIn + ", quality=" + quality + "}";
		}
	}
}
