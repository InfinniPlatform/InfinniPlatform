namespace InfinniPlatform.Auth.Internal.Identity
{
    /// <summary>
    /// Предоставляет методы хэширования пароля.
    /// </summary>
    internal class DefaultAppUserPasswordHasher : IAppUserPasswordHasher
    {
        /// <summary>
        /// Возвращает хэш пароля.
        /// </summary>
        public string HashPassword(string password)
        {
            return StringHasher.HashValue(password);
        }

        /// <summary>
        /// Проверяет, что пароль соответствует хэшу.
        /// </summary>
        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return StringHasher.VerifyValue(hashedPassword, providedPassword);
        }
    }
}