package com.gildedrose.service;

import java.util.List;

import com.gildedrose.model.Item;
import com.gildedrose.model.SystemDate;

public interface InventoryService {

	/**
	 * Retrieves the current date inventory items have been progressed to.
	 */
	SystemDate getInventoryDate();

	/**
	 * Retrieves all items which are available to sell. Resulting items are sorted
	 * first by name, then by sell-in date, and finally by quality.
	 */
	List<Item> getAvailableItems();

	/**
	 * Retrieves all items whose quality has reached zero, and need to be discarded
	 * from the inventory. Resulting items are sorted first by discarded date and
	 * then by name.
	 */
	List<Item> getDiscardedItems();

	/**
	 * Retrieves a single item by its id.
	 */
	Item getItem(long id);

	/**
	 * Retrieves a list of all items with the given name.
	 */
	List<Item> getItemsByName(String name);

	/**
	 * Instructs the system to progress the inventory date to the next day, thus
	 * recalculating the sell-in and quality values for each available item. Items
	 * whose quality becomes zero are marked as discarded.
	 */
	void progressDate();
}
