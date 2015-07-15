package com.moose.gildedrose.inventory;

import com.moose.gildedrose.inventory.model.ItemCategoryType;
import com.moose.gildedrose.inventory.model.RawInventoryItem;
import com.opencsv.bean.AbstractBeanField;
import com.opencsv.exceptions.CsvConstraintViolationException;
import java.util.Optional;

/**
 * Implementation of {@link AbstractBeanField} that converts the {@link String} representation of an {@link RawInventoryItem}'s
 * category into an {@link ItemCategoryType} enumeration for compiler-safety and data validation.
 */
public class StringToItemCategoryType extends AbstractBeanField<RawInventoryItem, Integer> {
    @Override
    public Object convert(final String value) throws CsvConstraintViolationException {
        return Optional.ofNullable(ItemCategoryType.getByDescription(value))
            .orElseThrow(() -> new CsvConstraintViolationException("No known category of " + value + ". Please add an appropriate ItemCategoryType."));
    }
}
