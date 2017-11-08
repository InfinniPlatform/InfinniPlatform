using System;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// Настройки провайдера аутентификации.
    /// </summary>
    public class AuthOptions
    {
        public const string SectionName = "auth";

        public const int DefaultUserCacheTimeout = 30;

        public const string DefaultLoginPath = "/";

        public const string DefaultLogoutPath = "/";

        public static readonly AuthOptions Default = new AuthOptions();


        public AuthOptions()
        {
            UserCacheTimeout = DefaultUserCacheTimeout;
        }


        /// <summary>
        /// Таймаут сброса кэша пользователей в минутах.
        /// </summary>
        public int UserCacheTimeout { get; set; }

        /// <summary>
        /// Фабрика для получения хранилища пользователей <see cref="IUserStore{TUser}"/>.
        /// </summary>
        public IUserStoreFactory UserStoreFactory { get; set; }

        /// <summary>
        /// Настройки ASP.NET Identity.
        /// </summary>
        public Action<IdentityOptions> IdentityOptions { get; set; }

        /// <summary>
        /// Фабрика для получения генератора хэшей для паролей <see cref="IPasswordHasher{TUser}"/>.
        /// </summary>
        public IPasswordHasherFactory PasswordHasherFactory { get; set; }

        /// <summary>
        /// Фабрика для получения валидаторов пользователей <see cref="IUserValidator{TUser}"/>.
        /// </summary>
        public IUserValidatorsFactory UserValidatorsFactory { get; set; }

        /// <summary>
        /// Фабрика для получения валидаторов паролей <see cref="IPasswordValidator{TUser}"/>.
        /// </summary>
        public IPasswordValidatorsFactory PasswordValidatorsFactory { get; set; }

        /// <summary>
        /// Фабрика для получения нормализатора ключей <see cref="ILookupNormalizer"/>.
        /// </summary>
        public ILookupNormalizerFactory LookupNormalizerFactory { get; set; }

        /// <summary>
        /// Фабрика для получения сервиса локализации ошибок аутентификации/авторизации <see cref="Microsoft.AspNetCore.Identity.IdentityErrorDescriber"/>.
        /// </summary>
        public IIdentityErrorDescriberFactory IdentityErrorDescriber { get; set; }
    }
}