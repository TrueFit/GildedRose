package org.uniqueculture.gildedrose.spi;

import java.util.List;

/**
 * Inventory of the store  
 *
 * @author Sergei Izvorean
 */
public interface Inventory {
    
    /**
     * Get all items within the inventory
     * 
     * @return List of inventory items
     */
    List<InventoryItem> getItems();
    
    /**
     * Search for a single item in the inventory by item name
     * 
     * @param name Item name to match
     * @return Inventory item or null
     */
    InventoryItem getInventoryItem(String name);
    
}
