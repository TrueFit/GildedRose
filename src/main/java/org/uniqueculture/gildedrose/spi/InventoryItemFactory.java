package org.uniqueculture.gildedrose.spi;

/**
 * Factory for inventory items to instantiate an Item and quality calculator 
 *
 * @author Sergei Izvorean
 */
public interface InventoryItemFactory {
    
    /**
     * Add quality calculator
     * 
     * @param qualityCalculator 
     */
    void addQualityCalculator(QualityCalculator qualityCalculator);
    
    /**
     * Remove quality calculator
     * 
     * @param qualityCalculator 
     */
    void removeQualityCalculator(QualityCalculator qualityCalculator);
    
    /**
     * Create an inventory item
     * 
     * @param name
     * @param category
     * @param sellIn
     * @param quality
     * @return 
     */
    InventoryItem getInventoryItem(String name, String category, int sellIn, int quality);
    
    /**
     * Create an inventory item wrapping an existing item
     * 
     * @param item
     * @return 
     */
    InventoryItem getInventoryItem(Item item);
    
}
