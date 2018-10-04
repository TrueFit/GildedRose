/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.uniqueculture.gildedrose;

import org.uniqueculture.gildedrose.impl.DefaultCategoryRegistry;
import org.uniqueculture.gildedrose.impl.MaxMinQualityConstraint;
import org.uniqueculture.gildedrose.impl.category.AgedBrieCategory;
import org.uniqueculture.gildedrose.impl.category.BackstagePassCategory;
import org.uniqueculture.gildedrose.impl.category.ConjuredCategory;
import org.uniqueculture.gildedrose.impl.category.DefaultCategory;
import org.uniqueculture.gildedrose.impl.category.SulfuraCategory;
import org.uniqueculture.gildedrose.spi.CategoryRegistry;
import org.uniqueculture.gildedrose.spi.QualityConstraint;

/**
 *
 * @author me
 */
public class Application {
    
    public static final void main(String[] args) {
        // Default quality constraints
        QualityConstraint constraints = new MaxMinQualityConstraint(50, 0);
        
        CategoryRegistry registry = new DefaultCategoryRegistry();
        // Default category for general items
        registry.setDefaultCategory(new DefaultCategory(constraints));
        
        // Custom categories for specific items
        registry.addCategory("Aged Brie", new AgedBrieCategory(constraints));
        registry.addCategory("Backstage Passes", new BackstagePassCategory(constraints));
        registry.addCategory("Conjured", new ConjuredCategory(constraints));
        registry.addCategory("Sulfuras", new SulfuraCategory());
    }
    
}
