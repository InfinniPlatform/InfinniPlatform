namespace InfinniPlatform.Auth.Internal.Identity
{
    /// <summary>
    /// Предоставляет методы хэширования пароля.
    /// </summary>
    internal interface IAppUserPasswordHasher
    {
        /// <summary>
        /// Возвращает хэш пароля.
        /// </summary>
        string HashPassword(string password);

        /// <summary>
        /// Проверяет, что пароль соответствует хэшу.
        /// </summary>
        bool VerifyHashedPassword(string hashedPassword, string providedPassword);
    }
}