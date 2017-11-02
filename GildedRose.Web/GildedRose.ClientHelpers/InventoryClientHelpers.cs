using GildedRose.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GildedRose.ClientHelpers
{
    public class InventoryClientHelpers
    {
        public InventoryItem InventoryItem { get; set; }
        public List<InventoryItem> InventoryItems { get; set; }

        public async Task GetItemByName(string name)
        {
            // Use default credentials aka Windows Service Account Credentials that this app pool is running under.
            var handler = new HttpClientHandler()
            {
                UseDefaultCredentials = true
            };

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["GildedRoseApiUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("api/InventoryItems/item/" + name).GetAwaiter().GetResult();
                InventoryItem = await response.Content.ReadAsAsync<InventoryItem>();
            }
        }

        public async Task GetInventoryItems()
        {
            // Use default credentials aka Windows Service Account Credentials that this app pool is running under.
            var handler = new HttpClientHandler()
            {
                UseDefaultCredentials = true
            };

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["GildedRoseApiUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("api/InventoryItems/items").GetAwaiter().GetResult();
                InventoryItems = await response.Content.ReadAsAsync<List<InventoryItem>>();
            }
        }

        public async Task GetZeroQualityInventoryItems()
        {
            // Use default credentials aka Windows Service Account Credentials that this app pool is running under.
            var handler = new HttpClientHandler()
            {
                UseDefaultCredentials = true
            };

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["GildedRoseApiUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("api/InventoryItems/Zero").GetAwaiter().GetResult();
                InventoryItems = await response.Content.ReadAsAsync<List<InventoryItem>>();
            }
        }

        public async Task ResetToInitialState()
        {
            // Use default credentials aka Windows Service Account Credentials that this app pool is running under.
            var handler = new HttpClientHandler()
            {
                UseDefaultCredentials = true
            };

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["GildedRoseApiUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("api/InventoryItems/InitialState").GetAwaiter().GetResult();
                InventoryItems = await response.Content.ReadAsAsync<List<InventoryItem>>();
            }
        }
    }
}
