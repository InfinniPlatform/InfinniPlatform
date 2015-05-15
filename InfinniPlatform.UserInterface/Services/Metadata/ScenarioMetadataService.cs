using System;
using System.Threading;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными бизнес-сценариев.
	/// </summary>
	sealed class ScenarioMetadataService : BaseMetadataService
	{
		public ScenarioMetadataService(string configId, string documentId)
		{
			_factory = new Lazy<ManagerFactoryDocument>(() => new ManagerFactoryDocument(configId, documentId), LazyThreadSafetyMode.ExecutionAndPublication);
		}


		private readonly Lazy<ManagerFactoryDocument> _factory;


		protected override IDataReader CreateDataReader()
		{
			return _factory.Value.BuildScenarioMetadataReader();
		}

		protected override IDataManager CreateDataManager()
		{
			return _factory.Value.BuildScenarioManager();
		}
	}
}