using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Api;

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
			_metadataApi = new InfinniMetadataApi(server, port.ToString(), route);
		}

		private readonly string _configId;
		private readonly string _documentId;
		private readonly InfinniMetadataApi _metadataApi;

		public string ConfigId
		{
			get { return _configId; }
		}

		public override object CreateItem()
		{
			return _metadataApi.CreateProcess(Version, ConfigId, _documentId);
		}

		public override void ReplaceItem(dynamic item)
		{
			_metadataApi.UpdateProcess(item, Version, ConfigId, _documentId);
		}

		public override void DeleteItem(string itemId)
		{
			_metadataApi.DeleteProcess(Version, ConfigId, _documentId, itemId);
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
			return processes.Values.Select(o=>o.Content);
		}
	}
}