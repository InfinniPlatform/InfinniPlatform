using System.Collections.Generic;

namespace InfinniPlatform.DocumentStorage.Tests.TestEntities
{
    internal class Inventory
    {
        public object _id { get; set; }

        public string dept { get; set; }

        public InventoryItem item { get; set; }

        public IEnumerable<string> sizes { get; set; }
    }
}