using System.Threading.Tasks;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.SystemConfig.Services
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
            string configuration = requestForm.Configuration;
            string documentType = requestForm.MetadataObject;
            string viewName = requestForm.MetadataName;

            var result = _metadataApi.GetView(configuration, documentType, viewName);

            return Task.FromResult<object>(result);
        }
    }
}