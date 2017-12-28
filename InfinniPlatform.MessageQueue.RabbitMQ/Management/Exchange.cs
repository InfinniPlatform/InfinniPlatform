#pragma warning disable 1591
using System.Collections.Generic;
using System.Diagnostics;

namespace InfinniPlatform.MessageQueue.Management
{
    /// <summary>
    /// Representation of RabbitMQ exchange.
    /// </summary>
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    public class Exchange
    {
        public string Name { get; set; }

        public string Vhost { get; set; }

        public string Type { get; set; }

        public bool Durable { get; set; }

        public bool AutoDelete { get; set; }

        public bool Internal { get; set; }

        public Dictionary<string, string> Arguments { get; set; }
    }
}