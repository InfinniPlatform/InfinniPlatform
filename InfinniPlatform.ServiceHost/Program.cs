using System;
using InfinniPlatform.Hosting;
using InfinniPlatform.Modules;
using InfinniPlatform.Sdk.Environment.Settings;
using InfinniPlatform.ServiceHost.Properties;
using InfinniPlatform.SystemConfig.Initializers;
using InfinniPlatform.WebApi.Logging;

using Topshelf;

namespace InfinniPlatform.ServiceHost
{
	class Program
	{
		static void Main()
		{
			AppDomain.CurrentDomain.SetupLogErrorHandler();

			var configurations = AppSettings.GetValue("ConfigurationList", "");

			if (string.IsNullOrEmpty(configurations))
			{
				throw new ArgumentException(Resources.ConfigurationListIsEmpty);
			}

			StartExtension.CreateBlobStorage();
			StartExtension.CreateEventStorage();

			if (AppSettings.GetValue("ConsoleMode", false))
			{
				var factory = new OwinHostingServiceFactory(ModuleExtension.LoadModulesAssemblies(configurations));
				var server = factory.CreateHostingService();
				try
				{
					Console.WriteLine(Resources.PleaseWaitWhileTheServerIsStarted);

					//заполняем глобальный контекст исполнения скриптов
					factory.InfinniPlatformHostServer.RegisterServerInitializer<GlobalContextInitializer>();
					//устанавливаем системные конфигурации
					factory.InfinniPlatformHostServer.RegisterServerInitializer<SystemConfigurationsInitializer>();
					//устанавливаем конфигурации из JSON-описаний
					factory.InfinniPlatformHostServer.RegisterServerInitializer<JsonConfigurationsInitializer>();
					//пользовательские обработчики бизнес-логики старта сервера
					factory.InfinniPlatformHostServer.RegisterServerInitializer<UserLogicInitializer>();

					factory.InfinniPlatformHostServer.RegisterServerInitializer<UserStorageInitializer>();
					server.Start();

					Console.WriteLine();
					Console.WriteLine(Resources.ServerStartedAt, string.Format("{0}:{1}", AppSettings.GetValue("AppServerName"), AppSettings.GetValue("AppServerPort")));
					Console.ReadKey();
				}
				finally
				{
					server.Stop();
				}
			}
			else
			{
				var assemblies = ModuleExtension.LoadModulesAssemblies(configurations);
				var hostService = new WindowsHostService(assemblies);
				var applicationName = AppSettings.GetValue("ApplicationServiceName", "InfinniPlatform");
				var applicationDescription = AppSettings.GetValue("ApplicationServiceDescription", "InfinniPlatform");
				HostFactory.Run(x =>
									{
										x.Service<WindowsHostService>(s =>
																		  {
																			  s.ConstructUsing(name => hostService);
																			  s.WhenStarted(svc => svc.Start());
																			  s.WhenStopped(svc => svc.Stop());
																		  });

										x.RunAsLocalSystem();
										x.SetDescription(applicationDescription);
										x.SetDisplayName(applicationName);
										x.SetServiceName(applicationName);
									});
			}
		}
	}
}