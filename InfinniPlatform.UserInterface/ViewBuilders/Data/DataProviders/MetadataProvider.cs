using System;
using System.Collections;
using System.Collections.Generic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.UserInterface.Services.Metadata;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data.DataProviders
{
    /// <summary>
    ///     Провайдер данных для метаданных.
    /// </summary>
    public sealed class MetadataProvider : IDataProvider
    {
        private readonly Dictionary<string, Func<string, string, string, IMetadataService>> MetadataServices
            = new Dictionary<string, Func<string, string, string, IMetadataService>>();
            

        private string _configId;
        private string _documentId;
        private IMetadataService _metadataService;
        private string _versionId;
        private readonly string _metadataType;
        private readonly string _server;
        private readonly int _port;

        public MetadataProvider(string metadataType, string server, int port)
        {
            FillServices();
            _metadataType = metadataType;
            _server = server;
            _port = port;
        }

        private void FillServices()
        {
            MetadataServices.Add(
                    MetadataType.Solution,
                    (version, configId, documentId) => new SolutionMetadataService(version, _server, _port)
            );
            MetadataServices.Add(
                    MetadataType.Configuration,
                    (version, configId, documentId) => new ConfigurationMetadataService(version, _server, _port)                
            );
            MetadataServices.Add(
                    MetadataType.Menu, (version, configId, documentId) => new MenuMetadataService(version, configId,_server, _port)
            );
            MetadataServices.Add(
                    MetadataType.Assembly, (version, configId, documentId) => new AssemblyMetadataService(version, configId,_server, _port)
            );
            MetadataServices.Add(
                    MetadataType.Register, (version, configId, documentId) => new RegisterMetadataService(version, configId,_server, _port)
            );
            MetadataServices.Add(
                    MetadataType.Document, (version, configId, documentId) => new DocumentMetadataService(version, configId, _server, _port)
            );

            MetadataServices.Add(
                    MetadataType.PrintView, (version, configId, documentId) => new PrintViewMetadataService(version, configId, documentId, _server, _port)
            );
            MetadataServices.Add(
                    MetadataType.View, (version, configId, documentId) => new ViewMetadataService(version, configId, documentId, _server, _port)
            );
            MetadataServices.Add(
                    MetadataType.Service, (version, configId, documentId) => new ServiceMetadataService(version, configId, documentId, _server, _port)
            );
            MetadataServices.Add(
                    MetadataType.Process, (version, configId, documentId) => new ProcessMetadataService(version, configId, documentId, _server, _port)
            );
            MetadataServices.Add(
                    MetadataType.Scenario, (version, configId, documentId) => new ScenarioMetadataService(version, configId, documentId, _server, _port)
            );
        }

        public string GetConfigId()
        {
            return _configId;
        }

        public void SetConfigId(string value)
        {
            if (Equals(_configId, value) == false)
            {
                _configId = value;

                ResetMetadataService();
            }
        }

        public string GetDocumentId()
        {
            return _documentId;
        }

        public void SetDocumentId(string value)
        {
            if (Equals(_documentId, value) == false)
            {
                _documentId = value;

                ResetMetadataService();
            }
        }

        public string GetVersion()
        {
            return _versionId;
        }

        public void SetVersion(string value)
        {
            if (Equals(_versionId, value) == false)
            {
                _versionId = value;

                ResetMetadataService();
            }
        }

        public object CreateItem()
        {
            return GetMetadataService().CreateItem();
        }

        public void ReplaceItem(object item)
        {
            GetMetadataService().ReplaceItem(item);
        }

        public void DeleteItem(string itemId)
        {
            GetMetadataService().DeleteItem(itemId);
        }

        public object GetItem(string itemId)
        {
            return GetMetadataService().GetItem(itemId);
        }

        public object CloneItem(string itemId)
        {
            return GetMetadataService().CloneItem(itemId);
        }

        public IEnumerable GetItems(IEnumerable criterias, int pageNumber, int pageSize)
        {
            return GetMetadataService().GetItems();
        }

        private void ResetMetadataService()
        {
            if (_metadataService != null)
            {
                lock (this)
                {
                    _metadataService = null;
                }
            }
        }

        private IMetadataService GetMetadataService()
        {
            if (_metadataService == null)
            {
                lock (this)
                {
                    if (_metadataService == null)
                    {
                        Func<string, string, string, IMetadataService> metadataServiceFunc;

                        if (MetadataServices.TryGetValue(_metadataType, out metadataServiceFunc) == false)
                        {
                            throw new NotSupportedException(_metadataType);
                        }

                        _metadataService = metadataServiceFunc(_versionId, _configId, _documentId);
                    }
                }
            }

            return _metadataService;
        }
    }
}