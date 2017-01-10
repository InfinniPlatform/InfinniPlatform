using System.Collections.Generic;
using System.Diagnostics;

namespace InfinniPlatform.MessageQueue.RabbitMq.Management.HttpAPI
{
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    public class Queue
    {
        public long Memory { get; set; }

        public string IdleSince { get; set; }

        public string Policy { get; set; }

        public string ExclusiveConsumerTag { get; set; }

        public int MessagesReady { get; set; }

        public int MessagesUnacknowledged { get; set; }

        public int Messages { get; set; }

        public int Consumers { get; set; }

        public int ActiveConsumers { get; set; }

        public string Name { get; set; }

        public string Vhost { get; set; }

        public bool Durable { get; set; }

        public bool AutoDelete { get; set; }

        public Dictionary<string, string> Arguments { get; set; }

        public string Node { get; set; }
    }
}