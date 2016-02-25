using System.Collections.Generic;

namespace InfinniPlatform.DocumentStorage.Tests.TestEntities
{
    internal class School
    {
        public object _id { get; set; }

        public string zipcode { get; set; }

        public IEnumerable<SchoolStudent> students { get; set; }
    }
}