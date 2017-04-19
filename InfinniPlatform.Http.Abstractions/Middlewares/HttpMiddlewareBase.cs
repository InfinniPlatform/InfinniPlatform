using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Базовый класс промежуточного слоя обработки HTTP запросов приложения <see cref="IHttpMiddleware"/>.
    /// </summary>
    public abstract class HttpMiddlewareBase<TOptions> : IHttpMiddleware where TOptions :IMiddlewareOptions
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="type">Тип промежуточного слоя.</param>
        protected HttpMiddlewareBase(HttpMiddlewareType type)
        {
            Type = type;
        }

        public HttpMiddlewareType Type { get; }

        public void Configure(IApplicationBuilder app, IMiddlewareOptions options)
        {
            Configure(app, (TOptions) options);

            options?.Configure(app);
        }

        /// <summary>
        /// Настраивает промежуточный слой.
        /// </summary>
        /// <param name="app">Объект для регистрации обработчиков запросов OWIN.</param>
        /// <param name="options">Настройки обработчика запросов OWIN</param>
        public abstract void Configure(IApplicationBuilder app, TOptions options);
    }
}