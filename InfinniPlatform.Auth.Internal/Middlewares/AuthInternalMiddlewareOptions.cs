using System;
using InfinniPlatform.Http.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Auth.Internal.Middlewares
{
    /// <summary>
    /// Настройки для <see cref="AuthInternalHttpMiddleware"/>.
    /// </summary>
    public class AuthInternalMiddlewareOptions : IMiddlewareOptions
    {
        public AuthInternalMiddlewareOptions(Action<IApplicationBuilder> configure)
        {
            Configure = configure ?? (b => { });
        }

        public Action<IApplicationBuilder> Configure { get; }
    }
}