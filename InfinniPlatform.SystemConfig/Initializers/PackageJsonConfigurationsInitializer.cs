﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Hosting;
using InfinniPlatform.Logging;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Log;
using InfinniPlatform.Sdk.Environment.Settings;
using InfinniPlatform.WebApi.Factories;

namespace InfinniPlatform.SystemConfig.Initializers
{
	public sealed class PackageJsonConfigurationsInitializer : IStartupInitializer
	{
		public PackageJsonConfigurationsInitializer(ISecurityComponent securityComponent)
		{
			_securityComponent = securityComponent;

			_configurations = new Lazy<IEnumerable<DynamicWrapper>>(LoadConfigsMetadata);

			var watcher = new FileSystemWatcher(AppSettings.GetValue("ContentDirectory", "content"), "*.json")
			{
				IncludeSubdirectories = true
			};

			watcher.Changed += (sender, args) =>
			{
				if (FileHistoryHelper.IsChanged(args.FullPath))
				{
					try
					{
						Console.WriteLine("[{1}] File {0} changed.", args.Name, DateTime.Now.TimeOfDay);

						foreach (dynamic configuration in _configurations.Value)
						{
							RemoveConfiguration(configuration.Name);
						}

						var updatedConfigurations = LoadConfigsMetadata();

						foreach (dynamic configuration in updatedConfigurations)
						{
							InstallConfiguration(configuration, configuration.Name);
						}

						Console.WriteLine("[{0}] Configurations successfully updated.", DateTime.Now.TimeOfDay);
					}
					catch (Exception e)
					{
					    Logger.Log.Error("Error during metadata update.", null, e);
					}
				}
			};

			watcher.EnableRaisingEvents = true;
		}

		private readonly ISecurityComponent _securityComponent;
		private readonly Lazy<IEnumerable<DynamicWrapper>> _configurations;


		public void OnStart(HostingContextBuilder contextBuilder)
		{
			// Получение списка всех установленных конфигураций
			var configurations = _configurations.Value;

			// Загрузка и кэширование метаданных каждой конфигурации
			foreach (dynamic configuration in configurations)
			{
				string configurationId = configuration.Name;
				string configurationVersion = configuration.Version;

				InstallConfiguration(configuration, configurationId, configurationVersion);
			}
		}


		private void InstallConfiguration(object configuration, string configId, string configVersion = null)
		{
			// Загрузка метеданных конфигурации для кэширования
			var metadataCacheFiller = LoadConfigurationMetadata(configuration);

			// Создание менеджера кэша метаданных конфигураций
			var metadataCacheManager = InfinniPlatformHostServer.Instance.CreateConfiguration(configId, false, configVersion);

			// Загрузка метаданных конфигурации в кэш
			metadataCacheFiller.InstallConfiguration(metadataCacheManager);

			// Создание обработчика скриптов
			metadataCacheManager.ScriptConfiguration.InitActionUnitStorage(configVersion);

			// Создание сервисов конфигурации
			InfinniPlatformHostServer.Instance.InstallServices(configVersion, metadataCacheManager.ServiceRegistrationContainer);

			// Загрузка ACL после установки конфигурации "Authorization"
			if (string.Equals(configId, "authorization", StringComparison.OrdinalIgnoreCase))
			{
				_securityComponent.WarmUpAcl();
			}
		}


		static void RemoveConfiguration(string configurationName)
		{
			// Удаление сервисов конфигурации
			InfinniPlatformHostServer.Instance.UninstallServices(configurationName);

			// Удаление метаданных конфигурации из кэша
			InfinniPlatformHostServer.Instance.RemoveConfiguration(configurationName);
		}


		private static PackageJsonConfigurationInstaller LoadConfigurationMetadata(dynamic configuration)
		{
			IEnumerable<dynamic> menu = configuration.Menu;
			IEnumerable<dynamic> registers = configuration.Registers;
			IEnumerable<dynamic> documents = configuration.Documents;
			IEnumerable<dynamic> scenarios = documents.SelectMany(i => (IEnumerable<dynamic>)i.Scenarios).ToArray();
			IEnumerable<dynamic> processes = documents.SelectMany(i => (IEnumerable<dynamic>)i.Processes).ToArray();
			IEnumerable<dynamic> services = documents.SelectMany(i => (IEnumerable<dynamic>)i.Services).ToArray();
			IEnumerable<dynamic> generators = documents.SelectMany(i => (IEnumerable<dynamic>)i.Generators).ToArray();
			IEnumerable<dynamic> views = documents.SelectMany(i => (IEnumerable<dynamic>)i.Views).ToArray();
			IEnumerable<dynamic> printViews = documents.SelectMany(i => (IEnumerable<dynamic>)i.PrintViews).ToArray();
			IEnumerable<dynamic> validationErrors = documents.SelectMany(i => (IEnumerable<dynamic>)i.ValidationErrors).ToArray();
			IEnumerable<dynamic> validationWarnings = documents.SelectMany(i => (IEnumerable<dynamic>)i.ValidationWarnings).ToArray();

			return new PackageJsonConfigurationInstaller(
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


		private static IEnumerable<DynamicWrapper> LoadConfigsMetadata()
		{
			var contentDirectory = AppSettings.GetValue("ContentDirectory", "content");

			var metadataDirectories = Directory.EnumerateDirectories(contentDirectory)
											   .Select(d => Path.Combine(d, "metadata"))
											   .Where(Directory.Exists)
											   .ToArray();

			return metadataDirectories
				.SelectMany(Directory.EnumerateDirectories)
				.Select(LoadConfigMetadata)
				.ToArray();
		}

		private static DynamicWrapper LoadConfigMetadata(string configDirectory)
		{
			var configFile = Path.Combine(configDirectory, "Configuration.json");

			dynamic configuration = LoadItemMetadata(configFile);

			object configId = configuration.Name;

			configuration.Version = null;
			configuration.Menu = LoadItemsMetadata(configDirectory, "Menu", configId);
			configuration.Registers = LoadItemsMetadata(configDirectory, "Registers", configId);
			configuration.Documents = LoadDocumentsMetadata(configDirectory, configId);

			return configuration;
		}

		private static IEnumerable<object> LoadDocumentsMetadata(string configDirectory, object configId)
		{
			var documentsDirectory = Path.Combine(configDirectory, "Documents");

			if (Directory.Exists(documentsDirectory))
			{
				return Directory.EnumerateDirectories(documentsDirectory)
						 .Select(d => LoadDocumentMetadata(d, configId))
						 .ToArray();
			}

			return Enumerable.Empty<object>();
		}

		private static object LoadDocumentMetadata(string documentDirectory, object configId)
		{
			var documentFile = Directory.EnumerateFiles(documentDirectory, "*.json").FirstOrDefault();

			dynamic document = LoadItemMetadata(documentFile);

			object documentId = document.Name;

			document.ConfigId = configId;
			document.Views = LoadItemsMetadata(documentDirectory, "Views", configId, documentId);
			document.PrintViews = LoadItemsMetadata(documentDirectory, "PrintViews", configId, documentId);
			document.Scenarios = LoadItemsMetadata(documentDirectory, "Scenarios", configId, documentId);
			document.Processes = LoadItemsMetadata(documentDirectory, "Processes", configId, documentId);
			document.Services = LoadItemsMetadata(documentDirectory, "Services", configId, documentId);
			document.Generators = LoadItemsMetadata(documentDirectory, "Generators", configId, documentId);
			document.ValidationWarnings = LoadItemsMetadata(documentDirectory, "ValidationWarnings", configId, documentId);
			document.ValidationErrors = LoadItemsMetadata(documentDirectory, "ValidationErrors", configId, documentId);
			document.DocumentStatuses = LoadItemsMetadata(documentDirectory, "DocumentStatuses", configId, documentId);

			return document;
		}

		private static IEnumerable<object> LoadItemsMetadata(string documentDirectory, string itemsContainer, object configId, object documentId = null)
		{
			var itemsDirectory = Path.Combine(documentDirectory, itemsContainer);

			if (Directory.Exists(itemsDirectory))
			{
				var itemsMetadata = Directory.EnumerateFiles(itemsDirectory, "*.json", SearchOption.AllDirectories)
											 .Select(LoadItemMetadata)
											 .ToArray();

				foreach (dynamic item in itemsMetadata)
				{
					item.ConfigId = configId;
					item.DocumentId = documentId;
				}

				return itemsMetadata;
			}

			return Enumerable.Empty<object>();
		}

		private static object LoadItemMetadata(string fileName)
		{
			using (var reader = File.OpenRead(fileName))
			{
				return JsonObjectSerializer.Default.Deserialize(reader, typeof(DynamicWrapper));
			}
		}
	}
}