/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.uniqueculture.gildedrose.impl;

import java.util.HashMap;
import java.util.Map;
import org.uniqueculture.gildedrose.spi.Category;
import org.uniqueculture.gildedrose.spi.CategoryRegistry;

/**
 *
 * @author me
 */
public class DefaultCategoryRegistry implements CategoryRegistry {
    
    private Category defaultCategory;
    private final Map<String, Category> categories = new HashMap<>();

    @Override
    public void setDefaultCategory(Category category) {
        assert(category != null);
        
        this.defaultCategory = category;
    }

    @Override
    public Category getDefaultCategory() {
        return this.defaultCategory;
    }

    @Override
    public Category addCategory(String name, Category impl) {
        assert(impl != null);
        
        return this.categories.put(name, impl);
    }

    @Override
    public Category getCategory(String name) {
        return this.categories.get(name);
    }
    
}
