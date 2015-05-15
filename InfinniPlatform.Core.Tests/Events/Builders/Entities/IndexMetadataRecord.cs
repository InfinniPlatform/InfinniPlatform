using System;
using System.Collections.Generic;

namespace InfinniPlatform.Core.Tests.Events.Builders.Entities
{
    public class IndexMetadataRecord
    {
        public ObjectMetadataRecord IndexedMetadata { get; set; }

        public Type IndexType { get; set; }

        public Func<IEnumerable<object>> IndexItems { get; set; } 
    }
}
