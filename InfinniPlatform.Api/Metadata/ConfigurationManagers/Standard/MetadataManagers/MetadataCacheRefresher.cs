using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers
{
    public sealed class MetadataCacheRefresher
    {
        private readonly string _configId;
        private readonly string _documentId;
        private readonly string _metadataType;

        public MetadataCacheRefresher(string configId, string documentId, string metadataType)
        {
            _configId = configId;
            _documentId = documentId;
            _metadataType = metadataType;
        }

        public void RefreshMetadataAfterChanging(string metadataName)
        {
            dynamic body = new DynamicWrapper();
            body.ConfigId = _configId;
            body.DocumentId = _documentId;
            body.MetadataType = _metadataType;
            body.MetadataName = metadataName;
            body.Version = "";
            RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "RefreshMetadataCache", null, body);
        }

        public void RefreshMetadataAfterDeleting(string metadataName)
        {
            dynamic body = new DynamicWrapper();
            body.ConfigId = _configId;
            body.DocumentId = _documentId;
            body.MetadataType = _metadataType;
            body.MetadataName = metadataName;
            body.IsElementDeleted = true;
            body.Version = "";
            RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "RefreshMetadataCache", null, body);
        }
    }
}