namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Уровень для обработки запросов после аутентификации пользователя.
    /// </summary>
    /// <remarks>
    /// Позволяет осуществлять любую логику после окончания аутентификации пользователя. Например, обрабатывать запросы к ASP.NET SignalR.
    /// </remarks>
    public interface IAfterAuthenticationAppLayer : IAppLayer
    {
    }
}