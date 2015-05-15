using System;
using System.Threading;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными документов.
	/// </summary>
	sealed class RegisterMetadataService : BaseMetadataService
	{
		public RegisterMetadataService(string configId)
		{
			_factory = new Lazy<ManagerFactoryConfiguration>(() => new ManagerFactoryConfiguration(configId), LazyThreadSafetyMode.ExecutionAndPublication);
		}


		private readonly Lazy<ManagerFactoryConfiguration> _factory;


		protected override IDataReader CreateDataReader()
		{
			return _factory.Value.BuildRegisterMetadataReader();
		}

		protected override IDataManager CreateDataManager()
		{
			return _factory.Value.BuildRegisterManager();
		}
	}
}