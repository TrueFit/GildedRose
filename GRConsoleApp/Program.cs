using System;
using System.Collections.Generic;
using System.Linq;
using GR.BusinessLogic;
using GR.BusinessLogic.Models;
using Microsoft.Extensions.CommandLineUtils;

namespace GRConsoleApp {
    class Program {
        static void Main (string[] args) {

            bool loopApp = true;
            string helpString = "-?|-h|--help";
            Console.WriteLine ("Welcome to the Glided Rose CLI. ");
            Console.WriteLine ("Enter a command.  Type --Help for help or Exit to close...");

            while (loopApp) {
                Console.WriteLine ("Giled Rose CLI> ");

                string commandArg = Console.ReadLine ().ToLower ();

                // Write a blank line.
                Console.WriteLine("");
                //Console.WriteLine (string.Format("You typed: {0}", commandArg));
                string[] appArgs = commandArg.Split(" ");

                if (commandArg.Length > 0) {
                    try {
                        var clApp = new CommandLineApplication ();
                        clApp.Name = "Gilded Rose CLI";
                        clApp.HelpOption (helpString);

                        clApp.Command ("inventory", (command) => {
                            command.Description = "Display all Item invneotry.";
                            command.OnExecute (() => {
                                ShowAllInventory ();
                                return 0;
                            });
                        });

                        clApp.Command ("show", (command) => {
                            command.Description = "Show a specific Item that is in Inventory";
                            command.HelpOption(helpString);

                            var itemArgument = command.Argument ("[name]",
                                "Name of the item to display");

                            command.OnExecute (() => {
                                if (itemArgument.Value == null) {
                                    Console.WriteLine("Please specify a Item to show.");
                                } 
                                else {
                                    ShowItem(itemArgument.Value);
                                }
                                return 0;

                            });
                        });

                        clApp.Command ("trash", (command) => {
                            command.Description = "Display list of items to send to the trash.";
                            command.HelpOption (helpString);
                            command.OnExecute (() => {
                                ShowTrash ();
                                return 0;
                            });
                        });

                        clApp.Command ("end-of-day", (command) => {
                            command.Description = "Preform the End of Day operaion.";
                            command.HelpOption (helpString);
                            command.OnExecute (() => {
                                PerformEndOfDay ();
                                return 0;
                            });
                        });

                        clApp.Command ("import", (command) => {
                            command.Description = "Preform Inventory Import from the text file inventory.txt.";
                            command.HelpOption (helpString);
                            command.OnExecute (() => {
                                ImportInventory();
                                return 0;
                            });
                        });

                        clApp.Command ("exit", (command) => {
                            command.Description = "Closes the CLI";
                            command.HelpOption (helpString);
                            command.OnExecute (() => {
                                loopApp = false;
                                return 0;
                            });
                        });

                        clApp.Execute (appArgs);

                        // Write a blank line.
                        Console.WriteLine("");

                    } 
                    catch (Exception ex) {
                        string errorMessage = ex.Message;
                        if (errorMessage.StartsWith("Unrecognized command or argument ")){
                            // Handle invalid command
                            Console.WriteLine (string.Format ("Unrecognized command: {0}.  Please try again..", commandArg));
                        }
                        else{
                            // Display other errors
                            Console.WriteLine(string.Format("Error: {0}", errorMessage));
                        }
                        // Write a blank line.
                        Console.WriteLine("");
                    }
                }
            }
        }

        private static void ShowAllInventory () {
            Inventory inventory = new Inventory ();

            Console.WriteLine ("Load All Items from database...");
            List<Item> items = inventory.GetAllItems ();

            foreach (Item item in items) {
                Console.WriteLine (string.Format ("Item: {0},  Category: {1}, SellIn: {2}, Quality: {3}",
                    item.Name, item.Category, item.SellIn, item.Quality));

            }

            if (items.Count == 0) {
                Console.WriteLine ("There are no items in the database..");
            }
        }

        private static void ShowItem(string itemName){
            Inventory inventory = new Inventory();
            Item item = inventory.GetItem(itemName);
            if (item == null){
                Console.WriteLine(string.Format("Item: {0} was not found in Inventory.", itemName));
                return;
            }

            Console.WriteLine(String.Format("Name: {0}", item.Name));
            Console.WriteLine(String.Format("Category: {0}", item.Category));
            Console.WriteLine(String.Format("Sell In: {0}", item.SellIn));
            Console.WriteLine(String.Format("Quality: {0}", item.Quality));
        }

         private static void PerformEndOfDay(){
            Console.WriteLine(string.Format("Start End Of Day Process.."));
            try{
                Inventory inventory = new Inventory();
                inventory.EndTheDay();

                Console.WriteLine(string.Format("End Of Day Process complete.."));
            }
            catch (Exception ex){
                Console.WriteLine(string.Format("Error in End of Day Process: {0}", ex.Message));
            } 
        }

         private static void ShowTrash(){
            Inventory inventory = new Inventory ();

            Console.WriteLine ("Get List of items for the trash...");
            List<Item> items = inventory.GetTrashList();

            foreach (Item item in items) {
                Console.WriteLine (string.Format ("Item: {0},  Catrgory: {1}, SellIn: {2}, Quality: {3}",
                    item.Name, item.Category, item.SellIn, item.Quality));
            }

            if (items.Count == 0) {
                Console.WriteLine ("There are no items for the trash today..");
            }
        }

        public static void ImportInventory(){
            Inventory inventory = new Inventory();
            Console.WriteLine("Importing Inventory from inventory.txt");

            inventory.ImportInventory();

            Console.WriteLine("Inventory Load Complete...");
        }
    }
}