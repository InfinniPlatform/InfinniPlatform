using System;
using System.Collections;
using System.Collections.Generic;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.UserInterface.Services.Metadata;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data.DataProviders
{
    /// <summary>
    /// Провайдер данных для метаданных.
    /// </summary>
    public sealed class MetadataProvider : IDataProvider
    {
        public MetadataProvider(string metadataType)
        {
            FillServices();

            _metadataType = metadataType;
        }

        private readonly Dictionary<string, Func<string, string, string, IMetadataService>> _metadataServices
            = new Dictionary<string, Func<string, string, string, IMetadataService>>();

        private readonly string _metadataType;
        private string _configId;
        private string _documentId;
        private IMetadataService _metadataService;
        private string _versionId;

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

        private void FillServices()
        {
            _metadataServices.Add(MetadataType.Solution, (version, configId, documentId) => new SolutionMetadataService());
            _metadataServices.Add(MetadataType.Configuration, (version, configId, documentId) => new ConfigurationMetadataService());
            _metadataServices.Add(MetadataType.Menu, (version, configId, documentId) => new MenuMetadataService(configId));
            _metadataServices.Add(MetadataType.Register, (version, configId, documentId) => new RegisterMetadataService(configId));
            _metadataServices.Add(MetadataType.Document, (version, configId, documentId) => new DocumentMetadataService(configId));
            _metadataServices.Add(MetadataType.PrintView, (version, configId, documentId) => new PrintViewMetadataService(configId, documentId));
            _metadataServices.Add(MetadataType.View, (version, configId, documentId) => new ViewMetadataService(configId, documentId));
            _metadataServices.Add(MetadataType.Service, (version, configId, documentId) => new ServiceMetadataService(configId, documentId));
            _metadataServices.Add(MetadataType.Process, (version, configId, documentId) => new ProcessMetadataService(configId, documentId));
            _metadataServices.Add(MetadataType.Scenario, (version, configId, documentId) => new ScenarioMetadataService(configId, documentId));
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

                        if (_metadataServices.TryGetValue(_metadataType, out metadataServiceFunc) == false)
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