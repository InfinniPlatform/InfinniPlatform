using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

using InfinniPlatform.Core.Extensions;
using InfinniPlatform.Core.Properties;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Logging;

using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;
using Nancy.ViewEngines;

namespace InfinniPlatform.Core.Http.Services
{
    /// <summary>
    /// Настройки Nancy для <see cref="IHttpService" />
    /// </summary>
    internal class HttpServiceNancyBootstrapper : DefaultNancyBootstrapper
    {
        private const string RazorViewFileExtension = ".cshtml";

        public HttpServiceNancyBootstrapper(INancyModuleCatalog nancyModuleCatalog,
                                            StaticContentSettings staticContentSettings,
                                            ILog log)
        {
            _nancyModuleCatalog = nancyModuleCatalog;
            _staticContentSettings = staticContentSettings;
            _log = log;
        }

        private readonly ILog _log;

        private readonly INancyModuleCatalog _nancyModuleCatalog;
        private readonly StaticContentSettings _staticContentSettings;

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get { return NancyInternalConfiguration.WithOverrides(c => c.ViewLocationProvider = typeof(AggregateViewLocationProvider)); }
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer nancyContainer)
        {
            nancyContainer.Register(_nancyModuleCatalog);

            // Соглашения обработки запросов должны устанавливаться явно, так как автоматический поиск соглашений в Mono/Linux не работает,
            // поскольку при поиске Nancy использует метод AppDomain.CurrentDomain.GetAssemblies(), который возвращает все сборки текущего
            // домена приложения, кроме той, который его вызывала. Ниже зарегистрированы соглашения, используемые Nancy по умолчанию.

            StaticConfiguration.DisableErrorTraces = false;

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
            RegisterRazorViews();
            RegisterEmbeddedRazorViews();
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
            foreach (var mapping in _staticContentSettings.StaticFilesMapping)
            {
                var requestedPath = mapping.Key;
                var contentPath = mapping.Value.ToWebPath();

                Conventions.StaticContentsConventions.AddDirectory(requestedPath, contentPath);

                _log.Info(Resources.ServingStaticContent, () => new Dictionary<string, object> { { "requestedPath", requestedPath }, { "contentPath", contentPath } });
            }
        }

        private void RegisterEmbeddedResource()
        {
            foreach (var mapping in _staticContentSettings.EmbeddedResourceMapping)
            {
                var requestedPath = mapping.Key;
                var assembly = Assembly.Load(mapping.Value);

                Conventions.StaticContentsConventions.AddDirectory(requestedPath, assembly);
            }
        }

        private void RegisterRazorViews()
        {
            Conventions.ViewLocationConventions.Add((viewName, model, context) => $"{_staticContentSettings.RazorViewsPath.ToWebPath()}/{viewName}");
        }

        private void RegisterEmbeddedRazorViews()
        {
            foreach (var mapping in _staticContentSettings.EmbeddedRazorViewsAssemblies)
            {
                var razorViews = Assembly.Load(mapping)
                                         .GetManifestResourceNames()
                                         .Where(s => s.EndsWith(RazorViewFileExtension));

                var rootNamespaces = new List<string>();

                foreach (var razorView in razorViews)
                {
                    var pathWithoutExtention = razorView.Replace(RazorViewFileExtension, string.Empty);

                    // Assuming views names does not contains '.'
                    var fileName = pathWithoutExtention.Split('.').Last();

                    rootNamespaces.Add(pathWithoutExtention.Replace($".{fileName}", string.Empty));
                }

                foreach (var path in rootNamespaces)
                {
                    ResourceViewLocationProvider.RootNamespaces.Add(Assembly.Load(mapping), path);
                }
            }
        }
    }
}