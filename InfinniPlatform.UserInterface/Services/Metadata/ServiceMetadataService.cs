using System;
using System.Collections.Generic;
using System.IO;

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
		public ServiceMetadataService(string configId, string documentId)
		{
			ConfigId = configId;
			_documentId = documentId;
		}

		private readonly string _documentId;

		public string ConfigId { get; }

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

			dynamic configuration = PackageMetadataLoader.GetConfiguration(ConfigId);
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
			dynamic service = PackageMetadataLoader.GetService(ConfigId, _documentId, itemId);

			File.Delete(service.FilePath);

			PackageMetadataLoader.UpdateCache();
		}

		public override object GetItem(string itemId)
		{
			return PackageMetadataLoader.GetService(ConfigId, _documentId, itemId);
		}

		public override IEnumerable<object> GetItems()
		{
			return PackageMetadataLoader.GetServices(ConfigId, _documentId);
		}
	}
}