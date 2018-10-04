/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.uniqueculture.gildedrose.impl.calculator;

import org.junit.Before;
import org.junit.Test;
import static org.junit.Assert.*;
import org.mockito.Mockito;
import static org.mockito.Mockito.when;
import org.uniqueculture.gildedrose.spi.Item;

/**
 *
 * @author Sergei Izvorean <uniqueculture at gmail.com>
 */
public class ConjuredQualityCalculatorTest {
    
    ConjuredQualityCalculator calculator;
    
    @Before
    public void setUp() {
        calculator = new ConjuredQualityCalculator((int quality) -> quality);
    }

    /**
     * Test of calculate method, of class ConjuredQualityCalculator.
     */
    @Test
    public void testCalculate() {
        Item item = Mockito.mock(Item.class);
        when(item.getInitialQuality()).thenReturn(10);
        when(item.getSellIn()).thenReturn(20);
        
        // "Conjured" items degrade in Quality twice as fast as normal items
        
        // 0th day - should be the same quality
        assertEquals(10, calculator.calculate(item, 0));
        
        // 1st day - should increase at 2x
        assertEquals(8, calculator.calculate(item, 1));
     
        // do not test for max/min since it's controlled by a constraint
    }

    /**
     * Test of appliesTo method, of class ConjuredQualityCalculator.
     */
    @Test
    public void testAppliesTo() {
        Item pass = Mockito.mock(Item.class);
        Item fail = Mockito.mock(Item.class);
        
        when(pass.getCategory()).thenReturn("Conjured");
        when(fail.getCategory()).thenReturn("Not conjured");
        
        assertTrue(calculator.appliesTo(pass));
        assertFalse(calculator.appliesTo(fail));
    }
    
}
