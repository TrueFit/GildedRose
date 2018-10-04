package org.uniqueculture.gildedrose.impl.calculator;

import org.uniqueculture.gildedrose.spi.Item;
import org.uniqueculture.gildedrose.spi.QualityConstraint;
import org.uniqueculture.gildedrose.spi.QualityCalculator;

/**
 * Quality calculator for Backstage Passes item category
 * 
 * 6. "Backstage passes", like aged brie, increases in Quality as it's SellIn 
 * value approaches; Quality increases by 2 when there are 10 days or less and 
 * by 3 when there are 5 days or less but Quality drops to 0 after the concert
 *
 * @author Sergei Izvorean
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
            // After the concert
            quality = 0;
        } else if (daysLeft >= 0 && daysLeft <= 5) {
            // 0-5 days before the concert
            quality = item.getInitialQuality() * 3;
        } else if (daysLeft > 5 && daysLeft <= 10) {
            // 6-10 days before the concert
            quality = item.getInitialQuality() * 2;
        } else {
            // more than 10 days before the, age like Brie
            quality = item.getInitialQuality() + day;
        }
        
        return constraint.apply(quality);
    }

    @Override
    public boolean appliesTo(Item item) {
        return item.getCategory().equals("Backstage Passes");
    }
}
