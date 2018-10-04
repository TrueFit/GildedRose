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
import org.uniqueculture.gildedrose.spi.QualityConstraint;

/**
 *
 * @author Sergei Izvorean <uniqueculture at gmail.com>
 */
public class BackstagePassQualityCalculatorTest {
    
    BackstagePassQualityCalculator calculator;

    @Before
    public void setUp() {
        calculator = new BackstagePassQualityCalculator((int quality) -> quality);
    }

    /**
     * Test of calculate method, of class BackstagePassQualityCalculator.
     */
    @Test
    public void testCalculate() {
        Item item = Mockito.mock(Item.class);
        when(item.getInitialQuality()).thenReturn(10);
        when(item.getSellIn()).thenReturn(20);
        
        // increases in Quality as it's SellIn value approaches; 
        // Quality increases by 2 when there are 10 days or less and 
        // by 3 when there are 5 days or less but Quality drops to 0 after the concert
        
        // 0th day - should be the same quality
        assertEquals(10, calculator.calculate(item, 0));
        
        // 1st day - should increase with number of days
        assertEquals(11, calculator.calculate(item, 1));
        
        // 11th day - should be twice as much
        assertEquals(20, calculator.calculate(item, 11));
        
        // 19th day - should be 3x
        assertEquals(30, calculator.calculate(item, 19));
     
        // do not test for max/min since it's controlled by a constraint
    }

    /**
     * Test of appliesTo method, of class BackstagePassQualityCalculator.
     */
    @Test
    public void testAppliesTo() {
        Item pass = Mockito.mock(Item.class);
        Item fail = Mockito.mock(Item.class);
        
        when(pass.getCategory()).thenReturn("Backstage Passes");
        when(fail.getCategory()).thenReturn("Backstage Pass");
        
        assertTrue(calculator.appliesTo(pass));
        assertFalse(calculator.appliesTo(fail));
    }
}
