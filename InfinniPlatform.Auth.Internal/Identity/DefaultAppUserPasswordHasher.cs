using InfinniPlatform.Auth.Identity.MongoDb;

using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity
{
    /// <summary>
    ///     Предоставляет методы хэширования пароля.
    /// </summary>
    internal class DefaultAppUserPasswordHasher : IPasswordHasher<IdentityUser>
    {
        /// <summary>
        ///     Возвращает хэш пароля.
        /// </summary>
        public string HashPassword(IdentityUser user, string password)
        {
            return StringHasher.HashValue(password);
        }

        /// <summary>
        ///     Проверяет, что пароль соответствует хэшу.
        /// </summary>
        public PasswordVerificationResult VerifyHashedPassword(IdentityUser user, string hashedPassword, string providedPassword)
        {
            var isVefified = StringHasher.VerifyValue(hashedPassword, providedPassword);

            return isVefified ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}