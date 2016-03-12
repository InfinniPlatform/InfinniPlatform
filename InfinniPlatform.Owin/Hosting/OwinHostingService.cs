using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Owin.Properties;

using Microsoft.Owin.Hosting;

using Owin;

namespace InfinniPlatform.Owin.Hosting
{
    /// <summary>
    /// Сервис хостинга приложения на базе OWIN.
    /// </summary>
    public sealed class OwinHostingService : IHostingService
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="hostingContext">Контекст подсистемы хостинга на базе OWIN.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public OwinHostingService(IOwinHostingContext hostingContext)
        {
            if (hostingContext == null)
            {
                throw new ArgumentNullException(nameof(hostingContext));
            }

            // Проверка наличия конфигурации

            if (hostingContext.Configuration == null)
            {
                throw new ArgumentNullException(Resources.ServerConfigurationCannotBeNull);
            }

            // Имя схемы протокола сервера

            var scheme = hostingContext.Configuration.Scheme;

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

            var server = hostingContext.Configuration.Name;

            if (string.IsNullOrWhiteSpace(server))
            {
                throw new ArgumentNullException(Resources.ServerNameCannotBeNullOrWhiteSpace);
            }

            if (!server.IsLocalAddress(out server))
            {
                throw new ArgumentOutOfRangeException(string.Format(Resources.ServerNameIsNotLocal, server));
            }

            // Номер порта сервера

            var port = hostingContext.Configuration.Port;

            if (port <= 0)
            {
                throw new ArgumentOutOfRangeException(string.Format(Resources.ServerPortIsIncorrect, port));
            }

            // Отпечаток сертификата

            var certificate = hostingContext.Configuration.Certificate;

            if (Uri.UriSchemeHttps.Equals(scheme, StringComparison.OrdinalIgnoreCase) && string.IsNullOrWhiteSpace(certificate))
            {
                throw new ArgumentNullException(Resources.ServerCertificateCannotBeNullOrWhiteSpace);
            }


            _hostingContext = hostingContext;

            _baseAddress = $"{scheme}://{server}:{port}/";
            _hostingModules = hostingContext.ContainerResolver.Resolve<IEnumerable<IOwinHostingModule>>().OrderBy(m => m.ModuleType);
        }


        private readonly IOwinHostingContext _hostingContext;

        private readonly string _baseAddress;
        private readonly IEnumerable<IOwinHostingModule> _hostingModules;

        private readonly object _hostSync = new object();
        private volatile IDisposable _host;


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
                            OnBeforeStart?.Invoke(this, EventArgs.Empty);

                            host = WebApp.Start(_baseAddress, Startup);

                            OnAfterStart?.Invoke(this, EventArgs.Empty);
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
                            OnBeforeStop?.Invoke(this, EventArgs.Empty);
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
                            OnAfterStop?.Invoke(this, EventArgs.Empty);
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


        public event EventHandler OnBeforeStart;

        public event EventHandler OnAfterStart;

        public event EventHandler OnBeforeStop;

        public event EventHandler OnAfterStop;


        private void Startup(IAppBuilder builder)
        {
            object httpListener;

            if (builder.Properties.TryGetValue(typeof(HttpListener).FullName, out httpListener) && httpListener is HttpListener)
            {
                // HttpListener should not return exceptions that occur when sending the response to the client
                ((HttpListener)httpListener).IgnoreWriteExceptions = true;
            }

            foreach (var module in _hostingModules)
            {
                module.Configure(builder, _hostingContext);
            }
        }
    }
}