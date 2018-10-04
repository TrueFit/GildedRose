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
public class AgedBrieCategory implements Category {
    
    private final QualityConstraint constraints;

    public AgedBrieCategory(QualityConstraint constraints) {
        this.constraints = constraints;
    }

    @Override
    public int calculateQuality(Item item, int day) {
        return constraints.apply(item.getInitialQuality() + day);
    }
    
}
