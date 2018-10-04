package org.uniqueculture.gildedrose.impl.calculator;

import org.uniqueculture.gildedrose.spi.Item;
import org.uniqueculture.gildedrose.spi.QualityConstraint;
import org.uniqueculture.gildedrose.spi.QualityCalculator;

/**
 * Quality calculator for Aged Brie item.
 * 
 * 3. "Aged Brie" actually increases in Quality the older it gets
 *
 * @author Sergei Izvorean
 */
public class AgedBrieQualityCalculator implements QualityCalculator {
    
    private final QualityConstraint constraints;

    public AgedBrieQualityCalculator(QualityConstraint constraints) {
        this.constraints = constraints;
    }

    @Override
    public int calculate(Item item, int day) {
        return constraints.apply(item.getInitialQuality() + day);
    }

    @Override
    public boolean appliesTo(Item item) {
        return item.getName().equals("Aged Brie");
    }
    
}
