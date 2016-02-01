using Microsoft.Owin;

namespace InfinniPlatform.Owin.Security
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