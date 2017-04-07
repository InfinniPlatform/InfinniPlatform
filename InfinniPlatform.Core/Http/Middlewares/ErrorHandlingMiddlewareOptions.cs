using System;
using InfinniPlatform.Http.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Core.Http.Middlewares
{
    /// <summary>
    /// Настройки для <see cref="ErrorHandlingHttpMiddleware"/>.
    /// </summary>
    public class ErrorHandlingMiddlewareOptions : IMiddlewareOptions
    {
        public ErrorHandlingMiddlewareOptions(Action<IApplicationBuilder> configure)
        {
            Configure = configure ?? (b => { });
        }

        public Action<IApplicationBuilder> Configure { get; }
    }
}