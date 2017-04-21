using System.Collections.Generic;

namespace InfinniPlatform.DocumentStorage.TestEntities
{
    internal class CollectionEntity<T>
    {
        public object _id { get; set; }

        public IEnumerable<T> items { get; set; }
    }
}