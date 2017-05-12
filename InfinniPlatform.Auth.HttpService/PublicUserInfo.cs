using System.Collections.Generic;
using InfinniPlatform.Auth.Identity;

namespace InfinniPlatform.Auth.HttpService
{
    /// <summary>
    /// Информация о пользователе, доступная через <see cref="AuthInternalHttpService{TUser}" />.
    /// </summary>
    internal class PublicUserInfo
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="displayName">Отображаемое имя пользователя.</param>
        /// <param name="description">Описание.</param>
        /// <param name="roles">Роли пользователя.</param>
        /// <param name="logins">Учетные записи пользователя.</param>
        /// <param name="claims">Утверждения пользователя.</param>
        public PublicUserInfo(string userName,
                              string displayName,
                              string description,
                              IEnumerable<string> roles,
                              IEnumerable<AppUserLogin> logins,
                              List<AppUserClaim> claims)
        {
            UserName = userName;
            DisplayName = displayName;
            Description = description;
            Roles = roles;
            Logins = logins;
            Claims = claims;
        }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public IEnumerable<AppUserLogin> Logins { get; set; }

        public List<AppUserClaim> Claims { get; set; }
    }
}