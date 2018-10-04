package org.uniqueculture.gildedrose.impl.calculator;

import org.uniqueculture.gildedrose.spi.Item;
import org.uniqueculture.gildedrose.spi.QualityConstraint;
import org.uniqueculture.gildedrose.spi.QualityCalculator;

/**
 * Quality calculator for item category Conjured
 * 
 * 7. "Conjured" items degrade in Quality twice as fast as normal items
 *
 * @author Sergei Izvorean
 */
public class ConjuredQualityCalculator implements QualityCalculator {
    
    private final QualityConstraint constraint;

    public ConjuredQualityCalculator(QualityConstraint constraint) {
        this.constraint = constraint;
    }
    
    @Override
    public int calculate(Item item, int day) {
        // "Conjured" items degrade in Quality twice as fast as normal items
        return constraint.apply(item.getInitialQuality() - 2 * day);
    }

    @Override
    public boolean appliesTo(Item item) {
        return item.getCategory().equals("Conjured");
    }
    
    
    
}
