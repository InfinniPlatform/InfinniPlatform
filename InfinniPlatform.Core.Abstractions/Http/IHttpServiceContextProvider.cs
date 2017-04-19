namespace InfinniPlatform.Core.Http
{
    /// <summary>
    /// Предоставляет доступ к контексту выполнения текущего запроса к сервису <see cref="IHttpServiceContext" />.
    /// </summary>
    public interface IHttpServiceContextProvider
    {
        /// <summary>
        /// Возвращает контекст выполнения текущего запроса.
        /// </summary>
        IHttpServiceContext GetContext();
    }
}