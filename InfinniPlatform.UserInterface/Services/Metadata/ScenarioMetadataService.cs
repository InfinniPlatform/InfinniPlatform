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
			string filePath;
			var serializedItem = JsonObjectSerializer.Formated.Serialize(item);

			dynamic configuration = PackageMetadataLoader.GetConfiguration(ConfigId);
			dynamic oldScenario = PackageMetadataLoader.GetScenario(ConfigId, _documentId, item.Name);
			if (oldScenario != null)
			{
				filePath = oldScenario.FilePath;
			}
			else
			{
				filePath = Path.Combine(Path.GetDirectoryName(configuration.FilePath),
										"Documents",
										_documentId,
										"Scenarios",
										string.Concat(item.Name, ".json"));
			}

			File.WriteAllBytes(filePath, serializedItem);

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