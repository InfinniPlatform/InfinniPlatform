using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Sdk.Dynamic;

using Newtonsoft.Json;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными документов.
	/// </summary>
	internal sealed class DocumentMetadataService : BaseMetadataService
	{
		public DocumentMetadataService(string version, string configId, string server, int port, string route)
			: base(version, server, port, route)
		{
			_configId = configId;
		}

		private readonly string _configId;

		public string ConfigId
		{
			get { return _configId; }
		}

		public override object CreateItem()
		{
			dynamic document = new DynamicWrapper();

			document.Id = Guid.NewGuid().ToString();
			document.Services = new dynamic[] { };
			document.Processes = new dynamic[] { };
			document.Scenarios = new dynamic[] { };
			document.Generators = new dynamic[] { };
			document.Views = new dynamic[] { };
			document.PrintViews = new dynamic[] { };
			document.ValidationWarnings = new dynamic[] { };
			document.ValidationErrors = new dynamic[] { };

			return document;
		}

		public override void ReplaceItem(dynamic item)
		{
			string filePath;
			var serializedItem = JsonObjectSerializer.Formated.Serialize(item);

			//TODO Wrapper for PackageMetadataLoader.Configurations
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			if (configuration.Documents.ContainsKey(item.Name))
			{
				dynamic oldDocument = configuration.Documents[item.Name];
				filePath = oldDocument.FilePath;
			}
			else
			{
				string directoryPath = Path.Combine(Path.GetDirectoryName(configuration.FilePath), "Documents", item.Name);
				Directory.CreateDirectory(directoryPath);

				filePath = Path.Combine(directoryPath, string.Concat(item.Name, ".json"));
			}

			File.WriteAllBytes(filePath, serializedItem);

			PackageMetadataLoader.UpdateCache();
		}

		public override void DeleteItem(string itemId)
		{
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			dynamic document = configuration.Documents[itemId];

			var documentDirectory = Path.GetDirectoryName(document.FilePath);

			if (documentDirectory != null)
			{
				Directory.Delete(documentDirectory, true);
				PackageMetadataLoader.UpdateCache();
			}
		}

		public override object GetItem(string itemId)
		{
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			return configuration.Documents[itemId].Content;
		}

		public override IEnumerable<object> GetItems()
		{
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			Dictionary<string, dynamic> documents = configuration.Documents;
			return documents.Values.Select(o => o.Content);
		}
	}
}