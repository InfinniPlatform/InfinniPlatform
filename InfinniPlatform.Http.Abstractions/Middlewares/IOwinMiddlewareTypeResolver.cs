using System;

using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Провайдер получения типа обработчика запросов OWIN для регистрации в <see cref="IApplicationBuilder "/>.
    /// </summary>
    public interface IOwinMiddlewareTypeResolver
    {
        /// <summary>
        /// Возвращает реальный тип обработчика запросов OWIN для регистрации в <see cref="IApplicationBuilder "/>.
        /// </summary>
        /// <typeparam name="TOwinMiddleware">Тип обработчика запросов OWIN.</typeparam>
        Type ResolveType<TOwinMiddleware>() where TOwinMiddleware : OwinMiddleware;
    }
}