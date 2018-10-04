package org.uniqueculture.gildedrose.spi;

/**
 * Constrain to applied to a quality after calculation
 *
 * @author Sergei Izvorean
 */
public interface QualityConstraint {
    
    /**
     * Apply the constraint onto item quality after calculation
     * 
     * @param quality
     * @return Quality of an item
     */
    int apply(int quality);
    
}
