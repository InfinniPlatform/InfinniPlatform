using InfinniPlatform.Sdk.Hosting;

using Owin;

namespace InfinniPlatform.Owin.Middleware
{
    /// <summary>
    /// Промежуточный слой обработки запросов приложения на базе OWIN.
    /// </summary>
    public abstract class OwinHostingMiddleware : IHostingMiddleware
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="type">Тип промежуточного слоя.</param>
        protected OwinHostingMiddleware(HostingMiddlewareType type)
        {
            Type = type;
        }


        /// <summary>
        /// Возвращает тип модуля хостинга.
        /// </summary>
        public HostingMiddlewareType Type { get; }


        /// <summary>
        /// Настраивает промежуточный слой.
        /// </summary>
        /// <param name="builder">Объект для регистрации обработчиков запросов.</param>
        public void Configure(object builder)
        {
            Configure((IAppBuilder)builder);
        }


        /// <summary>
        /// Настраивает модуль хостинга.
        /// </summary>
        /// <param name="builder">Объект для регистрации обработчиков запросов OWIN.</param>
        public abstract void Configure(IAppBuilder builder);
    }
}