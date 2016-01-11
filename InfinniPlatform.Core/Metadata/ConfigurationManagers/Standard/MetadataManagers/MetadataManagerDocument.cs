using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Core.Metadata.MetadataContainers;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.MetadataManagers
{
    public sealed class MetadataManagerDocument : IDataManager
    {
        public MetadataManagerDocument(string configurationId, IMetadataContainerInfo metadataContainerInfo, string metadataType)
        {
            _configurationId = configurationId;
            var metadataCacheRefresher = new MetadataCacheRefresher(configurationId,
                metadataContainerInfo.GetMetadataContainerName(), metadataType);
            _manager = new MetadataManagerElement(new ManagerIdentifiersStandard().GetConfigurationUid(_configurationId), metadataCacheRefresher,
                new MetadataReaderConfigurationElement(_configurationId, new MetadataContainerDocument()),
                metadataContainerInfo, metadataType);
        }

        private readonly string _configurationId;
        private readonly MetadataManagerElement _manager;

        public IDataReader MetadataReader
        {
            get { return _manager.MetadataReader; }
        }

        public dynamic CreateItem(string name)
        {
            dynamic document = new DynamicWrapper();

            document.Id = Guid.NewGuid().ToString();
            document.Name = name;
            document.Caption = name;
            document.Description = name;
            document.Versioning = 2;
            document.MetadataIndex = name;
            document.Services = new List<dynamic>();
            document.Processes = new List<dynamic>();
            document.Scenarios = new List<dynamic>();
            document.Generators = new List<dynamic>();
            document.Views = new List<dynamic>();
            document.PrintViews = new List<dynamic>();
            document.ValidationWarnings = new List<dynamic>();
            document.ValidationErrors = new List<dynamic>();

            return document;
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
                    new ManagerFactoryConfiguration(_configurationId).BuildDocumentMetadataReader(),
                    metadataObject);

            if (document != null)
            {
                try
                {
                    var factory = new ManagerFactoryDocument(_configurationId, document.Name);

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