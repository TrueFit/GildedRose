/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.uniqueculture.gildedrose.impl.calculator;

import org.junit.Test;
import static org.junit.Assert.*;
import org.mockito.Mockito;
import static org.mockito.Mockito.when;
import org.uniqueculture.gildedrose.spi.Item;
import org.uniqueculture.gildedrose.spi.QualityConstraint;

/**
 *
 * @author Sergei Izvorean <uniqueculture at gmail.com>
 */
public class AgedBrieQualityCalculatorTest {
    

    /**
     * Test of calculate method, of class AgedBrieQualityCalculator.
     */
    @Test
    public void testCalculate() {
        AgedBrieQualityCalculator agedBrieQualityCalculator = new AgedBrieQualityCalculator((int quality) -> quality);
        
        Item agedBrie = Mockito.mock(Item.class);
        when(agedBrie.getInitialQuality()).thenReturn(10);
        when(agedBrie.getSellIn()).thenReturn(10);
        
        // 0th day - should be the same quality
        assertEquals(10, agedBrieQualityCalculator.calculate(agedBrie, 0));
        
        // 5th day - should increase with number of days
        assertEquals(15, agedBrieQualityCalculator.calculate(agedBrie, 5));
     
        // do not test for max/min since it's controlled by a constraint
    }

    /**
     * Test of appliesTo method, of class AgedBrieQualityCalculator.
     */
    @Test
    public void testAppliesTo() {
        AgedBrieQualityCalculator agedBrieQualityCalculator = new AgedBrieQualityCalculator((int quality) -> quality);
        
        Item agedBrie = Mockito.mock(Item.class);
        Item notAgedBrie = Mockito.mock(Item.class);
        
        when(agedBrie.getName()).thenReturn("Aged Brie");
        when(notAgedBrie.getName()).thenReturn("Not Aged Brie");
        
        assertTrue(agedBrieQualityCalculator.appliesTo(agedBrie));
        assertFalse(agedBrieQualityCalculator.appliesTo(notAgedBrie));
    }
    
    
}
