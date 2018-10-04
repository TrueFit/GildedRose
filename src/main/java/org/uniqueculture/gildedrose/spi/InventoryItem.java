package org.uniqueculture.gildedrose.spi;

/**
 * Inventory item to wrap an Item and Quality calculator
 *
 * @author Sergei Izvorean
 */
public interface InventoryItem {
    
    /**
     * Get an item
     * 
     * @return 
     */
    Item getItem();
    
    
    /**
     * Quality calculator
     * 
     * @return 
     */
    QualityCalculator getQualityCalculator();
    
    /**
     * Calculate quality on a given day
     * 
     * @param day Day to calculate the quality for
     * @return Quality on the day
     */
    int getQuality(int day);
    
}
