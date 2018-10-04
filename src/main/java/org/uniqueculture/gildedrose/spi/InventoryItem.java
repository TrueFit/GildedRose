/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.uniqueculture.gildedrose.spi;

/**
 *
 * @author me
 */
public interface InventoryItem {
    
    Item getItem();
    
    QualityCalculator getQualityCalculator();
    
    int getQuality(int day);
    
}
