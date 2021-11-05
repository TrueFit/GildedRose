using GildedRose.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GildedRose.Server.IO
{
    /// <summary>
    /// A simple text file storing the inventory list.
    /// </summary>
    public class InventoryListDataSource : IDataSource
    {
        private readonly string _fileName;

        /// <summary>
        /// Constructor
        /// </summary>
        public InventoryListDataSource(string fileName)
        {
            _fileName = fileName;
        }

        /// <inheritdoc />
        public void CreateNew(IList<Item> items)
        {
            var streamWriter = new StreamWriter(_fileName, false, Encoding.UTF8);

            foreach (var item in items)
                streamWriter.WriteLine($"{item.Name},{item.Category},{item.SellIn},{item.Quality}");

            streamWriter.Close();
        }

        /// <inheritdoc />
        public IList<Item> GetAllItems(out IList<string> errors)
        {
            errors = new List<string>();

            var importedItems = new List<Item>();

            // Make sure, the file does exist.
            if (!File.Exists(_fileName))
            {
                return importedItems;
            }

            // Import the inventory file.
            var streamReader = new StreamReader(_fileName, Encoding.Default);

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
