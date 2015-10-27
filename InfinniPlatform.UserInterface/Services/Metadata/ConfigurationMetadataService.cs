using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Api;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными конфигурации.
	/// </summary>
	internal sealed class ConfigurationMetadataService : BaseMetadataService
	{
		public ConfigurationMetadataService(string version, string server, int port, string route) : base(version, server, port, route)
		{
			_metadataApi = new InfinniMetadataApi(server, port.ToString(), route);
		}

		readonly InfinniMetadataApi _metadataApi;

		public override object CreateItem()
		{
			return _metadataApi.CreateConfig();
		}

		public override void ReplaceItem(dynamic item)
		{
			_metadataApi.InsertConfig(item);
		}

		public override void DeleteItem(string itemId)
		{
			_metadataApi.DeleteConfig(Version, itemId);
		}

		public override object GetItem(string itemId)
		{
			return PackageMetadataLoader.Configurations[itemId].Content;
		}

		public override IEnumerable<object> GetItems()
		{
			return PackageMetadataLoader.Configurations.Values.Select(o => o.Content);
		}

	}
}