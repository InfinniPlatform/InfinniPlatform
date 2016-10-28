using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using InfinniPlatform.Core.Properties;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Http;

using Microsoft.Owin.Hosting;

using Owin;

namespace InfinniPlatform.Core.Http.Hosting
{
    /// <summary>
    /// Сервис хостинга приложения на базе OWIN.
    /// </summary>
    internal class OwinHostingService : IHostingService
    {
        public OwinHostingService(HostingConfig hostingConfig,
                                  IHostAddressParser hostAddressParser,
                                  IEnumerable<IHttpMiddleware> hostingMiddlewares = null,
                                  IEnumerable<IAppEventHandler> appEventHandlers = null)
        {
            // Имя схемы протокола сервера

            var scheme = hostingConfig.Scheme;

            if (string.IsNullOrWhiteSpace(scheme))
            {
                throw new ArgumentNullException(Resources.ServerSchemeCannotBeNullOrWhiteSpace);
            }

            if (!Uri.UriSchemeHttp.Equals(scheme, StringComparison.OrdinalIgnoreCase) &&
                !Uri.UriSchemeHttps.Equals(scheme, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentOutOfRangeException(string.Format(Resources.ServerSchemeIsNotSupported, scheme));
            }

            // Адрес или имя сервера

            var server = hostingConfig.Name;

            if (string.IsNullOrWhiteSpace(server))
            {
                throw new ArgumentNullException(Resources.ServerNameCannotBeNullOrWhiteSpace);
            }

            if (!hostAddressParser.IsLocalAddress(server, out server))
            {
                throw new ArgumentOutOfRangeException(string.Format(Resources.ServerNameIsNotLocal, server));
            }

            // Номер порта сервера

            var port = hostingConfig.Port;

            if (port <= 0)
            {
                throw new ArgumentOutOfRangeException(string.Format(Resources.ServerPortIsIncorrect, port));
            }

            _baseAddress = $"{scheme}://{server}:{port}/";

            // Функция настройки порядка обработки запросов приложения

            if (hostingMiddlewares != null)
            {
                hostingMiddlewares = hostingMiddlewares.OrderBy(m => m.Type);

                foreach (var hostingMiddleware in hostingMiddlewares)
                {
                    _configure += hostingMiddleware.Configure;
                }
            }

            // Функции обработки событий приложения

            if (appEventHandlers != null)
            {
                appEventHandlers = appEventHandlers.OrderBy(i => i.Order).ToList();

                foreach (var appEventHandler in appEventHandlers)
                {
                    _onInit += appEventHandler.OnInit;
                    _onBeforeStart += appEventHandler.OnBeforeStart;
                    _onAfterStart += appEventHandler.OnAfterStart;
                    _onBeforeStop += appEventHandler.OnBeforeStop;
                    _onAfterStop += appEventHandler.OnAfterStop;
                }
            }
        }


        private readonly string _baseAddress;

        private readonly Action<IAppBuilder> _configure;

        private readonly Action _onInit;
        private readonly Action _onBeforeStart;
        private readonly Action _onAfterStart;
        private readonly Action _onBeforeStop;
        private readonly Action _onAfterStop;

        private readonly object _hostSync = new object();
        private volatile IDisposable _host;


        public void Init()
        {
            if (_host == null)
            {
                lock (_hostSync)
                {
                    if (_host == null)
                    {
                        try
                        {
                            _onInit?.Invoke();
                        }
                        catch (Exception exception)
                        {
                            throw new AggregateException(Resources.CannotInitializeServiceCorrectly, exception);
                        }
                    }
                }
            }
        }

        public void Start()
        {
            if (_host == null)
            {
                lock (_hostSync)
                {
                    if (_host == null)
                    {
                        IDisposable host = null;

                        try
                        {
                            _onBeforeStart?.Invoke();

                            host = WebApp.Start(_baseAddress, Startup);

                            _onAfterStart?.Invoke();
                        }
                        catch (Exception exception)
                        {
                            try
                            {
                                host?.Dispose();
                            }
                            catch
                            {
                            }

                            throw new AggregateException(Resources.CannotStartServiceCorrectly, exception);
                        }

                        _host = host;
                    }
                }
            }
        }

        public void Stop()
        {
            if (_host != null)
            {
                lock (_hostSync)
                {
                    if (_host != null)
                    {
                        var exceptions = new List<Exception>();

                        try
                        {
                            _onBeforeStop?.Invoke();
                        }
                        catch (Exception exception)
                        {
                            exceptions.Add(exception);
                        }

                        try
                        {
                            _host.Dispose();
                        }
                        catch (Exception exception)
                        {
                            exceptions.Add(exception);
                        }
                        finally
                        {
                            _host = null;
                        }

                        try
                        {
                            _onAfterStop?.Invoke();
                        }
                        catch (Exception exception)
                        {
                            exceptions.Add(exception);
                        }

                        if (exceptions.Count > 0)
                        {
                            throw new AggregateException(Resources.CannotStopServiceCorrectly, exceptions);
                        }
                    }
                }
            }
        }

        private void Startup(IAppBuilder builder)
        {
            object httpListener;

            if (builder.Properties.TryGetValue(typeof(HttpListener).FullName, out httpListener) && httpListener is HttpListener)
            {
                // HttpListener should not return exceptions that occur when sending the response to the client
                ((HttpListener)httpListener).IgnoreWriteExceptions = true;
            }

            _configure?.Invoke(builder);
        }
    }
}