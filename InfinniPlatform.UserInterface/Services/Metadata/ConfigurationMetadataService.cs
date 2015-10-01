using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.Metadata;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
    /// <summary>
    ///     Сервис для работы с метаданными конфигурации.
    /// </summary>
    internal sealed class ConfigurationMetadataService : BaseMetadataService
    {

        private InfinniMetadataApi _metadataApi;

        public ConfigurationMetadataService(string version, string server, int port, string route) : base(version,server, port, route)
        {
           
            _metadataApi = new InfinniMetadataApi(server, port.ToString(), route);
        }


        public override object CreateItem()
        {
            return _metadataApi.CreateConfig();
        }

        public override void ReplaceItem(dynamic item)
        {
            _metadataApi.InsertConfig(item);
        }

        public override void DeleteItem(string itemId)
        {
            _metadataApi.DeleteConfig(Version, itemId);
        }

        public override object GetItem(string itemId)
        {
            return _metadataApi.GetConfig(Version, itemId);
        }

        public override IEnumerable<object> GetItems()
        {
            return _metadataApi.GetConfigList();
        }
    }
}