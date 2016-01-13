using System;

using InfinniPlatform.Sdk.Services;

using Nancy;

namespace InfinniPlatform.Owin.Services
{
    /// <summary>
    /// Регистратор обработчиков запросов сервиса.
    /// </summary>
    internal sealed class HttpServiceBuilder : IHttpServiceBuilder
    {
        public HttpServiceBuilder(NancyModule nancyModule)
        {
            Func<NancyContext> nancyContext = () => nancyModule.Context;

            Get = new HttpRouteBuilder(nancyModule.Get, nancyContext, () => OnBefore, () => OnAfter);
            Post = new HttpRouteBuilder(nancyModule.Post, nancyContext, () => OnBefore, () => OnAfter);
            Put = new HttpRouteBuilder(nancyModule.Put, nancyContext, () => OnBefore, () => OnAfter);
            Patch = new HttpRouteBuilder(nancyModule.Patch, nancyContext, () => OnBefore, () => OnAfter);
            Delete = new HttpRouteBuilder(nancyModule.Delete, nancyContext, () => OnBefore, () => OnAfter);
        }


        public IHttpRouteBuilder Get { get; }

        public IHttpRouteBuilder Post { get; }

        public IHttpRouteBuilder Put { get; }

        public IHttpRouteBuilder Patch { get; }

        public IHttpRouteBuilder Delete { get; }

        public Action<IHttpRequest> OnBefore { get; set; }

        public Action<IHttpRequest, IHttpResponse> OnAfter { get; set; }
    }
}