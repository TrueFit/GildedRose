package com.gildedrose.service;

public interface InventoryCalculationService {

	/**
	 * Instructs the system to progress the inventory date to the next day, thus
	 * recalculating the sell-in and quality values for each available item. Items
	 * whose quality becomes zero are marked as discarded.
	 */
	void progressDate();
}
