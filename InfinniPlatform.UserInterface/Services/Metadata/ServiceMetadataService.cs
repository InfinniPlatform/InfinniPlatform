using System;
using System.Collections.Generic;
using System.Threading;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.Metadata;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
    /// <summary>
    ///     Сервис для работы с метаданными бизнес-сервисов.
    /// </summary>
    internal sealed class ServiceMetadataService : BaseMetadataService
    {
        private readonly string _configId;
        private string _documentId;
        private InfinniMetadataApi _metadataApi;

        public ServiceMetadataService(string version, string configId, string documentId, string server, int port)
            : base(version, server, port)
        {
            _configId = configId;
            _documentId = documentId;
            _metadataApi = new InfinniMetadataApi(server, port.ToString(), version);
        }

        public string ConfigId
        {
            get { return _configId; }
        }


        public override object CreateItem()
        {
            return _metadataApi.CreateService(Version, ConfigId, _documentId);
        }

        public override void ReplaceItem(dynamic item)
        {
            _metadataApi.UpdateService(item, Version, ConfigId, _documentId);
        }

        public override void DeleteItem(string itemId)
        {
            _metadataApi.DeleteService(Version, ConfigId, _documentId, itemId);
        }

        public override object GetItem(string itemId)
        {
            return _metadataApi.GetService(Version, ConfigId, _documentId, itemId);
        }

        public override IEnumerable<object> GetItems()
        {
            return _metadataApi.GetServiceItems(Version, ConfigId, _documentId);
        }
    }
}