package org.uniqueculture.gildedrose.impl.calculator;

import org.uniqueculture.gildedrose.spi.Item;
import org.uniqueculture.gildedrose.spi.QualityConstraint;
import org.uniqueculture.gildedrose.spi.QualityCalculator;

/**
 * Quality calculator for all other items to apply general degradation logic
 *
 * @author Sergei Izvorean
 */
public class DefaultQualityCalculator implements QualityCalculator {
    
    QualityConstraint constraints;

    public DefaultQualityCalculator(QualityConstraint constraints) {
        this.constraints = constraints;
    }

    @Override
    public int calculate(Item item, int day) {
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

    @Override
    public boolean appliesTo(Item item) {
        return true;
    }
    
    

}
