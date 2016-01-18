using System;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Contracts;
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

            builder.Post["/GetManagedMetadata"] = CreateHandler(GetMetadataItem);
        }

        private static Func<IHttpRequest, object> CreateHandler(Action<IActionContext> action)
        {
            return new ChangeHttpRequestHandler(action).Action;
        }

        private void GetMetadataItem(IActionContext target)
        {
            string configuration = target.Item.Configuration;
            string documentType = target.Item.MetadataObject;
            string viewName = target.Item.MetadataName;

            var view = _metadataApi.GetView(configuration, documentType, viewName);

            target.Result = view;
        }
    }
}