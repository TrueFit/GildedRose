package com.gildedrose.service;

import java.time.LocalDate;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.gildedrose.model.Item;
import com.gildedrose.model.SystemDate;

import groovy.lang.GroovyShell;

@Service
@Transactional(readOnly = false)
public class InventoryCalculationServiceImpl implements InventoryCalculationService {

	@Autowired
	private InventoryService inventoryService;

	/* -- PUBLIC METHODS -- */

	@Override
	public void progressDate() {

		// Increment the inventory date
		SystemDate inventoryDate = inventoryService.getInventoryDate();
		inventoryDate.setDate(inventoryDate.getDate().plusDays(1));

		// Update the sell-in, quality and possibliy discarded date of each item
		for (Item item : inventoryService.getAvailableItems()) {
			updateItem(item, inventoryDate.getDate());
		}
	}

	/* -- INTERNAL METHODS -- */

	void updateItem(Item item, LocalDate inventoryDate) {

		// Assert that we are never updating an item that is already discarded
		if (item.isDiscarded())
			throw new IllegalStateException(String.format("Item with id %d is already discarded", item.getId()));

		// Calculate the new sell-in and quality
		updateSellIn(item);
		updateQuality(item);

		// Set the item as discarded if the quality has reached zero
		if (item.getQuality() == 0)
			item.setDiscardedDate(inventoryDate);
	}

	void updateSellIn(Item item) {

		// Default the sell-in change to subtract one day
		int sellInChange = -1;

		// Let the item's definition or category indicate if we change sell-in
		if (item.getDefinition().computeIgnoreSellIn())
			sellInChange = 0;

		// Update the sell-in value
		if (sellInChange != 0)
			item.setSellIn(item.getSellIn() + sellInChange);
	}

	void updateQuality(Item item) {

		// Default the quality change to subtract one point
		int qualityChange = -1;

		// Let the item's definition or category provide an alternate expression to
		// calculate the quality change
		String expression = item.getDefinition().computeQualityChangeExpression();

		if (expression != null) {
			GroovyShell shell = new GroovyShell();
			shell.setProperty("sellIn", item.getSellIn());
			shell.setProperty("quality", item.getQuality());
			shell.setProperty("defaultChange", qualityChange);
			qualityChange = (int) shell.evaluate(expression);
		}

		// Once the sell by date has passed, degrade quality twice as fast (only for
		// items which degrade)
		if (item.getSellIn() <= 0 && qualityChange < 0)
			qualityChange *= 2;

		// Calculate the new quality (without limits)
		int quality = item.getQuality() + qualityChange;

		// Ensure the quality isn't negative
		if (quality < 0)
			quality = 0;

		// Ensure the quality isn't greater than 50 (only for items with increasing
		// quality)
		if (quality > 50 && qualityChange > 0)
			quality = 50;

		// Update the quality
		item.setQuality(quality);
	}
}
