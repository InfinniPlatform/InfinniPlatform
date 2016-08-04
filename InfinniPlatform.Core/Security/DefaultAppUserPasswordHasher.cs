﻿namespace InfinniPlatform.Core.Security
{
    /// <summary>
    /// Предоставляет методы хэширования пароля.
    /// </summary>
    public sealed class DefaultAppUserPasswordHasher : IAppUserPasswordHasher
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