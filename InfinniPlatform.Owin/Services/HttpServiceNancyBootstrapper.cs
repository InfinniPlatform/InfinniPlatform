using System;
using System.Diagnostics;
using System.Globalization;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Services;

using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;

namespace InfinniPlatform.Owin.Services
{
    /// <summary>
    /// Настройки Nancy для <see cref="IHttpService" />
    /// </summary>
    internal sealed class HttpServiceNancyBootstrapper : DefaultNancyBootstrapper
    {
        public HttpServiceNancyBootstrapper(INancyModuleCatalog nancyModuleCatalog, MetadataSettings metadataSettings)
        {
            _nancyModuleCatalog = nancyModuleCatalog;
            _metadataSettings = metadataSettings;
        }

        private readonly MetadataSettings _metadataSettings;

        private readonly INancyModuleCatalog _nancyModuleCatalog;

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
            pipelines.AfterRequest += CheckForIfModifiedSince;

            base.ApplicationStartup(container, pipelines);
            Conventions.StaticContentsConventions.Clear();
            Conventions.StaticContentsConventions.AddDirectory("/content", $"{_metadataSettings.ContentDirectory}", "json");
        }

        public static void CheckForIfModifiedSince(NancyContext context)
        {
            Debugger.Launch();
            var request = context.Request;
            var response = context.Response;

            string responseLastModified;
            if (!response.Headers.TryGetValue("Last-Modified", out responseLastModified))
            {
                return;
            }

            DateTime lastModified;

            if (!request.Headers.IfModifiedSince.HasValue || !DateTime.TryParseExact(responseLastModified, "R", CultureInfo.InvariantCulture, DateTimeStyles.None, out lastModified))
            {
                return;
            }

            if (lastModified <= request.Headers.IfModifiedSince.Value)
            {
                context.Response = HttpStatusCode.NotModified;
            }
        }
    }
}
