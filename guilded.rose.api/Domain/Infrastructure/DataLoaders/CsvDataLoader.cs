using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CsvHelper;
using guilded.rose.api.Domain.Models;

namespace guilded.rose.api.Domain.Infrastructure.DataLoaders
{
    public class CsvDataLoader : IDataLoader
    {
        public string Source { get; private set; }
        public CsvDataLoader(string source)
        {
            this.Source = source;
        }

        public List<Import> Build()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(this.Source))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    var csvReader = new CsvReader(reader);
                    csvReader.Configuration.HasHeaderRecord = false;

                    var records = csvReader.GetRecords<Import>();

                    return records.ToList();
                }

            }
        }
    }
}