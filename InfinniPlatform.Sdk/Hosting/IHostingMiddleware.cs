namespace InfinniPlatform.Sdk.Hosting
{
    /// <summary>
    /// Промежуточный слой обработки запросов приложения.
    /// </summary>
    public interface IHostingMiddleware
    {
        /// <summary>
        /// Тип промежуточного слоя.
        /// </summary>
        HostingMiddlewareType Type { get; }

        /// <summary>
        /// Настраивает промежуточный слой.
        /// </summary>
        /// <param name="builder">Объект для регистрации обработчиков запросов.</param>
        void Configure(object builder);
    }
}