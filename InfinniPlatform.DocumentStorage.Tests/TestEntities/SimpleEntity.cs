using System;

namespace InfinniPlatform.DocumentStorage.TestEntities
{
    internal class SimpleEntity
    {
        public object _id { get; set; }

        public string prop1 { get; set; }

        public int? prop2 { get; set; }

        public DateTime? date { get; set; }
    }
}