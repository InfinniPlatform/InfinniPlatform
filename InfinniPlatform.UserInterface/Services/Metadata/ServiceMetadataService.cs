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
	/// Сервис для работы с метаданными бизнес-сервисов.
	/// </summary>
	internal sealed class ServiceMetadataService : BaseMetadataService
	{
		public ServiceMetadataService(string version, string configId, string documentId, string server, int port, string route)
			: base(version, server, port, route)
		{
			_configId = configId;
			_documentId = documentId;
		}

		private readonly string _configId;
		private readonly string _documentId;

		public string ConfigId
		{
			get { return _configId; }
		}

		public override object CreateItem()
		{
			dynamic service = new DynamicWrapper();

			service.Id = Guid.NewGuid().ToString();
			service.Name = string.Empty;
			service.Caption = string.Empty;
			service.Description = string.Empty;

			return service;
		}

		public override void ReplaceItem(dynamic item)
		{
			string filePath;
			var serializedItem = JsonObjectSerializer.Formated.Serialize(item);

			//TODO Wrapper for PackageMetadataLoader.Configurations
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			if (configuration.Documents[_documentId].Services.ContainsKey(item.Name))
			{
				dynamic oldServices = configuration.Documents[_documentId].Services[item.Name];
				filePath = oldServices.FilePath;
			}
			else
			{
				filePath = Path.Combine(Path.GetDirectoryName(configuration.FilePath),
										"Documents",
										_documentId,
										"Services",
										string.Concat(item.Name, ".json"));
			}

			File.WriteAllBytes(filePath, serializedItem);

			PackageMetadataLoader.UpdateCache();
		}

		public override void DeleteItem(string itemId)
		{
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			dynamic process = configuration.Documents[_documentId].Services[itemId];

			File.Delete(process.FilePath);

			PackageMetadataLoader.UpdateCache();
		}

		public override object GetItem(string itemId)
		{
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			return configuration.Documents[_documentId].Services[itemId].Content;
		}

		public override IEnumerable<object> GetItems()
		{
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			Dictionary<string, dynamic> processes = configuration.Documents[_documentId].Services;
			return processes.Values.Select(o => o.Content);
		}
	}
}