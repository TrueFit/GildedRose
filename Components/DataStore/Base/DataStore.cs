using System;
using System.Reflection;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using GildedRose.Core.Contracts;
using GildedRose.Store.Contracts;
using GildedRose.Store.Wrappers;

namespace GildedRose.Store.Base
{
    public class DataStore : IDataStore
    {
        private static Dictionary<string, string> sqlStatements;

        private readonly Func<IDbConnection> connectionFactory;

        static DataStore()
        {
            sqlStatements = new Dictionary<string, string>();
        }

        //TODO: Remove this overload when authentication is functional
        public DataStore(Func<IDbConnection> connection)
        {
            this.UserId = 1;
            this.connectionFactory = connection;
        }

        public DataStore(int userId, Func<IDbConnection> connection)
        {
            this.UserId = userId;
            this.connectionFactory = connection;
        }

        public int UserId { get; private set; }

        public async Task<T> FirstOrDefaultAsync<T>(string query, object parameters = null, int? timeout = null)
        {
            using (var connection = this.connectionFactory())
            {
                var result = await connection.QueryAsync<T>(query, parameters, commandTimeout: timeout);
                return result.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<T>> GetListAsync<T>(string query, object parameters = null, int? timeout = null)
        {
            using (var connection = this.connectionFactory())
            {
                var result = await connection.QueryAsync<T>(query, parameters, commandTimeout: timeout);
                return result.ToList();
            }
        }

        public async Task<IEnumerable<T>> GetListAsync<T>(string query, CommandType commandType, object parameters = null, int? timeout = null)
        {
            using (var connection = this.connectionFactory())
            {
                var result = await connection.QueryAsync<T>(sql: query, param: parameters, commandTimeout: timeout, commandType: commandType);
                return result.ToList();
            }
        }

        public async Task UpdateAsync<T>(string query, T entity, int? timeout = null)
            where T : class
        {
            var modifyableEntity = entity as IEditable;
            if (modifyableEntity != null)
            {
                modifyableEntity.Modified = DateTime.UtcNow;
                modifyableEntity.ModifiedBy = this.UserId;
            }

            using (var connection = this.connectionFactory())
            {
                await connection.ExecuteAsync(sql: query, param: entity, commandTimeout: timeout);
            }
        }

        public async Task<TResult> InsertAsync<TResult>(string query, object entity, int? timeout = null)
        {
            var creatableEntity = entity as ICreatable;
            if (creatableEntity != null)
            {
                creatableEntity.CreatedBy = this.UserId;
                creatableEntity.Created = DateTime.UtcNow;
            }

            using (var connection = this.connectionFactory())
            {
                return await connection.ExecuteScalarAsync<TResult>(sql: query, param: entity, commandTimeout: timeout);
            }
        }

        public async Task InsertAsync(string query, object entity, int? timeout = null)
        {
            var creatableEntity = entity as ICreatable;
            if (creatableEntity != null)
            {
                creatableEntity.CreatedBy = this.UserId;
                creatableEntity.Created = DateTime.UtcNow;
            }

            using (var connection = this.connectionFactory())
            {
                var results = await connection.ExecuteAsync(sql: query, param: entity, commandTimeout: timeout);
            }
        }

        public async Task<int> ExecuteAsync(string query, object parameters, int? timeout = null)
        {
            using (var connection = this.connectionFactory())
            {
                return await connection.ExecuteAsync(query, parameters, commandTimeout: timeout);
            }
        }

        public async Task<T> ExecuteScalerAsync<T>(string query, object parameters, int? timeout = null)
        {
            using (var connection = this.connectionFactory())
            {
                return await connection.ExecuteScalarAsync<T>(query, parameters, commandTimeout: timeout);
            }
        }

        public async Task<T> ExecuteScalerAsync<T>(string query, int? timeout = null)
        {
            using (var connection = this.connectionFactory())
            {
                return await connection.ExecuteScalarAsync<T>(query, commandTimeout: timeout);
            }
        }

        public async Task<int> ExecuteStoredProcedureAsync(string query, object parameters, int? timeout = null)
        {
            using (var connection = this.connectionFactory())
            {
                return await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
            }
        }

        public async Task<IGridReader> QueryMultipleAsync(string query, object parameters, int? timeout = null)
        {
            var connection = this.connectionFactory();
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            var result = await connection.QueryMultipleAsync(query, parameters, commandTimeout: timeout);
            return new DapperGridReader(result);
        }

        public async Task<string> GetResourceAsync(string resourceName)
        {
            string key = $"GildedRose.DataStore.{resourceName}";

            if (sqlStatements.ContainsKey(key))
            {
                return sqlStatements[key];
            }

            var assembly = typeof(DataStore).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(key);
            using (stream)
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = await reader.ReadToEndAsync();
                    sqlStatements.Add(key, result);
                    return result;
                }
            }
        }

        public string GetResource(string resourceName)
        {
            string key = $"GildedRose.Store.{resourceName}";

            if (sqlStatements.ContainsKey(key))
            {
                return sqlStatements[key];
            }

            var assembly = typeof(DataStore).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(key);
            using (stream)
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    sqlStatements.Add(key, result);
                    return result;
                }
            }
        }

        public async Task<Dictionary<TKey, TValue>> GetDictionaryAsync<TKey, TValue>(string query, string keyName, string valueName, object parameters = null, int? timeout = null)
        {
            var dict = new Dictionary<TKey, TValue>();
            using (var connection = this.connectionFactory())
            {
                using (var reader = await connection.ExecuteReaderAsync(query, parameters, commandTimeout: timeout))
                {
                    var keyOrdinal = reader.GetOrdinal(keyName);
                    var valueOrdinal = reader.GetOrdinal(valueName);
                    while (reader.Read())
                    {
                        var key = (TKey)reader[keyOrdinal];
                        var value = (TValue)reader[valueOrdinal];
                        dict.Add(key, value);
                    }
                }
            }

            return dict;
        }
    }
}