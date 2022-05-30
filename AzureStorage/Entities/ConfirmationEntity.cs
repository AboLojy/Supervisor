
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorage.Entities
{
    public class ConfirmationEntity:TableEntity
    {
        public ConfirmationEntity()
        {

        }

        public ConfirmationEntity( string partitionkey,string rowkey, int orderId, Guid agentId, string orderStatus)
        {
            OrderId = orderId;
            AgentId = agentId;
            OrderStatus = orderStatus;
            RowKey = rowkey;
            PartitionKey = partitionkey;

        }

        public int OrderId { get; set; }
        public Guid AgentId { get; set; }
        public string OrderStatus { get; set; }
    }
}
