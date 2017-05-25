namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Уровень для барьерной аутентификации пользователя.
    /// </summary>
    /// <remarks>
    /// Позволяет осуществлять логику барьерной аутентификации пользователя. Например, на основе Cookie или токена безопасности.
    /// </remarks>
    public interface IAuthenticationBarrierAppLayer : IAppLayer
    {
    }
}