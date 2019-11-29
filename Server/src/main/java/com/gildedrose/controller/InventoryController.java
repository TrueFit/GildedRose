package com.gildedrose.controller;

import java.util.List;
import java.util.stream.Collectors;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.gildedrose.dto.EmptyDTO;
import com.gildedrose.dto.ItemDTO;
import com.gildedrose.model.Item;
import com.gildedrose.service.InventoryService;

@RestController
@RequestMapping("/api/inventory")
public class InventoryController {

	@Autowired
	private InventoryService inventoryService;

	/* -- PUBLIC METHODS -- */

	/**
	 * Retrieves all items in the inventory, ordered by name and quality.
	 */
	@GetMapping("/items")
	public List<ItemDTO> getItems() {
		List<Item> items = inventoryService.getItems();

		return items.stream().map(i -> convertToDTO(i)).collect(Collectors.toList());
	}

	/**
	 * Progresses the inventory date one day, updating quality calculations for all
	 * items in the inventory.
	 */
	@PostMapping("/progress-date")
	public EmptyDTO progressDate() {
		inventoryService.progressDate();

		return new EmptyDTO();
	}

	/* -- PRIVATE METHODS -- */

	private static ItemDTO convertToDTO(Item item) {
		ItemDTO dto = new ItemDTO();
		dto.setId(item.getId());
		dto.setName(item.getDefinition().getName());
		dto.setCategory(item.getDefinition().getCategory().getName());
		dto.setSellIn(item.getSellIn());
		dto.setQuality(item.getQuality());
		return dto;
	}
}
