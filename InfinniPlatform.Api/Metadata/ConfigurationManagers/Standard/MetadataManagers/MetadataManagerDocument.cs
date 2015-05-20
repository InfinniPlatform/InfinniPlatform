using System.Collections.Generic;
using InfinniPlatform.Api.Deprecated;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Api.Metadata.MetadataContainers;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers
{
	public sealed class MetadataManagerDocument  : IDataManager
	{
		private readonly string _configurationId;
		private readonly IMetadataContainerInfo _metadataContainerInfo;
		private MetadataManagerElement _manager;

		public MetadataManagerDocument(string configurationId, IMetadataContainerInfo metadataContainerInfo, string metadataType)
		{
			_configurationId = configurationId;
			_metadataContainerInfo = metadataContainerInfo;
            var metadataCacheRefresher = new MetadataCacheRefresher(configurationId, metadataContainerInfo.GetMetadataContainerName(), metadataType);
            _manager = new MetadataManagerElement(new ManagerIdentifiersStandard().GetConfigurationUid(_configurationId), metadataCacheRefresher, new MetadataReaderConfigurationElement(_configurationId, new MetadataContainerDocument()), _metadataContainerInfo, metadataType);
		}

		public IDataReader MetadataReader
		{
			get { return _manager.MetadataReader; }
		}


    	public dynamic CreateItem(string name)
		{
			return MetadataBuilderExtensions.BuildDocument(name, name, name, name);
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
		            new ManagerFactoryConfiguration(_configurationId).BuildDocumentMetadataReader(), metadataObject);

		    if (document != null)
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

		}

		public void ApplyMetadataChanges(string metadataName, IEnumerable<object> eventDefinitions)
		{
			_manager.ApplyMetadataChanges(metadataName, eventDefinitions);
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
