using GildedRose.Store.Contracts;
using static Dapper.SqlBuilder;

namespace GildedRose.Stores.Wrappers
{
    public class DapperTemplate : ITemplate
    {
        private Template template;

        public DapperTemplate(Template template)
        {
            this.template = template;
        }

        public string RawSql => this.template.RawSql;

        public object Parameters => this.template.Parameters;
    }
}
