namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Уровень для глобальной обработки запросов.
    /// </summary>
    /// <remarks>
    /// Позволяет осуществлять глобальный контроль обработки запросов. Например, управлять жизненным циклом зависимостей.
    /// </remarks>
    public interface IGlobalHandlingMiddleware : IMiddleware
    {
    }
}