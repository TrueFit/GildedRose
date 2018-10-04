/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.uniqueculture.gildedrose.impl;

import java.util.List;
import org.uniqueculture.gildedrose.spi.InventoryItem;
import org.uniqueculture.gildedrose.spi.QualityCalculator;
import org.uniqueculture.gildedrose.spi.InventoryItemFactory;
import org.uniqueculture.gildedrose.spi.Item;

/**
 *
 * @author me
 */
public class DefaultItemFactory implements InventoryItemFactory {
    
    private final QualityCalculator defaultCalculator;
    private final List<QualityCalculator> calculators;

    public DefaultItemFactory(QualityCalculator defaultCalculator, List<QualityCalculator> calculators) {
        this.calculators = calculators;
        this.defaultCalculator = defaultCalculator;
    }

    @Override
    public InventoryItem getInventoryItem(String name, String category, int sellIn, int quality) {
        return getInventoryItem(new DefaultItem(name, category, sellIn, quality));
    }
    
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
