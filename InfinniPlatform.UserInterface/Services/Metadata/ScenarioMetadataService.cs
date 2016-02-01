using System;
using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
    /// <summary>
    /// Сервис для работы с метаданными бизнес-сценариев.
    /// </summary>
    internal sealed class ScenarioMetadataService : BaseMetadataService
    {
        public ScenarioMetadataService(string configId, string documentId)
        {
            ConfigId = configId;
            _documentId = documentId;
        }

        private readonly string _documentId;

        public string ConfigId { get; }

        public override object CreateItem()
        {
            dynamic scenario = new DynamicWrapper();

            scenario.Id = Guid.NewGuid().ToString();
            scenario.Name = string.Empty;
            scenario.Caption = string.Empty;
            scenario.Description = string.Empty;

            return scenario;
        }

        public override void ReplaceItem(dynamic item)
        {
            var serializedItem = JsonObjectSerializer.Formated.Serialize(item);

            File.WriteAllBytes(PackageMetadataLoader.GetScenarioPath(ConfigId, _documentId, item.Name), serializedItem);

            PackageMetadataLoader.UpdateCache();
        }

        public override void DeleteItem(string itemId)
        {
            dynamic scenario = PackageMetadataLoader.GetScenario(ConfigId, _documentId, itemId);

            File.Delete(scenario.FilePath);

            PackageMetadataLoader.UpdateCache();
        }

        public override object GetItem(string itemId)
        {
            return PackageMetadataLoader.GetScenario(ConfigId, _documentId, itemId);
        }

        public override IEnumerable<object> GetItems()
        {
            return PackageMetadataLoader.GetScenarios(ConfigId, _documentId);
        }
    }
}