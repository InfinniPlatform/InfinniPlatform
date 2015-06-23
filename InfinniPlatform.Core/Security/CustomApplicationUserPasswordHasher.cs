using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Security;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Security
{
    public sealed class CustomApplicationUserPasswordHasher : IApplicationUserPasswordHasher
    {
        private readonly IGlobalContext _globalContext;

        public CustomApplicationUserPasswordHasher(IGlobalContext globalContext)
        {
            _globalContext = globalContext;
        }

        /// <summary>
        ///     Возвращает хэш пароля.
        /// </summary>
        public string HashPassword(string password)
        {
            return StringHasher.HashValue(password);
        }

        /// <summary>
        ///     Проверяет, что пароль соответствует хэшу.
        /// </summary>
        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            var success = StringHasher.VerifyValue(hashedPassword, providedPassword);
            if (!success)
            {
                success = _globalContext.GetComponent<IPasswordVerifierComponent>(null)
                    .VerifyPassword(hashedPassword, providedPassword);
            }
            return success;
        }
    }
}