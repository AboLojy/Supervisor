
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Processor.Processors
//{
//    public class OrderProcessor
//    {
//        private readonly IQueueStorage queueStorage;

//        public ITableStorage TableStorage { get; }

//        public OrderProcessor(IQueueStorage queueStorage,ITableStorage tableStorage)
//        {
//            this.queueStorage = queueStorage;
//            TableStorage = tableStorage;
//        }
//        async Task<IQueueMessage> PlaceOrderAsync(byte[] message)
//        {
//            var msg = await queueStorage.InsertMessageAsync(message);
//            return msg;
            
//        }
//        Task UpdateOrder(IQueueMessage message)
//        {
//            throw new NotImplementedException();
//        }
//        Task<IQueueMessage> GetOrder()
//        {
//            throw new NotImplementedException();
//        }

//    }
//}
