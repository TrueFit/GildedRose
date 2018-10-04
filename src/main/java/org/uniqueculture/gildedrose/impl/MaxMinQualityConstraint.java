package org.uniqueculture.gildedrose.impl;

import org.uniqueculture.gildedrose.spi.QualityConstraint;

/**
 * Max/min quality constraint to ensure the item quality is between max and min numbers
 * if quality is less than minimum or more than maximum, then either minimum or maximum are returned
 * 
 * @author Sergei Izvorean
 */
public class MaxMinQualityConstraint implements QualityConstraint {
    
    private final int max;
    private final int min;

    /**
     * New quality constraint between maximum and minimum
     *  
     * @param max Maximum quality value, not inclusive
     * @param min Minimum quality value, not inclusive
     */
    public MaxMinQualityConstraint(int max, int min) {
        this.max = max;
        this.min = min;
    }

    /**
     * Applies the constraint
     * 
     * @param quality
     * @return Maximum value if quality is greater, minimum if smaller, original quality otherwise
     */
    @Override
    public int apply(int quality) {
        return quality > max ? max : (quality < min ? min : quality);
    }
    
    
    
}
