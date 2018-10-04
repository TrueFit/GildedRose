/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.uniqueculture.gildedrose.impl;

import org.uniqueculture.gildedrose.spi.InventoryItem;
import org.uniqueculture.gildedrose.spi.Item;
import org.uniqueculture.gildedrose.spi.QualityCalculator;

/**
 *
 * @author me
 */
public class DefaultInventoryItem implements InventoryItem {
    
    private final Item item;
    private final QualityCalculator calculator;

    public DefaultInventoryItem(Item item, QualityCalculator calculator) {
        this.item = item;
        this.calculator = calculator;
    }

    @Override
    public Item getItem() {
        return this.item;
    }

    @Override
    public QualityCalculator getQualityCalculator() {
        return this.calculator;
    }

    @Override
    public int getQuality(int day) {
        return this.calculator.calculate(item, day);
    }
    
}
