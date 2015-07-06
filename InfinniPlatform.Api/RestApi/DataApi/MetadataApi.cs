using System.Linq;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.Api.RestApi.DataApi
{
    public sealed class MetadataApi
    {
        public dynamic GetMetadataList(string configuration = null, string metadata = null, string metadataType = null,
            string metadataId = null)
        {
            return RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "getconfigmetadatalist", null, new
                {
                    Configuration = configuration,
                    Metadata = metadata,
                    MetadataType = metadataType,
                    MetadataName = metadataId
                }).ToDynamicList();
        }

        public dynamic GetDocumentSchema(string configuration, string metadata)
        {
            return RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "getconfigmetadatalist", null, new
                {
                    Configuration = configuration,
                    Metadata = metadata,
                    MetadataType = MetadataType.Schema
                }).ToDynamicList().FirstOrDefault();
        }
    }
}