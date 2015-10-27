using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	public class PackageMetadataLoader
	{
		private static Lazy<Dictionary<string, dynamic>> _configurations = new Lazy<Dictionary<string, dynamic>>(LoadConfigsMetadata);

		public static Dictionary<string, dynamic> Configurations
		{
			get { return _configurations.Value; }
		}

		public static void UpdateCache()
		{
			_configurations = new Lazy<Dictionary<string, dynamic>>(LoadConfigsMetadata);
		}

		public static Dictionary<string, dynamic> LoadConfigsMetadata()
		{
			var contentDirectory = AppSettings.GetValue("ContentDirectory", "..\\Assemblies\\content");

			var metadataDirectories = Directory.EnumerateDirectories(contentDirectory)
											   .Select(d => Path.Combine(d, "metadata"))
											   .Where(Directory.Exists)
											   .ToArray();

			IEnumerable<dynamic> loadConfigsMetadata = metadataDirectories
				.SelectMany(Directory.EnumerateDirectories)
				.Select(LoadConfigMetadata);

			var dictionary = loadConfigsMetadata.ToDictionary(config => (string)config.Content.Name, confog => confog);

			return dictionary;
		}

		public static DynamicWrapper LoadConfigMetadata(string configDirectory)
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

		public static Dictionary<string, dynamic> LoadDocumentsMetadata(string configDirectory, object configId)
		{
			var documentsDirectory = Path.Combine(configDirectory, "Documents");

			if (Directory.Exists(documentsDirectory))
			{
				IEnumerable<dynamic> enumerable = Directory.EnumerateDirectories(documentsDirectory)
														   .Select(d => LoadDocumentMetadata(d, configId));

				return enumerable.ToDictionary(document => (string)document.Content.Name, document => document);
			}

			return new Dictionary<string, dynamic>();
		}

		public static object LoadDocumentMetadata(string documentDirectory, object configId)
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

		public static Dictionary<string, dynamic> LoadItemsMetadata(string documentDirectory, string itemsContainer, object configId, object documentId = null)
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

			return new Dictionary<string, dynamic>();
		}

		public static object LoadItemMetadata(string filePath)
		{
			using (var reader = File.OpenRead(filePath))
			{
				dynamic loadItemMetadata = new DynamicWrapper();
				loadItemMetadata.FilePath = filePath;
				loadItemMetadata.Content = JsonObjectSerializer.Default.Deserialize(reader, typeof(DynamicWrapper));

				return loadItemMetadata;
			}
		}
	}
}