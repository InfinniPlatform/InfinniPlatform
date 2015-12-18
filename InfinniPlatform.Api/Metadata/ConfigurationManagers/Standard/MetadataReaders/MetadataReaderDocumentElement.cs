using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders
{
    internal sealed class MetadataReaderDocumentElement : MetadataReader
    {
        public MetadataReaderDocumentElement(string configurationId, string documentId, IMetadataContainerInfo metadataContainerInfo)
            : base(configurationId, metadataContainerInfo.GetMetadataTypeName())
        {
        }

        public override IEnumerable<dynamic> GetItems()
        {
            return Enumerable.Empty<object>();
        }

        public override dynamic GetItem(string metadataName)
        {
            return new DynamicWrapper();
        }
    }
}