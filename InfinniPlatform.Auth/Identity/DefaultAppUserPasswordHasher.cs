using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity
{
    /// <summary>
    ///     Предоставляет методы хэширования пароля.
    /// </summary>
    internal class DefaultAppUserPasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : AppUser
    {
        /// <summary>
        ///     Возвращает хэш пароля.
        /// </summary>
        public string HashPassword(TUser user, string password)
        {
            return StringHasher.HashValue(password);
        }

        /// <summary>
        ///     Проверяет, что пароль соответствует хэшу.
        /// </summary>
        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            var isVefified = StringHasher.VerifyValue(hashedPassword, providedPassword);

            return isVefified ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}