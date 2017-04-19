using InfinniPlatform.Auth.Identity.MongoDb;

namespace InfinniPlatform.Auth.UserStorage
{
    /// <summary>
    ///     Хранилище сведений о пользователях системы <see cref="IdentityUser" />.
    /// </summary>
    internal interface IAppUserStore
    {
        /// <summary>
        ///     Создает сведения о пользователе системы.
        /// </summary>
        /// <param name="user">Сведения о пользователе системы.</param>
        void CreateUser(IdentityUser user);

        /// <summary>
        ///     Обновляет сведения о пользователе системы.
        /// </summary>
        /// <param name="user">Сведения о пользователе системы.</param>
        void UpdateUser(IdentityUser user);

        /// <summary>
        ///     Удаляет сведения о пользователе системы.
        /// </summary>
        /// <param name="user">Сведения о пользователе системы.</param>
        void DeleteUser(IdentityUser user);

        /// <summary>
        ///     Возвращает сведения о пользователе системы по его идентификатору.
        /// </summary>
        /// <param name="userId">Уникальный идентификатор пользователя.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        IdentityUser FindUserById(string userId);

        /// <summary>
        ///     Возвращает сведения о пользователе системы по его имени.
        /// </summary>
        /// <param name="name">Уникальное имя пользователя.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        IdentityUser FindUserByName(string name);

        /// <summary>
        ///     Возвращает сведения о пользователе системы по его имени.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        IdentityUser FindUserByUserName(string userName);

        /// <summary>
        ///     Возвращает сведения о пользователе системы по его электронной почте.
        /// </summary>
        /// <param name="email">Электронная почта пользователя.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        IdentityUser FindUserByEmail(string email);

        /// <summary>
        ///     Возвращает сведения о пользователе системы по его номеру телефона.
        /// </summary>
        /// <param name="phoneNumber">Номер телефона пользователя.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        IdentityUser FindUserByPhoneNumber(string phoneNumber);

        /// <summary>
        ///     Возвращает сведения о пользователе системы по его имени у внешнего провайдера.
        /// </summary>
        /// <param name="userLogin">Имя входа пользователя системы у внешнего провайдера.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        IdentityUser FindUserByLogin(IdentityUserLogin userLogin);

        /// <summary>
        ///     Добавляет пользователя в указанную роль.
        /// </summary>
        /// <param name="user">Сведения о пользователе системы.</param>
        /// <param name="roleName">Наименование системной роли.</param>
        void AddUserToRole(IdentityUser user, string roleName);

        /// <summary>
        ///     Удаляет пользователя из указанной роли.
        /// </summary>
        /// <param name="user">Сведения о пользователе системы.</param>
        /// <param name="roleName">Наименование системной роли.</param>
        void RemoveUserFromRole(IdentityUser user, string roleName);

        /// <summary>
        ///     Добавляет пользователю утверждение.
        /// </summary>
        /// <param name="user">Сведения о пользователе системы.</param>
        /// <param name="claimType">Уникальный идентификатор типа утверждения.</param>
        /// <param name="claimValue">Значение утверждения заданного типа.</param>
        void AddUserClaim(IdentityUser user, string claimType, string claimValue);

        /// <summary>
        ///     Удаляет у пользователя утверждение.
        /// </summary>
        /// <param name="user">Сведения о пользователе системы.</param>
        /// <param name="claimType">Уникальный идентификатор типа утверждения.</param>
        /// <param name="claimValue">Значение утверждения заданного типа.</param>
        void RemoveUserClaim(IdentityUser user, string claimType, string claimValue);

        /// <summary>
        ///     Добавляет пользователю имя входа у внешнего провайдера.
        /// </summary>
        /// <param name="user">Сведения о пользователе системы.</param>
        /// <param name="userLogin">Имя входа пользователя системы у внешнего провайдера.</param>
        void AddUserLogin(IdentityUser user, IdentityUserLogin userLogin);

        /// <summary>
        ///     Удаляет у пользователя имя входа у внешнего провайдера.
        /// </summary>
        /// <param name="user">Сведения о пользователе системы.</param>
        /// <param name="userLogin">Имя входа пользователя системы у внешнего провайдера.</param>
        void RemoveUserLogin(IdentityUser user, IdentityUserLogin userLogin);
    }
}