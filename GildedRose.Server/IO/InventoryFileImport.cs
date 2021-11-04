using GildedRose.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GildedRose.Server.IO
{
    /// <summary>
    /// Helper class for file operations regarding inventory items.
    /// </summary>
    public class InventoryFileImport
    {
        /// <summary>
        /// Import the current inventory list.
        /// </summary>
        /// <param name="fileName">The full path to the inventory list.</param>
        /// <param name="errors">The errors that happened during the file import.</param>
        /// <returns>All imported items from the inventory list.</returns>
        public static IList<Item> ImportItems(string fileName, out IList<string> errors)
        {
            errors = new List<string>();

            var importedItems = new List<Item>();

            // Make sure, the file does exist.
            if (!File.Exists(fileName))
            {
                return importedItems;
            }

            // Import the inventory file.
            var streamReader = new StreamReader(fileName, Encoding.Default);

            var line = string.Empty;
            while ((line = streamReader.ReadLine()) != null)
            {
                // There could be empty line, if Allison got tipsy for closing down the store.
                if (!string.IsNullOrEmpty(line))
                {
                    // Let's split the line into fragments and ensure that all values are set and valid.
                    var parts = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 4)
                    {
                        if (int.TryParse(parts[2], out var sellIn) && int.TryParse(parts[3], out var quality))
                        {
                            importedItems.Add(new Item()
                            {
                                Name = parts[0],
                                Category = parts[1],
                                SellIn = sellIn,
                                Quality = quality
                            });
                        }
                        else
                        {
                            errors.Add($"Could not process the line \"{line}\". SellIn or quality value could not be parsed as number.");
                        }
                    }
                    else
                    {
                        errors.Add($"Could not process the line \"{line}\". There are not enough parameters.");
                    }
                }
            }

            streamReader.Close();

            return importedItems;
        }
    }
}
