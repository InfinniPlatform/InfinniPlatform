using System.Collections.Generic;

using InfinniPlatform.Sdk.Api;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
    /// <summary>
    ///     Сервис для работы с метаданными печатных представлений.
    /// </summary>
    internal sealed class PrintViewMetadataService : BaseMetadataService
    {
        private readonly string _configId;
        private readonly string _documentId;
        private InfinniMetadataApi _metadataApi;

        public PrintViewMetadataService(string version, string configId, string documentId, string server, int port, string route)
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
            return _metadataApi.CreatePrintView(Version, ConfigId,_documentId);
        }

        public override void ReplaceItem(dynamic item)
        {
            _metadataApi.UpdatePrintView(item, Version,ConfigId,_documentId);
        }

        public override void DeleteItem(string itemId)
        {
            _metadataApi.DeletePrintView(Version, ConfigId, _documentId, itemId);
        }

        public override object GetItem(string itemId)
        {
            return _metadataApi.GetPrintView(Version, ConfigId, _documentId, itemId);
        }

        public override IEnumerable<object> GetItems()
        {
            return _metadataApi.GetPrintViewItems(Version, ConfigId, _documentId);
        }
    }
}