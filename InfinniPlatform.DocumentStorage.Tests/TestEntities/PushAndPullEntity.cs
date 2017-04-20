using System.Collections.Generic;

namespace InfinniPlatform.DocumentStorage.TestEntities
{
    internal class PushAndPullEntity
    {
        public object _id { get; set; }

        public IEnumerable<int> propPush { get; set; }

        public IEnumerable<int> propPushAll { get; set; }

        public IEnumerable<int> propPushUnique { get; set; }

        public IEnumerable<int> propPushAllUnique { get; set; }

        public IEnumerable<int> propPopFirst { get; set; }

        public IEnumerable<int> propPopLast { get; set; }

        public IEnumerable<int> propPull { get; set; }

        public IEnumerable<int> propPullAll { get; set; }

        public IEnumerable<Item> propPullFilter { get; set; }


        public class Item
        {
            public int value { get; set; }
        }
    }
}