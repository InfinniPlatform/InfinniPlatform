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
        /// Функция конфигурирования этапа обработки запроса.
        /// </summary>
        Action<IApplicationBuilder> Configure { get; }
    }
}