using System;
using InfinniPlatform.Http.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Core.Http.Middlewares
{
    /// <summary>
    /// Настройки для <see cref="NancyHttpMiddleware"/>.
    /// </summary>
    public class NancyMiddlewareOptions : IMiddlewareOptions
    {
        public NancyMiddlewareOptions(Action<IApplicationBuilder> configure = null)
        {
            Configure = configure ?? (b => { });
        }


        /// <summary>
        /// Передавать управление последующим middleware.
        /// </summary>
        public bool PerformPassThrough { get; set; }

        public Action<IApplicationBuilder> Configure { get; }
    }
}