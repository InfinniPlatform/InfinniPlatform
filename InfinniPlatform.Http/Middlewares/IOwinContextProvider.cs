using Microsoft.Owin;

namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Предоставляет метод для получения контекста OWIN текущего запроса.
    /// </summary>
    public interface IOwinContextProvider
    {
        /// <summary>
        /// Возвращает контекст OWIN текущего запроса.
        /// </summary>
        IOwinContext GetOwinContext();
    }
}