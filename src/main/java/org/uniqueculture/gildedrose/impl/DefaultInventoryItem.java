package org.uniqueculture.gildedrose.impl;

import org.uniqueculture.gildedrose.spi.InventoryItem;
import org.uniqueculture.gildedrose.spi.Item;
import org.uniqueculture.gildedrose.spi.QualityCalculator;

/**
 * Inventory item implementation to hold an item and corresponding quality calculation 
 *
 * @author Sergei Izvorean
 */
public class DefaultInventoryItem implements InventoryItem {
    
    private final Item item;
    private final QualityCalculator calculator;

    /**
     * Create an inventory item
     * 
     * @param item Item data model
     * @param calculator Quality calculator logic
     */
    public DefaultInventoryItem(Item item, QualityCalculator calculator) {
        this.item = item;
        this.calculator = calculator;
    }

    /**
     * Get item
     * 
     * @return 
     */
    @Override
    public Item getItem() {
        return this.item;
    }

    /**
     * Get corresponding quality calculator
     * 
     * @return 
     */
    @Override
    public QualityCalculator getQualityCalculator() {
        return this.calculator;
    }

    /**
     * Calculate quality of an item on a given day
     * 
     * @param day Day for the item quality calculation. 0-based.
     * @return Quality of the item
     */
    @Override
    public int getQuality(int day) {
        return this.calculator.calculate(item, day);
    }
    
}
