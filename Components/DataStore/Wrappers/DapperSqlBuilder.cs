using GildedRose.Store.Contracts;
using Dapper;
using GildedRose.Store.Base;

namespace GildedRose.Store.Wrappers
{
    public class DapperSqlBuilder : ISqlBuilder
    {
        private SqlBuilder builder;

        public DapperSqlBuilder()
        {
            this.builder = new SqlBuilder();
        }

        public ISqlBuilder AddParameters(dynamic parameters)
        {
            this.builder = this.builder.AddParameters(parameters);
            return this;
        }

        public ITemplate AddTemplate(string sql, dynamic parameters = null)
        {
            var template = this.builder.AddTemplate(sql, parameters);
            return new DapperTemplate(template);
        }

        public object GetBaseBuilder()
        {
            return this.builder;
        }

        public ISqlBuilder GroupBy(string sql, dynamic parameters = null)
        {
            this.builder = this.builder.GroupBy(sql, parameters);
            return this;
        }

        public ISqlBuilder Having(string sql, dynamic parameters = null)
        {
            this.builder = this.builder.Having(sql, parameters);
            return this;
        }

        public ISqlBuilder InnerJoin(string sql, dynamic parameters = null)
        {
            this.builder = this.builder.InnerJoin(sql, parameters);
            return this;
        }

        public ISqlBuilder Intersect(string sql, dynamic parameters = null)
        {
            this.builder = this.builder.Intersect(sql, parameters);
            return this;
        }

        public ISqlBuilder Join(string sql, dynamic parameters = null)
        {
            this.builder = this.builder.Join(sql, parameters);
            return this;
        }

        public ISqlBuilder LeftJoin(string sql, dynamic parameters = null)
        {
            this.builder = this.builder.LeftJoin(sql, parameters);
            return this;
        }

        public ISqlBuilder OrderBy(string sql, dynamic parameters = null)
        {
            this.builder = this.builder.OrderBy(sql, parameters);
            return this;
        }

        public ISqlBuilder OrWhere(string sql, dynamic parameters = null)
        {
            this.builder = this.builder.OrWhere(sql, parameters);
            return this;
        }

        public ISqlBuilder RightJoin(string sql, dynamic parameters = null)
        {
            this.builder = this.builder.RightJoin(sql, parameters);
            return this;
        }

        public ISqlBuilder Select(string sql, dynamic parameters = null)
        {
            this.builder = this.builder.Select(sql, parameters);
            return this;
        }

        public ISqlBuilder Where(string sql, dynamic parameters = null)
        {
            this.builder = this.builder.Where(sql, parameters);
            return this;
        }
    }
}
