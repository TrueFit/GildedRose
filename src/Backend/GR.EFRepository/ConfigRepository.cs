using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GR.Models;

namespace GR.Repositories.EF
{
    public class ConfigRepository : EfRepositoryBase<Entities.InventoryDb>, IConfigRepository
    {
        public const string KEY_SIMULATION_DATE_OFFSET = "SimulationDateOffset";

        #region Constructor and Dependencies
        public ConfigRepository(Entities.ConfigDbFactory dbFactory)
            : base(dbFactory)
        { }
        #endregion

        public Config GetConfiguration()
        {
            //This needs to be done synchronously so it can be called from the ASP.NET Core 
            //  ConfigureServices method, which must run synchrously.
            using (var db = NewDbContext())
            {
                var configs = db.Configs.ToDictionary(cfg => cfg.Key, cfg => cfg.Value);
                return
                    new Models.Config
                    {
                        SimulationDateOffset = TryGetConfigValue(configs, KEY_SIMULATION_DATE_OFFSET,
                            value => TimeSpan.TryParse(value, out TimeSpan result) ? result : TimeSpan.Zero),
                    };
            }
        }

        public Task SetSimulationDateOffset(TimeSpan offset)
            => QueryDbAsync(
                async db =>
                {
                    (db.Configs.Find(KEY_SIMULATION_DATE_OFFSET)
                        ?? db.Configs.Add(new Entities.Config { Key = KEY_SIMULATION_DATE_OFFSET }))
                        .Value = offset.ToString();
                    await db.SaveChangesAsync();
                });

        private T TryGetConfigValue<T>(Dictionary<string, string> configDict, string key, Func<string, T> converterFunc)
            => configDict.TryGetValue(key, out string value) ? converterFunc(value) : default(T);
    }
}
