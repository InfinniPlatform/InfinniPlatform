using System.Collections.Generic;

namespace InfinniPlatform.Sdk.ContextComponents
{
    /// <summary>
    /// Предоставляет методы управления информацией текущего пользователя.
    /// </summary>
    public interface IApplicationUserManager
    {
        /// <summary>
        /// Возвращает сведения о текущем пользователе.
        /// </summary>
        object GetCurrentUser();

        /// <summary>
        /// Возвращает пользователя системы.
        /// </summary>
        /// <param name="userName">Логин пользователя.</param>
        /// <returns>Пользователь системы.</returns>
        object FindUserByName(string userName);


        /// <summary>
        /// Создает нового пользователя системы.
        /// </summary>
        /// <param name="userName">Пользователь системы.</param>
        /// <param name="password">Пароль пользователя.</param>
        void CreateUser(string userName, string password);

        /// <summary>
        /// Удаляет пользователя.
        /// </summary>
        /// <param name="userName">Логин пользователя.</param>
        void DeleteUser(string userName);


        /// <summary>
        /// Проверяет, что пользователь имеет пароль.
        /// </summary>
        bool HasPassword();

        /// <summary>
        /// Проверяет, что пользователь имеет пароль.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        bool HasPassword(string userName);


        /// <summary>
        /// Добавляет пользователю пароль.
        /// </summary>
        /// <param name="password">Пароль пользователя.</param>
        void AddPassword(string password);

        /// <summary>
        /// Добавляет пользователю пароль.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        void AddPassword(string userName, string password);


        /// <summary>
        /// Удаляет у пользователя пароль.
        /// </summary>
        void RemovePassword();

        /// <summary>
        /// Удаляет у пользователя пароль.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        void RemovePassword(string userName);


        /// <summary>
        /// Изменяет пользователю пароль.
        /// </summary>
        void ChangePassword(string currentPassword, string newPassword);

        /// <summary>
        /// Изменяет пользователю пароль.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="currentPassword">Текущий пароль пользователя.</param>
        /// <param name="newPassword">Новый пароль пользователя.</param>
        void ChangePassword(string userName, string currentPassword, string newPassword);


        /// <summary>
        /// Возвращает штамп изменения сведений пользователя.
        /// </summary>
        string GetSecurityStamp();

        /// <summary>
        /// Возвращает штамп изменения сведений пользователя.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        string GetSecurityStamp(string userName);


        /// <summary>
        /// Обновляет штамп изменения сведений пользователя.
        /// </summary>
        void UpdateSecurityStamp();

        /// <summary>
        /// Обновляет штамп изменения сведений пользователя.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        void UpdateSecurityStamp(string userName);


        /// <summary>
        /// Возвращает электронную почту пользователя.
        /// </summary>
        string GetEmail();

        /// <summary>
        /// Возвращает электронную почту пользователя.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        string GetEmail(string userName);


        /// <summary>
        /// Устанавливает электронную почту пользователя.
        /// </summary>
        /// <param name="email">Электронная почта пользователя.</param>
        void SetEmail(string email);

        /// <summary>
        /// Устанавливает электронную почту пользователя.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="email">Электронная почта пользователя.</param>
        void SetEmail(string userName, string email);


        /// <summary>
        /// Проверяет, что электронная почта пользователя подтверждена.
        /// </summary>
        bool IsEmailConfirmed();

        /// <summary>
        /// Проверяет, что электронная почта пользователя подтверждена.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        bool IsEmailConfirmed(string userName);


        /// <summary>
        /// Возвращает номер телефона пользователя.
        /// </summary>
        string GetPhoneNumber();

        /// <summary>
        /// Возвращает номер телефона пользователя.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        string GetPhoneNumber(string userName);


        /// <summary>
        /// Устанавливает номер телефона пользователя.
        /// </summary>
        /// <param name="phoneNumber">Номер телефона пользователя.</param>
        void SetPhoneNumber(string phoneNumber);

        /// <summary>
        /// Устанавливает номер телефона пользователя.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="phoneNumber">Номер телефона пользователя.</param>
        void SetPhoneNumber(string userName, string phoneNumber);


        /// <summary>
        /// Проверяет, что номер телефона пользователя подтвержден.
        /// </summary>
        bool IsPhoneNumberConfirmed();

        /// <summary>
        /// Проверяет, что номер телефона пользователя подтвержден.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        bool IsPhoneNumberConfirmed(string userName);


        /// <summary>
        /// Добавляет имя входа пользователя для внешнего провайдера.
        /// </summary>
        /// <param name="loginProvider">Провайдер входа в систему.</param>
        /// <param name="providerKey">Ключ, представляющий имя входа для провайдера.</param>
        void AddLogin(string loginProvider, string providerKey);

        /// <summary>
        /// Добавляет имя входа пользователя для внешнего провайдера.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="loginProvider">Провайдер входа в систему.</param>
        /// <param name="providerKey">Ключ, представляющий имя входа для провайдера.</param>
        void AddLogin(string userName, string loginProvider, string providerKey);


        /// <summary>
        /// Удаляет имя входа пользователя для внешнего провайдера.
        /// </summary>
        /// <param name="loginProvider">Провайдер входа в систему.</param>
        /// <param name="providerKey">Ключ, представляющий имя входа для провайдера.</param>
        void RemoveLogin(string loginProvider, string providerKey);

        /// <summary>
        /// Удаляет имя входа пользователя для внешнего провайдера.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="loginProvider">Провайдер входа в систему.</param>
        /// <param name="providerKey">Ключ, представляющий имя входа для провайдера.</param>
        void RemoveLogin(string userName, string loginProvider, string providerKey);


        /// <summary>
        /// Добавляет утверждение пользователя.
        /// </summary>
        /// <param name="claimType">Тип утверждения.</param>
        /// <param name="claimValue">Значение утверждения.</param>
        void AddClaim(string claimType, string claimValue);

        /// <summary>
        /// Добавляет утверждение пользователя.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="claimType">Тип утверждения.</param>
        /// <param name="claimValue">Значение утверждения.</param>
        void AddClaim(string userName, string claimType, string claimValue);


        /// <summary>
        /// Заменяет все утверждения данного типа.
        /// </summary>
        /// <param name="claimType">Тип утверждения.</param>
        /// <param name="claimValue">Значение утверждения.</param>
        void SetClaim(string claimType, string claimValue);

        /// <summary>
        /// Заменяет все утверждения данного типа.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="claimType">Тип утверждения.</param>
        /// <param name="claimValue">Значение утверждения.</param>
        void SetClaim(string userName, string claimType, string claimValue);


        /// <summary>
        /// Удаляет утверждение пользователя.
        /// </summary>
        /// <param name="claimType">Тип утверждения.</param>
        /// <param name="claimValue">Значение утверждения.</param>
        void RemoveClaim(string claimType, string claimValue);

        /// <summary>
        /// Удаляет утверждение пользователя.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="claimType">Тип утверждения.</param>
        /// <param name="claimValue">Значение утверждения.</param>
        void RemoveClaim(string userName, string claimType, string claimValue);


        /// <summary>
        /// Проверяет, что пользователь входит в роль.
        /// </summary>
        /// <param name="role">Имя роли.</param>
        bool IsInRole(string role);

        /// <summary>
        /// Проверяет, что пользователь входит в роль.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="role">Имя роли.</param>
        bool IsInRole(string userName, string role);


        /// <summary>
        /// Добавляет пользователя в роль.
        /// </summary>
        /// <param name="role">Имя роли.</param>
        void AddToRole(string role);

        /// <summary>
        /// Добавляет пользователя в роль.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="role">Имя роли.</param>
        void AddToRole(string userName, string role);

        /// <summary>
        /// Добавляет пользователя в роли.
        /// </summary>
        /// <param name="roles">Список ролей.</param>
        void AddToRoles(params string[] roles);

        /// <summary>
        /// Добавляет пользователя в роли.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="roles">Список ролей.</param>
        void AddToRoles(string userName, params string[] roles);

        /// <summary>
        /// Добавляет пользователя в роли.
        /// </summary>
        /// <param name="roles">Список ролей.</param>
        void AddToRoles(IEnumerable<string> roles);

        /// <summary>
        /// Добавляет пользователя в роли.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="roles">Список ролей.</param>
        void AddToRoles(string userName, IEnumerable<string> roles);


        /// <summary>
        /// Удаляет пользователя из роли.
        /// </summary>
        /// <param name="role">Имя роли.</param>
        void RemoveFromRole(string role);

        /// <summary>
        /// Удаляет пользователя из роли.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="role">Имя роли.</param>
        void RemoveFromRole(string userName, string role);

        /// <summary>
        /// Удаляет пользователя из ролей.
        /// </summary>
        void RemoveFromRoles(params string[] roles);

        /// <summary>
        /// Удаляет пользователя из ролей.
        /// </summary>
        void RemoveFromRoles(IEnumerable<string> roles);
    }
}