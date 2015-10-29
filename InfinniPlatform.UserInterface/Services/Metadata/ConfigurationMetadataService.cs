using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными конфигурации.
	/// </summary>
	internal sealed class ConfigurationMetadataService : BaseMetadataService
	{
		public override object CreateItem()
		{
			dynamic configuration = new DynamicWrapper();

			configuration.Id = Guid.NewGuid().ToString();
			configuration.Name = string.Empty;
			configuration.Caption = string.Empty;
			configuration.Description = string.Empty;
			configuration.Menu = new dynamic[] { };
			configuration.Documents = new dynamic[] { };
			configuration.Registers = new dynamic[] { };
			configuration.Assemblies = new dynamic[] { };
			configuration.Reports = new dynamic[] { };

			return configuration;
		}

		public override void ReplaceItem(dynamic item)
		{
			string filePath;
			var serializedItem = JsonObjectSerializer.Formated.Serialize(item);
			
			//TODO Wrapper for PackageMetadataLoader.Configurations
			if (PackageMetadataLoader.Configurations.ContainsKey(item.Name))
			{
				dynamic oldConfiguration = PackageMetadataLoader.Configurations[item.Name];
				filePath = oldConfiguration.FilePath;
			}
			else
			{
				var contentDirectory = AppSettings.GetValue("", "..\\Assemblies\\content");
				var configurationDirectory = Path.Combine(contentDirectory, item.Subfolder ?? "InfinniPlatform", "metadata", item.Name);
				Directory.CreateDirectory(configurationDirectory);
				filePath = Path.Combine(configurationDirectory, "Configuration.json");
			}

			File.WriteAllBytes(filePath, serializedItem);

			PackageMetadataLoader.UpdateCache();
		}

		public override void DeleteItem(string itemId)
		{
			dynamic configuration = PackageMetadataLoader.Configurations[itemId];

			var configurationDirectory = Path.GetDirectoryName(configuration.FilePath);

			if (configurationDirectory != null)
			{
				Directory.Delete(configurationDirectory, true);
				PackageMetadataLoader.UpdateCache();
			}
		}

		public override object GetItem(string itemId)
		{
			dynamic configuration = PackageMetadataLoader.Configurations[itemId];
			return configuration.Content;
		}

		public override IEnumerable<object> GetItems()
		{
			Dictionary<string, dynamic>.ValueCollection valueCollection = PackageMetadataLoader.Configurations.Values;
			return valueCollection.Select(o => o.Content);
		}
	}
}