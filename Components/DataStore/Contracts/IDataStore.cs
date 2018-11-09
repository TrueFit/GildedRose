using GildedRose.Store.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GildedRose.Store.Contracts
{
    public interface IDataStore
    {
        int UserId { get; }

        Task<T> FirstOrDefaultAsync<T>(string query, object parameters = null, int? timeout = null);

        Task<IEnumerable<T>> GetListAsync<T>(string query, object parameters = null, int? timeout = null);

        Task<Dictionary<TKey, TValue>> GetDictionaryAsync<TKey, TValue>(string query, string keyName, string valueName, object parameters = null, int? timeout = null);

        Task<IEnumerable<T>> GetListAsync<T>(string query, CommandType commandType, object parameters = null, int? timeout = null);

        Task UpdateAsync<T>(string query, T entity, int? timeout = null)
            where T : class;

        Task<TResult> InsertAsync<TResult>(string query, object entity, int? timeout = null);

        Task InsertAsync(string query, object entity, int? timeout = null);

        Task<int> ExecuteAsync(string query, object parameters, int? timeout = null);

        Task<T> ExecuteScalerAsync<T>(string query, object parameters, int? timeout = null);

        Task<T> ExecuteScalerAsync<T>(string query, int? timeout = null);

        Task<int> ExecuteStoredProcedureAsync(string query, object parameters, int? timeout = null);

        Task<IGridReader> QueryMultipleAsync(string query, object parameters, int? timeout = null); //Return was SqlMapper.GridReader

        Task<string> GetResourceAsync(string resourcePath);

        string GetResource(string resourcePath);
    }
}
