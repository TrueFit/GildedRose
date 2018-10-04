package org.uniqueculture.gildedrose.impl;

import java.util.List;
import org.uniqueculture.gildedrose.spi.InventoryItem;
import org.uniqueculture.gildedrose.spi.QualityCalculator;
import org.uniqueculture.gildedrose.spi.InventoryItemFactory;
import org.uniqueculture.gildedrose.spi.Item;

/**
 * Inventory item factory to combine an item with corresponding quality calculator
 *
 * @author Sergei Izvorean
 */
public class DefaultItemFactory implements InventoryItemFactory {
    
    private final QualityCalculator defaultCalculator;
    private final List<QualityCalculator> calculators;

    public DefaultItemFactory(QualityCalculator defaultCalculator, List<QualityCalculator> calculators) {
        this.calculators = calculators;
        this.defaultCalculator = defaultCalculator;
    }

    /**
     * Get an instance of a Item data model
     * 
     * @param name
     * @param category
     * @param sellIn
     * @param quality
     * @return New instance
     */
    @Override
    public InventoryItem getInventoryItem(String name, String category, int sellIn, int quality) {
        return getInventoryItem(new DefaultItem(name, category, sellIn, quality));
    }
    
    /**
     * Get new inventory item with either first corresponding quality calculator or
     * default quality calculator
     * 
     * @param item Item to wrap
     * @return Inventory item which wraps Item along with quality calculator
     */
    @Override
    public InventoryItem getInventoryItem(Item item) {
        for (QualityCalculator calculator : calculators) {
            // Find the first available calculator to the item
            if (calculator.appliesTo(item)) {
                return new DefaultInventoryItem(item, calculator);
            }
        }
        
        return new DefaultInventoryItem(item, defaultCalculator);
    }
    
}
