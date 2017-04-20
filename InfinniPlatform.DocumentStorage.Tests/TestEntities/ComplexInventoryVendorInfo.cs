using System.Collections.Generic;

namespace InfinniPlatform.DocumentStorage.TestEntities
{
    internal class ComplexInventoryVendorInfo
    {
        public string name { get; set; }

        public string address { get; set; }

        public IEnumerable<string> delivery { get; set; }
    }
}