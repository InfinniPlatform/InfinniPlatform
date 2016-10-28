using System;

using Microsoft.AspNet.Identity;

namespace InfinniPlatform.Auth.Internal.Identity
{
    /// <summary>
    /// Предоставляет методы хэширования пароля.
    /// </summary>
    internal class IdentityApplicationUserPasswordHasher : IPasswordHasher
    {
        public IdentityApplicationUserPasswordHasher(IAppUserPasswordHasher passwordHasher)
        {
            if (passwordHasher == null)
            {
                throw new ArgumentNullException(nameof(passwordHasher));
            }

            _passwordHasher = passwordHasher;
        }


        private readonly IAppUserPasswordHasher _passwordHasher;


        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return _passwordHasher.VerifyHashedPassword(hashedPassword, providedPassword) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}