/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.uniqueculture.gildedrose.impl.category;

import org.uniqueculture.gildedrose.spi.Category;
import org.uniqueculture.gildedrose.spi.Item;
import org.uniqueculture.gildedrose.spi.QualityConstraint;

/**
 *
 * @author me
 */
public class DefaultCategory implements Category {
    
    QualityConstraint constraints;

    public DefaultCategory(QualityConstraint constraints) {
        this.constraints = constraints;
    }

    @Override
    public int calculateQuality(Item item, int day) {
        int quality;

        if (day > item.getSellIn()) {
            // Degrade by each day and twice for days after sell in
            quality = item.getInitialQuality() - item.getSellIn() - 2 * (day - item.getSellIn());
        } else {
            // Degrade by each day
            quality = item.getInitialQuality() - day;
        }
        
        return constraints.apply(quality);
    }

}
