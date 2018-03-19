using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using GR.BusinessLogic.Models;

namespace GR.BusinessLogic{
    public class Inventory{
        Hashtable inventoryListHashTable = new Hashtable();
        DatabaseContext databaseContext ;
        public Inventory(){
            databaseContext = new DatabaseContext();
        }

        private void LoadAllInventory(){
            // Refresh the Inventory List
            databaseContext = new DatabaseContext();    
        }

        public void EndTheDay(){
            LoadAllInventory();

            int qualityDegradation = 1;
            int sellInDegradation = 1;
           
            
            List<Item> itemList = databaseContext.Items.ToList();
            foreach (Item item in itemList){
                int qualtityMultiplier = 1;
                int sellInMultipler = 1;
                int maxQualtity = 50;
                
                 // set the sell in and quality multipliers based on the category
                switch (item.Category){
                    case "Aged Brie":
                        // Increases in qiality as it get older
                        qualtityMultiplier = -1;
                        break;
                    
                    case "Sulfuras":
                        // Legendary item - never decreases quality of sellin
                        //                - Qualtity can be higher than normal max quality
                        maxQualtity = item.Quality;
                        qualtityMultiplier = 0;
                        sellInMultipler = 0;
                        break;
                     
                    case "Backstage passes":
                        // Increases in qiality as it get older
                        qualtityMultiplier = -1;
                        
                        if (item.SellIn <= 9){
                            qualtityMultiplier = -2;
                        }
                        if (item.SellIn <= 5){
                            qualtityMultiplier = -3;
                        }

                        // Once the Concert has passed, qualtiy drops to 0;
                        if (item.SellIn == 0){
                            item.Quality = 0;
                            qualtityMultiplier = 1;
                        }
                        break;
                    
                    case "Conjured":
                        // Decreases twice as fast as normal items
                        qualtityMultiplier = 2;
                        break;

                    default:
                        break;
                }

                // if SellIn has passed Item degrades twice as fast
                if (item.SellIn == 0){
                    qualtityMultiplier = 2;
                }

                item.Quality = item.Quality - qualityDegradation * qualtityMultiplier;
                item.SellIn = item.SellIn - sellInDegradation * sellInMultipler;

                // An Items Sell In and Qualtity can never be less than 0;
                if (item.Quality < 0){
                    item.Quality = 0;
                }

                if (item.SellIn < 0){
                    item.SellIn = 0;
                }

                // An Items Qualtity can never be more that the max qualtity
                if (item.Quality > maxQualtity){
                    item.Quality = maxQualtity;
                }
               
                // Update the database context
                databaseContext.Update(item);
            }

            // Save the results to the database
            databaseContext.SaveChanges();
        }

        public Item GetItem(string itemName){
            LoadAllInventory();
            Item item = databaseContext.Items.Find(itemName);
            return item;
        }

        public List<Item> GetAllItems(){
            LoadAllInventory();
            List<Item> itemList = databaseContext.Items.ToList();
            return itemList;
        }

        public List<Item> GetTrashList(){
            LoadAllInventory();
            List<Item> trashList = databaseContext.Items.Where(i => i.Quality == 0).ToList();
            return trashList;           
        }

        public void ImportInventory(){
            // Import Inventory form the Inventory.txt file
        }
    }
}