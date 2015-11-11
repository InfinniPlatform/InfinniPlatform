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
		/// <returns>Результат добавления.</returns>
		void CreateUser(string userName, string password);

		/// <summary>
		/// Удаляет пользователя.
		/// </summary>
		/// <param name="userName">Логин пользователя.</param>
		/// <returns>Результат удаления пользователя.</returns>
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
		/// Обновляет штамп изменения сведений пользователя.
		/// </summary>
		void UpdateSecurityStamp();


		/// <summary>
		/// Возвращает электронную почту пользователя.
		/// </summary>
		string GetEmail();

		/// <summary>
		/// Устанавливает электронную почту пользователя.
		/// </summary>
		void SetEmail(string email);

		/// <summary>
		/// Проверяет, что электронная почта пользователя подтверждена.
		/// </summary>
		bool IsEmailConfirmed();


		/// <summary>
		/// Возвращает номер телефона пользователя.
		/// </summary>
		string GetPhoneNumber();

		/// <summary>
		/// Устанавливает номер телефона пользователя.
		/// </summary>
		void SetPhoneNumber(string phoneNumber);

		/// <summary>
		/// Проверяет, что номер телефона пользователя подтвержден.
		/// </summary>
		bool IsPhoneNumberConfirmed();


		/// <summary>
		/// Добавляет имя входа пользователя для внешнего провайдера.
		/// </summary>
		void AddLogin(string loginProvider, string providerKey);

		/// <summary>
		/// Удаляет имя входа пользователя для внешнего провайдера.
		/// </summary>
		void RemoveLogin(string loginProvider, string providerKey);


		/// <summary>
		/// Добавляет утверждение пользователя.
		/// </summary>
		void AddClaim(string claimType, string claimValue);

		/// <summary>
		/// Удаляет утверждение пользователя.
		/// </summary>
		void RemoveClaim(string claimType, string claimValue);


		/// <summary>
		/// Проверяет, что пользователь входит в роль.
		/// </summary>
		bool IsInRole(string role);

		/// <summary>
		/// Добавляет пользователя в роль.
		/// </summary>
		void AddToRole(string role);

		/// <summary>
		/// Добавляет пользователя в роли.
		/// </summary>
		void AddToRoles(params string[] roles);

		/// <summary>
		/// Добавляет пользователя в роли.
		/// </summary>
		void AddToRoles(IEnumerable<string> roles);

		/// <summary>
		/// Удаляет пользователя из роли.
		/// </summary>
		void RemoveFromRole(string role);

		/// <summary>
		/// Удаляет пользователя из ролей.
		/// </summary>
		void RemoveFromRoles(params string[] roles);

		/// <summary>
		/// Удаляет пользователя из ролей.
		/// </summary>
		void RemoveFromRoles(IEnumerable<string> roles);

		/// <summary>
		/// Устанавливает утверждение пользователя (заменяет все утверждения данного типа).
		/// </summary>
		/// <param name="claimType">Тип утверждения.</param>
		/// <param name="claimValue">Значение утверждения.</param>
		void SetClaim(string claimType, string claimValue);
	}
}