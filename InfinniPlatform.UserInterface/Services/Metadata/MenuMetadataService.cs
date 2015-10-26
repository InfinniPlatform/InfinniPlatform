using System.Collections.Generic;

using InfinniPlatform.Sdk.Api;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными меню.
	/// </summary>
	internal sealed class MenuMetadataService : BaseMetadataService
	{
		public MenuMetadataService(string version, string configId, string server, int port, string route)
			: base(version, server, port, route)
		{
			_configId = configId;
			_metadataApi = new InfinniMetadataApi(server, port.ToString(), route);
		}

		readonly string _configId;
		readonly InfinniMetadataApi _metadataApi;

		public string ConfigId
		{
			get { return _configId; }
		}

		public override object CreateItem()
		{
			return _metadataApi.CreateMenu(Version, ConfigId);
		}

		public override void ReplaceItem(dynamic item)
		{
			_metadataApi.UpdateMenu(item, Version, ConfigId);
		}

		public override void DeleteItem(string itemId)
		{
			_metadataApi.DeleteMenu(Version, ConfigId, itemId);
		}

		public override object GetItem(string itemId)
		{
			var configPath = @"C:\Projects\InfinniPlatform\Assemblies\content";

			return PackageMetadataLoader.LoadDocumentMetadata(ConfigId, configPath);
		}

		public override IEnumerable<object> GetItems()
		{
			return PackageMetadataLoader.LoadDocumentsMetadata(ConfigId, 1).Values;
		}
	}
}