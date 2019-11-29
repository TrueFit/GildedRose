package com.gildedrose.service;

import java.util.List;

import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;

import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.gildedrose.model.Item;

@Service
@Transactional(readOnly = true)
class InventoryServiceImpl implements InventoryService {

	@PersistenceContext
	private EntityManager entityManager;

	private static final String getItemsQuery =
	// @formatter:off
		"SELECT i " +
		"FROM Item i " +
		"INNER JOIN FETCH i.definition d " +
		"INNER JOIN FETCH d.category " +
		"ORDER BY d.name ASC, i.quality ASC";
	// @formatter:on

	/* -- PUBLIC METHODS -- */

	@SuppressWarnings("unchecked")
	@Override
	public List<Item> getItems() {
		return entityManager.createQuery(getItemsQuery).getResultList();
	}

	@Override
	@Transactional(readOnly = false)
	public void progressDate() {

		// Update the sell-in and quality for each item
		for (Item item : getItems()) {
			updateItem(item);
		}
	}

	/* -- INTERNAL METHODS -- */

	void updateItem(Item item) {
		updateSellIn(item);
		updateQuality(item);
	}

	void updateSellIn(Item item) {

		// Default the sell-in change to subtract one day
		int sellInChange = -1;

		// Sulfuras never need to be sold, so never change its sell-in value
		if ("Sulfuras".equals(item.getDefinition().getCategory().getName()))
			sellInChange = 0;

		// Update the sell-in value
		if (sellInChange != 0)
			item.setSellIn(item.getSellIn() + sellInChange);
	}

	void updateQuality(Item item) {

		// Default the quality change to subtract one point
		int qualityChange = -1;

		// Sulfuras never decreases in quality
		if ("Sulfuras".equals(item.getDefinition().getCategory().getName()))
			qualityChange = 0;

		// Backstage passes increase more near sell-in, then fall to zero once the
		// sell-in date is past
		if ("Backstage Passes".equals(item.getDefinition().getCategory().getName())) {
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
		if ("Conjured".equals(item.getDefinition().getCategory().getName()))
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
