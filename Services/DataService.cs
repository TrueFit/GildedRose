using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CsvHelper;
using GildedRose.Data;

namespace GildedRose.Services
{
    public interface IDataService
    {
        Task<StreamReader> DownloadData();
    }

    public class DataService : IDataService
    {
        private const string InventoryUrl =
            "https://raw.githubusercontent.com/crickard62/GildedRose/main/inventory.txt";

        public DataService()
        {
        }

        public async Task<StreamReader> DownloadData()
        {
            var inventoryRequest = WebRequest.Create(InventoryUrl);

            // As a general rule, every piece of code that is not in a view model
            // and/or that does not need to go back on the main thread should use ConfigureAwait false
            var response = await inventoryRequest.GetResponseAsync().ConfigureAwait(false);

            // possible null assignment to a non-nullable entity; if so, throw exception 
           return new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException());
        }

    }
}