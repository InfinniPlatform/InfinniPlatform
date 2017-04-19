using System.Collections;
using System.Collections.Generic;

namespace InfinniPlatform.Core.TestEntities
{
    internal class OrderHistory : IEnumerable<Order>
    {
        private readonly List<Order> _orders;

        public OrderHistory(IEnumerable<Order> orders = null)
        {
            _orders = (orders != null) ? new List<Order>(orders) : new List<Order>();
        }


        public IEnumerator<Order> GetEnumerator()
        {
            return _orders.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _orders.GetEnumerator();
        }

        public void AddOrder(Order order)
        {
            _orders.Add(order);
        }
    }
}