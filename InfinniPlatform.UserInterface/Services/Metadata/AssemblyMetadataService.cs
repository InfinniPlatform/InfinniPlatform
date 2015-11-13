using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
    /// <summary>
    /// Сервис для работы с метаданными сборок.
    /// </summary>
    internal sealed class AssemblyMetadataService : BaseMetadataService
    {
        public AssemblyMetadataService(string configId)
        {
            ConfigId = configId;
        }

        public string ConfigId { get; }

        public override object CreateItem()
        {
            dynamic assembly = new DynamicWrapper();

            assembly.Id = Guid.NewGuid().ToString();
            assembly.Name = string.Empty;
            assembly.Caption = string.Empty;
            assembly.Description = string.Empty;

            return assembly;
        }

        public override void ReplaceItem(dynamic item)
        {
            dynamic configurationContent = PackageMetadataLoader.GetConfigurationContent(ConfigId);
            var assemblies = PackageMetadataLoader.GetAssemblies(ConfigId);

            var newAssembliesList = new List<object>(assemblies) { item };

            configurationContent.Assemblies = newAssembliesList;

            var filePath = (dynamic)PackageMetadataLoader.GetConfigurationPath(ConfigId);

            var serializedItem = JsonObjectSerializer.Formated.Serialize(configurationContent.Content);

            File.WriteAllBytes(filePath, serializedItem);

            PackageMetadataLoader.UpdateCache();
        }

        public override void DeleteItem(string itemId)
        {
            dynamic configurationContent = PackageMetadataLoader.GetConfigurationContent(ConfigId);
            IEnumerable<dynamic> assemblies = PackageMetadataLoader.GetAssemblies(ConfigId);

            var newAssembliesList = assemblies.Where(assembly => assembly.Name != itemId)
                                              .ToArray();

            configurationContent.Assemblies = newAssembliesList;

            var filePath = (dynamic)PackageMetadataLoader.GetConfigurationPath(ConfigId);

            var serializedItem = JsonObjectSerializer.Formated.Serialize(configurationContent);

            File.WriteAllBytes(filePath, serializedItem);

            PackageMetadataLoader.UpdateCache();
        }

        public override object GetItem(string itemId)
        {
            return PackageMetadataLoader.GetAssembly(ConfigId, itemId);
        }

        public override IEnumerable<object> GetItems()
        {
            return PackageMetadataLoader.GetAssemblies(ConfigId);
        }
    }
}