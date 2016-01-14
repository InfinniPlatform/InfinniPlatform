using System;
using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
    /// <summary>
    /// Сервис для работы с метаданными документов.
    /// </summary>
    internal sealed class DocumentMetadataService : BaseMetadataService
    {
        public DocumentMetadataService(string configId)
        {
            ConfigId = configId;
        }

        public string ConfigId { get; }

        public override object CreateItem()
        {
            dynamic document = new DynamicWrapper();

            document.Id = Guid.NewGuid().ToString();
            document.Services = new object[] { };
            document.Processes = new object[] { };
            document.Scenarios = new object[] { };
            document.Generators = new object[] { };
            document.Views = new object[] { };
            document.PrintViews = new object[] { };
            document.ValidationWarnings = new object[] { };
            document.ValidationErrors = new object[] { };

            return document;
        }

        public override void ReplaceItem(dynamic item)
        {
            var serializedItem = JsonObjectSerializer.Formated.Serialize(item);

            File.WriteAllBytes(PackageMetadataLoader.GetDocumentPath(ConfigId, item.Name), serializedItem);

            PackageMetadataLoader.UpdateCache();
        }

        public override void DeleteItem(string itemId)
        {
            var documentPath = PackageMetadataLoader.GetDocumentPath(ConfigId, itemId);

            var documentDirectory = Path.GetDirectoryName(documentPath);

            if (!string.IsNullOrEmpty(documentDirectory) && Directory.Exists(documentDirectory))
            {
                Directory.Delete(documentDirectory, true);

                PackageMetadataLoader.UpdateCache();
            }
        }

        public override object GetItem(string itemId)
        {
            return PackageMetadataLoader.GetDocument(ConfigId, itemId);
        }

        public override IEnumerable<object> GetItems()
        {
            return PackageMetadataLoader.GetDocuments(ConfigId);
        }
    }
}