namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Уровень для аутентификации пользователя на основе внешнего провайдера.
    /// </summary>
    /// <remarks>
    /// Позволяет осуществлять аутентификацию пользователя на основе внешнего провайдера. Например, Google, Facebook, Twitter и т.п.
    /// </remarks>
    public interface IExternalAuthenticationAppLayer : IAppLayer
    {
    }
}