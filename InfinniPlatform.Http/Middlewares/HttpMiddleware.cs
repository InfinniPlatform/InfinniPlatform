using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Базовый класс промежуточного слоя обработки HTTP запросов приложения <see cref="IHttpMiddleware"/>.
    /// </summary>
    public abstract class HttpMiddleware : IHttpMiddleware
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="type">Тип промежуточного слоя.</param>
        protected HttpMiddleware(HttpMiddlewareType type)
        {
            Type = type;
        }


        /// <summary>
        /// Тип промежуточного слоя.
        /// </summary>
        public HttpMiddlewareType Type { get; }


        /// <summary>
        /// Настраивает промежуточный слой.
        /// </summary>
        /// <param name="app">Объект для регистрации обработчиков запросов OWIN.</param>
        public abstract void Configure(IApplicationBuilder app);
    }
}