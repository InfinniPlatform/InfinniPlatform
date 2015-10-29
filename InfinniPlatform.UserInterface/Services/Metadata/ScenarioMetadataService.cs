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
		public ScenarioMetadataService(string version, string configId, string documentId, string server, int port, string route)
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

			//TODO Wrapper for PackageMetadataLoader.Configurations
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			if (configuration.Documents[_documentId].Scenarios.ContainsKey(item.Name))
			{
				dynamic oldScenario = configuration.Documents[_documentId].Scenarios[item.Name];
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
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			dynamic process = configuration.Documents[_documentId].Scenarios[itemId];

			File.Delete(process.FilePath);

			PackageMetadataLoader.UpdateCache();
		}

		public override object GetItem(string itemId)
		{
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			return configuration.Documents[_documentId].Scenarios[itemId].Content;
		}

		public override IEnumerable<object> GetItems()
		{
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			Dictionary<string, dynamic> processes = configuration.Documents[_documentId].Scenarios;
			return processes.Values.Select(o => o.Content);
		}
	}
}