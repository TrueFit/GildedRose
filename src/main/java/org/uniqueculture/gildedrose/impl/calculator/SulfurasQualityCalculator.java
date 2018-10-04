package org.uniqueculture.gildedrose.impl.calculator;

import org.uniqueculture.gildedrose.spi.Item;
import org.uniqueculture.gildedrose.spi.QualityCalculator;

/**
 * Quality calculation for Sulfuras
 *
 * 5. "Sulfuras", being a legendary item, never has to be sold or decreases in Quality
 * 
 * @author Sergei Izvorean
 */
public class SulfurasQualityCalculator implements QualityCalculator {

    @Override
    public int calculate(Item item, int day) {
        // 5. "Sulfuras", being a legendary item, never has to be sold or decreases in Quality
        // however "Sulfuras" is a legendary item and as such its Quality is 80 and it never alters. (irrelevant)
        return item.getInitialQuality();
    }

    @Override
    public boolean appliesTo(Item item) {
        return item.getCategory().equals("Sulfuras");
    }

}
