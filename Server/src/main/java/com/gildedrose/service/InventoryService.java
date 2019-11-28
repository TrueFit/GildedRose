package com.gildedrose.service;

import java.util.List;

import com.gildedrose.model.Item;

public interface InventoryService {

	List<Item> getItems();

	void progressDate();
}
