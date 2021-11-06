using GildedRose.Contracts;
using GildedRose.DataSource.Utils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace GildedRose.DataSource
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
            RunQuery<object>(null, (cmd) =>
            {
                cmd.CommandText = $"INSERT INTO inventory (guid, name, category, sellIn, quality) VALUES ('{item.Guid}', '{item.Name}', '{item.Category}', {item.SellIn}, {item.Quality})";
                cmd.ExecuteNonQuery();

                return null;
            });
        }

        /// <inheritdoc />
        public void AddItems(IList<Item> items)
        {
            RunQuery<object>(null, (cmd) =>
            {
                foreach (var item in items)
                {
                    cmd.CommandText = $"INSERT INTO inventory (guid, name, category, sellIn, quality) VALUES ('{item.Guid}', '{item.Name}', '{item.Category}', {item.SellIn}, {item.Quality})";
                    cmd.ExecuteNonQuery();
                }

                return null;
            });
        }

        /// <inheritdoc />
        public Item GetItemByName(string name)
        {
            return RunQuery($"SELECT * FROM inventory WHERE name='{name}'", (cmd) =>
            {
                Item item = null;

                using (var reader = cmd.ExecuteReader())
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

                return item;
            });
        }

        /// <inheritdoc />
        public void UpdateConditions(IList<ProgressedItem> progressedItems)
        {
            RunQuery<object>(null, (cmd) =>
            {
                foreach (var progressedItem in progressedItems)
                {
                    cmd.CommandText = $"UPDATE inventory SET sellIn = '{progressedItem.SellIn}', quality = '{progressedItem.Quality}' WHERE guid = '{progressedItem.Guid}'";
                    cmd.ExecuteNonQuery();
                }

                return null;
            });
        }

        /// <inheritdoc />
        public void RemoveItems(IList<string> guids)
        {
            RunQuery<object>(null, (cmd) =>
            {
                foreach (var guid in guids)
                {
                    cmd.CommandText = $"DELETE FROM inventory WHERE guid = '{guid}'";
                    cmd.ExecuteNonQuery();
                }

                return null;
            });
        }

        private T RunQuery<T>(string query, Func<SQLiteCommand, T> func) where T : new()
        {
            var result = new T();

            using (var connection = new SQLiteConnection($"URI=file:{FileUtils.GetDataBaseFileName()}"))
            {
                connection.Open();

                if (string.IsNullOrEmpty(query))
                {
                    using (var command = new SQLiteCommand(connection))
                    {
                        if (func != null)
                        {
                            result = func(command);
                        }
                    }
                }
                else
                {
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        if (func != null)
                        {
                            result = func(command);
                        }
                    }
                }

                connection.Close();
            }

            return result;
        }
    }
}
