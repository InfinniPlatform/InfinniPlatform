using System.Threading.Tasks;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Core.Services
{
    /// <summary>
    /// Реализует REST-сервис для MetadataApi.
    /// </summary>
    internal sealed class MetadataApiHttpService : IHttpService
    {
        public MetadataApiHttpService(IMetadataApi metadataApi)
        {
            _metadataApi = metadataApi;
        }

        private readonly IMetadataApi _metadataApi;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "/SystemConfig/StandardApi/metadata";

            builder.Post["/GetManagedMetadata"] = GetMetadataItem;
        }

        private Task<object> GetMetadataItem(IHttpRequest request)
        {
            dynamic requestForm = request.Form.changesObject;
            string documentType = requestForm.MetadataObject;
            string viewName = requestForm.MetadataName;

            var result = _metadataApi.GetMetadata($"Views.{documentType}.{viewName}");

            return Task.FromResult<object>(result);
        }
    }
}