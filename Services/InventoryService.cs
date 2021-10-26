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
        private IEnumerable<IItem> _list;

        public InventoryService(IDataService service, IItemFactory factory)
        {
            _service = service;
            _factory = factory;
        }

        public async Task<IEnumerable<IItem>> GetInventory()
        {
            var reader = await _service.DownloadData();
            using (reader)
            {
                _list = await CreateItemList(reader);
            }

            return _list;
        }

        public IEnumerable<IItem> CloseOutDay()
        {
            foreach (var item in _list)
            {
                item.UpdateItem();
            }

            return _list;
        }
        private async Task<IEnumerable<IItem>> CreateItemList(StreamReader reader)
        {
            var records = new List<IItem>();
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
