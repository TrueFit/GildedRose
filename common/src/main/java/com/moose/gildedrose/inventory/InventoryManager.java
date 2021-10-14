package com.moose.gildedrose.inventory;

import static com.moose.gildedrose.LambdaExceptionHandler.rethrowFunction;
import com.moose.gildedrose.inventory.behavior.ItemBehaviorFactory;
import com.moose.gildedrose.inventory.common.Constants;
import com.moose.gildedrose.inventory.model.InventoryItem;
import com.moose.gildedrose.inventory.model.RawInventoryItem;
import com.opencsv.bean.ColumnPositionMappingStrategy;
import com.opencsv.bean.CsvToBeanBuilder;
import com.opencsv.bean.StatefulBeanToCsvBuilder;
import com.opencsv.exceptions.CsvDataTypeMismatchException;
import com.opencsv.exceptions.CsvRequiredFieldEmptyException;
import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.Reader;
import java.io.StringWriter;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.List;
import java.util.Optional;
import java.util.stream.Collectors;
import lombok.AccessLevel;
import lombok.NoArgsConstructor;
import lombok.NonNull;
import org.apache.commons.lang3.StringUtils;

/**
 * A class that handles the loading operation of the {@code inventory.txt} file.
 * Expects a filepath to the appropriate inventory CSV file.
 * It uses the OpenCSV library to map all values to a {@link List} of {@link InventoryItem}.
 * It does mapping from {@link String} to {@link com.moose.gildedrose.inventory.model.ItemCategoryType} using the {@link StringToItemCategoryType} class,
 * additional bean validation from the {@link RawItemValidator}, and then finally maps the underlying CSV to appropriate {@link InventoryItem} with
 * the {@link ItemBehaviorFactory}.
 */
@NoArgsConstructor(access = AccessLevel.PRIVATE)
public class InventoryManager {
	private static List<InventoryItem> cachedInventory;

	public static List<InventoryItem> getInventory() throws IOException {
		if (InventoryManager.cachedInventory == null) {
			InventoryManager.cachedInventory = InventoryManager.loadInventory(Constants.DEFAULT_INVENTORY_FILE);
		}
		return InventoryManager.cachedInventory;
	}

	public static List<InventoryItem> ageInventory() throws IOException {
		final List<InventoryItem> inventory = InventoryManager.getInventory();
		inventory.forEach(InventoryItem::ageItem);
		InventoryManager.saveInventoryData(Constants.DEFAULT_INVENTORY_FILE, InventoryManager.writeInventoryToCsv(inventory));
		InventoryManager.cachedInventory = inventory;
		return inventory;
	}

	public static List<InventoryItem> trashInventory() throws IOException {
		final List<InventoryItem> inventory = InventoryManager.getInventory().stream().filter(item -> item.getQuality() > 0).collect(Collectors.toList());
		InventoryManager.saveInventoryData(Constants.DEFAULT_INVENTORY_FILE, InventoryManager.writeInventoryToCsv(inventory));
		InventoryManager.cachedInventory = inventory;
		return inventory;
	}

	/**
	 * Loads the inventory CSV file and constructs a {@link List} of {@link InventoryItem}s from it.
	 * @param filePath The path to the appropriate CSV file. Expected format is listed at the package-level.
	 * @return A {@link List} of properly-validated {@link InventoryItem}s - may be empty if none were specified.
	 * @throws IOException If there is a problem accessing the provided filePath.
	 */
	public static List<InventoryItem> loadInventory(@NonNull final String filePath) throws IOException {
		if (StringUtils.isEmpty(filePath)) {
			throw new IllegalArgumentException("filePath must not be empty.");
		}
		return InventoryManager.parseInventoryData(Optional.ofNullable(Thread.currentThread().getContextClassLoader().getResourceAsStream(filePath))
				.map(rethrowFunction(InputStream::readAllBytes))
				.orElseThrow(() -> new IOException(filePath + " not found.")));
	}

	public static List<InventoryItem> updateInventory(@NonNull final String fileContents) throws IOException {
		InventoryManager.cachedInventory = InventoryManager.parseInventoryData(fileContents.getBytes());
		InventoryManager.saveInventoryData(Constants.DEFAULT_INVENTORY_FILE, fileContents.getBytes());
		return InventoryManager.getInventory();
	}

	/**
	 * Loads the inventory file contents and constructs a {@link List} of {@link InventoryItem}s from it.
	 * @param fileContents The {@code byte[]} containing all contents of the CSV file. Expected format is listed at the package-level.
	 * @return A {@link List} of properly-validated {@link InventoryItem}s - may be empty if none were specified.
	 * @throws IOException If there is a problem with the data.
	 */
	public static List<InventoryItem> parseInventoryData(@NonNull final byte[] fileContents) throws IOException {
		try (Reader reader = new InputStreamReader(new ByteArrayInputStream(fileContents))) {
			return new CsvToBeanBuilder<RawInventoryItem>(reader)
					.withVerifier(new RawItemValidator())
					.withType(RawInventoryItem.class)
					.build()
					.parse()
					.stream()
					.map(InventoryManager::wrapRawInventoryItem)
					.collect(Collectors.toList());
		}
	}

	private static byte[] writeInventoryToCsv(final List<InventoryItem> beans) throws IOException {
		try (StringWriter writer = new StringWriter()) {
			final ColumnPositionMappingStrategy<RawInventoryItem> columnPositionMappingStrategy = new ColumnPositionMappingStrategy<>();
			columnPositionMappingStrategy.setType(RawInventoryItem.class);
			final StatefulBeanToCsvBuilder<RawInventoryItem> builder = new StatefulBeanToCsvBuilder<>(writer);
			builder.withMappingStrategy(columnPositionMappingStrategy)
					.withSeparator(',')
					.withApplyQuotesToAll(false)
					.build()
					.write(beans.stream().map(InventoryManager::unwrapInventoryItem).collect(Collectors.toList()));
			return writer.toString().getBytes();
		} catch (CsvRequiredFieldEmptyException | CsvDataTypeMismatchException ex) {
			throw new IOException(ex);
		}
	}

	public static void saveInventoryData(@NonNull final String fileName, @NonNull final byte[] fileContents) throws IOException {
		try (OutputStream outputStream = Files.newOutputStream(Paths.get(fileName))) {
			outputStream.write(fileContents);
		}
	}

	private static RawInventoryItem unwrapInventoryItem(@NonNull final InventoryItem inventoryItem) {
		return new RawInventoryItem(inventoryItem.getName(), inventoryItem.getCategory(), inventoryItem.getSellByDays(), inventoryItem.getQuality());
	}

    private static InventoryItem wrapRawInventoryItem(@NonNull final RawInventoryItem raw) {
        return new InventoryItem(raw.getName(),
                raw.getCategory(),
                raw.getSellByDays(),
                raw.getQuality(),
                ItemBehaviorFactory.getItemBehavior(raw));
    }
}
