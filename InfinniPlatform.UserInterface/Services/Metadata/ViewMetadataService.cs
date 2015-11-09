using System;
using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными представлений.
	/// </summary>
	internal sealed class ViewMetadataService : BaseMetadataService
	{
		public ViewMetadataService(string configId, string documentId)
		{
			ConfigId = configId;
			_documentId = documentId;
		}

		private readonly string _documentId;

		public string ConfigId { get; }

		public override object CreateItem()
		{
			dynamic view = new DynamicWrapper();

			view.Id = Guid.NewGuid().ToString();
			view.Name = string.Empty;
			view.Caption = string.Empty;
			view.DataSources = new object[] { };
			view.Parameters = new object[] { };
			view.LayoutPanel = new object();
			view.Scripts = new object[] { };

			return view;
		}

		public override void ReplaceItem(dynamic item)
		{
			string filePath;
			var serializedItem = JsonObjectSerializer.Formated.Serialize(item);

			dynamic configuration = PackageMetadataLoader.GetConfiguration(ConfigId);
			dynamic oldView = PackageMetadataLoader.GetView(ConfigId, _documentId, item.Name);
			if (oldView != null)
			{
				
				filePath = oldView.FilePath;
			}
			else
			{
				filePath = Path.Combine(Path.GetDirectoryName(configuration.FilePath),
										"Documents",
										_documentId,
										"Views",
										string.Concat(item.Name, ".json"));
			}

			File.WriteAllBytes(filePath, serializedItem);

			PackageMetadataLoader.UpdateCache();
		}

		public override void DeleteItem(string itemId)
		{
			dynamic view = PackageMetadataLoader.GetView(ConfigId, _documentId, itemId);

			File.Delete(view.FilePath);

			PackageMetadataLoader.UpdateCache();
		}

		public override object GetItem(string itemId)
		{
			return PackageMetadataLoader.GetView(ConfigId, _documentId, itemId);
		}

		public override IEnumerable<object> GetItems()
		{
			return PackageMetadataLoader.GetViews(ConfigId, _documentId);
		}
	}
}