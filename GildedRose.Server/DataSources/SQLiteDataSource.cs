using GildedRose.Contracts;
using GildedRose.Server.Utils;
using System.Collections.Generic;
using System.Data.SQLite;

namespace GildedRose.Server.IO
{
    /// <summary>
    /// SQLite implementation of a data source
    /// </summary>
    public class SQLiteDataSource : IDataSource
    {
        /// <inheritdoc />
        public void CreateNew(IList<Item> items)
        {
            using (var connection = new SQLiteConnection($"URI=file:{FileUtils.GetDataBaseFileName()}"))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "DROP TABLE IF EXISTS inventory";
                    command.ExecuteNonQuery();

                    command.CommandText = "CREATE TABLE inventory (id INTEGER PRIMARY KEY, name VARCHAR(255), category VARCHAR(255), sellIn INT, quality INT)";
                    command.ExecuteNonQuery();

                    foreach (var item in items)
                    {
                        command.CommandText = $"INSERT INTO inventory (name, category, sellIn, quality) VALUES ('{item.Name}', '{item.Category}', {item.SellIn}, {item.Quality})";
                        command.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }
        }

        /// <inheritdoc />
        public IList<Item> GetAllItems(out IList<string> errors)
        {
            errors = new List<string>();

            var importedItems = new List<Item>();

            using (var connection = new SQLiteConnection($"URI=file:{FileUtils.GetDataBaseFileName()}"))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT * FROM inventory", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            importedItems.Add(new Item()
                            {
                                Name = reader.GetString(1),
                                Category = reader.GetString(2),
                                SellIn = reader.GetInt32(3),
                                Quality = reader.GetInt32(4)
                            });
                        }
                    }
                }

                connection.Close();
            }

            return importedItems;
        }
    }
}
