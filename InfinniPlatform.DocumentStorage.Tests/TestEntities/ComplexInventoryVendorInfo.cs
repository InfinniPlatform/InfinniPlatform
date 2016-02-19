using System.Collections.Generic;

namespace InfinniPlatform.DocumentStorage.Tests.TestEntities
{
    internal class ComplexInventoryVendorInfo
    {
        public string name { get; set; }

        public string address { get; set; }

        public IEnumerable<string> delivery { get; set; }
    }
}