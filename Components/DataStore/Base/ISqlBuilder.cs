using GildedRose.Store.Contracts;

namespace GildedRose.Store.Base
{
    public interface ISqlBuilder
    {
        ISqlBuilder AddParameters(dynamic parameters);

        ITemplate AddTemplate(string sql, dynamic parameters = null);

        ISqlBuilder GroupBy(string sql, dynamic parameters = null);

        ISqlBuilder Having(string sql, dynamic parameters = null);

        ISqlBuilder InnerJoin(string sql, dynamic parameters = null);

        ISqlBuilder Intersect(string sql, dynamic parameters = null);

        ISqlBuilder Join(string sql, dynamic parameters = null);

        ISqlBuilder LeftJoin(string sql, dynamic parameters = null);

        ISqlBuilder OrderBy(string sql, dynamic parameters = null);

        ISqlBuilder OrWhere(string sql, dynamic parameters = null);

        ISqlBuilder RightJoin(string sql, dynamic parameters = null);

        ISqlBuilder Select(string sql, dynamic parameters = null);

        ISqlBuilder Where(string sql, dynamic parameters = null);

        object GetBaseBuilder();
    }
}