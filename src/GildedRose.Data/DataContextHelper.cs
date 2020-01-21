using System.Collections.Generic;
using System.IO;
using System.Reflection;
using GildedRose.Entities.Inventory.Aging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GildedRose.Data
{
    public class DataContextHelper
    {
        public const string AgingRulesFileName = "AgingRules.json";

        /// <summary>
        /// Setup a new database and run migration to setup the initial database state
        /// </summary>
        public static void SetupCleanDatabase()
        {
            var databasePath = GetDatabasePath();
            if (File.Exists(databasePath))
            {
                File.Delete(databasePath);
            }

            var dc = new DataContext();
            dc.Database.Migrate();
        }

        #region Path Methods

        public static string GetRootPath()
        {
            var executionPath = GetDatabaseRootPath();
            return $"{executionPath.Substring(0, executionPath.IndexOf("\\bin"))}\\..\\..";
        }

        public static string GetDatabasePath()
        {
            var executionPath = GetDatabaseRootPath();
            var databasePath = $"{executionPath.Substring(0, executionPath.LastIndexOf("\\"))}\\GildedRose.db";
            return databasePath;
        }

        public static string GetDatabaseRootPath()
        {
            return Assembly.GetExecutingAssembly().Location;
        }

        public static string GetAgingRulesPath()
        {
            return $"{GetRootPath()}\\{AgingRulesFileName}";
        }

        #endregion

        #region Aging Rule Methods

        public static void SaveAgingRules(List<BaseAgingRule> rules)
        {
            var serializedRules = JsonConvert.SerializeObject(rules, new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All});
            File.WriteAllText($"{GetAgingRulesPath()}", serializedRules);
        }

        public static List<BaseAgingRule> LoadAgingRules()
        {
            var serializedContent = File.ReadAllText($"{GetAgingRulesPath()}");
            return JsonConvert.DeserializeObject<List<BaseAgingRule>>(serializedContent, new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto});
        }

        #endregion
    }
}
