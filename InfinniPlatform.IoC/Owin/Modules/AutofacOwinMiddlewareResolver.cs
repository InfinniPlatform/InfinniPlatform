using System;

using InfinniPlatform.IoC.Owin.Middleware;
using InfinniPlatform.Owin.Modules;

using Microsoft.Owin;

namespace InfinniPlatform.IoC.Owin.Modules
{
    internal sealed class AutofacOwinMiddlewareResolver : IOwinMiddlewareResolver
    {
        public Type ResolveType<TOwinMiddleware>() where TOwinMiddleware : OwinMiddleware
        {
            return typeof(AutofacWrapperOwinMiddleware<TOwinMiddleware>);
        }
    }
}