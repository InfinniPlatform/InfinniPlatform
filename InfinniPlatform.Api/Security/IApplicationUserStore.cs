using System;

namespace InfinniPlatform.Api.Security
{
	/// <summary>
	/// Хранилище сведений о пользователях системы <see cref="ApplicationUser"/>.
	/// </summary>
	public interface IApplicationUserStore : IDisposable
	{
		/// <summary>
		/// Создает сведения о пользователе системы.
		/// </summary>
		/// <param name="user">Сведения о пользователе системы.</param>
		void CreateUser(ApplicationUser user);

		/// <summary>
		/// Обновляет сведения о пользователе системы.
		/// </summary>
		/// <param name="user">Сведения о пользователе системы.</param>
		void UpdateUser(ApplicationUser user);

		/// <summary>
		/// Удаляет сведения о пользователе системы.
		/// </summary>
		/// <param name="user">Сведения о пользователе системы.</param>
		void DeleteUser(ApplicationUser user);

		/// <summary>
		/// Возвращает сведения о пользователе системы по его идентификатору.
		/// </summary>
		/// <param name="userId">Уникальный идентификатор пользователя.</param>
		/// <returns>Сведения о пользователе системы.</returns>
		ApplicationUser FindUserById(string userId);

		/// <summary>
		/// Возвращает сведения о пользователе системы по его имени.
		/// </summary>
		/// <param name="name">Уникальное имя пользователя.</param>
		/// <returns>Сведения о пользователе системы.</returns>
		ApplicationUser FindUserByName(string name);

		/// <summary>
		/// Возвращает сведения о пользователе системы по его имени.
		/// </summary>
		/// <param name="userName">Имя пользователя.</param>
		/// <returns>Сведения о пользователе системы.</returns>
		ApplicationUser FindUserByUserName(string userName);

		/// <summary>
		/// Возвращает сведения о пользователе системы по его электронной почте.
		/// </summary>
		/// <param name="email">Электронная почта пользователя.</param>
		/// <returns>Сведения о пользователе системы.</returns>
		ApplicationUser FindUserByEmail(string email);

		/// <summary>
		/// Возвращает сведения о пользователе системы по его номеру телефона.
		/// </summary>
		/// <param name="phoneNumber">Номер телефона пользователя.</param>
		/// <returns>Сведения о пользователе системы.</returns>
		ApplicationUser FindUserByPhoneNumber(string phoneNumber);

		/// <summary>
		/// Возвращает сведения о пользователе системы по его имени у внешнего провайдера.
		/// </summary>
		/// <param name="userLogin">Имя входа пользователя системы у внешнего провайдера.</param>
		/// <returns>Сведения о пользователе системы.</returns>
		ApplicationUser FindUserByLogin(ApplicationUserLogin userLogin);

		/// <summary>
		/// Добавляет пользователя в указанную роль.
		/// </summary>
		/// <param name="user">Сведения о пользователе системы.</param>
		/// <param name="roleName">Наименование системной роли.</param>
		void AddUserToRole(ApplicationUser user, string roleName);

		/// <summary>
		/// Удаляет пользователя из указанной роли.
		/// </summary>
		/// <param name="user">Сведения о пользователе системы.</param>
		/// <param name="roleName">Наименование системной роли.</param>
		void RemoveUserFromRole(ApplicationUser user, string roleName);

		/// <summary>
		/// Добавляет пользователю утверждение.
		/// </summary>
		/// <param name="user">Сведения о пользователе системы.</param>
		/// <param name="claimType">Уникальный идентификатор типа утверждения.</param>
		/// <param name="claimValue">Значение утверждения заданного типа.</param>
		/// <param name="overwrite">Признак необходимости перезаписать значение имеющегося утверждения.</param>
		void AddUserClaim(ApplicationUser user, string claimType, string claimValue, bool overwrite = true);

		/// <summary>
		/// Удаляет у пользователя утверждение.
		/// </summary>
		/// <param name="user">Сведения о пользователе системы.</param>
		/// <param name="claimType">Уникальный идентификатор типа утверждения.</param>
		void RemoveUserClaim(ApplicationUser user, string claimType);

		/// <summary>
		/// Добавляет пользователю имя входа у внешнего провайдера.
		/// </summary>
		/// <param name="user">Сведения о пользователе системы.</param>
		/// <param name="userLogin">Имя входа пользователя системы у внешнего провайдера.</param>
		void AddUserLogin(ApplicationUser user, ApplicationUserLogin userLogin);

		/// <summary>
		/// Удаляет у пользователеля имя входа у внешнего провайдера.
		/// </summary>
		/// <param name="user">Сведения о пользователе системы.</param>
		/// <param name="userLogin">Имя входа пользователя системы у внешнего провайдера.</param>
		void RemoveUserLogin(ApplicationUser user, ApplicationUserLogin userLogin);
	}
}