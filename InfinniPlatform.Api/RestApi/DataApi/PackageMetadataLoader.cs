using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.Api.RestApi.DataApi
{
	/// <summary>
	/// Кэш метаданных для конфигуратора (UserInterface.exe).
	/// </summary>
	public class PackageMetadataLoader
	{
		/// <summary>
		/// Кэш конфигураций.
		/// </summary>
		public static Dictionary<string, object> Configurations { get; private set; } = LoadConfigsMetadata();

		/// <summary>
		/// Обновить кэш конфигураций.
		/// </summary>
		public static void UpdateCache(string metadataPath = null)
		{
			Configurations = LoadConfigsMetadata(metadataPath);
		}

		private static Dictionary<string, object> LoadConfigsMetadata(string metadataDirectory = null)
		{
			if (metadataDirectory == null)
			{
				metadataDirectory = Path.GetFullPath(AppSettings.GetValue("ContentDirectory", Path.Combine("..", "Assemblies", "content")));
			}

			var metadataDirectories = Directory.EnumerateDirectories(metadataDirectory)
			                                   .Select(d => Path.Combine(d, "metadata"))
			                                   .Where(Directory.Exists)
			                                   .ToArray();

			IEnumerable<dynamic> loadConfigsMetadata = metadataDirectories
				.SelectMany(Directory.EnumerateDirectories)
				.Select(LoadConfigMetadata);

			Dictionary<string, object> dictionary = loadConfigsMetadata.ToDictionary(config => (string)config.Content.Name, config => config);

			return dictionary;
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

		private static Dictionary<string, object> LoadDocumentsMetadata(string configDirectory, object configId)
		{
			var documentsDirectory = Path.Combine(configDirectory, "Documents");

			if (Directory.Exists(documentsDirectory))
			{
				IEnumerable<dynamic> enumerable = Directory.EnumerateDirectories(documentsDirectory)
				                                           .Select(d => LoadDocumentMetadata(d, configId));

				return enumerable.ToDictionary(document => (string)document.Content.Name, document => document);
			}

			return new Dictionary<string, object>();
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

		private static Dictionary<string, object> LoadItemsMetadata(string documentDirectory, string itemsContainer, object configId, object documentId = null)
		{
			var itemsDirectory = Path.Combine(documentDirectory, itemsContainer);

			if (Directory.Exists(itemsDirectory))
			{
				dynamic[] itemsMetadata = Directory.EnumerateFiles(itemsDirectory, "*.json", SearchOption.AllDirectories)
				                                   .Select(LoadItemMetadata)
				                                   .ToArray();

				foreach (var item in itemsMetadata)
				{
					item.ConfigId = configId;
					item.DocumentId = documentId;
				}

				return itemsMetadata.ToDictionary(item => (string)item.Content.Name, item => item);
			}

			return new Dictionary<string, object>();
		}

		private static object LoadItemMetadata(string filePath)
		{
			using (var reader = File.OpenRead(filePath))
			{
				dynamic loadItemMetadata = new DynamicWrapper();
				loadItemMetadata.FilePath = filePath;
				loadItemMetadata.Content = JsonObjectSerializer.Default.Deserialize(reader, typeof(DynamicWrapper));

				return loadItemMetadata;
			}
		}

		public static object GetConfiguration(string configId)
		{
			dynamic configuration = Configurations[configId];
			return configuration.Content;
		}

		public static IEnumerable<object> GetConfigurations()
		{
			Dictionary<string, dynamic>.ValueCollection valueCollection = Configurations.Values;
			return valueCollection.Select(o => o.Content);
		}

		private static object GetConfigurationElements(string configId)
		{
			dynamic configuration = Configurations[configId];
			return configuration;
		}

		public static object GetDocument(string configId, string docId)
		{
			dynamic configuration = Configurations[configId];
			return configuration.Documents[docId].Content;
		}

		public static IEnumerable<object> GetDocuments(string configId)
		{
			dynamic configuration = Configurations[configId];
			Dictionary<string, dynamic> documents = configuration.Documents;
			return documents.Values.Select(o => o.Content);
		}

		public static object GetView(string configId, string documentId, string viewId)
		{
			dynamic configuration = Configurations[configId];
			return configuration.Documents[documentId].Views[viewId].Content;
		}

		public static IEnumerable<object> GetViews(string configId, string documentId)
		{
			dynamic configuration = Configurations[configId];
			Dictionary<string, dynamic> views = configuration.Documents[documentId].Views;
			return views.Values.Select(o => o.Content);
		}

		public static object GetAssembly(string configId, string assemblyId)
		{
			dynamic configuration = Configurations[configId];
			IEnumerable<dynamic> assemblies = configuration.Content.Assemblies;
			return assemblies.FirstOrDefault(assembly => assembly.Name == assemblyId);
		}

		public static IEnumerable<object> GetAssemblies(string configId)
		{
			dynamic configuration = Configurations[configId];
			return configuration.Content.Assemblies;
		}

		public static object GetMenu(string configId, string menuId)
		{
			dynamic configuration = Configurations[configId];
			return configuration.Menu[menuId].Content;
		}

		public static IEnumerable<object> GetMenus(string configId)
		{
			dynamic configuration = Configurations[configId];
			Dictionary<string, dynamic> documents = configuration.Menu;
			return documents.Values.Select(o => o.Content);
		}

		public static object GetPrintView(string configId, string documentId, string printViewId)
		{
			dynamic configuration = Configurations[configId];
			return configuration.Documents[documentId].PrintViews[printViewId].Content;
		}

		public static IEnumerable<object> GetPrintViews(string configId, string documentId)
		{
			dynamic configuration = Configurations[configId];
			Dictionary<string, dynamic> printViews = configuration.Documents[documentId].PrintViews;
			return printViews.Values.Select(o => o.Content);
		}

		public static object GetProcess(string configId, string documentId, string processId)
		{
			dynamic configuration = Configurations[configId];
			return configuration.Documents[documentId].Processes[processId].Content;
		}

		public static IEnumerable<object> GetProcesses(string configId, string documentId)
		{
			dynamic configuration = Configurations[configId];
			Dictionary<string, dynamic> processes = configuration.Documents[documentId].Processes;
			return processes.Values.Select(o => o.Content);
		}

		public static object GetRegister(string configId, string registerId)
		{
			dynamic configuration = Configurations[configId];
			return configuration.Registers[registerId].Content;
		}

		public static IEnumerable<object> GetRegisters(string configId)
		{
			dynamic configuration = Configurations[configId];
			Dictionary<string, dynamic> documents = configuration.Registers;
			return documents.Values.Select(o => o.Content);
		}

		public static object GetService(string configId, string documentId, string serviceId)
		{
			dynamic configuration = Configurations[configId];
			return configuration.Documents[documentId].Services[serviceId].Content;
		}

		public static IEnumerable<object> GetServices(string configId, string documentId)
		{
			dynamic configuration = Configurations[configId];
			Dictionary<string, dynamic> processes = configuration.Documents[documentId].Services;
			return processes.Values.Select(o => o.Content);
		}

		public static object GetScenario(string configId, string documentId, string scenarioId)
		{
			dynamic configuration = Configurations[configId];
			return configuration.Documents[documentId].Scenarios[scenarioId].Content;
		}

		public static IEnumerable<object> GetScenarios(string configId, string documentId)
		{
			dynamic configuration = Configurations[configId];
			Dictionary<string, dynamic> processes = configuration.Documents[documentId].Scenarios;
			return processes.Values.Select(o => o.Content);
		}
	}
}