using InfinniPlatform.Auth.Identity;
using InfinniPlatform.Auth.Identity.DocumentStorage;
using InfinniPlatform.Auth.Identity.UserCache;
using InfinniPlatform.DocumentStorage;
using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// Настройки внутреннего провайдера аутентификации.
    /// </summary>
    public class AuthInternalOptions
    {
        public const string SectionName = "authInternal";

        public const int DefaultUserCacheTimeout = 30;

        public const string DefaultLoginPath = "/";

        public const string DefaultLogoutPath = "/";

        public static readonly AuthInternalOptions Default = new AuthInternalOptions();


        public AuthInternalOptions()
        {
            UserCacheTimeout = DefaultUserCacheTimeout;
            LoginPath = DefaultLoginPath;
            LogoutPath = DefaultLogoutPath;
            UserStoreFactory = new UserStoreFactory();
        }


        /// <summary>
        /// Таймаут сброса кэша пользователей в минутах.
        /// </summary>
        public int UserCacheTimeout { get; set; }

        /// <summary>
        /// Путь для перенаправления при возврате 401 Unauthorized.
        /// </summary>
        public string LoginPath { get; set; }

        /// <summary>
        /// Путь для перенаправления при выходе из системы.
        /// </summary>
        public string LogoutPath { get; set; }

        /// <summary>
        /// Фабрика для получения хранилища пользователей.
        /// </summary>
        public UserStoreFactory UserStoreFactory { get; set; }
    }

    /// <summary>
    /// Фабрика для получения хранилища пользователей.
    /// </summary>
    public class UserStoreFactory
    {
        public virtual IUserStore<TUser> GetUserStore<TUser>(IContainerResolver resolver) where TUser : AppUser
        {
            return new UserStore<TUser>(resolver.Resolve<ISystemDocumentStorageFactory>(), resolver.Resolve<UserCache<AppUser>>());
        }
    }
}