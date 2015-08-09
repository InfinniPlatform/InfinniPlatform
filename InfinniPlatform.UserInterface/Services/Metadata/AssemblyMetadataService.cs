using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using InfinniPlatform.Api.Deprecated;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.Metadata;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
    /// <summary>
    ///     Сервис для работы с метаданными сборок.
    /// </summary>
    internal sealed class AssemblyMetadataService : BaseMetadataService
    {
        private readonly string _configId;
        private InfinniMetadataApi _metadataApi;

        public AssemblyMetadataService(string version, string configId, string server, int port)
            : base(version, server, port)
        {
            _configId = configId;
            _metadataApi = new InfinniMetadataApi(server, port.ToString(),version);
        }

        public string ConfigId
        {
            get { return _configId; }
        }


        public override object CreateItem()
        {
            return _metadataApi.CreateAssembly(Version, ConfigId);
        }

        public override void ReplaceItem(dynamic item)
        {
            _metadataApi.UpdateAssembly(item,Version,ConfigId);
        }

        public override void DeleteItem(string itemId)
        {
            _metadataApi.DeleteAssembly(Version, ConfigId, itemId);
        }

        public override object GetItem(string itemId)
        {
            return _metadataApi.GetAssembly(Version, ConfigId, itemId);
        }

        public override IEnumerable<object> GetItems()
        {
            return _metadataApi.GetAssemblies(Version, ConfigId);
        }
    }
}