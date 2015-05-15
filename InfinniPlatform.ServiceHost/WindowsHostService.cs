using System.Collections.Generic;
using System.Reflection;
using InfinniPlatform.Hosting;
using InfinniPlatform.SystemConfig.Initializers;

namespace InfinniPlatform.ServiceHost
{
	public class WindowsHostService
	{
		private readonly IHostingService _server;

		public WindowsHostService(IEnumerable<Assembly> assemblies)
		{
			var factory = new OwinHostingServiceFactory(assemblies);
			_server = factory.CreateHostingService();
            //заполняем глобальный контекст исполнения скриптов
            factory.InfinniPlatformHostServer.RegisterServerInitializer<GlobalContextInitializer>();
			//устанавливаем системные конфигурации
			factory.InfinniPlatformHostServer.RegisterServerInitializer<SystemConfigurationsInitializer>();
			//устанавливаем конфигурации из JSON-описаний
			factory.InfinniPlatformHostServer.RegisterServerInitializer<JsonConfigurationsInitializer>();
			//пользовательские обработчики бизнес-логики старта сервера
			factory.InfinniPlatformHostServer.RegisterServerInitializer<UserLogicInitializer>();
			//обработчик настройки хранилища пользователей
			factory.InfinniPlatformHostServer.RegisterServerInitializer<UserStorageInitializer>();
		}

		public void Start()
		{
			_server.Start();
		}

		public void Stop()
		{
			_server.Stop();
		}
	}
}