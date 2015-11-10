using System;
using System.Collections.Generic;
using System.IO;

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
			configuration.Menu = new object[] { };
			configuration.Documents = new object[] { };
			configuration.Registers = new object[] { };
			configuration.Assemblies = new object[] { };
			configuration.Reports = new object[] { };

			return configuration;
		}

		public override void ReplaceItem(dynamic item)
		{
			string filePath;
			var serializedItem = JsonObjectSerializer.Formated.Serialize(item);

			var oldConfiguration = PackageMetadataLoader.GetConfiguration(item.Name);

			if (oldConfiguration != null)
			{
				filePath = oldConfiguration.FilePath;
			}
			else
			{
				var contentDirectory = AppSettings.GetValue("ContentDirectory", "..\\Assemblies\\content");
				var configurationDirectory = Path.Combine(contentDirectory, item.Subfolder ?? "InfinniPlatform", "metadata", item.Name);
				Directory.CreateDirectory(configurationDirectory);
				filePath = Path.Combine(configurationDirectory, "Configuration.json");
			}

			File.WriteAllBytes(filePath, serializedItem);

			PackageMetadataLoader.UpdateCache();
		}

		public override void DeleteItem(string itemId)
		{
			dynamic configuration = PackageMetadataLoader.GetConfiguration(itemId);

			var configurationDirectory = Path.GetDirectoryName(configuration.FilePath);

			if (configurationDirectory != null)
			{
				Directory.Delete(configurationDirectory, true);
				PackageMetadataLoader.UpdateCache();
			}
		}

		public override object GetItem(string itemId)
		{
			return PackageMetadataLoader.GetConfigurationContent(itemId);
		}

		public override IEnumerable<object> GetItems()
		{
			return PackageMetadataLoader.GetConfigurations();
		}
	}
}