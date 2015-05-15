using System;
using System.Threading;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными печатных представлений.
	/// </summary>
	sealed class PrintViewMetadataService : BaseMetadataService
	{
		public PrintViewMetadataService(string configId, string documentId)
		{
			_factory = new Lazy<ManagerFactoryDocument>(() => new ManagerFactoryDocument(configId, documentId), LazyThreadSafetyMode.ExecutionAndPublication);
		}


		private readonly Lazy<ManagerFactoryDocument> _factory;


		protected override IDataReader CreateDataReader()
		{
			return _factory.Value.BuildPrintViewMetadataReader();
		}

		protected override IDataManager CreateDataManager()
		{
			return _factory.Value.BuildPrintViewManager();
		}
	}
}