package org.uniqueculture.gildedrose.impl;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.List;
import java.util.stream.Collectors;
import org.uniqueculture.gildedrose.spi.Inventory;
import org.uniqueculture.gildedrose.spi.InventoryItem;
import org.uniqueculture.gildedrose.spi.InventoryItemFactory;

/**
 * Helper class to parse inventory saved in CSV file
 *
 * @author Sergei Izvorean
 */
public class CsvInventory implements Inventory {

    private final InventoryItemFactory itemFactory;
    private final List<InventoryItem> items;

    /**
     * Read the inventory file and create a list of items
     * 
     * @param filePath File path of the CSV
     * @param itemFactory Item factory to instantiate items
     * @throws IOException 
     */
    public CsvInventory(Path filePath, InventoryItemFactory itemFactory) throws IOException {
        this.itemFactory = itemFactory;

        // Read the file and populate the items
        List<String> lines = Files.readAllLines(filePath);
        this.items = lines.stream().map(this::buildItem).collect(Collectors.toList());
    }

    /**
     * Create item instance
     * 
     * @param csvString Comma separated values for an item
     * @return Inventory item instance
     */
    protected InventoryItem buildItem(String csvString) {
        String[] cols = csvString.split(",");
        if (cols.length < 4) {
            throw new IllegalArgumentException("Invalid inventory item: " + csvString);
        }
        
        return itemFactory.getInventoryItem(cols[0], cols[1], Integer.parseInt(cols[2]), Integer.parseInt(cols[3]));
    }
    
    /**
     * @see Inventory::getItems()
     * @return List of items in the inventory
     */
    @Override
    public List<InventoryItem> getItems() {
        return this.items;
    }

    /**
     * Get the inventory item by name
     * 
     * @param name Name of them item
     * @return Inventory item
     */
    @Override
    public InventoryItem getInventoryItem(String name) {
        return items.stream().filter((item) -> item.getItem().getName().equals(name)).findFirst().orElse(null);
    }
    
}
