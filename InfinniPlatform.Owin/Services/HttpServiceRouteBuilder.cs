using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Owin.Services
{
    internal sealed class HttpServiceRouteBuilder : IHttpServiceRouteBuilder
    {
        private readonly List<IHttpServiceRoute> _routes = new List<IHttpServiceRoute>();

        public IEnumerable<IHttpServiceRoute> Routes => _routes.AsReadOnly();

        public Func<IHttpRequest, Task<object>> this[string path]
        {
            set
            {
                if (string.IsNullOrEmpty(path))
                {
                    throw new ArgumentNullException(nameof(path));
                }

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _routes.Add(new HttpServiceRoute(path, value));
            }
        }
    }
}