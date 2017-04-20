using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Loader;

using InfinniPlatform.Extensions;
using InfinniPlatform.Logging;
using InfinniPlatform.Properties;
using InfinniPlatform.Settings;

using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;

namespace InfinniPlatform.Http.Services
{
    /// <summary>
    /// Настройки Nancy для <see cref="IHttpService" />
    /// </summary>
    public class HttpServiceNancyBootstrapper : DefaultNancyBootstrapper
    {
        public HttpServiceNancyBootstrapper(INancyModuleCatalog nancyModuleCatalog,
                                            AppOptions appOptions,
                                            ILog log,
                                            IViewEngineBootstrapperExtension viewEngineBootstrapperExtension = null)
        {
            _nancyModuleCatalog = nancyModuleCatalog;
            _appOptions = appOptions;
            _log = log;
            _viewEngineBootstrapperExtension = viewEngineBootstrapperExtension;
        }

        private readonly ILog _log;

        private readonly INancyModuleCatalog _nancyModuleCatalog;
        private readonly AppOptions _appOptions;
        private readonly IViewEngineBootstrapperExtension _viewEngineBootstrapperExtension;

        protected override Func<ITypeCatalog, NancyInternalConfiguration> InternalConfiguration
        {
            get
            {
                // регистрирует тип локатора Razor-представлений, если подключен пакет InfinniPlatform.Plugins.ViewEngine.
                return _viewEngineBootstrapperExtension != null
                    ? NancyInternalConfiguration.WithOverrides(c => c.ViewLocationProvider = _viewEngineBootstrapperExtension.ViewLocatorType)
                    : NancyInternalConfiguration.Default;
            }
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer nancyContainer)
        {
            nancyContainer.Register(_nancyModuleCatalog);

            // Соглашения обработки запросов должны устанавливаться явно, так как автоматический поиск соглашений в Mono/Linux не работает,
            // поскольку при поиске Nancy использует метод AppDomain.CurrentDomain.GetAssemblies(), который возвращает все сборки текущего
            // домена приложения, кроме той, который его вызывала. Ниже зарегистрированы соглашения, используемые Nancy по умолчанию.

            nancyContainer.RegisterMultiple<IConvention>(new[]
                                                         {
                                                             typeof(DefaultViewLocationConventions),
                                                             typeof(DefaultStaticContentsConventions),
                                                             typeof(DefaultAcceptHeaderCoercionConventions),
                                                             typeof(DefaultCultureConventions)
                                                         }).AsSingleton();
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            var conventions = ApplicationContainer.ResolveAll<IConvention>();

            if (conventions != null)
            {
                foreach (var convention in conventions)
                {
                    convention.Initialise(nancyConventions);
                }
            }

            base.ConfigureConventions(nancyConventions);
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            // Проверка заголовка Last-Modified при обработке запросов к файлам.
            pipelines.AfterRequest += CheckForIfModifiedSince;
            pipelines.AfterRequest += CheckForIfNonMatch;

            base.ApplicationStartup(container, pipelines);

            // Добавление сопоставления между виртуальными (запрашиваемый в браузере путь) и физическими директориями в файловой системе.
            Conventions.StaticContentsConventions.Clear();

            RegisterStaticFiles();
            RegisterEmbeddedResource();

            // Добавляет источники Razor-представлений, если подключен пакет InfinniPlatform.Plugins.ViewEngine.
            _viewEngineBootstrapperExtension?.RegisterViewLocators(Conventions);
        }

        private static void CheckForIfModifiedSince(NancyContext context)
        {
            var request = context.Request;
            var response = context.Response;

            string responseLastModified;

            if (!response.Headers.TryGetValue("Last-Modified", out responseLastModified))
            {
                return;
            }

            DateTime lastModified;

            if (!request.Headers.IfModifiedSince.HasValue
                || !DateTime.TryParseExact(responseLastModified, "R", CultureInfo.InvariantCulture, DateTimeStyles.None, out lastModified))
            {
                return;
            }

            if (lastModified <= request.Headers.IfModifiedSince.Value)
            {
                context.Response = HttpStatusCode.NotModified;
            }
        }

        private static void CheckForIfNonMatch(NancyContext context)
        {
            var request = context.Request;
            var response = context.Response;

            string responseETag;

            if (!response.Headers.TryGetValue("ETag", out responseETag))
            {
                return;
            }

            if (request.Headers.IfNoneMatch.Contains(responseETag))
            {
                context.Response = HttpStatusCode.NotModified;
            }
        }

        private void RegisterStaticFiles()
        {
            foreach (var mapping in _appOptions.StaticFilesMapping)
            {
                var requestedPath = mapping.Key;
                var contentPath = mapping.Value.ToWebPath();

                Conventions.StaticContentsConventions.AddDirectory(requestedPath, contentPath);

                _log.Info(Resources.ServingStaticContent, () => new Dictionary<string, object> { { "requestedPath", requestedPath }, { "contentPath", contentPath } });
            }
        }

        private void RegisterEmbeddedResource()
        {
            foreach (var mapping in _appOptions.EmbeddedResourceMapping)
            {
                var requestedPath = mapping.Key;
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(requestedPath);

                Conventions.StaticContentsConventions.AddDirectory(requestedPath, assembly);
            }
        }
    }
}