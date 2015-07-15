package com.moose.gildedrose.inventory.model;

import com.moose.gildedrose.inventory.StringToItemCategoryType;
import com.opencsv.bean.CsvBindByPosition;
import com.opencsv.bean.CsvCustomBindByPosition;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;

/**
 * Class representation of an inventory item that came from the pre-loaded {@code inventory.txt} file.
 * This class uses the {@link CsvBindByPosition} and {@link CsvCustomBindByPosition} annotations from the OpenCSV library
 * in order to appropriately map the expected values from the text file to this object.
 * As the name implies, you specify the position of the value in a line of the file for each of the mapped fields.
 * So an example:
 * <pre>{@code
 * Inventory.txt: A,B,C,D
 * An annotation of @CsvBindByPosition(position = 2) would bind the value of "C" to the given field.
 * }</pre>
 */
@Getter
@NoArgsConstructor
@AllArgsConstructor
public class RawInventoryItem {
    @CsvBindByPosition(required = true, position = 0)
    private String name;

    @CsvCustomBindByPosition(required = true, position = 1, converter = StringToItemCategoryType.class)
    private ItemCategoryType category;

    @CsvBindByPosition(required = true, position = 2)
    private int sellByDays;

    @CsvBindByPosition(required = true, position = 3)
    private int quality;
}
