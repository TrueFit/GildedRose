package com.gildedrose.service;

import java.util.List;

import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.gildedrose.model.Item;

@Service
@Transactional(readOnly = true)
class InventoryServiceImpl implements InventoryService {

	/* -- PUBLIC METHODS -- */

	@Override
	public List<Item> getItems() {
		// TODO Auto-generated method stub
		return null;
	}

}
