using System.Collections.Generic;

namespace InfinniPlatform.DocumentStorage.TestEntities
{
    internal class ComplexInventory
    {
        public object _id { get; set; }

        public string type { get; set; }

        public string item { get; set; }

        public IEnumerable<int> ratings { get; set; }

        public ComplexInventoryClassification classification { get; set; }

        public ComplexInventoryVendor vendor { get; set; }
    }
}