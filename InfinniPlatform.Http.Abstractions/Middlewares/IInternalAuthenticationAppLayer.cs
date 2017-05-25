namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Уровень для аутентификации пользователя средствами приложения.
    /// </summary>
    /// <remarks>
    /// Позволяет осщуествлять аутентификацию пользователя на основе логики приложения. Например, на основе базы данных
    /// пользователей.
    /// </remarks>
    public interface IInternalAuthenticationAppLayer : IAppLayer
    {
    }
}