/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.uniqueculture.gildedrose.impl.category;

import org.uniqueculture.gildedrose.spi.Category;
import org.uniqueculture.gildedrose.spi.Item;

/**
 *
 * @author me
 */
public class SulfuraCategory implements Category {

    @Override
    public int calculateQuality(Item item, int day) {
        // 5. "Sulfuras", being a legendary item, never has to be sold or decreases in Quality
        // however "Sulfuras" is a legendary item and as such its Quality is 80 and it never alters. (irrelevant)
        return item.getInitialQuality();
    }
    
}
