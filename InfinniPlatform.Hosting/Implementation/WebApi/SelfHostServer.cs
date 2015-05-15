using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;
using Autofac;
using InfinniPlatform.Hosting.WebApi.Implementation.Controllers;
using InfinniPlatform.Hosting.WebApi.Implementation.Logging;
using Thinktecture.IdentityModel.Http.Cors.WebApi;

namespace InfinniPlatform.Hosting.WebApi.Implementation.WebApi
{
	/// <summary>
	/// Осуществляет хостинг приложения.
	/// </summary>
	public sealed class SelfHostServer
	{
		public SelfHostServer(string baseAddress, IEnumerable<Assembly> modules)
		{
			_hostConfiguration = new HttpSelfHostConfiguration(baseAddress)
									 {
										 IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always,
										 MaxReceivedMessageSize = 100 * 1024 * 1024, // 100 Mb
										 TransferMode = TransferMode.StreamedRequest
									 };
			_hostConfiguration.Services.Replace(typeof(IHttpControllerSelector), new HttpControllerSelector(_hostConfiguration));

			_hostServer = new HttpSelfHostServer(_hostConfiguration);
			_modules = modules;

			_containerBuilder = new ContainerBuilder();
			_apiControllerFactory = new ApiControllerFactory(() => _container);
			_containerBuilder.RegisterInstance(_apiControllerFactory).AsSelf().AsImplementedInterfaces().SingleInstance();


			_containerBuilder.RegisterType<StandardApiController>().AsSelf();
			_containerBuilder.RegisterType<FileController>().AsSelf();
			_containerBuilder.RegisterType<StandardStreamedApiController>().AsSelf();

			_webApiCorsConfiguration = new WebApiCorsConfiguration();

			RemoveXmlFormatter();
			SetupLogExceptionFilter();
		}

		private void RemoveXmlFormatter()
		{
			var appXmlType = _hostConfiguration.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");

			if (appXmlType != null)
			{
				_hostConfiguration.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
			}

			_hostConfiguration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
		}

		private void SetupLogExceptionFilter()
		{
			_hostConfiguration.SetupLogErrorHandler();
		}


		private readonly HttpSelfHostServer _hostServer;
		private readonly HttpSelfHostConfiguration _hostConfiguration;

		/// <summary>
		/// Конфигурация хостинг-сервера.
		/// </summary>
		public HttpSelfHostConfiguration HostConfiguration
		{
			get { return _hostConfiguration; }
		}


		private readonly IEnumerable<Assembly> _modules;

		/// <summary>
		/// Список модулей для хостинга.
		/// </summary>
		public IEnumerable<Assembly> Modules
		{
			get { return _modules; }
		}


		private readonly ContainerBuilder _containerBuilder;

		/// <summary>
		/// Фабрика для конфигурирования IoC-контейнера.
		/// </summary>
		public ContainerBuilder ContainerBuilder
		{
			get { return _containerBuilder; }
		}


		private readonly ApiControllerFactory _apiControllerFactory;

		/// <summary>
		/// Фабрика для создания контроллеров.
		/// </summary>
		public ApiControllerFactory ApiControllerFactory
		{
			get { return _apiControllerFactory; }
		}


		private readonly WebApiCorsConfiguration _webApiCorsConfiguration;

		/// <summary>
		/// Конфигурация CORS.
		/// </summary>
		public WebApiCorsConfiguration WebApiCorsConfiguration
		{
			get { return _webApiCorsConfiguration; }
		}



		/// <summary>
		/// Хостинг-сервер запущен.
		/// </summary>
		public bool ServerStarted { get; private set; }

		/// <summary>
		/// Запустить хостинг-сервер.
		/// </summary>
		public void StartServer()
		{
			BuildDependencies();

			_hostServer.OpenAsync().Wait();

			ServerStarted = true;
		}

		/// <summary>
		/// Остановить хостинг-сервер.
		/// </summary>
		public void StopServer()
		{
			if (_hostServer != null)
			{
				_hostServer.CloseAsync().Wait();

				ServerStarted = false;
			}
		}


		private Action _actionBefore;
		private Action _actionAfter;
		private Action<ContainerBuilder> _actionUpdate;
		private IContainer _container;

		/// <summary>
		/// Метод для получения IoC-контейнера.
		/// </summary>
		public Func<IContainer> Container
		{
			get { return () => _container; }
		}

		/// <summary>
		/// Инициализировать IoC-контейнер.
		/// </summary>
		private void BuildDependencies()
		{
			if (_actionBefore != null)
			{
				_actionBefore.Invoke();
			}

			_container = ContainerBuilder.Build();

			if (_actionAfter != null)
			{
				_actionAfter.Invoke();
			}

			if (_actionUpdate != null)
			{
				var containerBuilderUpdate = new ContainerBuilder();
				_actionUpdate.Invoke(containerBuilderUpdate);
				containerBuilderUpdate.Update(_container);
			}

			HostConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(_container);

		}

		/// <summary>
		/// Зарегистрировать действие, которое будет вызываться перед инициализацией IoC-контейнера. 
		/// </summary>
		/// <param name="actionBefore">action to invoke before initializing IoC container</param>
		public void BeforeCreateContainer(Action actionBefore)
		{
			_actionBefore = (Action)Delegate.Combine(_actionBefore, actionBefore);
		}

		/// <summary>
		/// Зарегистрироват действие, которое будет вызываться после инициализации IoC-контейнера.
		/// </summary>
		public void AfterCreateContainer(Action actionAfter)
		{
			_actionAfter = (Action)Delegate.Combine(_actionAfter, actionAfter);
		}

		/// <summary>
		/// Создать шаблон REST-сервиса.
		/// </summary>
		public IRestVerbsContainer CreateTemplate(string apiControllerName)
		{
			return _apiControllerFactory.CreateTemplate(apiControllerName);
		}

		/// <summary>
		///   Обновить регистрацию зависимостей в контейнере
		/// </summary>
		public void ActionUpdateContainer(Action<ContainerBuilder>  actionUpdate)
		{
			_actionUpdate = (Action<ContainerBuilder>) Delegate.Combine(_actionUpdate, actionUpdate);
		}
	}
}