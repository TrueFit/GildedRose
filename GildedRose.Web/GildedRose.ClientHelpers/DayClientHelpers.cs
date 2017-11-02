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
    public class DayClientHelpers
    {
        public List<InventoryItem> InventoryItems { get; set; }

        public async Task EndDay(int numDays = 1)
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

                try
                {
                    HttpResponseMessage response = client.GetAsync("api/Day/" + numDays).GetAwaiter().GetResult();
                    InventoryItems = await response.Content.ReadAsAsync<List<InventoryItem>>();
                }
                catch (HttpRequestException e)
                {
                    Console.Out.WriteLine(e.Message);
                    throw;
                }
            }
        }
    }
}
