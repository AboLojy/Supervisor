using System.Threading.Tasks;

namespace AzureStorage.Interfaces
{

    public interface IQueueStorage
    {
        Task<IQueueMessage> InsertMessageAsync(string message);
        Task<IQueueMessage> GetMessageAsync();
        Task<IQueueMessage> PeekMessageAsync();
        Task UpdateMessageAsync(IQueueMessage dataMessage);
        Task CreateIfNotExistsAsync();
        Task DeleteMessageAsync(IQueueMessage msg);


    }
}
