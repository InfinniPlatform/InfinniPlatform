﻿using System;

using InfinniPlatform.Http.Middlewares;

using Microsoft.Owin;

namespace InfinniPlatform.Core.IoC.Http
{
    internal sealed class AutofacOwinMiddlewareResolver : IOwinMiddlewareTypeResolver
    {
        public Type ResolveType<TOwinMiddleware>() where TOwinMiddleware : OwinMiddleware
        {
            return typeof(AutofacWrapperOwinMiddleware<TOwinMiddleware>);
        }
    }
}