using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GildedRose.Data;
using CsvHelper;

namespace GildedRose.Services
{
    public class InventoryService
    {
        private readonly IDataService _service;
        private readonly IItemFactory _factory;
        private static IEnumerable<IItem> _list;

        public InventoryService(IDataService service, IItemFactory factory)
        {
            _service = service;
            _factory = factory;
        }

        public IEnumerable<IItem> ViewActiveInventory() 
        {
            // active items have non-zero Quality
            return _list.Where(i => i.Quality != 0); 
        }

        public IEnumerable<IItem> ViewExpiredInventory()
        {
            // expired items have zero Quality
            return _list.Where(i => i.Quality == 0);
        }
        public bool IsLoaded()
        {
            if ((_list != null))
                // true if there are values; false otherwise. Which means we need to load...
                return _list.Any();
            else
                return false;
        }

        public async Task<IEnumerable<IItem>> GetInventory()
        {
            var reader = await _service.DownloadData();

            //need a using here because we want to make sure we dispose of the reader
            using (reader)
            {
                _list = await CreateItemList(reader);
            }

            return _list;
        }

        public IEnumerable<IItem> CloseOutDay()
        {
            // call UpdateItem for each Item instance in the list
            foreach (var item in _list)
            {
                item.UpdateItem();
            }

            return _list;
        }

        public IEnumerable<IItem> EmptyTrash()
        {
            //replace the existing list with a list that does not contain items with Quality 0
            _list = _list.Where(i => i.Quality != 0).ToList();
            return ViewExpiredInventory();
        }

        private async Task<IEnumerable<IItem>> CreateItemList(StreamReader reader)
        {
            var records = new List<IItem>();

            //need a using here because we want to make sure we dispose of the reader
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            // there's no header in the test data file so we'll build a list by hand
            while (await csv.ReadAsync())
            {
                var record = _factory.CreateItem(
                    name: (string)csv.GetField(0), 
                    category: csv.GetField(1), 
                    sellIn: csv.GetField<int>(2), 
                    quality:  csv.GetField<int>(3));
                records.Add(record);
            }

            return records;
        }

    }
}
