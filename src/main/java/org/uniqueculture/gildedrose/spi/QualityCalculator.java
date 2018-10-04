package org.uniqueculture.gildedrose.spi;

/**
 * Quality calculator for inventory items
 *
 * @author Sergei Izvorean
 */
public interface QualityCalculator {
    
    /**
     * Check if implementation applies to an item
     * 
     * @param item Item to check
     * @return True if quality calculator implementation applies to a given item, false otherwise
     */
    boolean appliesTo(Item item);
    
    /**
     * Calculate item quality degradation for a given day
     * 
     * @param item
     * @param day
     * @return Quality on a given day
     */
    int calculate(Item item, int day);
    
}
