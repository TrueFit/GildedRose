/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.uniqueculture.gildedrose.impl.calculator;

import java.util.Arrays;
import java.util.Collections;
import java.util.List;
import org.uniqueculture.gildedrose.spi.Item;
import org.uniqueculture.gildedrose.spi.QualityConstraint;
import org.uniqueculture.gildedrose.spi.QualityCalculator;

/**
 *
 * @author me
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
