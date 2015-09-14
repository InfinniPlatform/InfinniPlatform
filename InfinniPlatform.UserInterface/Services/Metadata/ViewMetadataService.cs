using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.Metadata;
using InfinniPlatform.UserInterface.Configurations;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
    /// <summary>
    ///     Сервис для работы с метаданными представлений.
    /// </summary>
    internal sealed class ViewMetadataService : BaseMetadataService
    {
        private readonly string _configId;
        private string _documentId;
        private InfinniMetadataApi _metadataApi;

        public ViewMetadataService(string version, string configId, string documentId, string server, int port, string route)
            : base(version, server, port, route)
        {
            _configId = configId;
            _documentId = documentId;
            _metadataApi = new InfinniMetadataApi(server, port.ToString(), route);
        }

        public string ConfigId
        {
            get { return _configId; }
        }

        public override object CreateItem()
        {
            return _metadataApi.CreateView(Version, ConfigId, _documentId);
        }

        public override void ReplaceItem(dynamic item)
        {
            _metadataApi.UpdateView(item, Version, ConfigId, _documentId);
        }

        public override void DeleteItem(string itemId)
        {
            _metadataApi.DeleteView(Version, ConfigId, _documentId, itemId);
        }

        public override object GetItem(string itemId)
        {
            return ConfigResourceRepository.GetView(_configId, _documentId, itemId)
                ?? _metadataApi.GetView(Version, ConfigId, _documentId, itemId);
        }

        public override IEnumerable<object> GetItems()
        {
            return ConfigResourceRepository.GetViews(_configId, _documentId) ?? _metadataApi.GetViewItems(Version, ConfigId, _documentId);
        }
    }
}