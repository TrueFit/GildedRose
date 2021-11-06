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

                    command.CommandText = "CREATE TABLE inventory (id INTEGER PRIMARY KEY, guid TEXT, name TEXT, category TEXT, sellIn INT, quality INT)";
                    command.ExecuteNonQuery();

                    foreach (var item in items)
                    {
                        command.CommandText = $"INSERT INTO inventory (guid, name, category, sellIn, quality) VALUES ('{item.Guid}', '{item.Name}', '{item.Category}', {item.SellIn}, {item.Quality})";
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
                                Guid = reader.GetString(1),
                                Name = reader.GetString(2),
                                Category = reader.GetString(3),
                                SellIn = reader.GetInt32(4),
                                Quality = reader.GetInt32(5)
                            });
                        }
                    }
                }

                connection.Close();
            }

            return importedItems;
        }

        /// <inheritdoc />
        public void AddItem(Item item)
        {
            using (var connection = new SQLiteConnection($"URI=file:{FileUtils.GetDataBaseFileName()}"))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = $"INSERT INTO inventory (guid, name, category, sellIn, quality) VALUES ('{item.Guid}', '{item.Name}', '{item.Category}', {item.SellIn}, {item.Quality})";
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        /// <inheritdoc />
        public Item GetItemByName(string name)
        {
            Item item = null;

            using (var connection = new SQLiteConnection($"URI=file:{FileUtils.GetDataBaseFileName()}"))
            {
                connection.Open();

                using (var command = new SQLiteCommand($"SELECT * FROM inventory WHERE name='{name}'", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            item = new Item()
                            {
                                Guid = reader.GetString(1),
                                Name = reader.GetString(2),
                                Category = reader.GetString(3),
                                SellIn = reader.GetInt32(4),
                                Quality = reader.GetInt32(5)
                            };
                        }
                    }
                }

                connection.Close();
            }

            return item;
        }

        /// <inheritdoc />
        public void UpdateConditions(IList<ProgressedItem> progressedItems)
        {
            using (var connection = new SQLiteConnection($"URI=file:{FileUtils.GetDataBaseFileName()}"))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    foreach (var progressedItem in progressedItems)
                    {
                        command.CommandText = $"UPDATE inventory SET sellIn = '{progressedItem.SellIn}', quality = '{progressedItem.Quality}' WHERE guid = '{progressedItem.Guid}'";
                        command.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }
        }

        /// <inheritdoc />
        public void RemoveItems(IList<string> guids)
        {
            using (var connection = new SQLiteConnection($"URI=file:{FileUtils.GetDataBaseFileName()}"))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    foreach (var guid in guids)
                    {
                        command.CommandText = $"DELETE FROM inventory WHERE guid = '{guid}'";
                        command.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }
        }
    }
}
