namespace InfinniPlatform.Api.RestApi.CommonApi
{
    /// <summary>
    ///     Интерфейс трассировки запросов API
    /// </summary>
    public interface IRouteTrace
    {
        /// <summary>
        ///     Трассировать запрос
        /// </summary>
        /// <param name="configuration">Конфигурация</param>
        /// <param name="metadata">Метаданные</param>
        /// <param name="action">Действие</param>
        /// <param name="url">Url трассируемого запроса</param>
        /// <param name="body">Тело трассируемого запроса</param>
        /// <param name="verbType">Тип запроса (POST/GET)</param>
        /// <param name="content">Результат запроса</param>
        void Trace(
            string configuration,
            string metadata,
            string action,
            string url,
            object body,
            string verbType,
            string content);
    }
}