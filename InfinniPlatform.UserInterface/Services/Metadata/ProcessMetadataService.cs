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
	/// Сервис для работы с метаданными бизнес-процессов.
	/// </summary>
	internal sealed class ProcessMetadataService : BaseMetadataService
	{
		public ProcessMetadataService(string version, string configId, string documentId, string server, int port, string route)
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
			dynamic process = new DynamicWrapper();

			process.Id = Guid.NewGuid().ToString();
			process.Name = string.Empty;
			process.Caption = string.Empty;
			process.Description = string.Empty;

			return process;
		}

		public override void ReplaceItem(dynamic item)
		{
			string filePath;
			var serializedItem = JsonObjectSerializer.Formated.Serialize(item);

			//TODO Wrapper for PackageMetadataLoader.Configurations
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			if (configuration.Documents[_documentId].Processes.ContainsKey(item.Name))
			{
				dynamic oldProcess = configuration.Documents[_documentId].Processes[item.Name];
				filePath = oldProcess.FilePath;
			}
			else
			{
				filePath = Path.Combine(Path.GetDirectoryName(configuration.FilePath),
										"Documents",
										_documentId,
										"Processes",
										string.Concat(item.Name, ".json"));
			}

			File.WriteAllBytes(filePath, serializedItem);

			PackageMetadataLoader.UpdateCache();
		}

		public override void DeleteItem(string itemId)
		{
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			dynamic process = configuration.Documents[_documentId].Processes[itemId];

			File.Delete(process.FilePath);

			PackageMetadataLoader.UpdateCache();
		}

		public override object GetItem(string itemId)
		{
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			return configuration.Documents[_documentId].Processes[itemId].Content;
		}

		public override IEnumerable<object> GetItems()
		{
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			Dictionary<string, dynamic> processes = configuration.Documents[_documentId].Processes;
			return processes.Values.Select(o => o.Content);
		}
	}
}