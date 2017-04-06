using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Промежуточный слой обработки HTTP запросов приложения.
    /// </summary>
    public interface IHttpMiddleware
    {
        /// <summary>
        /// Тип промежуточного слоя.
        /// </summary>
        HttpMiddlewareType Type { get; }

        /// <summary>
        /// Настраивает промежуточный слой.
        /// </summary>
        /// <param name="app">Объект для регистрации обработчиков запросов.</param>
        void Configure(IApplicationBuilder app);
    }
}