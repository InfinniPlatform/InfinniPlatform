using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Hosting;
using InfinniPlatform.Hosting.Implementation.Modules;
using InfinniPlatform.Runtime;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.SystemConfig.Multitenancy;
using InfinniPlatform.WebApi.Factories;

namespace InfinniPlatform.SystemConfig.Initializers
{
	public sealed class PackageJsonConfigurationsInitializer : IStartupInitializer
	{
		public PackageJsonConfigurationsInitializer(IChangeListener changeListener,
													IIndexFactory indexFactory,
													ISecurityComponent securityComponent,
													IMetadataConfigurationProvider metadataConfigurationProvider)
		{
			_changeListener = changeListener;
			_indexFactory = indexFactory;
			_securityComponent = securityComponent;
			_metadataConfigurationProvider = metadataConfigurationProvider;
		}


		private readonly IChangeListener _changeListener;
		private readonly IIndexFactory _indexFactory;
		private readonly ISecurityComponent _securityComponent;
		private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;


		public void OnStart(HostingContextBuilder contextBuilder)
		{
			// Получение списка всех установленных конфигураций
			var configurations = GetConfigurations();

			// Загрузка и кэширование метаданных каждой конфигурации
			foreach (var configuration in configurations)
			{
				InstallConfiguration(configuration.Version, configuration.Name);
			}

			// Регистрация обработчика для сброса кэша метаданных конфигураций
			_changeListener.RegisterOnChange("JsonConfig", OnChangeModules, Order.NoMatter);
		}


		private void InstallConfiguration(string version, string configurationId)
		{
			// Загрузка метеданных конфигурации для кэширования
			var metadataCacheFiller = LoadConfigurationMetadata(version);

			// Создание менеджера кэша метаданных конфигураций
			var metadataCacheManager = InfinniPlatformHostServer.Instance.CreateConfiguration(version, configurationId, false);

			// Загрузка метаданных конфигурации в кэш
			metadataCacheFiller.InstallConfiguration(metadataCacheManager);

			// Создание обработчика скриптов
			metadataCacheManager.ScriptConfiguration.InitActionUnitStorage(version);

			// Создание сервисов конфигурации
			InfinniPlatformHostServer.Instance.InstallServices(version, metadataCacheManager.ServiceRegistrationContainer);

			// Загрузка ACL после установки конфигурации "Authorization"
			if (string.Equals(configurationId, "authorization", StringComparison.OrdinalIgnoreCase))
			{
				_securityComponent.WarmUpAcl();
			}
		}


		private JsonConfigurationInstaller LoadConfigurationMetadata(string version)
		{
			// Загрузка конфигураций
			var configs = GetConfigurationsMetadata(version);

			// Загрузка метаданных конфигурации

			var configDictionary = configs.ToDictionary(c => (string)c.Id, c => (string)c.Name);

			var menu = GetMenuMetadata(version, configDictionary);
			var documents = GetDocumentsMetadata(version, configDictionary);
			var registers = GetRegistersMetadata(version, configDictionary);

			// Загрузка метаданных документов конфигурации

			var documentDictionary = documents.ToDictionary(c => (string)c.Id, c => c);

			var scenarios = GetScenariosMetadata(documentDictionary, configDictionary);
			var processes = GetProcessesMetadata(documentDictionary, configDictionary);
			var services = GetServicesMetadata(documentDictionary, configDictionary);
			var generators = GetGeneratorsMetadata(documentDictionary, configDictionary);
			var views = GetViewsMetadata(documentDictionary, configDictionary);
			var printViews = GetPrintViewsMetadata(documentDictionary, configDictionary);
			var validationErrors = GetValidationErrorsMetadata(documentDictionary, configDictionary);
			var validationWarnings = GetValidationWarningsMetadata(documentDictionary, configDictionary);

			return new JsonConfigurationInstaller(
				documents,
				menu,
				scenarios,
				processes,
				services,
				generators,
				views,
				printViews,
				validationErrors,
				validationWarnings,
				registers);
		}



		/// <summary>
		///     Обновление конфигурации при получении события обновления сборок
		///     Пока атомарность обновления не обеспечивается - в момент обновления обращающиеся к серверу запросы получат отлуп
		/// </summary>
		/// <param name="version">Версия измененяемого модуля</param>
		/// <param name="configurationId">Идентификатор конфигурации</param>
		private void OnChangeModules(string version, string configurationId)
		{
			//если конфигурация уже установлена - перезагружаем метаданные конфигурации и прикладные сборки
			IMetadataConfiguration config = _metadataConfigurationProvider.GetMetadataConfiguration(version,
																									configurationId);

			//если обновляется несистемная конфигурация (не встроенная конфигурация), то чистим метаданные загруженной конфигурации
			if (config != null && !config.IsEmbeddedConfiguration)
			{
				InfinniPlatformHostServer.Instance.RemoveConfiguration(version, configurationId);
				InfinniPlatformHostServer.Instance.UninstallServices(version, configurationId);
			}

			//если конфигурация еще не загружена или это не системная (встроенная) конфигурация, то устанавливаем конфигурацию
			if (config == null || !config.IsEmbeddedConfiguration)
			{
				InstallConfiguration(version, configurationId);
			}
		}





		private static IEnumerable<dynamic> GetConfigurations()
		{
			var managerApi = ManagerFactoryConfiguration.BuildConfigurationMetadataReader(null, true);
			var configurations = managerApi.GetItems();
			return configurations;
		}



		public IEnumerable<dynamic> GetConfigurationsMetadata(string version)
		{
			return _indexFactory.BuildVersionProvider("systemconfig", "metadata", MultitenancyExtensions.SystemTenant, version).GetDocument(null, 0, 100000);
		}

		public IEnumerable<dynamic> GetMenuMetadata(string version, Dictionary<string, string> configDictionary)
		{
			var menu = _indexFactory.BuildVersionProvider("systemconfig", "menumetadata", MultitenancyExtensions.SystemTenant, version).GetDocument(null, 0, 1000000);

			foreach (var menuItem in menu)
			{
				menuItem.ConfigId = menuItem.ParentId;
			}

			SetConfigId(menu, configDictionary);

			return menu;
		}

		public IEnumerable<dynamic> GetDocumentsMetadata(string version, Dictionary<string, string> configDictionary)
		{
			var documents = _indexFactory.BuildVersionProvider("systemconfig", "documentmetadata", MultitenancyExtensions.SystemTenant, version).GetDocument(null, 0, 1000000);

			SetConfigId(documents, configDictionary);

			return documents;
		}

		public IEnumerable<dynamic> GetRegistersMetadata(string version, Dictionary<string, string> configDictionary)
		{
			var registers = _indexFactory.BuildVersionProvider("systemconfig", "registermetadata", MultitenancyExtensions.SystemTenant, version).GetDocument(null, 0, 1000000);

			foreach (var register in registers)
			{
				register.ConfigId = register.ParentId;
			}

			SetConfigId(registers, configDictionary);

			return registers;
		}

		public IEnumerable<dynamic> GetScenariosMetadata(Dictionary<string, dynamic> documentDictionary, Dictionary<string, string> configDictionary)
		{
			return GetDocumentItemMetadata("scenariometadata", documentDictionary, configDictionary);
		}

		public IEnumerable<dynamic> GetProcessesMetadata(Dictionary<string, dynamic> documentDictionary, Dictionary<string, string> configDictionary)
		{
			return GetDocumentItemMetadata("processmetadata", documentDictionary, configDictionary);
		}

		public IEnumerable<dynamic> GetServicesMetadata(Dictionary<string, dynamic> documentDictionary, Dictionary<string, string> configDictionary)
		{
			return GetDocumentItemMetadata("servicemetadata", documentDictionary, configDictionary);
		}

		public IEnumerable<dynamic> GetGeneratorsMetadata(Dictionary<string, dynamic> documentDictionary, Dictionary<string, string> configDictionary)
		{
			return GetDocumentItemMetadata("generatormetadata", documentDictionary, configDictionary);
		}

		public IEnumerable<dynamic> GetViewsMetadata(Dictionary<string, dynamic> documentDictionary, Dictionary<string, string> configDictionary)
		{
			return GetDocumentItemMetadata("viewmetadata", documentDictionary, configDictionary);
		}

		public IEnumerable<dynamic> GetPrintViewsMetadata(Dictionary<string, dynamic> documentDictionary, Dictionary<string, string> configDictionary)
		{
			return GetDocumentItemMetadata("printviewmetadata", documentDictionary, configDictionary);
		}

		public IEnumerable<dynamic> GetValidationWarningsMetadata(Dictionary<string, dynamic> documentDictionary, Dictionary<string, string> configDictionary)
		{
			return GetDocumentItemMetadata("validationwarningmetadata", documentDictionary, configDictionary);
		}

		public IEnumerable<dynamic> GetValidationErrorsMetadata(Dictionary<string, dynamic> documentDictionary, Dictionary<string, string> configDictionary)
		{
			return GetDocumentItemMetadata("validationerrormetadata", documentDictionary, configDictionary);
		}


		private IEnumerable<dynamic> GetDocumentItemMetadata(string metadataType, Dictionary<string, dynamic> documentDictionary, Dictionary<string, string> configDictionary)
		{
			var items = _indexFactory.BuildVersionProvider("systemconfig", metadataType, MultitenancyExtensions.SystemTenant, null).GetDocument(null, 0, 1000000);

			foreach (var item in items)
			{
				SetDocumentId(item, documentDictionary, configDictionary);
				ConvertStringToJsonProperties(metadataType, item);
			}

			return items;
		}

		private static void SetDocumentId(dynamic item, Dictionary<string, dynamic> documentDictionary, Dictionary<string, string> configDictionary)
		{
			dynamic document;
			string documentUid = item.ParentId;

			if (documentUid != null && documentDictionary.TryGetValue(documentUid, out document))
			{
				item.DocumentId = document.Name;

				SetConfigId(item, document.ParentId, configDictionary);
			}
		}

		private static void SetConfigId(IEnumerable<dynamic> items, Dictionary<string, string> configDictionary)
		{
			foreach (var item in items)
			{
				SetConfigId(item, item.ParentId, configDictionary);
			}
		}

		private static void SetConfigId(dynamic item, string configUid, Dictionary<string, string> configDictionary)
		{
			string configId;

			if (configUid != null && configDictionary.TryGetValue(configUid, out configId))
			{
				item.ConfigId = configId;
			}
		}


		private static void ConvertStringToJsonProperties(string metadataType, dynamic resultItem)
		{
			// Некоторые свойства, представляющие собой сложные объекты, хранятся
			// в виде строки в формате JSON. Нижеследующий код десериализует
			// эти строки в динамические объекты. Код, естественно, ужасен,
			// но, по все видимости, это какие-то "пережитки прошлого".

			if (resultItem != null)
			{
				if (string.Equals(metadataType, "viewmetadata", StringComparison.OrdinalIgnoreCase))
				{
					resultItem.LayoutPanel = DeserializeJsonString(resultItem.LayoutPanel, metadataType);
					resultItem.ChildViews = DeserializeJsonString(resultItem.ChildViews, metadataType);
					resultItem.DataSources = DeserializeJsonString(resultItem.DataSources, metadataType);
					resultItem.Parameters = DeserializeJsonString(resultItem.Parameters, metadataType);
				}
				else if (string.Equals(metadataType, "validationerrormetadata", StringComparison.OrdinalIgnoreCase))
				{
					resultItem.ValidationOperator = DeserializeJsonString(resultItem.ValidationOperator, metadataType);
				}
				else if (string.Equals(metadataType, "validationwarningmetadata", StringComparison.OrdinalIgnoreCase))
				{
					resultItem.ValidationOperator = DeserializeJsonString(resultItem.ValidationOperator, metadataType);
				}
				else if (string.Equals(metadataType, "printviewmetadata", StringComparison.OrdinalIgnoreCase))
				{
					resultItem.Blocks = DeserializeJsonString(resultItem.Blocks, metadataType);
				}
				else if (string.Equals(metadataType, "reportmetadata", StringComparison.OrdinalIgnoreCase))
				{
					resultItem.Content = DeserializeJsonString(resultItem.Content, metadataType);
				}
			}
		}

		private static object DeserializeJsonString(object value, string metadataType)
		{
			dynamic dynamicValue = value as IDynamicMetaObjectProvider;

			if (dynamicValue != null && dynamicValue.JsonString is string)
			{
				try
				{
					return DynamicWrapperExtensions.ToDynamic(dynamicValue.JsonString);
				}
				catch (Exception e)
				{
					throw new InvalidOperationException(string.Format("Cannot deserialize '{0}' from JsonString.", metadataType), e);
				}
			}

			return value;
		}
	}
}