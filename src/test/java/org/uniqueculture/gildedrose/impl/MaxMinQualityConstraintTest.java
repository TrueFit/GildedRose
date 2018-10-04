/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.uniqueculture.gildedrose.impl;

import org.junit.Test;
import static org.junit.Assert.*;

/**
 *
 * @author Sergei Izvorean <uniqueculture at gmail.com>
 */
public class MaxMinQualityConstraintTest {
    
    public MaxMinQualityConstraintTest() {
    }

    /**
     * Test of apply method, of class MaxMinQualityConstraint.
     */
    @Test
    public void testApply() {
        MaxMinQualityConstraint constraint = new MaxMinQualityConstraint(10, 0);
        assertEquals(0, constraint.apply(-1));
        assertEquals(0, constraint.apply(-10));
        assertEquals(0, constraint.apply(0));
        assertEquals(10, constraint.apply(10));
        assertEquals(10, constraint.apply(11));
        assertEquals(5, constraint.apply(5));
    }
    
}
