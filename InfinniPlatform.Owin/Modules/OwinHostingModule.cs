using System;
using InfinniPlatform.Hosting;
using Owin;

namespace InfinniPlatform.Owin.Modules
{
    /// <summary>
    ///     Модуль хостинга на базе OWIN (Open Web Interface for .NET).
    /// </summary>
    public abstract class OwinHostingModule
    {
        /// <summary>
        ///     Обработчик события старта модуля хостинга.
        /// </summary>
        public Action<HostingContextBuilder, IHostingContext> OnStart { get; protected set; }

        /// <summary>
        ///     Обработчик события остановки модуля хостинга.
        /// </summary>
        public Action<IHostingContext> OnStop { get; protected set; }

        /// <summary>
        ///     Конфигурировать модуль хостинга.
        /// </summary>
        /// <param name="builder">Объект для регистрации модуля хостинга.</param>
        /// <param name="context">Контекст подсистемы хостинга.</param>
        public abstract void Configure(IAppBuilder builder, IHostingContext context);
    }
}