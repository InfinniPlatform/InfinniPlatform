using System;
using System.Collections;
using System.Threading;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.UserInterface.Configurations;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными меню.
	/// </summary>
	sealed class MenuMetadataService : BaseMetadataService
	{
		public MenuMetadataService(string configId)
		{
			_configId = configId;
			_factory = new Lazy<ManagerFactoryConfiguration>(() => new ManagerFactoryConfiguration(configId), LazyThreadSafetyMode.ExecutionAndPublication);
		}


		private readonly string _configId;
		private readonly Lazy<ManagerFactoryConfiguration> _factory;


		protected override IDataReader CreateDataReader()
		{
			return _factory.Value.BuildMenuMetadataReader();
		}

		protected override IDataManager CreateDataManager()
		{
			return _factory.Value.BuildMenuManager();
		}


		public override IEnumerable GetItems()
		{
			return ConfigResourceRepository.GetMenu(_configId)
				   ?? base.GetItems();
		}

		public override object GetItem(string itemId)
		{
			return ConfigResourceRepository.GetMenu(_configId, itemId)
				   ?? base.GetItem(itemId);
		}
	}
}