using System;
using System.Threading;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными сборок.
	/// </summary>
	sealed class AssemblyMetadataService : BaseMetadataService
	{
		public AssemblyMetadataService(string version, string configId)
		{
			_factory = new Lazy<ManagerFactoryConfiguration>(() => new ManagerFactoryConfiguration(version, configId), LazyThreadSafetyMode.ExecutionAndPublication);
		}


		private readonly Lazy<ManagerFactoryConfiguration> _factory;


		protected override IDataReader CreateDataReader()
		{
			return _factory.Value.BuildAssemblyMetadataReader();
		}

		protected override IDataManager CreateDataManager()
		{
			return _factory.Value.BuildAssemblyManager();
		}
	}
}