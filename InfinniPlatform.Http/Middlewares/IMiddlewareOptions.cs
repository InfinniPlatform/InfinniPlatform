using System;
using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Настройки для <see cref="IHttpMiddleware"/>.
    /// </summary>
    public interface IMiddlewareOptions
    {
        /// <summary>
        /// 
        /// </summary>
        Action<IApplicationBuilder> Configure { get; }
    }
}