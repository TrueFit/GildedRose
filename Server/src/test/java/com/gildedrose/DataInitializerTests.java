package com.gildedrose;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNull;
import static org.junit.jupiter.api.Assertions.assertTrue;

import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

import org.junit.jupiter.api.Test;
import org.springframework.core.io.ClassPathResource;

import com.gildedrose.DataInitializer.FileRecord;
import com.gildedrose.model.Item;
import com.gildedrose.model.ItemCategory;
import com.gildedrose.model.ItemDefinition;

public class DataInitializerTests {

	private static final String BackstagePassesExpression =
	// @formatter:off
		"if (sellIn <= 0) return quality * -1; \n" +
		"if (sellIn <= 5) return 3; \n" +
		"if (sellIn <= 10) return 2; \n" +
		"return 1;";
	// @formatter:on

	/* -- PUBLIC METHODS -- */

	@Test
	public void readInventoryFile() throws IOException {

		// Arrange
		DataInitializer dataInitializer = new DataInitializer();
		InputStream inputStream = new ClassPathResource("test-inventory.txt").getInputStream();

		// Act
		List<FileRecord> fileRecords = dataInitializer.readInventoryFile(inputStream);

		// Assert
		assertEquals(2, fileRecords.size());

		FileRecord fr1 = fileRecords.get(0);
		assertEquals("Elixir of the Mongoose", fr1.itemName);
		assertEquals("Potion", fr1.categoryName);
		assertEquals(5, fr1.sellIn);
		assertEquals(7, fr1.quality);

		FileRecord fr2 = fileRecords.get(1);
		assertEquals("+5 Dexterity Vest", fr2.itemName);
		assertEquals("Armor", fr2.categoryName);
		assertEquals(10, fr2.sellIn);
		assertEquals(20, fr2.quality);
	}

	@Test
	public void buildEntities() {

		// Arrange
		DataInitializer dataInitializer = new DataInitializer();

		List<FileRecord> fileRecords = new ArrayList<>();

		FileRecord fr1 = new FileRecord();
		fr1.itemName = "name1";
		fr1.categoryName = "category1";
		fr1.sellIn = 2;
		fr1.quality = 1;
		fileRecords.add(fr1);

		FileRecord fr2 = new FileRecord();
		fr2.itemName = "name1";
		fr2.categoryName = "category1";
		fr2.sellIn = 4;
		fr2.quality = 3;
		fileRecords.add(fr2);

		FileRecord fr3 = new FileRecord();
		fr3.itemName = "name2";
		fr3.categoryName = "category2";
		fr3.sellIn = 6;
		fr3.quality = 5;
		fileRecords.add(fr3);

		// Act
		Collection<ItemCategory> categories = dataInitializer.buildEntities(fileRecords);

		// Assert
		assertEquals(2, categories.size());

		ItemCategory category1 = categories.stream().filter(c -> "category1".equals(c.getName())).findFirst().get();
		ItemCategory category2 = categories.stream().filter(c -> "category2".equals(c.getName())).findFirst().get();

		assertEquals(1, category1.getDefinitions().size());
		assertEquals(1, category2.getDefinitions().size());

		ItemDefinition definition1 = category1.getDefinitions().stream().filter(d -> "name1".equals(d.getName()))
				.findFirst().get();
		ItemDefinition definition2 = category2.getDefinitions().stream().filter(d -> "name2".equals(d.getName()))
				.findFirst().get();

		assertEquals(2, definition1.getItems().size());
		assertEquals(1, definition2.getItems().size());

		Item item1 = definition2.getItems().get(0);

		assertEquals("name2", item1.getName());
		assertEquals("category2", item1.getCategoryName());
		assertEquals(6, item1.getSellIn());
		assertEquals(5, item1.getQuality());
		assertNull(item1.getDefinition().getIgnoreSellIn());
		assertNull(item1.getCategory().getIgnoreSellIn());
	}

	@Test
	public void buildEntities_agedBrieSpecialBehavior() {
		DataInitializer dataInitializer = new DataInitializer();

		List<FileRecord> fileRecords = new ArrayList<>();

		FileRecord fr1 = new FileRecord();
		fr1.itemName = "Aged Brie";
		fr1.categoryName = "Food";
		fr1.sellIn = 2;
		fr1.quality = 1;
		fileRecords.add(fr1);

		// Act
		Collection<ItemCategory> categories = dataInitializer.buildEntities(fileRecords);

		// Assert
		ItemCategory category = categories.stream().filter(c -> "Food".equals(c.getName())).findFirst().get();
		ItemDefinition definition = category.getDefinitions().get(0);

		assertNull(category.getIgnoreSellIn());
		assertTrue(definition.getIgnoreSellIn());
		assertEquals("1", definition.getQualityChangeExpression());
	}

	@Test
	public void buildEntities_sulfurasSpecialBehavior() {
		DataInitializer dataInitializer = new DataInitializer();

		List<FileRecord> fileRecords = new ArrayList<>();

		FileRecord fr1 = new FileRecord();
		fr1.itemName = "Hand of Ragnaros";
		fr1.categoryName = "Sulfuras";
		fr1.sellIn = 2;
		fr1.quality = 1;
		fileRecords.add(fr1);

		// Act
		Collection<ItemCategory> categories = dataInitializer.buildEntities(fileRecords);

		// Assert
		ItemCategory category = categories.stream().filter(c -> "Sulfuras".equals(c.getName())).findFirst().get();
		ItemDefinition definition = category.getDefinitions().get(0);

		assertTrue(category.getIgnoreSellIn());
		assertNull(definition.getIgnoreSellIn());
		assertEquals("0", category.getQualityChangeExpression());
	}

	@Test
	public void buildEntities_conjuredSpecialBehavior() {
		DataInitializer dataInitializer = new DataInitializer();

		List<FileRecord> fileRecords = new ArrayList<>();

		FileRecord fr1 = new FileRecord();
		fr1.itemName = "Giant Slayer";
		fr1.categoryName = "Conjured";
		fr1.sellIn = 2;
		fr1.quality = 1;
		fileRecords.add(fr1);

		// Act
		Collection<ItemCategory> categories = dataInitializer.buildEntities(fileRecords);

		// Assert
		ItemCategory category = categories.stream().filter(c -> "Conjured".equals(c.getName())).findFirst().get();

		assertEquals("defaultChange * 2", category.getQualityChangeExpression());
	}

	@Test
	public void buildEntities_backstagePassesSpecialBehavior() {
		DataInitializer dataInitializer = new DataInitializer();

		List<FileRecord> fileRecords = new ArrayList<>();

		FileRecord fr1 = new FileRecord();
		fr1.itemName = "I am Murloc";
		fr1.categoryName = "Backstage Passes";
		fr1.sellIn = 2;
		fr1.quality = 1;
		fileRecords.add(fr1);

		// Act
		Collection<ItemCategory> categories = dataInitializer.buildEntities(fileRecords);

		// Assert
		ItemCategory category = categories.stream().filter(c -> "Backstage Passes".equals(c.getName())).findFirst()
				.get();

		assertEquals(BackstagePassesExpression, category.getQualityChangeExpression());
	}
}
