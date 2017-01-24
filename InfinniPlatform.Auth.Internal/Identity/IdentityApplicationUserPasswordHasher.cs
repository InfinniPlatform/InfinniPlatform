using System;
using InfinniPlatform.Auth.Internal.Identity.MongoDb;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Internal.Identity
{
    /// <summary>
    /// Предоставляет методы хэширования пароля.
    /// </summary>
    internal class IdentityApplicationUserPasswordHasher : IPasswordHasher<IdentityUser>
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


        public string HashPassword(IdentityUser user, string password)
        {
            return _passwordHasher.HashPassword(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(IdentityUser user, string hashedPassword, string providedPassword)
        {
            return _passwordHasher.VerifyHashedPassword(hashedPassword, providedPassword) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}