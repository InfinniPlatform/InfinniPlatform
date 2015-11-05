using System.Collections.Generic;

namespace InfinniPlatform.Sdk.ContextComponents
{
	/// <summary>
	/// Предоставляет методы управления информацией текущего пользователя.
	/// </summary>
	public interface IApplicationUserManager
	{
		/// <summary>
		/// Возвращает сведения о пользователе.
		/// </summary>
		object GetCurrentUser();


		/// <summary>
		/// Проверяет, что пользователь имеет пароль.
		/// </summary>
		bool HasPassword();
		bool HasPassword(string userName);

		/// <summary>
		/// Добавляет пользователю пароль.
		/// </summary>
		void AddPassword(string password);
		void AddPassword(string userName, string password);

		/// <summary>
		/// Удаляет у пользователя пароль.
		/// </summary>
		void RemovePassword();
		void RemovePassword(string userName);

		/// <summary>
		/// Изменяет пользователю пароль.
		/// </summary>
		void ChangePassword(string currentPassword, string newPassword);
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
	}
}