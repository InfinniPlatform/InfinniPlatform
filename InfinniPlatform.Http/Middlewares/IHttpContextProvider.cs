using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Предоставляет метод для получения контекста OWIN текущего запроса.
    /// </summary>
    public interface IHttpContextProvider
    {
        /// <summary>
        /// Возвращает контекст OWIN текущего запроса.
        /// </summary>
        HttpContext GetHttpContext();
    }
}