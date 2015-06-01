using System.Linq;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.Security;
using InfinniPlatform.Factories;
using InfinniPlatform.Hosting;
using InfinniPlatform.Metadata;
using InfinniPlatform.Runtime;
using InfinniPlatform.Security;
using InfinniPlatform.SystemConfig.UserStorage;

namespace InfinniPlatform.SystemConfig.Initializers
{
	public sealed class UserStorageInitializer : IStartupInitializer
	{
		private readonly IChangeListener _changeListener;
		private readonly IGlobalContext _globalContext;

		public UserStorageInitializer(IChangeListener changeListener,IGlobalContext globalContext)
		{
			_changeListener = changeListener;
			_globalContext = globalContext;
		}

		public void OnStart(HostingContextBuilder contextBuilder)
		{
			var applicationUserStore = new ApplicationUserStorePersistentStorage();
			var applicationUserPasswordHasher = new CustomApplicationUserPasswordHasher(_globalContext);

			contextBuilder.SetEnvironment<IApplicationUserStore>(applicationUserStore);
			contextBuilder.SetEnvironment<IApplicationUserPasswordHasher>(applicationUserPasswordHasher);

			ApplicationUserStorePersistentStorageExtensions.CheckStorage();
			_changeListener.RegisterOnChange("UserStorage", OnChangeModules);
		}

		//TODO Дублирование кода обработчиков в нескольких Initializer
		/// <summary>
		///   Обновление конфигурации при получении события обновления сборок
		///   Пока атомарность обновления не обеспечивается - в момент обновления обращающиеся к серверу запросы получат отлуп
		/// </summary>
		/// <param name="configurationId">Идентификатор конфигурации</param>
		private void OnChangeModules(string configurationId)
		{
			if (configurationId == "Authorization")
			{
				ApplicationUserStorePersistentStorageExtensions.CheckStorage();
			}

		}
	}
}
