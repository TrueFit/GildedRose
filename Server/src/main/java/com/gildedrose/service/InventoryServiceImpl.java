package com.gildedrose.service;

import java.util.List;

import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;

import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.gildedrose.Constants;
import com.gildedrose.InvocationException;
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
		"ORDER BY i.discardedDate DESC, d.name ASC";
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
		@SuppressWarnings("unchecked")
		List<Item> items = entityManager.createQuery(getItemQuery).setParameter("id", id).getResultList();

		if (items.isEmpty())
			throw new InvocationException(String.format("No item found for id %d", id));

		if (items.size() > 1)
			throw new IllegalStateException(String.format("Multiple items found for id %d", id));

		return items.get(0);
	}

	@Override
	@SuppressWarnings("unchecked")
	public List<Item> getItemsByName(String name) {
		return (List<Item>) entityManager.createQuery(getItemsByNameQuery).setParameter("name", name).getResultList();
	}
}
