using System;

using InfinniPlatform.Http.Middlewares;

using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Auth.Middlewares
{
    /// <summary>
    /// Настройки для <see cref="AuthCookieHttpMiddleware"/>.
    /// </summary>
    public class AuthCookieMiddlewareOptions : IMiddlewareOptions
    {
        public AuthCookieMiddlewareOptions(Action<IApplicationBuilder> configure)
        {
            Configure = configure ?? (b => { });
        }

        public Action<IApplicationBuilder> Configure { get; }
    }
}