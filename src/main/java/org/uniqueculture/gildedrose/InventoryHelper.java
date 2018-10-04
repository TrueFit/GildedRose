/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.uniqueculture.gildedrose;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.Collections;
import java.util.List;
import java.util.stream.Collectors;
import org.uniqueculture.gildedrose.impl.CsvInventory;
import org.uniqueculture.gildedrose.impl.DefaultInventoryItemFactory;
import org.uniqueculture.gildedrose.impl.MaxMinQualityConstraint;
import org.uniqueculture.gildedrose.impl.calculator.AgedBrieQualityCalculator;
import org.uniqueculture.gildedrose.impl.calculator.BackstagePassQualityCalculator;
import org.uniqueculture.gildedrose.impl.calculator.ConjuredQualityCalculator;
import org.uniqueculture.gildedrose.impl.calculator.DefaultQualityCalculator;
import org.uniqueculture.gildedrose.impl.calculator.SulfurasQualityCalculator;
import org.uniqueculture.gildedrose.spi.Inventory;
import org.uniqueculture.gildedrose.spi.InventoryItem;
import org.uniqueculture.gildedrose.spi.InventoryItemFactory;
import org.uniqueculture.gildedrose.spi.QualityConstraint;

/**
 * Helper class with inventory query methods
 *
 * @author Sergei Izvorean <uniqueculture at gmail.com>
 */
public class InventoryHelper {

    private final Inventory inventory;

    public InventoryHelper(Inventory inventory) {
        this.inventory = inventory;
    }
    
    /**
     * Helper method to create an inventory per Gilded Rose rules
     * 
     * @param csvFile
     * @return Inventory helper instance
     */
    public static InventoryHelper getCsvInventory(String csvFile) {
        Path filePath = Paths.get(csvFile);
        if (!(Files.exists(filePath) && Files.isReadable(filePath))) {
            return null;
        }
        
        // Default quality constraints
        QualityConstraint constraints = new MaxMinQualityConstraint(50, 0);
        
        // Create a default item factory
        InventoryItemFactory itemFactory = new DefaultInventoryItemFactory(new DefaultQualityCalculator(constraints));
        itemFactory.addQualityCalculator(new AgedBrieQualityCalculator(constraints));
        itemFactory.addQualityCalculator(new BackstagePassQualityCalculator(constraints));
        itemFactory.addQualityCalculator(new ConjuredQualityCalculator(constraints));
        itemFactory.addQualityCalculator(new SulfurasQualityCalculator());
        
        try {
            Inventory csvInventory = new CsvInventory(filePath, itemFactory);
            return new InventoryHelper(csvInventory);
        } catch (IOException e) {
            return null;
        }
    }

    public List<InventoryItem> getInventoryItems() {
        return Collections.unmodifiableList(inventory.getItems());
    }

    public InventoryItem getInventoryItem(String name) {
        return inventory.getInventoryItem(name);
    }

    public List<InventoryItem> getTrash(int day) {
        return inventory.getItems().stream()
                // Only return items whose quality on a given day will be 0
                .filter((inventoryItem) -> inventoryItem.getQuality(day) == 0)
                .collect(Collectors.toList());
    }

}
