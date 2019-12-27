using GildedRoseInventory;
using System;
using System.Collections.Generic;
using System.IO;

namespace GildedRose
{
    class Program
    {
        static void Main(string[] args)
        {
            // read arguments and validate file exists
            if (args.Length < 1)
            {
                Console.WriteLine($"Usage: {System.AppDomain.CurrentDomain.FriendlyName} inventory_file");
                return;
            }

            var fileName = args[0];
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"ERROR: Inventory file not found. \"{fileName}\"");
                return;
            }

            try
            {
                Inventory inventory = new Inventory(fileName);

                while (true)
                {
                    Console.Clear();
                    Header(fileName);

                    var response = MainMenu();
                    if (response == "1")
                    {
                        Console.Clear();
                        DisplayItems(inventory.GetProducts());
                        AnyKeyToContinue();
                    }
                    else if (response == "2")
                    {
                        Console.Clear();
                        var itemName = PrompName();
                        var item = inventory.GetProduct(itemName);
                        if (item == null)
                        {
                            Console.WriteLine($"Item \"{itemName}\" not found in inventory!");
                            Console.WriteLine();
                            AnyKeyToContinue();
                        }
                        else
                        {
                            while (true)
                            {
                                Console.WriteLine();
                                DisplayItem(item);
                                var res = ItemMenu(item);
                                if (res == "1")
                                {
                                    break;
                                }
                                else if (res == "2")
                                {
                                    if (PromptYesNo("Are you sure you want to remove item (Y/N)? "))
                                    {
                                        inventory.RemoveProduct(item);
                                        break;
                                    }
                                }
                                else if (res == "3")
                                {
                                    item.Name = PromptNewName(item);
                                    inventory.SortProducts();
                                }
                                else if (res == "4")
                                {
                                    item.Category = PromptNewCategory(item);
                                    inventory.SortProducts();
                                }
                                else if (item.Category != "Sulfuras" && res == "5")
                                {
                                    item.SellIn = PromptNewSellIn();
                                }
                                else if (item.Category != "Sulfuras" && res == "6")
                                {
                                    item.Quality = PromptNewQuality();
                                }
                                else
                                    Console.Beep();
                                Console.Clear();
                            }
                        }
                    }
                    else if (response == "3")
                    {
                        Console.Clear();
                        inventory.NextDay();
                        DisplayItems(inventory.GetProducts());
                        Console.WriteLine($"Days that have passed: {inventory.DaysPassed}");
                        Console.WriteLine();
                        AnyKeyToContinue();
                    }
                    else if (response == "4")
                    {
                        Console.Clear();
                        var trashItems = inventory.GetTrashProducts();
                        DisplayItems(trashItems);
                        if (trashItems.Count > 0)
                        {
                            if (PromptYesNo("Would you like to remove all trash items (Y/N)? "))
                            {
                                foreach (var item in trashItems)
                                    inventory.RemoveProduct(item);
                            }
                        }
                        else
                            AnyKeyToContinue();
                    }
                    else if (response == "5")
                    {
                        break;
                    }
                    else
                    {
                        Console.Beep();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }


        private static void Header(string fileName)
        {
            Console.WriteLine(@" _____ _ _     _          _  ______               ");
            Console.WriteLine(@"|  __ (_) |   | |        | | | ___ \              ");
            Console.WriteLine(@"| |  \/_| | __| | ___  __| | | |_/ /___  ___  ___ ");
            Console.WriteLine(@"| | __| | |/ _` |/ _ \/ _` | |    // _ \/ __|/ _ \");
            Console.WriteLine(@"| |_\ \ | | (_| |  __/ (_| | | |\ \ (_) \__ \  __/");
            Console.WriteLine(@" \____/_|_|\__,_|\___|\__,_| \_| \_\___/|___/\___|");
            Console.WriteLine(@"                                                  ");
            Console.WriteLine($"Inventory System");
            Console.WriteLine($"[{fileName}]");
        }

        private static string MainMenu()
        {
            Console.WriteLine();
            Console.WriteLine("1. List entire inventory");
            Console.WriteLine("2. Details of a single item");
            Console.WriteLine("3. Progress to the next day");
            Console.WriteLine("4. List of trash to throw away");
            Console.WriteLine("5. Exit");
            Console.WriteLine();
            Console.Write("Enter number (1-5): ");
            return Console.ReadLine();
        }

        private static void DisplayItems(IList<Product> items)
        {
            if (items.Count == 0)
            {
                Console.WriteLine("No items found");
                return;
            }

            Console.Write("Name".PadRight(25));
            Console.Write("Category".PadRight(18));
            Console.Write("Sell In".PadLeft(8));
            Console.WriteLine("Quality".PadLeft(10));
            Console.WriteLine("=============================================================");

            foreach (var item in items)
            {
                Console.Write(item.Name.PadRight(25));
                Console.Write(item.Category.PadRight(18));
                Console.Write(item.SellIn.ToString().PadLeft(8));
                Console.WriteLine(item.Quality.ToString().PadLeft(10));
            }
            Console.WriteLine();
        }

        private static void DisplayItem(Product item)
        {
            Console.Write("Name: ".PadRight(12));
            Console.WriteLine(item.Name);
            Console.Write("Category: ".PadRight(12));
            Console.WriteLine(item.Category);
            Console.Write("Sell In: ".PadRight(12));
            Console.WriteLine(item.SellIn);
            Console.Write("Quality: ".PadRight(12));
            Console.WriteLine(item.Quality);
        }

        private static void AnyKeyToContinue()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }

        private static string ItemMenu(Product item)
        {
            Console.WriteLine();
            Console.WriteLine("1. Back to main menu");
            Console.WriteLine("2. Remove item");
            Console.WriteLine("3. Change name");
            Console.WriteLine("4. Change category");
            if (item.Category != "Sulfuras")
            {
                Console.WriteLine("5. Change sell in");
                Console.WriteLine("6. Change quantity");
                Console.WriteLine();
                Console.Write("Enter number (1-6): ");
            }
            else
            {
                Console.WriteLine();
                Console.Write("Enter number (1-4): ");
            }
            return Console.ReadLine();
        }

        private static string PrompName()
        {
            while (true)
            {
                Console.Write("Enter the item's name: ");
                var itemName = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(itemName))
                    return itemName;

                Console.Beep();
            }
        }

        private static string PromptNewName(Product item)
        {
            while (true)
            {
                Console.Write($"Enter new name ({item.Name}): ");
                var itemName = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(itemName))
                    return itemName;

                Console.Beep();
            }
        }

        private static string PromptNewCategory(Product item)
        {
            while (true)
            {
                Console.Write($"Enter new category ({item.Category}): ");
                var itemCategory = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(itemCategory))
                    return itemCategory;

                Console.Beep();
            }
        }

        private static int PromptNewSellIn()
        {
            while (true)
            {
                Console.Write("Enter new sell in number (0-1000): ");
                var itemSellInStr = Console.ReadLine();

                int itemSellIn;
                if (!string.IsNullOrWhiteSpace(itemSellInStr) &&
                    int.TryParse(itemSellInStr, out itemSellIn) &&
                    itemSellIn > 0)
                {
                    return itemSellIn;
                }

                Console.Beep();
            }
        }

        private static int PromptNewQuality()
        {
            while (true)
            {
                Console.Write($"Enter new quality number (0-50): ");

                var itemQualityStr = Console.ReadLine();

                int itemQuality;
                if (!string.IsNullOrWhiteSpace(itemQualityStr) &&
                    int.TryParse(itemQualityStr, out itemQuality) &&
                    itemQuality >= 0 && itemQuality <= 50)
                {
                    return itemQuality;
                }

                Console.Beep();
            }
        }

        private static bool PromptYesNo(string msg)
        {
            while (true)
            {
                Console.WriteLine();
                Console.Write(msg);
                var res = Console.ReadLine().ToLower();
                if (res == "y" || res == "n")
                    return res == "y";
                Console.Beep();
            }
        }
    }
}
