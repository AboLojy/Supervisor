using AzureStorage.Interfaces;

using System;

namespace AzureStorage.DTO
{
    public class DataQueueMessage: IQueueMessage
    {
        public DateTimeOffset? ExpiresOn { get;  }
        public DateTimeOffset? InsertedOn { get;  }
        public string PopReceipt { get;  }
        public string MessageId { get;  }
        public DateTimeOffset? NextVisibleOn { get;  }
        public string Body { get;  }

        

        public DataQueueMessage(DateTimeOffset? expiresOn, DateTimeOffset? insertedOn, string popReceipt, string messageId, DateTimeOffset? nextVisibleOn, string body)
        {
            ExpiresOn = expiresOn;
            InsertedOn = insertedOn;
            PopReceipt = popReceipt;
            MessageId = messageId ?? throw new ArgumentNullException(nameof(messageId));
            NextVisibleOn = nextVisibleOn;
            Body = body ?? throw new ArgumentNullException(nameof(body));
        }
    }
}
