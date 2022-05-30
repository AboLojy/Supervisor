using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorage.Interfaces
{
    public interface IQueueMessage
    {
         DateTimeOffset? ExpiresOn { get; }
         DateTimeOffset? InsertedOn { get;  }
         string PopReceipt { get;  }
         string MessageId { get; }
         DateTimeOffset? NextVisibleOn { get; }
         string Body { get; }
    }
}
