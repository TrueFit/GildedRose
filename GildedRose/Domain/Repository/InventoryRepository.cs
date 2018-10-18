using GildedRose.Domain.Entity;
using GildedRose.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace GildedRose.Domain.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private IList<Item> items;
        ItemFactory factory = new ConcreteItemFactory();

        public InventoryRepository()
        {
            items = ReadInventoryFile();
        }

        public IList<Item> All()
        {
            if (items == null)
            {
                items = ReadInventoryFile();
            }
            return items;
        }

        /**
         * Specialized repository query
         * if using an ORM or other data storage repository, will want to
         * define this to not filter off of ALL data, but instead use the database
         * or storage tech's querying logic instead
         **/
        public Item GetByName(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return null;
            return All().Where(i => name.Equals(i.Name)).SingleOrDefault();
        }

        /**
         * pass a boolean function used to filter the data
         * this method can be used in place of speicialezd queries in the repository, if you're so inclined
         **/
        public IList<Item> GetByCondition(Func<Item, bool> whereCondition)
        {
            return All().Where(whereCondition).ToList();
        }

        /**
         * NOTE: this is NOT threadsafe!!!
         * for threadsafety, use a real datasource, not a file
         **/
        public void Save(IList<Item> items)
        {
            string filename = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "inventory.txt");
            string format = "MM-dd-yyyy_HHmmss";
            string archiveFilename = String.Format("{0}_{1}", filename, DateTime.Now.ToString(format));
            if (File.Exists(filename))
            {
                File.Move(filename, archiveFilename);
            }

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(filename))
            {
                foreach (Item item in items)
                {
                        file.WriteLine(item.ToString());
                }
            }

        }

        /**
         * A true repository wouldn't have this, but would instead create a connection pool
         * to a database, a queue,... some storage tech.  Reading a file here for brevity
         * If the point of the exercise was to prove ability to connect to a database,
         * would be happy to set one up and add an ORM to this project instead
         **/
        private IList<Item> ReadInventoryFile()
        {
            IList<Item> fileItems = new List<Item>();
            using (StreamReader sr = new StreamReader(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "inventory.txt")))
            {
                String line = sr.ReadLine();
                while (line != null)
                {
                    try
                    {
                        string[] fields = line.Split(',');
                        if (fields == null || fields.Length != 4)
                        {
                            throw new Exception("Incorrect number of fields in the data.");
                        }
                        string name = fields[0];
                        string category = fields[1];
                        int sellIn = int.Parse(fields[2]);
                        int quality = int.Parse(fields[3]);
                        Item fileItem = factory.GetItem(name, category, sellIn, quality);
                        fileItems.Add(fileItem);
                    } catch (Exception e)
                    {
                        Console.WriteLine(String.Format("Exception: {0}", e.Message));
                    }

                    line = sr.ReadLine();
                }
            }

            return fileItems;
        }
    }
}