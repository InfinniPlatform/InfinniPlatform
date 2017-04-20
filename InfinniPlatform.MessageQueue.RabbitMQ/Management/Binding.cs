using System.Collections.Generic;
using System.Diagnostics;

namespace InfinniPlatform.MessageQueue.Management
{
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