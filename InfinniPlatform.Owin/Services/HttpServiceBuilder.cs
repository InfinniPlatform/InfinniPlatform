using System;

using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Owin.Services
{
    internal sealed class HttpServiceBuilder : IHttpServiceBuilder
    {
        public HttpServiceBuilder()
        {
            Get = new HttpServiceRouteBuilder();
            Post = new HttpServiceRouteBuilder();
            Put = new HttpServiceRouteBuilder();
            Patch = new HttpServiceRouteBuilder();
            Delete = new HttpServiceRouteBuilder();
        }

        public string ServicePath { get; set; }

        public IHttpServiceRouteBuilder Get { get; }

        public IHttpServiceRouteBuilder Post { get; }

        public IHttpServiceRouteBuilder Put { get; }

        public IHttpServiceRouteBuilder Patch { get; }

        public IHttpServiceRouteBuilder Delete { get; }

        public Func<IHttpRequest, object> OnBefore { get; set; }

        public Func<IHttpRequest, object, object> OnAfter { get; set; }

        public Func<IHttpRequest, Exception, object> OnError { get; set; }

        public Func<object, IHttpResponse> ResultConverter { get; set; }
    }
}