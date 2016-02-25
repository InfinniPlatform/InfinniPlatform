using System;

namespace InfinniPlatform.DocumentStorage.Tests.TestEntities
{
    internal class Sale
    {
        public object _id { get; set; }

        public string item { get; set; }

        public double price { get; set; }

        public double quantity { get; set; }

        public DateTime date { get; set; }
    }
}