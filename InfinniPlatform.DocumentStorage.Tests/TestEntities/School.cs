using System.Collections.Generic;

namespace InfinniPlatform.DocumentStorage.TestEntities
{
    internal class School
    {
        public object _id { get; set; }

        public string zipcode { get; set; }

        public IEnumerable<SchoolStudent> students { get; set; }
    }
}