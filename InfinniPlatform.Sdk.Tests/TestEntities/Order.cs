using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Tests.TestEntities
{
    internal class Order
    {
        public IEnumerable<OrderItem> Items { get; set; }
        public Person Client { get; set; }
    }
}