/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.uniqueculture.gildedrose.impl;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.List;
import java.util.stream.Collectors;
import org.uniqueculture.gildedrose.spi.Inventory;
import org.uniqueculture.gildedrose.spi.InventoryItem;
import org.uniqueculture.gildedrose.spi.InventoryItemFactory;
import org.uniqueculture.gildedrose.spi.Item;
import org.uniqueculture.gildedrose.spi.QualityCalculator;

/**
 *
 * @author me
 */
public class CsvInventory implements Inventory {

    private final InventoryItemFactory itemFactory;
    private final List<InventoryItem> items;

    public CsvInventory(Path filePath, InventoryItemFactory itemFactory) throws IOException {
        this.itemFactory = itemFactory;

        // Read the file and populate the items
        List<String> lines = Files.readAllLines(filePath);
        this.items = lines.stream().map(this::buildItem).collect(Collectors.toList());
    }

    protected InventoryItem buildItem(String csvString) {
        String[] cols = csvString.split(",");
        if (cols.length < 4) {
            throw new IllegalArgumentException("Invalid inventory item: " + csvString);
        }
        
        return itemFactory.getInventoryItem(cols[0], cols[1], Integer.parseInt(cols[2]), Integer.parseInt(cols[3]));
    }

    @Override
    public List<InventoryItem> getItems() {
        return this.items;
    }

    @Override
    public InventoryItem getInventoryItem(String name) {
        return items.stream().filter((item) -> item.getItem().getName().equals(name)).findFirst().orElse(null);
    }
    
}
