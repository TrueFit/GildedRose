using System;
using System.Linq;
using System.Collections;
using GR.BusinessLogic;

namespace GR.BusinessLogic{
    public class Inventory{
        Hashtable inventoryListHashTable = new Hashtable();
        public Inventory(){
        }

        public void LoadAllInventoy(){
            // Load the entire inventory list

            // Mock for now...
            for (int i = 1; i<= 5; i++){
                Item item = new Item();
                item.Name = string.Format("Item {0}", i);
                item.Quality = 5*i;
                item.SellIn = 12*i;
                inventoryListHashTable.Add(item.Name, item);
            }
        }

        public void EndTheDay(){
            // set the sell in and quality for the end of the data

            // save the results to the database

            // reload the Item list

        }

        public Item GetItemInfo(string itemName){
            Item item = new Item();
            if (inventoryListHashTable.Contains(itemName)){
                item = (Item)inventoryListHashTable[itemName];
                return item;
            }
            return null;

        }

        public Item[] GetAllItems(){
            Item[] items = new Item[inventoryListHashTable.Count];

            int itemCount = 0;
            foreach (Item item in inventoryListHashTable.Values){
                items[itemCount++] = item;
            }  
            return items;
        }

        public Item[] GetTrashList(){
            Item[] items = GetAllItems();
            Item[] trashItems = items.Select(i => i.Quality == 0)
            return (Item[])trashItems;
        }
    }
}