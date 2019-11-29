package com.gildedrose.service;

import java.time.LocalDate;
import java.util.List;

import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;

import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.gildedrose.Constants;
import com.gildedrose.model.Item;
import com.gildedrose.model.SystemDate;

@Service
@Transactional(readOnly = true)
class InventoryServiceImpl implements InventoryService {

	@PersistenceContext
	private EntityManager entityManager;

	private static final String getInventoryDateQuery =
	// @formatter:off
		"SELECT d " +
		"FROM SystemDate d " +
		"WHERE d.id = '" + Constants.InventoryDateId + "'"; 
	// @formatter:on

	private static final String getAvailableItemsQuery =
	// @formatter:off
		"SELECT i " +
		"FROM Item i " +
		"INNER JOIN FETCH i.definition d " +
		"INNER JOIN FETCH d.category " +
		"WHERE i.discardedDate IS NULL " +
		"ORDER BY d.name ASC, i.sellIn ASC, i.quality ASC";
	// @formatter:on

	private static final String getDiscardedItemsQuery =
	// @formatter:off
		"SELECT i " +
		"FROM Item i " +
		"INNER JOIN FETCH i.definition d " +
		"INNER JOIN FETCH d.category " +
		"WHERE i.discardedDate IS NOT NULL " +
		"ORDER BY d.name ASC, i.sellIn ASC, i.quality ASC";
	// @formatter:on

	private static final String getItemQuery =
	// @formatter:off
		"SELECT i " +
		"FROM Item i " +
		"INNER JOIN FETCH i.definition d " +
		"INNER JOIN FETCH d.category " +
		"WHERE i.id = :id";
	// @formatter:on

	private static final String getItemsByNameQuery =
	// @formatter:off
		"SELECT i " +
		"FROM Item i " +
		"INNER JOIN FETCH i.definition d " +
		"INNER JOIN FETCH d.category " +
		"WHERE d.name = :name";
	// @formatter:on

	/* -- PUBLIC METHODS -- */

	@Override
	public SystemDate getInventoryDate() {
		return (SystemDate) entityManager.createQuery(getInventoryDateQuery).getSingleResult();
	}

	@Override
	@SuppressWarnings("unchecked")
	public List<Item> getAvailableItems() {
		return entityManager.createQuery(getAvailableItemsQuery).getResultList();
	}

	@Override
	@SuppressWarnings("unchecked")
	public List<Item> getDiscardedItems() {
		return entityManager.createQuery(getDiscardedItemsQuery).getResultList();
	}

	@Override
	public Item getItem(long id) {
		return (Item) entityManager.createQuery(getItemQuery).setParameter("id", id).getSingleResult();
	}

	@Override
	@SuppressWarnings("unchecked")
	public List<Item> getItemsByName(String name) {
		return (List<Item>) entityManager.createQuery(getItemsByNameQuery).setParameter("name", name).getResultList();
	}

	@Override
	@Transactional(readOnly = false)
	public void progressDate() {

		// Increment the inventory date
		SystemDate inventoryDate = getInventoryDate();
		inventoryDate.setDate(inventoryDate.getDate().plusDays(1));

		// Update the sell-in, quality and possibliy discarded date of each item
		for (Item item : getAvailableItems()) {
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

		// Aged brie never need to be sold, so never change its sell-in value
		if ("Aged Brie".equals(item.getName()))
			sellInChange = 0;

		// Sulfuras never need to be sold, so never change its sell-in value
		if ("Sulfuras".equals(item.getCategoryName()))
			sellInChange = 0;

		// Update the sell-in value
		if (sellInChange != 0)
			item.setSellIn(item.getSellIn() + sellInChange);
	}

	void updateQuality(Item item) {

		// Default the quality change to subtract one point
		int qualityChange = -1;

		// Aged brie increases in quality
		if ("Aged Brie".equals(item.getName()))
			qualityChange = 1;

		// Sulfuras never decreases in quality
		if ("Sulfuras".equals(item.getCategoryName()))
			qualityChange = 0;

		// Backstage passes increase more near sell-in, then fall to zero once the
		// sell-in date is past
		if ("Backstage Passes".equals(item.getCategoryName())) {
			if (item.getSellIn() <= 0)
				qualityChange = item.getQuality() * -1;
			else if (item.getSellIn() <= 5)
				qualityChange = 3;
			else if (item.getSellIn() <= 10)
				qualityChange = 2;
			else
				qualityChange = 1;
		}

		// Conjured items degrade twice as fast as normal items
		if ("Conjured".equals(item.getCategoryName()))
			qualityChange = -2;

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
