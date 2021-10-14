package com.moose.gildedrose;

import com.moose.gildedrose.inventory.common.Constants;
import com.moose.gildedrose.inventory.model.InventoryItem;
import com.moose.gildedrose.inventory.InventoryManager;
import java.io.IOException;
import java.util.List;
import java.util.Optional;
import java.util.Scanner;
import java.util.stream.Collectors;
import lombok.AccessLevel;
import lombok.NoArgsConstructor;
import org.apache.commons.lang3.StringUtils;

@NoArgsConstructor(access = AccessLevel.PRIVATE)
public class Main {

    public static void main(final String[] args) throws IOException {
        String inventoryFilePath = Constants.DEFAULT_INVENTORY_FILE;
        if (args.length == 0) {
            System.out.println("No input file selected, defaulting to inventory.txt.");
        } else {
            inventoryFilePath = args[0];
        }

        final List<InventoryItem> coolList = InventoryManager.loadInventory(inventoryFilePath);
        try (Scanner reader = new Scanner(System.in)) {
            while (true) {
                System.out.println("Command List:");
                System.out.println("    1: Print Inventory");
                System.out.println("    2: Item Detail");
                System.out.println("    3: Progress Day");
                System.out.println("    4: List Trash");
                System.out.println("    5: Exit");
                switch (Optional.ofNullable(reader.nextLine()).map(String::trim).orElse(StringUtils.EMPTY)) {
                    case "1":
                        System.out.println("---Inventory List---");
                        System.out.println(Main.printList(coolList));
                        break;
                    case "2":
                        System.out.println("What item would you like to know more about?");
                        final String userValue = reader.nextLine();
                        System.out.println(coolList.stream()
                                .filter(item -> item.getName().equalsIgnoreCase(userValue))
                                .findFirst()
                                .map(InventoryItem::toString)
                                .orElseGet(() -> String.format("'%s' is not a valid item.", userValue)));
                        break;
                    case "3":
                        coolList.forEach(InventoryItem::ageItem);
                        break;
                    case "4":
                        System.out.println("---Trash List---");
                        System.out.println(Main.printList(coolList.stream().filter(item -> item.getQuality() <= 0).collect(Collectors.toList())));
                        break;
                    case "5":
                        return;
                    default:
                        System.out.println("Invalid value. Please input a number between 1 and 5.");
                        break;
                }
            }
        }
    }

    private static String printList(final List<InventoryItem> itemList) {
        if (itemList == null || itemList.isEmpty()) {
            return "No Items";
        }
        return itemList.stream().map(InventoryItem::toString).collect(Collectors.joining(System.lineSeparator()));
    }

}
