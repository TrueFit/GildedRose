/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.uniqueculture.gildedrose;

import org.uniqueculture.gildedrose.spi.InventoryItem;
import org.uniqueculture.gildedrose.spi.Item;

/**
 *
 * @author me
 */
public class Application {

    public static final void main(String[] args) {
        String inventoryFile = null;
        String command = null;
        String commandParam = null;

        if (args.length < 2) {
            printUsage("Invalid number of arguments");
            System.exit(0);
        }

        inventoryFile = args[0];
        command = args[1].toLowerCase();
        commandParam = args.length > 2 ? args[2] : null;

        // Create the inventory
        InventoryHelper helper = InventoryHelper.getCsvInventory(inventoryFile);
        if (helper == null) {
            printUsage("Unable to read inventory file");
            System.exit(0);
        }

        // Interpret the command
        switch (command) {
            case "list":
                // List all inventory items
                helper.getInventoryItems().stream()
                        .forEach((inventoryItem) -> printItem(inventoryItem.getItem()));
                break;
            case "item":
                // Details of one item
                if (commandParam == null) {
                    printUsage("Name parameter of the item is required");
                    System.exit(0);
                }

                InventoryItem item = helper.getInventoryItem(commandParam);
                if (item == null) {
                    System.out.println("Not found");
                } else {
                    printItem(item.getItem());
                }
                break;
            case "progress":
                // List all inventory items with adjusted quality on a given day
                if (commandParam == null) {
                    printUsage("Day parameter is require");
                    System.exit(0);
                }

                try {
                    int day = Integer.parseUnsignedInt(commandParam);
                    // List all items
                    helper.getInventoryItems().stream()
                            .forEach((inventoryItem) -> printItem(inventoryItem.getItem(), day, inventoryItem.getQuality(day)));
                } catch (NumberFormatException ex) {
                    printUsage("Day parameter is not an integer");
                    System.exit(0);
                }
                break;
            case "trash":
                // List all trash items on a given day
                if (commandParam == null) {
                    printUsage("Day parameter is require");
                    System.exit(0);
                }

                try {
                    int day = Integer.parseUnsignedInt(commandParam);
                    // List all items
                    helper.getTrash(day).stream()
                            .forEach((inventoryItem) -> printItem(inventoryItem.getItem()));
                } catch (NumberFormatException ex) {
                    printUsage("Day parameter is not an integer");
                    System.exit(0);
                }
                break;
            default:
                // Unknown command
                printUsage("Unknown commad: " + command);
                System.exit(0);
                break;
        }
    }

    public static void printUsage(String error) {
        if (error != null) {
            System.err.println("Error: " + error);
            System.err.println("");
        }
        
        System.out.println("Usage: java -jar GildedRose-1.0-SNAPSHOT.jar <inventory csv> <command> [<command parameter>]");
        System.out.println("Commands:");
        System.out.println(" list \t\t\t- list all inventory items");
        System.out.println(" item <name>\t\t- find an item by name");
        System.out.println(" progress <day>\t\t- list all inventory items with calculated quality on the given day. Day 0 is the first day.");
        System.out.println(" trash <day>\t\t- list inventory with quality of 0 on a given day. Day 0 is the first day.");
    }

    public static void printItem(Item item) {
        System.out.println("------- Item ---------");
        System.out.println("Name: " + item.getName());
        System.out.println("Category: " + item.getCategory());
        System.out.println("Sell In: " + item.getSellIn());
        System.out.println("Quality: " + item.getInitialQuality());
    }

    public static void printItem(Item item, int day, int quality) {
        System.out.println("------- Item ---------");
        System.out.println("Name: " + item.getName());
        System.out.println("Category: " + item.getCategory());
        System.out.println("Sell In: " + item.getSellIn());
        System.out.println("Quality: " + item.getInitialQuality());
        System.out.println("Qaulity on day " + day + ": " + quality);
    }

}
