package com.moose.gildedrose.inventory;

import com.moose.gildedrose.inventory.model.ItemCategoryType;
import com.moose.gildedrose.inventory.model.RawInventoryItem;
import com.opencsv.bean.BeanVerifier;
import com.opencsv.exceptions.CsvConstraintViolationException;

/**
 * A {@link BeanVerifier} for {@link RawInventoryItem}s.
 * This provides runtime validation against any potential data issues when constructing classes from the {@code inventory.txt} file.
 */
public class RawItemValidator implements BeanVerifier<RawInventoryItem> {
    @Override
    public boolean verifyBean(final RawInventoryItem item) throws CsvConstraintViolationException {
        if (item.getQuality() > 50 && !item.getCategory().equals(ItemCategoryType.LEGENDARY)) {
            throw new CsvConstraintViolationException(String.format("The quality of an item is never more than 50, unless %s.", ItemCategoryType.LEGENDARY));
        }
        if (item.getQuality() != 80 && item.getCategory().equals(ItemCategoryType.LEGENDARY)) {
            throw new CsvConstraintViolationException(String.format("The quality of a %s item is supposed to be 80.", ItemCategoryType.LEGENDARY));
        }
        if (item.getQuality() < 0) {
            throw new CsvConstraintViolationException("The quality of an item is never negative.");
        }
        return true;
    }
}
