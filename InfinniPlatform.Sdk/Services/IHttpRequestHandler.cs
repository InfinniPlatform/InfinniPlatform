namespace InfinniPlatform.Sdk.Services
{
    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    public interface IHttpRequestHandler
    {
        /// <summary>
        /// Обрабатывает запрос.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Результат выполнения запроса.</returns>
        object Action(IHttpRequest request);
    }
}