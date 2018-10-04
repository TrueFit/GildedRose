/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.uniqueculture.gildedrose.impl.calculator;

import org.uniqueculture.gildedrose.impl.calculator.DefaultQualityCalculator;
import org.junit.Assert;
import org.junit.Test;
import org.mockito.Matchers;
import org.mockito.Mockito;
import static org.mockito.Mockito.when;
import org.uniqueculture.gildedrose.spi.Item;
import org.uniqueculture.gildedrose.spi.QualityConstraint;
import org.uniqueculture.gildedrose.spi.QualityCalculator;

/**
 *
 * @author me
 */
public class DefaultQualityCalculatorTest {

    @Test
    public void testCalculateQuality() {
        Item mock = Mockito.mock(Item.class);
        QualityConstraint constraints = Mockito.mock(QualityConstraint.class);
        
        when(mock.getInitialQuality()).thenReturn(30);
        when(mock.getSellIn()).thenReturn(10);
        when(constraints.apply(Matchers.anyInt())).then((invocation) -> {
            int q = (int) invocation.getArguments()[0];
            return q > 50 ? 50 : (q < 0 ? 0 : q);
        });
        
        QualityCalculator cat = new DefaultQualityCalculator(constraints);
        int quality = cat.calculate(mock, 0);
        Assert.assertEquals(30, quality);
        
        quality = cat.calculate(mock, 1);
        Assert.assertEquals(29, quality);
        
        quality = cat.calculate(mock, 10);
        Assert.assertEquals(20, quality);
        
        quality = cat.calculate(mock, 15);
        Assert.assertEquals(10, quality);
        
        quality = cat.calculate(mock, 20);
        Assert.assertEquals(0, quality);
        
        quality = cat.calculate(mock, 100);
        Assert.assertEquals(0, quality);
        
        quality = cat.calculate(mock, -100);
        Assert.assertEquals(50, quality);
    }
    
}
