/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.uniqueculture.gildedrose.impl;

import org.junit.Test;
import static org.junit.Assert.*;
import org.uniqueculture.gildedrose.impl.calculator.DefaultQualityCalculator;
import org.uniqueculture.gildedrose.spi.InventoryItem;
import org.uniqueculture.gildedrose.spi.Item;
import org.uniqueculture.gildedrose.spi.QualityCalculator;

/**
 *
 * @author Sergei Izvorean <uniqueculture at gmail.com>
 */
public class DefaultInventoryItemFactoryTest {
    
    public DefaultInventoryItemFactoryTest() {
    }
    
    

    /**
     * Test of getInventoryItem method, of class DefaultItemFactory.
     */
    @Test
    public void testNull() {
        try {
            DefaultInventoryItemFactory defaultItemFactory = new DefaultInventoryItemFactory(null);
            fail("Should not allow null for default calculator");
            
            defaultItemFactory.addQualityCalculator(null);
            fail("Should not allow null for calculator");
        } catch (AssertionError ex) {
            
        }
        
        
        
    }

    /**
     * Test of getInventoryItem method, of class DefaultItemFactory.
     */
    @Test
    public void testDefaultCalculator() {
        DefaultInventoryItemFactory factory = new DefaultInventoryItemFactory(new DummyCalculator());
        InventoryItem item = factory.getInventoryItem("Test", "test", 10, 10);
        assertTrue(item.getQualityCalculator() instanceof DummyCalculator);
    }
    
    /**
     * Test specific calculator
     */
    @Test
    public void testCalculator() {
        DefaultInventoryItemFactory factory = new DefaultInventoryItemFactory(new DefaultQualityCalculator(new MaxMinQualityConstraint(50, 10)));
        factory.addQualityCalculator(new DummyCalculator());
        
        InventoryItem item = factory.getInventoryItem("Test", "test", 10, 10);
        assertTrue(item.getQualityCalculator() instanceof DummyCalculator);
    }
    
    class DummyCalculator implements QualityCalculator {

        @Override
        public boolean appliesTo(Item item) {
            return item.getName().equals("Test");
        }

        @Override
        public int calculate(Item item, int day) {
            return item.getInitialQuality();
        }
        
    }
    
}
