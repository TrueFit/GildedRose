package com.moose.gildedrose.inventory;

import com.moose.gildedrose.inventory.model.InventoryItem;
import io.swagger.annotations.ApiResponse;
import io.swagger.annotations.ApiResponses;
import java.io.IOException;
import java.util.List;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

@RestController
class InventoryController {
	private static final String GET_INVENTORY_ENDPOINT = "/inventory";
	private static final String AGE_INVENTORY_ENDPOINT = "/inventory/age";
	private static final String UPDATE_INVENTORY_ENDPOINT = "/inventory/update";
	private static final String TRASH_INVENTORY_ENDPOINT = "/inventory/trash";
	private static final String JSON_TYPE = "application/json";

	@GetMapping(value = InventoryController.GET_INVENTORY_ENDPOINT, produces = "application/json")
	public List<InventoryItem> getInventory() throws IOException {
		return InventoryManager.getInventory();
	}

	@PostMapping(value = InventoryController.UPDATE_INVENTORY_ENDPOINT, consumes = "text/plain", produces = InventoryController.JSON_TYPE)
	@ApiResponses(
			@ApiResponse(code = 400, message = "The provided inventory file was malformed.")
	)
	public List<InventoryItem> updateInventory(@RequestBody final String csvString) throws IOException {
		return InventoryManager.updateInventory(csvString);
	}

	@PostMapping(value = InventoryController.AGE_INVENTORY_ENDPOINT, consumes = "text/plain", produces = InventoryController.JSON_TYPE)
	public List<InventoryItem> ageInventory() throws IOException {
		return InventoryManager.ageInventory();
	}

	@PostMapping(value = InventoryController.TRASH_INVENTORY_ENDPOINT, consumes = "text/plain", produces = InventoryController.JSON_TYPE)
	public List<InventoryItem> trashInventory() throws IOException {
		return InventoryManager.trashInventory();
	}

}
