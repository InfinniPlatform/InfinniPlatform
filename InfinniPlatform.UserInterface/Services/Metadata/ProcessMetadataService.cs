using System;
using System.Threading;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными бизнес-процессов.
	/// </summary>
	sealed class ProcessMetadataService : BaseMetadataService
	{
		public ProcessMetadataService(string version, string configId, string documentId)
		{
			_factory = new Lazy<ManagerFactoryDocument>(() => new ManagerFactoryDocument(version, configId, documentId), LazyThreadSafetyMode.ExecutionAndPublication);
		}


		private readonly Lazy<ManagerFactoryDocument> _factory;


		protected override IDataReader CreateDataReader()
		{
			return _factory.Value.BuildProcessMetadataReader();
		}

		protected override IDataManager CreateDataManager()
		{
			return _factory.Value.BuildProcessManager();
		}
	}
}