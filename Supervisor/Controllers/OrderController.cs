using AzureStorage.Entities;
using AzureStorage.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Supervisor.Controllers.DTO;
using Supervisor.Trackers;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Supervisor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly ITracker tracker;
        private readonly ITableStorage azureTableStorage;
        private readonly IQueueStorage azureQueueStorage;

        public OrderController(ILogger<OrderController> logger, ITracker tracker, ITableStorage azureTableStorage, IQueueStorage azureQueueStorage)
        {
            this.logger = logger;
            this.tracker = tracker;
            this.azureTableStorage = azureTableStorage;
            this.azureQueueStorage = azureQueueStorage;
        }
        /// <summary>
        /// Place an order to be processed
        /// </summary>
        /// <param name="orderDto"></param>
        /// <returns></returns>
        [HttpPost]

        public async Task<ActionResult> Post(OrderDTO orderDto)
        {
            try
            {
                // check that order text is not empty
                if (string.IsNullOrWhiteSpace(orderDto.OrderText)) return BadRequest("Invalid Order");
                var random = new Random();

                var order = new Order()
                {
                    OrderId = tracker.Count() + 1,
                    RandomNumber = random.Next(1, 10),
                    OrderText = orderDto.OrderText
                };
                // begin tracking the order and log it to the console
                var result = tracker.Add(order.OrderId, order, $"Send order {order.OrderId} with random number {order.RandomNumber}");
                string AgentId = null;
                ConfirmationEntity confirmation = null;
               
                // send the order to the queue
                await azureQueueStorage.InsertMessageAsync(JsonSerializer.Serialize(order));

                 while (AgentId == null)
                {
                    confirmation = await azureTableStorage.GetAsync<ConfirmationEntity>("confirmation", "orders", order.OrderId.ToString());
                    AgentId = confirmation?.AgentId.ToString();
                    Thread.Sleep(500);
                }

                

                return Ok(confirmation);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500);
            }



        }
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(new { orderCount = tracker.Count() });
        }
    }
}
