namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Уровень для обработки запросов до аутентификации пользователя.
    /// </summary>
    /// <remarks>
    /// Позволяет осуществлять любую логику до начала аутентификации пользователя. Например, добавлять заголовки CORS.
    /// </remarks>
    public interface IBeforeAuthenticationMiddleware : IMiddleware
    {
    }
}