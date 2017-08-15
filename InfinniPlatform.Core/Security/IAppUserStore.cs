using System.Threading.Tasks;

using InfinniPlatform.Sdk.Security;

namespace InfinniPlatform.Core.Security
{
    /// <summary>
    /// Хранилище сведений о пользователях системы <see cref="ApplicationUser" />.
    /// </summary>
    public interface IAppUserStore
    {
        /// <summary>
        /// Создает сведения о пользователе системы.
        /// </summary>
        /// <param name="user">Сведения о пользователе системы.</param>
        Task CreateUserAsync(ApplicationUser user);

        /// <summary>
        /// Обновляет сведения о пользователе системы.
        /// </summary>
        /// <param name="user">Сведения о пользователе системы.</param>
        Task UpdateUserAsync(ApplicationUser user);

        /// <summary>
        /// Удаляет сведения о пользователе системы.
        /// </summary>
        /// <param name="user">Сведения о пользователе системы.</param>
        Task DeleteUserAsync(ApplicationUser user);

        /// <summary>
        /// Возвращает сведения о пользователе системы по его идентификатору.
        /// </summary>
        /// <param name="userId">Уникальный идентификатор пользователя.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        Task<ApplicationUser> FindUserByIdAsync(string userId);

        /// <summary>
        /// Возвращает сведения о пользователе системы по его имени.
        /// </summary>
        /// <param name="name">Уникальное имя пользователя.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        Task<ApplicationUser> FindUserByNameAsync(string name);

        /// <summary>
        /// Возвращает сведения о пользователе системы по его имени.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        Task<ApplicationUser> FindUserByUserNameAsync(string userName);

        /// <summary>
        /// Возвращает сведения о пользователе системы по его электронной почте.
        /// </summary>
        /// <param name="email">Электронная почта пользователя.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        Task<ApplicationUser> FindUserByEmailAsync(string email);

        /// <summary>
        /// Возвращает сведения о пользователе системы по его номеру телефона.
        /// </summary>
        /// <param name="phoneNumber">Номер телефона пользователя.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        Task<ApplicationUser> FindUserByPhoneNumberAsync(string phoneNumber);

        /// <summary>
        /// Возвращает сведения о пользователе системы по его имени у внешнего провайдера.
        /// </summary>
        /// <param name="userLogin">Имя входа пользователя системы у внешнего провайдера.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        Task<ApplicationUser> FindUserByLoginAsync(ApplicationUserLogin userLogin);

        /// <summary>
        /// Добавляет пользователя в указанную роль.
        /// </summary>
        /// <param name="user">Сведения о пользователе системы.</param>
        /// <param name="roleName">Наименование системной роли.</param>
        Task AddUserToRoleAsync(ApplicationUser user, string roleName);

        /// <summary>
        /// Удаляет пользователя из указанной роли.
        /// </summary>
        /// <param name="user">Сведения о пользователе системы.</param>
        /// <param name="roleName">Наименование системной роли.</param>
        Task RemoveUserFromRoleAsync(ApplicationUser user, string roleName);

        /// <summary>
        /// Добавляет пользователю утверждение.
        /// </summary>
        /// <param name="user">Сведения о пользователе системы.</param>
        /// <param name="claimType">Уникальный идентификатор типа утверждения.</param>
        /// <param name="claimValue">Значение утверждения заданного типа.</param>
        Task AddUserClaimAsync(ApplicationUser user, string claimType, string claimValue);

        /// <summary>
        /// Удаляет у пользователя утверждение.
        /// </summary>
        /// <param name="user">Сведения о пользователе системы.</param>
        /// <param name="claimType">Уникальный идентификатор типа утверждения.</param>
        /// <param name="claimValue">Значение утверждения заданного типа.</param>
        Task RemoveUserClaimAsync(ApplicationUser user, string claimType, string claimValue);

        /// <summary>
        /// Добавляет пользователю имя входа у внешнего провайдера.
        /// </summary>
        /// <param name="user">Сведения о пользователе системы.</param>
        /// <param name="userLogin">Имя входа пользователя системы у внешнего провайдера.</param>
        Task AddUserLoginAsync(ApplicationUser user, ApplicationUserLogin userLogin);

        /// <summary>
        /// Удаляет у пользователя имя входа у внешнего провайдера.
        /// </summary>
        /// <param name="user">Сведения о пользователе системы.</param>
        /// <param name="userLogin">Имя входа пользователя системы у внешнего провайдера.</param>
        Task RemoveUserLoginAsync(ApplicationUser user, ApplicationUserLogin userLogin);
    }
}