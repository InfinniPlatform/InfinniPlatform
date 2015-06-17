using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Deprecated;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestQuery.EventObjects.EventSerializers;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers
{
	/// <summary>
	///   API для работы с CRUD операциями элементов метаданных
	/// </summary>
	public class MetadataManagerElement : IDataManager
	{
		private readonly string _parentUid;
	    private readonly string _version;
		private readonly MetadataCacheRefresher _metadataCacheRefresher;

		private readonly IDataReader _metadataReader;
		private readonly string _metadataContainerName;
		private readonly string _metadataType;
		private readonly string _rootMetadataIndex;

	    /// <summary>
	    ///   Публичный конструктор менеджера метаданных объектов конфигурации
	    /// </summary>
	    /// <param name="version">Версия конфигурации</param>
	    /// <param name="parentUid">Идентификатор (Id) родительского контейнера метаданных</param>
	    /// <param name="metadataCacheRefresher">Механизм обновления кэша метаданных</param>
	    /// <param name="metadataReader">Провайдер для чтения метаданных указанного типа</param>
	    /// <param name="metadataContainerInfo">Информация о контейнере метаданных</param>
	    /// <param name="ownerMetadataType">Контейнер, содержащий метаданные, по которым выполняется поиск данных</param>
	    public MetadataManagerElement(string version, string parentUid, MetadataCacheRefresher metadataCacheRefresher, IDataReader metadataReader, IMetadataContainerInfo metadataContainerInfo, string ownerMetadataType)
		{
			if (parentUid == null)
			{
				throw new ArgumentException(String.Format("metadata identifier not found"));
			}

			_parentUid = parentUid;
	        _version = version;
			_metadataCacheRefresher = metadataCacheRefresher;

			_metadataReader = metadataReader;
			_metadataContainerName = metadataContainerInfo.GetMetadataContainerName();

			_metadataType = metadataContainerInfo.GetMetadataTypeName();
			_rootMetadataIndex = MetadataExtension.GetMetadataIndex(ownerMetadataType);
		}

		/// <summary>
		///   Провайдер метаданных для чтения
		/// </summary>
		public IDataReader MetadataReader
		{
			get { return _metadataReader; }
		}

	    /// <summary>
	    ///   Сформировать предзаполненный объект метаданных
	    /// </summary>
	    /// <param name="name"></param>
	    /// <returns>Предзаполненный объект метаданных</returns>
	    public dynamic CreateItem(string name)
		{
			var instance = new DynamicWrapper();
			instance
				.BuildId(Guid.NewGuid().ToString())
                .BuildProperty("Version",_version)
				.BuildName(name)
				.BuildCaption(name)
				.BuildDescription(name);
			return instance;
		}

		/// <summary>
		///   Добавить метаданные объекта конфигурации
		/// </summary>
		/// <param name="objectToCreate">Метаданные создаваемого объекта</param>
		public void InsertItem(dynamic objectToCreate)
		{

            MergeItem(objectToCreate);

		}

		private void SaveOrUpdateMetadata(dynamic objectToCreate)
		{
		    var updatingMetadata = MetadataExtensions.GetStoredMetadata(_metadataReader, objectToCreate) ?? objectToCreate;
            
			objectToCreate.ParentId = _parentUid;
		    objectToCreate.Version = _version;

			//создаем заголовочную часть метаданных
			dynamic metadataHeader = MetadataBuilderExtensions.BuildMetadataHeader(objectToCreate);

			//если объект содержит идентификатор, копируем его в создаваемый
			metadataHeader.Id = objectToCreate.Id;

			if (string.IsNullOrEmpty(objectToCreate.Name))
			{
				throw new ArgumentException("metadata name should be specified!");
			}

			//ищем существующий объект метаданных в конфигурации
            dynamic container = GetMetadataContainer(updatingMetadata.Name);

			object body;


			if (container.Index > -1)
			{
				//если находим, то апдейтим существующее
				body = new UpdateCollectionItem(_metadataContainerName, container.Index, metadataHeader,_version);
				
			}
			else
			{
				//иначе создаем новое 
				body = new AddCollectionItem(_metadataContainerName, metadataHeader,_version);
			}


			//обновляем заголовочную часть в метаданных родительского объекта (ссылку на метаданные конкретного типа)
			//заменяем объект заголовочной части
			RestQueryApi.QueryPostRaw("SystemConfig", RootMetadataIndex, "changemetadata", _parentUid, body,_version);

			//обновляем полные метаданные конкретного типа
			//заменяем полные метаданные в индексе
			RestQueryApi.QueryPostRaw("SystemConfig", MetadataIndex, "changemetadata", objectToCreate.Id, objectToCreate,_version, true);

			if (_metadataCacheRefresher != null)
			{
                _metadataCacheRefresher.RefreshMetadataAfterChanging(updatingMetadata.Name);
			}
		}

		/// <summary>
		///   Индекс, в котором хранятся полные метаданные типа, указанного при создании менеджера (например metadata - для индекса конфигурации)
		/// </summary>
		private string MetadataIndex
		{
			get { return string.Format("{0}metadata", _metadataType); }
		}

		/// <summary>
		///   Индекс, в котором хранятся ссылки (заголовки) на полные метаданные типов 
		///  Например: metadata хранит объекты конфигурации, содержащие ссылки на documentmetadata, menumetadata, etc
		/// </summary>
		protected string RootMetadataIndex
		{
			get { return _rootMetadataIndex; }
		}

	    /// <summary>
	    ///   Удалить метаданные указанного объекта в указанной конфигурации
	    /// </summary>
	    /// <param name="metadataObject">Удаляемый объект</param>
	    public void DeleteItem(dynamic metadataObject)
	    {
            //получаем сохраненный элемент метаданных
	        var element = MetadataExtensions.GetStoredMetadata(_metadataReader, metadataObject);

	        if (element != null)
	        {

	            //удаляем из метаданных конфигурации
	            var configurationBody =
                    new RemoveCollectionItem(string.Format("{0}.$.Name:{1}", _metadataContainerName, element.Name), _version);

	            dynamic metadataContainer = GetMetadataContainer(element.Name);

	            RestQueryApi.QueryPostRaw("SystemConfig", RootMetadataIndex, "changemetadata", _parentUid,
	                                      configurationBody, _version);

	            if (metadataContainer.Index > -1)
	            {

	                if (metadataContainer.Id != null)
	                {
	                    RestQueryApi.QueryPostJsonRaw("SystemConfig", MetadataIndex, "deletemetadata", metadataContainer.Id, null, _version);

                        if (_metadataCacheRefresher != null)
                        {
                            _metadataCacheRefresher.RefreshMetadataAfterDeleting(element.Name);
                        }

	                }
	            }
	        }

	    }


		/// <summary>
		///   Получить контейнер объектов, содержащий сами метаданные и индекс этого объекта в родительской коллекции
		/// </summary>
		/// <returns>Контейнер меню</returns>
		private dynamic GetMetadataContainer(string metadataName)
		{
			var metadata = MetadataReader.GetItems().ToList();
			dynamic result = new DynamicWrapper();
			result.Index = -1;

			var metadataObject = metadata.FirstOrDefault(m => m.Name.ToLowerInvariant() == metadataName.ToLowerInvariant());

			if (metadataObject != null)
			{
				result.Metadata = metadataObject;
				var index = metadata.ToList().IndexOf(metadataObject);
				result.Index = index;
				result.Id = metadataObject.Id;
			}
			return result;
		}



		/// <summary>
		///  Преобразовать JSON в строку (Чтобы не возникало конфликтов при сохранении документов в ES из-за разной схемы JSON)
		/// </summary>
		/// <param name="objectToCreate">Сохраняемый объект</param>
		private void StringifyJsonProperties(dynamic objectToCreate)
		{
			if (_metadataType == MetadataType.View)
			{

				dynamic layoutPanel = objectToCreate.LayoutPanel;
				if (layoutPanel != null && !(layoutPanel is string))
				{
					objectToCreate.LayoutPanel = new DynamicWrapper();
					objectToCreate.LayoutPanel.JsonString = layoutPanel.ToString();
					objectToCreate.LayoutPanel.StringifiedJson = true;
				}
				dynamic childViews = objectToCreate.ChildViews;
				if (childViews != null && !(childViews is string))
				{
					objectToCreate.ChildViews = new DynamicWrapper();
					objectToCreate.ChildViews.JsonString = DynamicWrapperExtensions.DynamicEnumerableToString(childViews);
					objectToCreate.ChildViews.StringifiedJson = true;
				}
				dynamic dataSources = objectToCreate.DataSources;
				if (dataSources != null && !(dataSources is string))
				{
					objectToCreate.DataSources = new DynamicWrapper();
					objectToCreate.DataSources.JsonString = DynamicWrapperExtensions.DynamicEnumerableToString(dataSources);
					objectToCreate.DataSources.StringifiedJson = true;
				}
				dynamic parameters = objectToCreate.Parameters;
				if (parameters != null && !(parameters is string))
				{
					objectToCreate.Parameters = new DynamicWrapper();
					objectToCreate.Parameters.JsonString = DynamicWrapperExtensions.DynamicEnumerableToString(parameters);
					objectToCreate.Parameters.StringifiedJson = true;
				}
			}
			if (_metadataType == MetadataType.ValidationError || _metadataType == MetadataType.ValidationWarning)
			{
				dynamic op = objectToCreate.ValidationOperator;
				if (op != null && !(op is string))
				{
					dynamic validationOperator = new DynamicWrapper();
					validationOperator.JsonString = op.ToString();
					validationOperator.StringifiedJson = true;
					objectToCreate.ValidationOperator = validationOperator;
				}				
			}
			if (_metadataType == MetadataType.PrintView)
			{
				dynamic blocks = objectToCreate.Blocks;
				if (blocks != null && !(blocks is string))
				{
					objectToCreate.Blocks = new DynamicWrapper();
					objectToCreate.Blocks.JsonString = DynamicWrapperExtensions.DynamicEnumerableToString(blocks);
					objectToCreate.Blocks.StringifiedJson = true;
				}
			}
			if (_metadataType == MetadataType.Report)
			{

				dynamic content = objectToCreate.Content;
				if (content != null && !(content is string))
				{
					objectToCreate.Content = new DynamicWrapper();
					objectToCreate.Content.JsonString = content.ToString();
					objectToCreate.Content.StringifiedJson = true;
				}

			}
		}

        /// <summary>
        ///   Обновить метаданные указанного объекта  в указанной конфигурации
        /// </summary>
        /// <param name="objectToCreate">Метаданные создаваемого объекта метаданных</param>
        public void MergeItem(dynamic objectToCreate)
        {
            objectToCreate = objectToCreate.Clone();

            StringifyJsonProperties(objectToCreate);

            SaveOrUpdateMetadata(objectToCreate);
        }
	}
}
