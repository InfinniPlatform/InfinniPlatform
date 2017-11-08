using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;

namespace InfinniPlatform.Http
{
    /// <summary>
    /// Настройки Nancy для <see cref="IHttpService" />
    /// </summary>
    public class HttpServiceNancyBootstrapper : DefaultNancyBootstrapper
    {
        public HttpServiceNancyBootstrapper(INancyModuleCatalog nancyModuleCatalog,
                                            ILogger<HttpServiceNancyBootstrapper> logger)
        {
            _nancyModuleCatalog = nancyModuleCatalog;
            _logger = logger;
        }

        private readonly INancyModuleCatalog _nancyModuleCatalog;
        private readonly ILogger<HttpServiceNancyBootstrapper> _logger;

        protected override void ConfigureApplicationContainer(TinyIoCContainer nancyContainer)
        {
            nancyContainer.Register(_nancyModuleCatalog);

            // Соглашения обработки запросов должны устанавливаться явно, так как автоматический поиск соглашений в Mono/Linux не работает,
            // поскольку при поиске Nancy использует метод AppDomain.CurrentDomain.GetAssemblies(), который возвращает все сборки текущего
            // домена приложения, кроме той, который его вызывала. Ниже зарегистрированы соглашения, используемые Nancy по умолчанию.

            nancyContainer.RegisterMultiple<IConvention>(new[]
                                                         {
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

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            pipelines.OnError += (ctx, e) =>
            {
                _logger.LogError(e, BuildLogContext(ctx));

                return null;
            };

            base.RequestStartup(container, pipelines, context);
        }

        private static Func<Dictionary<string, object>> BuildLogContext(NancyContext ctx)
        {
            return () => new Dictionary<string, object>
            {
                {"path", ctx.Request.Path}
            };
        }
    }
}