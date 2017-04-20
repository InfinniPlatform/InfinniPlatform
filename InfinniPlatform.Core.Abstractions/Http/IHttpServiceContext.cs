namespace InfinniPlatform.Http
{
    /// <summary>
    /// Контекст выполнения запроса к сервису <see cref="IHttpService" />.
    /// </summary>
    public interface IHttpServiceContext
    {
        /// <summary>
        /// Запрос.
        /// </summary>
        IHttpRequest Request { get; }
    }
}