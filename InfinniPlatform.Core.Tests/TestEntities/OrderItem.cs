namespace InfinniPlatform.Core.TestEntities
{
    internal class OrderItem
    {
        public IProduct Product { get; set; }
        public int Count { get; set; }
        public float Price { get; set; }
    }
}