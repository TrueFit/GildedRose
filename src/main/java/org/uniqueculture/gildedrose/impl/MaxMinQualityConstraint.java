/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.uniqueculture.gildedrose.impl;

import org.uniqueculture.gildedrose.spi.QualityConstraint;

/**
 *
 * @author me
 */
public class MaxMinQualityConstraint implements QualityConstraint {
    
    private final int max;
    private final int min;

    public MaxMinQualityConstraint(int max, int min) {
        this.max = max;
        this.min = min;
    }

    @Override
    public int apply(int quality) {
        return quality > max ? max : (quality < min ? min : quality);
    }
    
    
    
}
