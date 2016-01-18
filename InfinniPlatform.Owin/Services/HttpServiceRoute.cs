using System;

using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Owin.Services
{
    internal sealed class HttpServiceRoute : IHttpServiceRoute
    {
        public HttpServiceRoute(string path, Func<IHttpRequest, object> action)
        {
            Path = path;
            Action = action;
        }

        public string Path { get; }

        public Func<IHttpRequest, object> Action { get; }
    }
}