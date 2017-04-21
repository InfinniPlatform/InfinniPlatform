using System;

using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Http.Middlewares
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