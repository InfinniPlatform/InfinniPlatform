using System;
using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Sdk.Dynamic;

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
			string filePath;
			var serializedItem = JsonObjectSerializer.Formated.Serialize(item);

			dynamic oldDocument = PackageMetadataLoader.GetDocument(ConfigId, item.Name);
			if (oldDocument != null)
			{
				
				filePath = oldDocument.FilePath;
			}
			else
			{
				dynamic config = PackageMetadataLoader.GetConfiguration(ConfigId);
				string directoryPath = Path.Combine(Path.GetDirectoryName(config.FilePath), "Documents", item.Name);
				Directory.CreateDirectory(directoryPath);

				filePath = Path.Combine(directoryPath, string.Concat(item.Name, ".json"));
			}

			File.WriteAllBytes(filePath, serializedItem);

			PackageMetadataLoader.UpdateCache();
		}

		public override void DeleteItem(string itemId)
		{
			dynamic document = PackageMetadataLoader.GetDocument(ConfigId, itemId);

			var documentDirectory = Path.GetDirectoryName(document.FilePath);

			if (documentDirectory != null)
			{
				Directory.Delete(documentDirectory, true);
				PackageMetadataLoader.UpdateCache();
			}
		}

		public override object GetItem(string itemId)
		{
			return PackageMetadataLoader.GetDocumentContent(ConfigId, itemId);
		}

		public override IEnumerable<object> GetItems()
		{
			return PackageMetadataLoader.GetDocuments(ConfigId);
		}
	}
}