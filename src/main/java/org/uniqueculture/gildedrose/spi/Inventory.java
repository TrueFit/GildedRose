/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.uniqueculture.gildedrose.spi;

import java.util.List;

/**
 *
 * @author me
 */
public interface Inventory {
    
    void addItem(Item item);
    
    List<Item> getItems();
    
    Item getItem(String name);
    
    Item removeItem(Item item);
    
    Item removeItem(String name);
    
}
