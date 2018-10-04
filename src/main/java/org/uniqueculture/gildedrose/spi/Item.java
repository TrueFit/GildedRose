package org.uniqueculture.gildedrose.spi;

/**
 * Base interface for an item
 *
 * @author Sergei Izvorean
 */
public interface Item {
    
    String getName();
    
    String getCategory();
    
    int getSellIn();
    
    int getInitialQuality();
    
}
