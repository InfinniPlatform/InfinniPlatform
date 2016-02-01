using System;
using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;

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
            configuration.Subfolder = "InfinniPlatform";

            return configuration;
        }

        public override void ReplaceItem(dynamic item)
        {
            var serializedItem = JsonObjectSerializer.Formated.Serialize(item);

            File.WriteAllBytes(PackageMetadataLoader.GetConfigurationPath(item.Name, item.Subfolder), serializedItem);

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
            return PackageMetadataLoader.GetConfiguration(itemId);
        }

        public override IEnumerable<object> GetItems()
        {
            return PackageMetadataLoader.GetConfigurations();
        }
    }
}