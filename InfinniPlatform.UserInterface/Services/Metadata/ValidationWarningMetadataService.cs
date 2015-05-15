using System;
using System.Threading;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными проверки документов на предупреждения.
	/// </summary>
	sealed class ValidationWarningMetadataService : BaseMetadataService
	{
		public ValidationWarningMetadataService(string configId, string documentId)
		{
			_factory = new Lazy<ManagerFactoryDocument>(() => new ManagerFactoryDocument(configId, documentId), LazyThreadSafetyMode.ExecutionAndPublication);
		}


		private readonly Lazy<ManagerFactoryDocument> _factory;


		protected override IDataReader CreateDataReader()
		{
			return _factory.Value.BuildValidationWarningsMetadataReader();
		}

		protected override IDataManager CreateDataManager()
		{
			return _factory.Value.BuildValidationWarningsManager();
		}
	}
}