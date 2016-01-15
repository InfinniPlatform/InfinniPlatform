namespace InfinniPlatform.Sdk.RestApi
{
    /// <summary>
    /// API для аутентификации пользователей.
    /// </summary>
    public sealed class InfinniSignInApi : BaseApi
    {
        public InfinniSignInApi(string server, int port) : base(server, port)
        {
        }

        /// <summary>
        /// Осуществляет вход пользователя в систему через внутренний провайдер.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <param name="remember">Запомнить пользователя.</param>
        public object SignInInternal(string userName, string password, bool remember = true)
        {
            return RequestExecutor.QueryPost($"http://{Server}:{Port}/Auth/SignInInternal", new { UserName = userName, Password = password, Remember = remember });
        }

        /// <summary>
        /// Изменяет пароль текущего пользователя.
        /// </summary>
        /// <param name="oldPassword">Старый пароль пользователя.</param>
        /// <param name="newPassword">Новый пароль пользователя.</param>
        public object ChangePassword(string oldPassword, string newPassword)
        {
            return RequestExecutor.QueryPost($"http://{Server}:{Port}/Auth/ChangePassword", new { OldPassword = oldPassword, NewPassword = newPassword });
        }

        /// <summary>
        /// Осуществляет выход пользователя из системы.
        /// </summary>
        public object SignOut()
        {
            return RequestExecutor.QueryPost($"http://{Server}:{Port}/Auth/SignOut");
        }
    }
}