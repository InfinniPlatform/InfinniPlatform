using System;
using System.Collections.Generic;

using Microsoft.Owin.Hosting;

using Owin;

using InfinniPlatform.Hosting;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Owin.Properties;

namespace InfinniPlatform.Owin.Hosting
{
	/// <summary>
	/// Сервис хостинга на базе OWIN (Open Web Interface for .NET).
	/// </summary>
	public sealed class OwinHostingService : IHostingService
	{
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="contextConfig">Функция настройки контекста подсистемы хостинга.</param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public OwinHostingService(Action<HostingContextBuilder> contextConfig)
		{
			// Настройка контекста

			_contextBuilder = new HostingContextBuilder();

			if (contextConfig != null)
			{
				contextConfig(_contextBuilder);
			}

			// Проверка наличия конфигурации

			if (_contextBuilder.Context.Configuration == null)
			{
				throw new ArgumentNullException(Resources.ServerConfigurationCannotBeNull);
			}

			// Имя схемы протокола сервера

			var scheme = _contextBuilder.Context.Configuration.ServerScheme;

			if (string.IsNullOrWhiteSpace(scheme))
			{
				throw new ArgumentNullException(Resources.ServerSchemeCannotBeNullOrWhiteSpace);
			}

			if (!Uri.UriSchemeHttp.Equals(scheme, StringComparison.OrdinalIgnoreCase) && !Uri.UriSchemeHttps.Equals(scheme, StringComparison.OrdinalIgnoreCase))
			{
				throw new ArgumentOutOfRangeException(string.Format(Resources.ServerSchemeIsNotSupported, scheme));
			}

			// Адрес или имя сервера

			var server = _contextBuilder.Context.Configuration.ServerName;

			if (string.IsNullOrWhiteSpace(server))
			{
				throw new ArgumentNullException(Resources.ServerNameCannotBeNullOrWhiteSpace);
			}

			if (!server.IsLocalAddress(out server))
			{
				throw new ArgumentOutOfRangeException(string.Format(Resources.ServerNameIsNotLocal, server));
			}

			// Номер порта сервера

			var port = _contextBuilder.Context.Configuration.ServerPort;

			if (port <= 0)
			{
				throw new ArgumentOutOfRangeException(string.Format(Resources.ServerPortIsIncorrect, port));
			}

			// Отпечаток сертификата

			var certificate = _contextBuilder.Context.Configuration.ServerCertificate;

			if (Uri.UriSchemeHttps.Equals(scheme, StringComparison.OrdinalIgnoreCase) && string.IsNullOrWhiteSpace(certificate))
			{
				throw new ArgumentNullException(Resources.ServerCertificateCannotBeNullOrWhiteSpace);
			}


			Context = _contextBuilder.Context;			

			_baseAddress = string.Format("{0}://{1}:{2}/", scheme, server, port);
			_hostingModules = new List<OwinHostingModule>();
		}


		private readonly string _baseAddress;
		private readonly List<OwinHostingModule> _hostingModules;


		/// <summary>
		/// Контекст подсистемы хостинга.
		/// </summary>
		public IHostingContext Context { get; private set; }


		/// <summary>
		/// Зарегистрировать модуль хостинга.
		/// </summary>
		/// <param name="module">Модуль хостинга.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public void RegisterModule(OwinHostingModule module)
		{
			if (module == null)
			{
				throw new ArgumentNullException("module");
			}

			_hostingModules.Add(module);
		}


		private volatile IDisposable _host;
		private readonly object _hostSync = new object();
		private readonly HostingContextBuilder _contextBuilder;


		/// <summary>
		/// Запустить хостинг.
		/// </summary>
		public void Start()
		{
			if (_host == null)
			{
				lock (_hostSync)
				{
					if (_host == null)
					{
						BindCertificate();

						_host = WebApp.Start(_baseAddress, Startup);

						foreach (var module in _hostingModules)
						{
							if (module.OnStart != null)
							{
								module.OnStart(_contextBuilder, Context);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Остановить хостинг.
		/// </summary>
		public void Stop()
		{
			if (_host != null)
			{
				lock (_hostSync)
				{
					if (_host != null)
					{
						try
						{
							foreach (var module in _hostingModules)
							{
								if (module.OnStop != null)
								{
									module.OnStop(Context);
								}
							}

							_host.Dispose();
						}
						finally
						{
							_host = null;
						}
					}
				}
			}
		}


		private void Startup(IAppBuilder builder)
		{
			foreach (var module in _hostingModules)
			{
				module.Configure(builder, Context);
			}
		}

		private void BindCertificate()
		{
			var config = Context.Configuration;

			if (Uri.UriSchemeHttps.Equals(config.ServerScheme, StringComparison.OrdinalIgnoreCase))
			{
				if (string.IsNullOrWhiteSpace(config.ServerCertificate))
				{
					throw new ArgumentNullException(Resources.ServerCertificateCannotBeNullOrWhiteSpace);
				}

				OwinExtensions.BindCertificate(config.ServerPort, config.ServerCertificate);
			}
		}
	}
}