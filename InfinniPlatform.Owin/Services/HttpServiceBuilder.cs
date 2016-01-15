using System;
using System.Security.Principal;

using InfinniPlatform.Sdk.Services;

using Nancy;

namespace InfinniPlatform.Owin.Services
{
    /// <summary>
    /// Регистратор обработчиков запросов сервиса.
    /// </summary>
    internal sealed class HttpServiceBuilder : IHttpServiceBuilder
    {
        public HttpServiceBuilder(NancyModule nancyModule, Func<IIdentity> userIdentityProvider)
        {
            Get = CreateHttpRouteBuilder(nancyModule.Get, nancyModule, userIdentityProvider);
            Post = CreateHttpRouteBuilder(nancyModule.Post, nancyModule, userIdentityProvider);
            Put = CreateHttpRouteBuilder(nancyModule.Put, nancyModule, userIdentityProvider);
            Patch = CreateHttpRouteBuilder(nancyModule.Patch, nancyModule, userIdentityProvider);
            Delete = CreateHttpRouteBuilder(nancyModule.Delete, nancyModule, userIdentityProvider);
        }


        public IHttpRouteBuilder Get { get; }

        public IHttpRouteBuilder Post { get; }

        public IHttpRouteBuilder Put { get; }

        public IHttpRouteBuilder Patch { get; }

        public IHttpRouteBuilder Delete { get; }

        public Action<IHttpRequest> OnBefore { get; set; }

        public Action<IHttpRequest, IHttpResponse, Exception> OnAfter { get; set; }


        private IHttpRouteBuilder CreateHttpRouteBuilder(NancyModule.RouteBuilder nancyRouteBuilder, NancyModule nancyModule, Func<IIdentity> userIdentityProvider)
        {
            return new HttpRouteBuilder(nancyRouteBuilder, () => nancyModule.Context, userIdentityProvider, () => OnBefore, () => OnAfter);
        }
    }
}