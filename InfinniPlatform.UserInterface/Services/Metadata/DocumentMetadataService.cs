using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.Environment.Settings;

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
			_metadataApi = new InfinniMetadataApi(server, port.ToString(), route);
		}


		private readonly string _configId;
		private readonly InfinniMetadataApi _metadataApi;


		public string ConfigId
		{
			get { return _configId; }
		}

		public override object CreateItem()
		{
			return _metadataApi.CreateDocument(Version, ConfigId);
		}

		public override void ReplaceItem(dynamic item)
		{
			var name = (item.Name ?? string.Empty).ToString().Trim();
			var metadataIndex = (item.MetadataIndex ?? string.Empty).ToString().Trim();

			if (string.IsNullOrEmpty(metadataIndex))
			{
				item.MetadataIndex = name;
			}

			_metadataApi.UpdateDocument(item, Version, ConfigId);
		}

		public override void DeleteItem(string itemId)
		{
			var enumerateFiles = Directory.EnumerateFiles(AppSettings.GetValue("ContentDirectory"), "*.json",SearchOption.AllDirectories);
			var enumerable = enumerateFiles.Where(s => s.Contains(string.Format("{0}.json", itemId)));
			_metadataApi.DeleteDocument(Version, ConfigId, itemId);
		}

		public override object GetItem(string itemId)
		{
			return PackageMetadataLoader.Configurations.Value[ConfigId].Documents[itemId].Content;
		}

		public override IEnumerable<object> GetItems()
		{
			Dictionary<string, dynamic> documents = PackageMetadataLoader.Configurations.Value[ConfigId].Documents;
			return documents.Values.Select(o => o.Content);
		}
	}
}