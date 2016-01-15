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
        /// <returns>Ответ.</returns>
        IHttpResponse Action(IHttpRequest request);
    }
}