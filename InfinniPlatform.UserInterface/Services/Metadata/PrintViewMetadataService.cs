using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными печатных представлений.
	/// </summary>
	internal sealed class PrintViewMetadataService : BaseMetadataService
	{
		public PrintViewMetadataService(string version, string configId, string documentId, string server, int port, string route)
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
			dynamic printView = new DynamicWrapper();

			printView.Id = Guid.NewGuid().ToString();
			printView.Name = string.Empty;
			printView.Caption = string.Empty;
			printView.Description = string.Empty;

			return printView;
		}

		public override void ReplaceItem(dynamic item)
		{
			string filePath;
			var serializedItem = JsonObjectSerializer.Formated.Serialize(item);

			//TODO Wrapper for PackageMetadataLoader.Configurations
			if (PackageMetadataLoader.Configurations[_configId].Documents[_documentId].Views.ContainsKey(item.Name))
			{
				dynamic oldView = PackageMetadataLoader.Configurations[_configId].Documents[_documentId].PrintViews[item.Name];
				filePath = oldView.FilePath;
			}
			else
			{
				filePath = Path.Combine(Path.GetDirectoryName(PackageMetadataLoader.Configurations[_configId].FilePath),
										"Documents",
										_documentId,
										"PrintViews",
										string.Concat(item.Name, ".json"));
			}

			File.WriteAllBytes(filePath, serializedItem);

			PackageMetadataLoader.UpdateCache();
		}

		public override void DeleteItem(string itemId)
		{
			dynamic document = PackageMetadataLoader.Configurations[_configId].Documents[_documentId].PrintViews[itemId];

			File.Delete(document.FilePath);

			PackageMetadataLoader.UpdateCache();
		}

		public override object GetItem(string itemId)
		{
			return PackageMetadataLoader.Configurations[_configId].Documents[_documentId].PrintViews[itemId].Content;
		}

		public override IEnumerable<object> GetItems()
		{
			Dictionary<string, dynamic> printViews = PackageMetadataLoader.Configurations[_configId].Documents[_documentId].PrintViews;
			return printViews.Values.Select(o => o.Content);
		}
	}
}