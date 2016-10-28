using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Owin.Services
{
    internal class HttpServiceRoute : IHttpServiceRoute
    {
        public HttpServiceRoute(string path, Func<IHttpRequest, Task<object>> action)
        {
            Path = path;
            Action = action;
        }

        public string Path { get; }

        public Func<IHttpRequest, Task<object>> Action { get; }
    }
}