using System;

using InfinniPlatform.Owin.Middleware;

using Microsoft.Owin;

namespace InfinniPlatform.IoC.Owin
{
    internal sealed class AutofacOwinMiddlewareResolver : IOwinMiddlewareResolver
    {
        public Type ResolveType<TOwinMiddleware>() where TOwinMiddleware : OwinMiddleware
        {
            return typeof(AutofacWrapperOwinMiddleware<TOwinMiddleware>);
        }
    }
}