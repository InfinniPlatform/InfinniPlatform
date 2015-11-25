using System;

using Microsoft.Owin;

using Owin;

namespace InfinniPlatform.Owin.Modules
{
    /// <summary>
    /// Провайдер разрешения типа обработчика запросов OWIN для регистрации в <see cref="IAppBuilder"/>.
    /// </summary>
    public interface IOwinMiddlewareResolver
    {
        /// <summary>
        /// Возвращает реальный тип обработчика запросов OWIN для регистрации в <see cref="IAppBuilder"/>.
        /// </summary>
        /// <typeparam name="TOwinMiddleware">Тип обработчика запросов OWIN.</typeparam>
        Type ResolveType<TOwinMiddleware>() where TOwinMiddleware : OwinMiddleware;
    }
}