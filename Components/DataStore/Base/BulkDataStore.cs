using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Common;
using FastMember;

namespace GildedRose.Stores.Base
{
    public class BulkDataStore
    {
        private readonly Func<IDbConnection> connectionFactory;

        public BulkDataStore(int userId, Func<IDbConnection> connection)
        {
            this.UserId = userId;
            this.connectionFactory = connection;
        }

        public int UserId { get; private set; }

        public async Task BulkInsertAsync(DbDataReader reader, string destinationTableName, int? timeout = null)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this.connectionFactory().ConnectionString))
            {
                bulkCopy.DestinationTableName = destinationTableName;
                if (timeout != null)
                {
                    bulkCopy.BulkCopyTimeout = (int)timeout;
                }

                await bulkCopy.WriteToServerAsync(reader);
            }
        }

        public async Task BulkInsertAsync<T>(IEnumerable<T> records, string destinationTableName, int? timeout = null)
        {
            var fields = typeof(T).GetProperties().Select(x => x.Name).ToArray();
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this.connectionFactory().ConnectionString))
            {
                using (var reader = ObjectReader.Create(records, fields))
                {
                    foreach (var member in fields)
                    {
                        bulkCopy.ColumnMappings.Add(member, member);
                    }

                    if (timeout != null)
                    {
                        bulkCopy.BulkCopyTimeout = (int)timeout;
                    }

                    bulkCopy.DestinationTableName = destinationTableName;

                    await bulkCopy.WriteToServerAsync(reader);
                }
            }
        }
    }
}