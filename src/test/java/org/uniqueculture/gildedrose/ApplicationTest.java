/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.uniqueculture.gildedrose;

import java.io.IOException;
import java.net.URI;
import java.net.URISyntaxException;
import java.nio.file.Paths;
import java.util.Arrays;
import static org.junit.Assert.*;
import org.junit.Test;
import org.uniqueculture.gildedrose.impl.CsvInventory;
import org.uniqueculture.gildedrose.impl.DefaultItemFactory;
import org.uniqueculture.gildedrose.impl.MaxMinQualityConstraint;
import org.uniqueculture.gildedrose.impl.calculator.AgedBrieQualityCalculator;
import org.uniqueculture.gildedrose.impl.calculator.BackstagePassQualityCalculator;
import org.uniqueculture.gildedrose.impl.calculator.ConjuredQualityCalculator;
import org.uniqueculture.gildedrose.impl.calculator.DefaultQualityCalculator;
import org.uniqueculture.gildedrose.impl.calculator.SulfurasQualityCalculator;
import org.uniqueculture.gildedrose.spi.Inventory;
import org.uniqueculture.gildedrose.spi.InventoryItemFactory;
import org.uniqueculture.gildedrose.spi.QualityConstraint;

/**
 *
 * @author me
 */
public class ApplicationTest {

    @Test
    public void test() throws IOException, URISyntaxException {
        // Default quality constraints
        QualityConstraint constraints = new MaxMinQualityConstraint(50, 0);

        InventoryItemFactory itemFactory = new DefaultItemFactory(new DefaultQualityCalculator(constraints), Arrays.asList(
                new AgedBrieQualityCalculator(constraints),
                new BackstagePassQualityCalculator(constraints),
                new ConjuredQualityCalculator(constraints),
                new SulfurasQualityCalculator()
        ));

        // Load all the items
        URI inventoryUri = this.getClass().getClassLoader().getResource("inventory.txt").toURI();
        Inventory inventory = new CsvInventory(Paths.get(inventoryUri), itemFactory);
        assertTrue(inventory.getItems().size() > 0);

        int quality = inventory.getInventoryItem("Aged Brie").getQuality(5);
        assertEquals(15, quality);
    }

}
