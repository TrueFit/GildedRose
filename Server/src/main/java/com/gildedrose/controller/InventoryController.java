package com.gildedrose.controller;

import java.util.List;
import java.util.stream.Collectors;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.gildedrose.dto.DateDTO;
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

	@GetMapping("/inventory-date")
	public DateDTO getInventoryDate() {
		return new DateDTO(inventoryService.getInventoryDate());
	}

	@GetMapping("/available-items")
	public List<ItemDTO> getAvailableItems() {
		return inventoryService.getAvailableItems().stream().map(i -> convertToDTO(i)).collect(Collectors.toList());
	}

	@GetMapping("/discarded-items")
	public List<ItemDTO> getDiscardedItems() {
		return inventoryService.getDiscardedItems().stream().map(i -> convertToDTO(i)).collect(Collectors.toList());
	}

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
		dto.setDiscardedDate(item.getDiscardedDate());
		return dto;
	}
}
