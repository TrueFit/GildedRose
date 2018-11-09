using GildedRose.Core.Contracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace GildedRose.Configuration
{
    public class ConfigurationStore : IConfigurationStore
    {
        private IConfigurationRoot configuration;

        public ConfigurationStore(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        public string GetConfiguration(string configurationName)
        {
            return this.configuration[configurationName];
        }

        public T GetConfiguration<T>(string configurationName)
        {
            var value = this.configuration[configurationName];

            if (!string.IsNullOrEmpty(value))
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }

            return default(T);
        }
    }
}
