using System;
using System.Linq;

namespace InfinniPlatform.Api.RestApi.DataApi
{
    public sealed class MetadataApi
    {
		[Obsolete("Metadata is now stored locally")]
        public dynamic GetMetadataList(string configuration = null, string metadata = null, string metadataType = null,
            string metadataId = null)
        {
            return Enumerable.Empty<object>();
        }

		[Obsolete("Metadata is now stored locally")]
        public dynamic GetDocumentSchema(string configuration, string metadata)
        {
            return null;
        }
    }
}