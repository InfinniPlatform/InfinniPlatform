using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.MetadataReaders
{
    public sealed class MetadataReaderSolution : IDataReader
    {
        public IEnumerable<dynamic> GetItems()
        {
            return Enumerable.Empty<object>();
        }

        public dynamic GetItem(string metadataName)
        {
            return new DynamicWrapper();
        }
    }
}