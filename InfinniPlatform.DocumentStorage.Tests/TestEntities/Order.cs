namespace InfinniPlatform.DocumentStorage.TestEntities
{
    internal class Order
    {
        public object _id { get; set; }

        public OrderItem item { get; set; }

        public double amount { get; set; }
    }
}