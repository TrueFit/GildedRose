/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.uniqueculture.gildedrose.spi;

/**
 *
 * @author me
 */
public interface InventoryItemFactory {
    
    InventoryItem getInventoryItem(String name, String category, int sellIn, int quality);
    
    InventoryItem getInventoryItem(Item item);
    
}
