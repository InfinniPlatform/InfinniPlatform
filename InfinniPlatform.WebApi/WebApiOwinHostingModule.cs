using System.Collections.Generic;
using System.Reflection;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Hosting;
using Owin;

using InfinniPlatform.Api.LocalRouting;
using InfinniPlatform.Api.Profiling;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Factories;
using InfinniPlatform.Hosting;
using InfinniPlatform.Logging;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.WebApi.Factories;

namespace InfinniPlatform.WebApi
{
	/// <summary>
	/// Модуль хостинга ASP.NET Web API на базе OWIN.
	/// </summary>
	public sealed class WebApiOwinHostingModule : OwinHostingModule
	{
		public WebApiOwinHostingModule(IEnumerable<Assembly> assemblies,
									   IServiceRegistrationContainerFactory serviceRegistrationContainerFactory,
									   IServiceTemplateConfiguration configuration)
		{
			HostServer = InfinniPlatformHostServer.ConstructHostServer(assemblies, serviceRegistrationContainerFactory, configuration);

			// Устанавливаем локальный роутинг для прохождения запросов внутри серверных потоков обработки запросов
			RestQueryApi.SetRoutingType(RoutingType.Local);
			RequestLocal.ApiControllerFactory = HostServer.GetApiControllerFactory();

			// Обработчик события старта модуля хостинга
			OnStart += OnStartHandler;
		}

		public InfinniPlatformHostServer HostServer { get; private set; }

		public override void Configure(IAppBuilder builder, IHostingContext context)
		{
			if (context.Configuration.ServerProfileQuery)
			{
				RestQueryApi.SetProfilingType(ProfilingType.ProfileWatch);
				RestQueryApi.Log = new Log4NetLogFactory().CreateLog();
			}

			HostServer.Build(context);
			builder.UseWebApi(HostServer.HttpConfiguration);
		}

		private void OnStartHandler(HostingContextBuilder contextBuilder, IHostingContext context)
		{
			HostServer.OnStartHost(contextBuilder, context);
		}
	}
}