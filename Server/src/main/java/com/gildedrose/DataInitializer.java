package com.gildedrose;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import javax.persistence.EntityManager;
import javax.persistence.EntityManagerFactory;

import org.apache.commons.lang3.StringUtils;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.ApplicationArguments;
import org.springframework.boot.ApplicationRunner;
import org.springframework.core.io.Resource;
import org.springframework.core.io.ResourceLoader;
import org.springframework.stereotype.Component;

import com.gildedrose.model.Item;
import com.gildedrose.model.ItemCategory;
import com.gildedrose.model.ItemDefinition;
import com.gildedrose.model.SystemDate;

/**
 * Populates the database with initial data upon application startup.
 */
@Component
public class DataInitializer implements ApplicationRunner {

	@Autowired
	private EntityManagerFactory entityManagerFactory;

	@Autowired
	private ResourceLoader resourceLoader;

	/* -- PUBLIC METHODS -- */

	@Override
	public void run(ApplicationArguments args) throws IOException {

		// Read the inventory file into a list of file records
		Resource resource = resourceLoader.getResource("classpath:inventory.txt");
		Collection<FileRecord> fileRecords = readInventoryFile(resource.getInputStream());

		// Build and persist entities
		Entities entities = new Entities();
		entities.categories = buildEntities(fileRecords);
		entities.systemDates = new ArrayList<>();

		SystemDate inventoryDate = new SystemDate();
		inventoryDate.setId(Constants.InventoryDateId);
		inventoryDate.setDate(LocalDate.now());
		entities.systemDates.add(inventoryDate);

		persistEntities(entities);
	}

	/* -- INTERNAL METHODS -- */

	List<FileRecord> readInventoryFile(InputStream inputStream) throws IOException {
		List<FileRecord> inventoryItems = new ArrayList<>();

		try (BufferedReader reader = new BufferedReader(new InputStreamReader(inputStream))) {
			String line = reader.readLine();

			while (line != null) {
				line = line.trim();

				if (!StringUtils.isEmpty(line)) {
					String[] lineParts = line.split(",");

					FileRecord inventoryItem = new FileRecord();
					inventoryItem.itemName = lineParts[0];
					inventoryItem.categoryName = lineParts[1];
					inventoryItem.sellIn = Integer.parseInt(lineParts[2]);
					inventoryItem.quality = Integer.parseInt(lineParts[3]);
					inventoryItems.add(inventoryItem);
				}

				line = reader.readLine();
			}
		}

		return inventoryItems;
	}

	Collection<ItemCategory> buildEntities(Collection<FileRecord> fileRecords) {
		Map<String, ItemCategory> categoriesByName = new HashMap<>();
		Map<String, ItemDefinition> definitionsByName = new HashMap<>();

		for (FileRecord fileRecord : fileRecords) {

			// If the category does not yet exist, create it
			if (!categoriesByName.containsKey(fileRecord.categoryName)) {
				ItemCategory category = new ItemCategory();
				category.setName(fileRecord.categoryName);
				applySpecialBehaviorSettings(category);
				categoriesByName.put(category.getName(), category);
			}

			// If the definition does not yet exist, create it
			if (!definitionsByName.containsKey(fileRecord.itemName)) {
				ItemDefinition definition = new ItemDefinition();
				definition.setName(fileRecord.itemName);
				definition.setCategory(categoriesByName.get(fileRecord.categoryName));
				definition.getCategory().getDefinitions().add(definition);
				applySpecialBehaviorSettings(definition);
				definitionsByName.put(definition.getName(), definition);
			}

			// Create the item
			Item item = new Item();
			item.setSellIn(fileRecord.sellIn);
			item.setQuality(fileRecord.quality);
			item.setDefinition(definitionsByName.get(fileRecord.itemName));
			item.getDefinition().getItems().add(item);
		}

		return categoriesByName.values();
	}

	void persistEntities(Entities entities) {
		EntityManager entityManager = entityManagerFactory.createEntityManager();
		entityManager.getTransaction().begin();

		try {
			for (ItemCategory category : entities.categories) {
				entityManager.persist(category);
			}

			for (SystemDate systemDate : entities.systemDates) {
				entityManager.persist(systemDate);
			}

			entityManager.getTransaction().commit();
		}
		finally {
			entityManager.getTransaction().rollback();
		}
	}

	void applySpecialBehaviorSettings(ItemCategory category) {

		// Sulfuras never need to be sold, so ignore its sell-in value
		if ("Sulfuras".equals(category.getName()))
			category.setIgnoreSellIn(true);
	}

	void applySpecialBehaviorSettings(ItemDefinition definition) {

		// Aged Brie only increases in quality, so ignore its sell-in value
		if ("Aged Brie".equals(definition.getName()))
			definition.setIgnoreSellIn(true);
	}

	/* -- CLASSES -- */

	static class FileRecord {
		public String itemName;
		public String categoryName;
		public int sellIn;
		public int quality;
	}

	static class Entities {
		public Collection<ItemCategory> categories;
		public Collection<SystemDate> systemDates;
	}
}
