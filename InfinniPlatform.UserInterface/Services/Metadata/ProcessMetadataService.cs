using System;
using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Core.Serialization;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
    /// <summary>
    /// Сервис для работы с метаданными бизнес-процессов.
    /// </summary>
    internal sealed class ProcessMetadataService : BaseMetadataService
    {
        public ProcessMetadataService(string configId, string documentId)
        {
            ConfigId = configId;
            _documentId = documentId;
        }

        private readonly string _documentId;

        public string ConfigId { get; }

        public override object CreateItem()
        {
            dynamic process = new DynamicWrapper();

            process.Id = Guid.NewGuid().ToString();
            process.Name = string.Empty;
            process.Caption = string.Empty;
            process.Description = string.Empty;

            return process;
        }

        public override void ReplaceItem(dynamic item)
        {
            var serializedItem = JsonObjectSerializer.Formated.Serialize(item);

            File.WriteAllBytes(PackageMetadataLoader.GetProcessPath(ConfigId, _documentId, item.Name), serializedItem);

            PackageMetadataLoader.UpdateCache();
        }

        public override void DeleteItem(string itemId)
        {
            File.Delete(PackageMetadataLoader.GetProcessPath(ConfigId, _documentId, itemId));

            PackageMetadataLoader.UpdateCache();
        }

        public override object GetItem(string itemId)
        {
            return PackageMetadataLoader.GetProcess(ConfigId, _documentId, itemId);
        }

        public override IEnumerable<object> GetItems()
        {
            return PackageMetadataLoader.GetProcesses(ConfigId, _documentId);
        }
    }
}