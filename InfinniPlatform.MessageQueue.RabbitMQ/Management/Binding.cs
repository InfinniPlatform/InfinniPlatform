#pragma warning disable 1591
using System.Collections.Generic;
using System.Diagnostics;

namespace InfinniPlatform.MessageQueue.Management
{
    /// <summary>
    /// Representation of RabbitMQ binding.
    /// </summary>
    [DebuggerDisplay("{" + nameof(Destination) + "}")]
    public class Binding
    {
        public string Source { get; set; }

        public string Vhost { get; set; }

        public string Destination { get; set; }

        public string DestinationType { get; set; }

        public string RoutingKey { get; set; }

        public Dictionary<string, string> Arguments { get; set; }

        public string PropertiesKey { get; set; }
    }
}