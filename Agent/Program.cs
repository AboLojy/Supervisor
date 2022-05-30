using AzureStorage.Entities;
using AzureStorage.StorageManager;
using Models;
using System;
using System.Text.Json;
using System.Threading;

namespace Agent
{
    class Program
    {
        static void Main(string[] args)
        {
            var AgentId = Guid.NewGuid();
            var random = new Random();
            var MagicNumber = random.Next(1, 10);

            var AzureConnection = "UseDevelopmentStorage=true";// just to avoid project complexity
            var AzureQueue = new AzureQueueStorage(AzureConnection, "orders");
            var AzureTable = new AzureTableStorage(AzureConnection);
            Console.WriteLine($"I’m agent {AgentId}, my magic number is {MagicNumber}.");
            ConfirmationEntity confirmationTable = null;
            while (true)
            {
                try
                {

                
                //Create queue if its not exist
                AzureQueue.CreateIfNotExistsAsync().Wait();
                // get message 
                var msg = AzureQueue.GetMessageAsync();
                var queueMsg = msg.Result;
                if (queueMsg == null) continue;
                // Deserialize order from queque
                var order = JsonSerializer.Deserialize<Order>(queueMsg.Body);

                // end when order randomNumber is equal to agent magic number
                if (order.RandomNumber == MagicNumber)
                {
                    break;
                }
                // desplay the order that under processing
                Console.WriteLine($" orderid: {order.OrderId} , RN:{order.RandomNumber}");
                // dequeue the message
                AzureQueue.DeleteMessageAsync(queueMsg).Wait();
                
                confirmationTable = new ConfirmationEntity(partitionkey: "orders",
                                                               rowkey: order.OrderId.ToString(),
                                                               orderId: order.OrderId,
                                                               orderStatus: "Processed",
                                                               agentId: AgentId);
                }
                catch (AggregateException e)
                {
                    foreach (Exception ex in e.InnerExceptions)
                        Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                // this special try is just to avoid problem if there was a data from previous tests
                try
                {
                   
                    AzureTable.AddAsync("confirmation", confirmationTable).Wait();
                    Thread.Sleep(500);
                }
                catch (Exception)
                {
                    Console.WriteLine("Order row already exist, make sure that table is empty");
                }


            }
            Console.WriteLine("Oh no, my magic number was found");
            Console.ReadKey();
        }
    }
}
