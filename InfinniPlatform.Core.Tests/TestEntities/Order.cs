using System.Collections.Generic;

namespace InfinniPlatform.Core.TestEntities
{
    internal class Order
    {
        public IEnumerable<OrderItem> Items { get; set; }
        public Person Client { get; set; }
    }
}