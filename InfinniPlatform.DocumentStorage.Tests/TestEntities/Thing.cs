using System.Collections.Generic;

namespace InfinniPlatform.DocumentStorage.Tests.TestEntities
{
    internal class Thing
    {
        public object _id { get; set; }

        public string code { get; set; }

        public string[] tags { get; set; }

        public IEnumerable<ThingItem> qty { get; set; }
    }
}