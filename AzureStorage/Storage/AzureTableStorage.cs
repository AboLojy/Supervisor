
using AzureStorage.Interfaces;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureStorage.StorageManager
{
    public class AzureTableStorage : ITableStorage
    {
        private readonly CloudTableClient _client;
        private ConcurrentDictionary<string, CloudTable> _tables;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureTableStorage" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public AzureTableStorage(string connectionString)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            _client = account.CreateCloudTableClient();

            _tables = new ConcurrentDictionary<string, CloudTable>();
        }

        public async Task<T> GetAsync<T>(string tableName, string partitionKey, string rowKey) where T : class, ITableEntity
        {
            var table = await EnsureTable(tableName).ConfigureAwait(false);

            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);

            TableResult result = await table.ExecuteAsync(retrieveOperation).ConfigureAwait(false);

            return result.Result as T;
        }





        /// <summary>
        /// Adds the or update entity.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task<object> AddOrUpdateAsync(string tableName, ITableEntity entity)
        {
            var table = await EnsureTable(tableName).ConfigureAwait(false);

            TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(entity);

            TableResult result = await table.ExecuteAsync(insertOrReplaceOperation).ConfigureAwait(false);

            return result.Result;
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task<object> DeleteAsync(string tableName, ITableEntity entity)
        {
            var table = await EnsureTable(tableName).ConfigureAwait(false);

            TableOperation deleteOperation = TableOperation.Delete(entity);

            TableResult result = await table.ExecuteAsync(deleteOperation).ConfigureAwait(false);

            return result.Result;
        }

        /// <summary>
        /// Add the entity.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task<object> AddAsync(string tableName, ITableEntity entity)
        {
            var table = await EnsureTable(tableName).ConfigureAwait(false);

            TableOperation insertOperation = TableOperation.Insert(entity);

            TableResult result = await table.ExecuteAsync(insertOperation).ConfigureAwait(false);

            return result.Result;
        }


        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task<object> UpdateAsync(string tableName, ITableEntity entity)
        {
            var table = await EnsureTable(tableName).ConfigureAwait(false);

            TableOperation replaceOperation = TableOperation.Replace(entity);

            TableResult result = await table.ExecuteAsync(replaceOperation).ConfigureAwait(false);

            return result.Result;
        }

        /// <summary>
        /// Ensures existence of the table.
        /// </summary>
        private async Task<CloudTable> EnsureTable(string tableName)
        {
            if (!_tables.ContainsKey(tableName))
            {
                var table = _client.GetTableReference(tableName);
                await table.CreateIfNotExistsAsync().ConfigureAwait(false);
                _tables[tableName] = table;
            }

            return _tables[tableName];
        }
        
        /// <summary>
        /// Gets entities by query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private async Task<IEnumerable<T>> QueryAsync<T>(CloudTable table, TableQuery<T> query)
            where T : class, ITableEntity, new()
        {
            var entities = new List<T>();

            TableContinuationToken token = null;
            do
            {
                var queryResult = await table.ExecuteQuerySegmentedAsync(query, token).ConfigureAwait(false);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);

            return entities;
        }


    
    }
}
