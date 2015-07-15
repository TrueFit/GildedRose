package com.moose.gildedrose.inventory;

import com.moose.gildedrose.inventory.behavior.ItemBehaviorFactory;
import com.moose.gildedrose.inventory.model.InventoryItem;
import com.moose.gildedrose.inventory.model.RawInventoryItem;
import com.opencsv.bean.CsvToBeanBuilder;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.Reader;
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
public class InventoryLoader {

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
        try (Reader reader = new InputStreamReader(Optional.ofNullable(Thread.currentThread().getContextClassLoader().getResourceAsStream(filePath))
                .orElseThrow(() -> new IOException(filePath + " not found.")))) {
            return new CsvToBeanBuilder<RawInventoryItem>(reader)
                .withVerifier(new RawItemValidator())
                .withType(RawInventoryItem.class)
                .build()
                .parse()
                .stream()
                .map(InventoryLoader::wrapRawInventoryItem)
                .collect(Collectors.toList());
        }
    }

    private static InventoryItem wrapRawInventoryItem(@NonNull final RawInventoryItem raw) {
        return new InventoryItem(raw.getName(),
                raw.getCategory(),
                raw.getSellByDays(),
                raw.getQuality(),
                ItemBehaviorFactory.getItemBehavior(raw));
    }
}
