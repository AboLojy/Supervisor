using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Supervisor.Trackers
{
    public class OrderTracker : ITracker
    {
        private readonly ILogger<OrderTracker> logger;

        
        public ConcurrentDictionary<int, object> Orders { get; private set; } = new ConcurrentDictionary<int, object>();
        
        public OrderTracker(ILogger<OrderTracker> logger)
        {
            this.logger = logger;

        }

        /// <summary>
        /// Add order to a dictionary for keep orders tracked 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="order"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Add(int orderId,object order, string info)
        {
            lock (this)
            {
                // try add which is thread safe
                Orders.TryAdd(orderId, order);
                logger.LogInformation(info);
                return true;
            }
            
        }

        public int Count()
        {
            return Orders.Count;
        }
    }
}
