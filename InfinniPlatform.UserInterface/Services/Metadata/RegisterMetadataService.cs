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
    ///     Сервис для работы с метаданными документов.
    /// </summary>
    internal sealed class RegisterMetadataService : BaseMetadataService
    {
        private readonly string _configId;
        private InfinniMetadataApi _metadataApi;

        public RegisterMetadataService(string version, string configId, string server, int port, string route)
            : base(version, server, port, route)
        {
            _configId = configId;
            _metadataApi = new InfinniMetadataApi(server, port.ToString(), route);
        }

        public string ConfigId
        {
            get { return _configId; }
        }

        public override object CreateItem()
        {
            return _metadataApi.CreateRegister(Version, ConfigId);
        }

        public override void ReplaceItem(dynamic item)
        {
            _metadataApi.UpdateRegister(item, Version, ConfigId);
        }

        public override void DeleteItem(string itemId)
        {
            _metadataApi.DeleteRegister(Version, ConfigId, itemId);
        }

        public override object GetItem(string itemId)
        {
            return _metadataApi.GetRegister(Version, ConfigId, itemId);
        }

        public override IEnumerable<object> GetItems()
        {
            return _metadataApi.GetRegisterItems(Version, ConfigId);
        }
    }
}