using System.Collections.Generic;

namespace InfinniPlatform.Api.Security
{
	/// <summary>
	/// Сведения о пользователе системы.
	/// </summary>
	public class ApplicationUser
	{
		public ApplicationUser()
		{
			Roles = new List<ForeignKey>();
			Logins = new List<ApplicationUserLogin>();
			Claims = new List<ApplicationUserClaim>();
		}

		/// <summary>
		/// Уникальный идентификатор пользователя.
		/// </summary>
		/// <example>
		/// 55088CAE-6F34-457E-AC2A-2FAE316C4D0C
		/// </example>
		public string Id { get; set; }

		/// <summary>
		/// Уникальное имя пользователя.
		/// </summary>
		/// <example>
		/// User1
		/// </example>
		public string UserName { get; set; }

		/// <summary>
		/// Электронная почта пользователя.
		/// </summary>
		/// <example>
		/// user1@gmail.com
		/// </example>
		public string Email { get; set; }

		/// <summary>
		/// Электронная почта пользователя подтверждена.
		/// </summary>
		/// <example>
		/// true
		/// </example>
		public bool EmailConfirmed { get; set; }

		/// <summary>
		/// Номер телефона пользователя.
		/// </summary>
		/// <example>
		/// +7 (123) 456-78-90
		/// </example>
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Номер телефона пользователя подтвержден.
		/// </summary>
		/// <example>
		/// true
		/// </example>
		public bool PhoneNumberConfirmed { get; set; }

		/// <summary>
		/// Отображаемое имя пользователя.
		/// </summary>
		/// <example>
		/// Иванов И.И.
		/// </example>
		public string DisplayName { get; set; }

		/// <summary>
		/// Описание пользователя.
		/// </summary>
		/// <remarks>
		/// Троль 80-го уровня
		/// </remarks>
		public string Description { get; set; }

		/// <summary>
		/// Хэш пароля пользователя.
		/// </summary>
		/// <remarks>
		/// Пароли пользователей не хранятся в явном виде.
		/// </remarks>
		/// <example>
		/// ANggHNufJKJth4FV6jQ9xmYhx04/RgkWDn4PKveYziI38B4MmkZ8NZwlwUHIcrX1Tg==
		/// </example>
		public string PasswordHash { get; set; }

		/// <summary>
		/// Штамп изменения сведений пользователя.
		/// </summary>
		/// <remarks>
		/// Должен быть уникальным и изменяется при изменении сведений о пользователе.
		/// </remarks>
		/// <example>
		/// 3f7cc31d-b1dc-43a2-a4eb-8e6c6257366d
		/// </example>
		public string SecurityStamp { get; set; }

		/// <summary>
		/// Роль пользователя по умолчанию.
		/// </summary>
		/// <remarks>
        ///     Пользователь должен входить в данную роль (<see cref="Roles" />).
		/// Ссылка на роль, в которой пользователь работает большую часть своего времени.
		/// </remarks>
		/// <example>
		/// { Id: 'ProjectManager', DisplayName: 'Менеджер проекта' }
		/// </example>
		public ForeignKey DefaultRole { get; set; }

		/// <summary>
		/// Список ролей пользователя.
		/// </summary>
		/// <remarks>
		/// Список ссылок на все роли, в которые входит пользователь системы.
		/// </remarks>
		public IEnumerable<ForeignKey> Roles { get; set; }

		/// <summary>
		/// Список утверждений пользователя.
		/// </summary>
		/// <remarks>
		/// Дополнительные сведения о пользователе в виде типизированного списка утверждений.
		/// </remarks>
		public IEnumerable<ApplicationUserClaim> Claims { get; set; }

		/// <summary>
		/// Список имен входа пользователя у внешних провайдеров.
		/// </summary>
		/// <remarks>
		/// Заполняется в случае, если пользователь входит в систему через внешние провайдеры.
		/// </remarks>
		public IEnumerable<ApplicationUserLogin> Logins { get; set; }
	}
}