using System;
using System.Collections;
using System.Threading;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.UserInterface.Configurations;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными представлений.
	/// </summary>
	sealed class ViewMetadataService : BaseMetadataService
	{
		public ViewMetadataService(string configId, string documentId)
		{
			_configId = configId;
			_documentId = documentId;
			_factory = new Lazy<ManagerFactoryDocument>(() => new ManagerFactoryDocument(configId, documentId), LazyThreadSafetyMode.ExecutionAndPublication);
		}


		private readonly string _configId;
		private readonly string _documentId;
		private readonly Lazy<ManagerFactoryDocument> _factory;


		protected override IDataReader CreateDataReader()
		{
			return _factory.Value.BuildViewMetadataReader();
		}

		protected override IDataManager CreateDataManager()
		{
			return _factory.Value.BuildViewManager();
		}


		public override IEnumerable GetItems()
		{
			return ConfigResourceRepository.GetViews(_configId, _documentId)
				   ?? base.GetItems();
		}

		public override object GetItem(string itemId)
		{
			return ConfigResourceRepository.GetView(_configId, _documentId, itemId)
				   ?? base.GetItem(itemId);
		}
	}
}