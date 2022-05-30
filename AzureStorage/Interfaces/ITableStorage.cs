
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureStorage.Interfaces
{
    public interface ITableStorage
    {
        Task<T> GetAsync<T>(string tableName, string partitionKey, string rowKey) where T : class, ITableEntity;
        Task<object> DeleteAsync(string tableName, ITableEntity entity);
        Task<object> AddOrUpdateAsync(string tableName, ITableEntity entity);
        Task<object> AddAsync(string tableName, ITableEntity entity);
        Task<object> UpdateAsync(string tableName, ITableEntity entity);

    }
}