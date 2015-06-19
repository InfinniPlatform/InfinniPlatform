using System;
using InfinniPlatform.Api.Deprecated;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Api.Metadata.MetadataContainers;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers
{
    public sealed class MetadataManagerDocument : IDataManager
    {
        private readonly string _configurationId;
        private readonly MetadataManagerElement _manager;
        private readonly string _version;

        public MetadataManagerDocument(string version, string configurationId,
            IMetadataContainerInfo metadataContainerInfo, string metadataType)
        {
            _version = version;
            _configurationId = configurationId;
            var metadataCacheRefresher = new MetadataCacheRefresher(version, configurationId,
                metadataContainerInfo.GetMetadataContainerName(), metadataType);
            _manager = new MetadataManagerElement(version,
                new ManagerIdentifiersStandard().GetConfigurationUid(_version, _configurationId), metadataCacheRefresher,
                new MetadataReaderConfigurationElement(_version, _configurationId, new MetadataContainerDocument()),
                metadataContainerInfo, metadataType);
        }

        public IDataReader MetadataReader
        {
            get { return _manager.MetadataReader; }
        }

        public dynamic CreateItem(string name)
        {
            return MetadataBuilderExtensions.BuildDocument(name, name, name, name, _version);
        }

        public void InsertItem(dynamic objectToCreate)
        {
            InitNewDocument(objectToCreate);

            _manager.InsertItem(objectToCreate);
        }

        public void DeleteItem(dynamic metadataObject)
        {
            var document =
                MetadataExtensions.GetStoredMetadata(
                    new ManagerFactoryConfiguration(_version, _configurationId).BuildDocumentMetadataReader(),
                    metadataObject);

            if (document != null)
            {
                try
                {
                    var factory = new ManagerFactoryDocument(_version, _configurationId, document.Name);

                    foreach (var metadataType in MetadataType.GetDocumentMetadataTypes())
                    {
                        var managerType = factory.BuildManagerByType(metadataType);
                        if (managerType != null)
                        {
                            var items = managerType.MetadataReader.GetItems();
                            foreach (var item in items)
                            {
                                managerType.DeleteItem(item);
                            }
                        }
                    }

                    _manager.DeleteItem(document);
                }
                catch (ApplicationException e)
                {
                    //логировать сообщение об ошибке
                }
            }
        }

        public void MergeItem(dynamic objectToCreate)
        {
            var instanceToSave = StringifyJSONProperties(objectToCreate);

            _manager.MergeItem(instanceToSave);
        }

        private dynamic InitNewDocument(dynamic objectToCreate)
        {
            var instanceToSave = StringifyJSONProperties(objectToCreate);

            if (string.IsNullOrEmpty(objectToCreate.MetadataIndex as string))
            {
                instanceToSave.MetadataIndex = objectToCreate.Name;
            }

            return instanceToSave;
        }

        private dynamic StringifyJSONProperties(dynamic objectToCreate)
        {
            var clonedObjectToCreate = objectToCreate.Clone();
            dynamic schema = clonedObjectToCreate.Schema;
            if (schema != null && !(schema is string))
            {
                objectToCreate.Schema = new DynamicWrapper();
                objectToCreate.Schema.JsonString = schema.ToString();
                objectToCreate.Schema.StringifiedJson = true;
            }
            return clonedObjectToCreate;
        }
    }
}