using Azure.Storage.Queues;
using AzureStorage.DTO;
using AzureStorage.Interfaces;
using System.Threading.Tasks;

namespace AzureStorage.StorageManager

{
    public class AzureQueueStorage : IQueueStorage
    {
        QueueClient _client;
        
        public AzureQueueStorage(string connectionString, string name)
        {
            _client = new QueueClient(connectionString, name);
        }

        /// <summary>
        /// Delete message from Queue
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task DeleteMessageAsync(IQueueMessage msg)
        {
            if (!_client.Exists()) return;
            
            await _client.DeleteMessageAsync(msg.MessageId, msg.PopReceipt).ConfigureAwait(false);
            
        }
        /// <summary>
        /// Create Queue if not exists 
        /// </summary>
        /// <returns></returns>
        public async Task CreateIfNotExistsAsync()
        {
            await _client.CreateIfNotExistsAsync().ConfigureAwait(false);
        }


        /// <summary>
        /// Get message from queue
        /// </summary>
        /// <returns></returns>
        public async Task<IQueueMessage> GetMessageAsync()
        {
            if (!_client.Exists()) return null;//TimeSpan.FromMilliseconds(Milliseconds)
            var receivedMessage = await _client.ReceiveMessageAsync().ConfigureAwait(false);
            if (receivedMessage.Value == null) return null;
            var msg = new DataQueueMessage( receivedMessage.Value.ExpiresOn,
                                            receivedMessage.Value.InsertedOn,
                                            receivedMessage.Value.PopReceipt,
                                            receivedMessage.Value.MessageId,
                                            receivedMessage.Value.NextVisibleOn,
                                            receivedMessage.Value.Body.ToString());
            return msg;
        }

        /// <summary>
        /// Insert message into the queue
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<IQueueMessage> InsertMessageAsync(string message)
        {
            await _client.CreateIfNotExistsAsync().ConfigureAwait(false);
            
            var receivedMessage = await _client.SendMessageAsync(message).ConfigureAwait(false);
            var msg = new DataQueueMessage(receivedMessage.Value.ExpirationTime,
                                            receivedMessage.Value.InsertionTime,
                                            receivedMessage.Value.MessageId,
                                            receivedMessage.Value.PopReceipt,
                                            null,
                                            message);
            return msg;
        }

        /// <summary>
        /// Peek a message from queue
        /// </summary>
        /// <returns>IQueueMessage</returns>
        public async Task<IQueueMessage> PeekMessageAsync()
        {
            
            if (!_client.Exists()) return null;
            var receivedMessage = await _client.PeekMessageAsync().ConfigureAwait(false);
            var msg = new DataQueueMessage(receivedMessage.Value.ExpiresOn,
                                            receivedMessage.Value.InsertedOn,
                                            null,
                                            receivedMessage.Value.MessageId,
                                            null,
                                            receivedMessage.Value.Body.ToString());
            return msg;
        }

        /// <summary>
        /// Update message 
        /// </summary>
        /// <param name="dataMessage"> edited message </param>
        /// <returns></returns>
        public async Task UpdateMessageAsync(IQueueMessage dataMessage)
        {
            if (!_client.Exists()) return ;
                
            var result = await _client.UpdateMessageAsync(dataMessage.MessageId, dataMessage.PopReceipt, dataMessage.Body).ConfigureAwait(false);
             
        }
    }
}
