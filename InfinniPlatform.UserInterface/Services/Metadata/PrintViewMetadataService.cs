using System;
using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
    /// <summary>
    /// Сервис для работы с метаданными печатных представлений.
    /// </summary>
    internal sealed class PrintViewMetadataService : BaseMetadataService
    {
        public PrintViewMetadataService(string configId, string documentId)
        {
            ConfigId = configId;
            _documentId = documentId;
        }

        private readonly string _documentId;

        public string ConfigId { get; }

        public override object CreateItem()
        {
            dynamic printView = new DynamicWrapper();

            printView.Id = Guid.NewGuid().ToString();
            printView.Name = string.Empty;
            printView.Caption = string.Empty;
            printView.Description = string.Empty;

            return printView;
        }

        public override void ReplaceItem(dynamic item)
        {
            var serializedItem = JsonObjectSerializer.Formated.Serialize(item);

            File.WriteAllBytes(PackageMetadataLoader.GetPrintViewPath(ConfigId, _documentId, item.Name), serializedItem);

            PackageMetadataLoader.UpdateCache();
        }

        public override void DeleteItem(string itemId)
        {
            File.Delete(PackageMetadataLoader.GetPrintViewPath(ConfigId, _documentId, itemId));

            PackageMetadataLoader.UpdateCache();
        }

        public override object GetItem(string itemId)
        {
            return PackageMetadataLoader.GetPrintView(ConfigId, _documentId, itemId);
        }

        public override IEnumerable<object> GetItems()
        {
            return PackageMetadataLoader.GetPrintViews(ConfigId, _documentId);
        }
    }
}