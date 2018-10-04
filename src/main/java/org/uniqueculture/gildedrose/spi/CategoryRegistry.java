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
public interface CategoryRegistry {
    
    void setDefaultCategory(Category category);
    
    Category getDefaultCategory();
    
    Category addCategory(String name, Category impl);
    
    Category getCategory(String name);
    
}
