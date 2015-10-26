using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Api;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными представлений.
	/// </summary>
	internal sealed class ViewMetadataService : BaseMetadataService
	{
		public ViewMetadataService(string version, string configId, string documentId, string server, int port, string route)
			: base(version, server, port, route)
		{
			_configId = configId;
			_documentId = documentId;
			_metadataApi = new InfinniMetadataApi(server, port.ToString(), route);
		}

		readonly string _configId;
		readonly string _documentId;
		readonly InfinniMetadataApi _metadataApi;

		public string ConfigId
		{
			get { return _configId; }
		}

		public override object CreateItem()
		{
			return _metadataApi.CreateView(Version, ConfigId, _documentId);
		}

		public override void ReplaceItem(dynamic item)
		{
			_metadataApi.UpdateView(item, Version, ConfigId, _documentId);
		}

		public override void DeleteItem(string itemId)
		{
			_metadataApi.DeleteView(Version, ConfigId, _documentId, itemId);
		}

		public override object GetItem(string itemId)
		{
			Dictionary<string, dynamic> documents = PackageMetadataLoader.Configurations.Value[_configId].Documents;
			var views = documents[_documentId].Views;
			var view = views[itemId];
			return view.Content;
		}

		public override IEnumerable<object> GetItems()
		{
			Dictionary<string, dynamic> documents = PackageMetadataLoader.Configurations.Value[_configId].Documents;
			Dictionary<string, dynamic> views = documents[_documentId].Views;
			return views.Values.Select(o => o.Content);
		}
	}
}