package org.uniqueculture.gildedrose.impl;

import org.uniqueculture.gildedrose.spi.Item;

/**
 * State-less item data model
 *
 * @author Sergei Izvorean
 */
public class DefaultItem implements Item {
    
    private final String name;
    private final String categoryName;
    private final int sellInDay;
    private final int initialQuality;

    /**
     * Create new instance
     * 
     * @param name
     * @param categoryName
     * @param sellInDay
     * @param initialQuality 
     */
    public DefaultItem(String name, String categoryName, int sellInDay, int initialQuality) {
        this.name = name;
        this.categoryName = categoryName;
        this.sellInDay = sellInDay;
        this.initialQuality = initialQuality;
    }

    @Override
    public String getName() {
        return this.name;
    }

    @Override
    public String getCategory() {
        return categoryName;
    }

    @Override
    public int getSellIn() {
        return this.sellInDay;
    }

    @Override
    public int getInitialQuality() {
        return this.initialQuality;
    }
}
