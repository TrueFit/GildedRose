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
public class ConjuredCategory implements Category {
    
    private final QualityConstraint constraint;

    public ConjuredCategory(QualityConstraint constraint) {
        this.constraint = constraint;
    }
    
    @Override
    public int calculateQuality(Item item, int day) {
        // "Conjured" items degrade in Quality twice as fast as normal items
        return constraint.apply(item.getInitialQuality() - 2 * day);
    }
    
}
