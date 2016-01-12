namespace InfinniPlatform.Sdk.RestApi
{
    /// <summary>
    /// API для аутентификации пользователей.
    /// </summary>
    public interface ISignInApi
    {
        /// <summary>
        /// Осуществляет вход пользователя в систему через внутренний провайдер.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <param name="remember">Запомнить пользователя.</param>
        object SignInInternal(string userName, string password, bool remember = true);

        /// <summary>
        /// Изменяет пароль текущего пользователя.
        /// </summary>
        /// <param name="oldPassword">Старый пароль пользователя.</param>
        /// <param name="newPassword">Новый пароль пользователя.</param>
        object ChangePassword(string oldPassword, string newPassword);

        /// <summary>
        /// Осуществляет выход пользователя из системы.
        /// </summary>
        object SignOut();
    }
}