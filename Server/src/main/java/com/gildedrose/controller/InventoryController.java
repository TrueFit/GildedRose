package com.gildedrose.controller;

import java.util.List;
import java.util.stream.Collectors;

import javax.websocket.server.PathParam;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.gildedrose.dto.DateDTO;
import com.gildedrose.dto.EmptyDTO;
import com.gildedrose.dto.ItemDTO;
import com.gildedrose.model.Item;
import com.gildedrose.service.InventoryCalculationService;
import com.gildedrose.service.InventoryService;

@RestController
@RequestMapping("/api/inventory")
public class InventoryController {

	@Autowired
	private InventoryService inventoryService;

	@Autowired
	private InventoryCalculationService inventoryCalculationService;

	/* -- PUBLIC METHODS -- */

	@GetMapping("/inventory-date")
	public DateDTO getInventoryDate() {
		return new DateDTO(inventoryService.getInventoryDate().getDate());
	}

	@GetMapping("/available-items")
	public List<ItemDTO> getAvailableItems() {
		return inventoryService.getAvailableItems().stream().map(i -> convertToDTO(i)).collect(Collectors.toList());
	}

	@GetMapping("/discarded-items")
	public List<ItemDTO> getDiscardedItems() {
		return inventoryService.getDiscardedItems().stream().map(i -> convertToDTO(i)).collect(Collectors.toList());
	}

	@GetMapping("/items/{id}")
	public ItemDTO getItem(@PathVariable("id") long id) {
		return convertToDTO(inventoryService.getItem(id));
	}

	@GetMapping("/items")
	public List<ItemDTO> getItemsByName(@PathParam("name") String name) {
		return inventoryService.getItemsByName(name).stream().map(i -> convertToDTO(i)).collect(Collectors.toList());
	}

	@PostMapping("/progress-date")
	public EmptyDTO progressDate() {
		inventoryCalculationService.progressDate();
		return new EmptyDTO();
	}

	/* -- PRIVATE METHODS -- */

	private static ItemDTO convertToDTO(Item item) {
		ItemDTO dto = new ItemDTO();
		dto.setId(item.getId());
		dto.setName(item.getName());
		dto.setCategory(item.getCategoryName());
		dto.setSellIn(item.getSellIn());
		dto.setQuality(item.getQuality());
		dto.setDiscardedDate(item.getDiscardedDate());
		return dto;
	}
}
