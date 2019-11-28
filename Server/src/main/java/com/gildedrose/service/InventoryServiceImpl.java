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

		// Retrieve all items from inventory
		List<Item> items = getItems();

		// If there are not items, just return
		if (items.isEmpty())
			return;

	}
}
