package com.gildedrose;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
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
		Collection<FileRecord> fileRecords = readInventoryFile();

		// Build the necessary entities. They can all be attached through categories
		Collection<ItemCategory> categories = buildEntities(fileRecords);

		// Persist the entities to the database
		persistEntities(categories);
	}

	/* -- PRIVATE METHODS -- */

	private Collection<FileRecord> readInventoryFile() throws IOException {
		Collection<FileRecord> inventoryItems = new ArrayList<>();
		Resource resource = resourceLoader.getResource("classpath:inventory.txt");

		try (BufferedReader reader = new BufferedReader(new InputStreamReader(resource.getInputStream()))) {
			String line = reader.readLine();

			while (line != null) {
				line = line.trim();

				if (StringUtils.isEmpty(line))
					continue;

				String[] lineParts = line.split(",");

				FileRecord inventoryItem = new FileRecord();
				inventoryItem.itemName = lineParts[0];
				inventoryItem.categoryName = lineParts[1];
				inventoryItem.sellIn = Integer.parseInt(lineParts[2]);
				inventoryItem.quality = Integer.parseInt(lineParts[3]);
				inventoryItems.add(inventoryItem);

				line = reader.readLine();
			}
		}

		return inventoryItems;
	}

	private Collection<ItemCategory> buildEntities(Collection<FileRecord> fileRecords) {
		Map<String, ItemCategory> categoriesByName = new HashMap<>();
		Map<String, ItemDefinition> definitionsByName = new HashMap<>();
		LocalDate today = LocalDate.now();

		for (FileRecord fileRecord : fileRecords) {

			// If the category does not yet exist, create it
			if (!categoriesByName.containsKey(fileRecord.categoryName)) {
				ItemCategory category = new ItemCategory();
				category.setName(fileRecord.categoryName);
				categoriesByName.put(category.getName(), category);
			}

			// If the definition does not yet exist, create it
			if (!definitionsByName.containsKey(fileRecord.itemName)) {
				ItemDefinition definition = new ItemDefinition();
				definition.setName(fileRecord.itemName);
				definition.setCategory(categoriesByName.get(fileRecord.categoryName));
				definition.getCategory().getDefinitions().add(definition);
				definitionsByName.put(definition.getName(), definition);
			}

			// Create the item
			Item item = new Item();
			item.setQuality(fileRecord.quality);
			item.setSellByDate(today.plusDays(fileRecord.sellIn));
			item.setQualityLastCalculatedDate(today);
			item.setDefinition(definitionsByName.get(fileRecord.itemName));
			item.getDefinition().getItems().add(item);
		}

		return categoriesByName.values();
	}

	private void persistEntities(Collection<ItemCategory> categories) {
		EntityManager entityManager = entityManagerFactory.createEntityManager();
		entityManager.getTransaction().begin();

		try {
			for (ItemCategory category : categories) {
				entityManager.persist(category);
			}

			entityManager.getTransaction().commit();
		}
		finally {
			entityManager.getTransaction().rollback();
		}
	}

	/* -- CLASSES -- */

	private class FileRecord {
		public String itemName;
		public String categoryName;
		public int sellIn;
		public int quality;
	}
}
