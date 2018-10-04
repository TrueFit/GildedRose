/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.uniqueculture.gildedrose.impl.calculator;

import org.uniqueculture.gildedrose.spi.Item;
import org.uniqueculture.gildedrose.spi.QualityConstraint;
import org.uniqueculture.gildedrose.spi.QualityCalculator;

/**
 *
 * @author me
 */
public class BackstagePassQualityCalculator implements QualityCalculator {

    private final QualityConstraint constraint;

    public BackstagePassQualityCalculator(QualityConstraint constraint) {
        this.constraint = constraint;
    }
    
    @Override
    public int calculate(Item item, int day) {
        int quality;
        int daysLeft = item.getSellIn() - day;
        
        if (daysLeft < 0) {
            quality = 0;
        } else if (daysLeft >= 0 && daysLeft <= 5) {
            quality = item.getInitialQuality() * 3;
        } else if (daysLeft > 5 && daysLeft <= 10) {
            quality = item.getInitialQuality() * 2;
        } else {
            quality = item.getInitialQuality() + day;
        }
        
        return constraint.apply(quality);
    }

    @Override
    public boolean appliesTo(Item item) {
        return item.getCategory().equals("Backstage Passes");
    }
}
