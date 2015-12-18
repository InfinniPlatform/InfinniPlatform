using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;

using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Environment.Log;
using InfinniPlatform.WebApi.Factories;
using InfinniPlatform.WebApi.Logging;
using InfinniPlatform.WebApi.WebApi;

using Newtonsoft.Json;

using Owin;

namespace InfinniPlatform.WebApi.Modules
{
    /// <summary>
    /// Модуль хостинга обработки прикладных запросов (на базе ASP.NET WebApi).
    /// </summary>
    /// <remarks>
    /// Старый и используемый на данный момент набор сервисов.
    /// </remarks>
    internal sealed class ApplicationWebApiOwinHostingModule : IOwinHostingModule
    {
        public ApplicationWebApiOwinHostingModule(ILog log, ApplicationHostServer applicationHostServer, IApiControllerFactory apiControllerFactory, Func<IDependencyResolver> webApiDependencyResolverFactory)
        {
            _log = log;
            _applicationHostServer = applicationHostServer;
            _apiControllerFactory = apiControllerFactory;
            _webApiDependencyResolverFactory = webApiDependencyResolverFactory;
        }


        private readonly ILog _log;
        private readonly ApplicationHostServer _applicationHostServer;
        private readonly IApiControllerFactory _apiControllerFactory;
        private readonly Func<IDependencyResolver> _webApiDependencyResolverFactory;


        public OwinHostingModuleType ModuleType => OwinHostingModuleType.Application;


        public void Configure(IAppBuilder builder, IOwinHostingContext context)
        {
            var hostConfiguration = new HttpConfiguration { IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always };
            SetCustomRoutes(hostConfiguration);
            SetControllerSelector(hostConfiguration);
            SetMediaTypeFormatters(hostConfiguration);
            SetExceptionLogger(hostConfiguration, _log);
            SetDependencyResolver(hostConfiguration, _webApiDependencyResolverFactory());
            builder.UseWebApi(hostConfiguration);

            _applicationHostServer.RegisterServices();
        }


        private static void SetControllerSelector(HttpConfiguration hostConfiguration)
        {
            hostConfiguration.Services.Replace(typeof(IHttpControllerSelector), new HttpControllerSelector(hostConfiguration));
        }

        private static void SetMediaTypeFormatters(HttpConfiguration hostConfiguration)
        {
            var appXmlType = hostConfiguration.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");

            if (appXmlType != null)
            {
                hostConfiguration.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
            }

            hostConfiguration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }

        private static void SetExceptionLogger(HttpConfiguration hostConfiguration, ILog log)
        {
            hostConfiguration.Filters.Add(new LogExceptionFilterAttribute(log));
        }

        private static void SetCustomRoutes(HttpConfiguration hostConfiguration)
        {
            hostConfiguration.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{action}");
            hostConfiguration.Routes.MapHttpRoute("Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            hostConfiguration.Routes.MapHttpRoute("DefaultWithAction", "api/{controller}/{action}/{id}", new { id = RouteParameter.Optional });
            hostConfiguration.Routes.MapHttpRoute("RestControllerConfiguration", "{controller}/{service}", new { service = RouteParameter.Optional, id = RouteParameter.Optional });
            hostConfiguration.Routes.MapHttpRoute("RestControllerDefault", "{configuration}/{controller}/{metadata}/{service}/{id}", new { id = RouteParameter.Optional });
        }

        private static void SetDependencyResolver(HttpConfiguration hostConfiguration, IDependencyResolver dependencyResolver)
        {
            hostConfiguration.DependencyResolver = dependencyResolver;
        }
    }
}