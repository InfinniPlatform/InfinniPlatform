using System.Security.Claims;

namespace InfinniPlatform.Api.Security
{
	/// <summary>
	/// Сведения о типе утверждения.
	/// </summary>
	public sealed class ApplicationClaimType
	{
		/// <summary>
		/// Уникальный идентификатор типа утверждения.
		/// </summary>
		/// <remarks>
		/// Идентификатор утверждения должен совпадать со значениями из <see cref="ClaimTypes"/> или <see cref="ApplicationClaimTypes"/>.
		/// </remarks>
		/// <example>
		/// http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone
		/// </example>
		public string Id { get; set; }

		/// <summary>
		/// Наименование типа утверждения.
		/// </summary>
		/// <remarks>
		/// Идентификатор утверждения должно совпадать с наименованиями из <see cref="ClaimTypes"/> или <see cref="ApplicationClaimTypes"/>.
		/// </remarks>
		/// <example>
		/// MobilePhone
		/// </example>
		public string Name { get; set; }

		/// <summary>
		/// Заголовок типа утверждения.
		/// </summary>
		/// <remarks>
		/// Отображаемое наименование утверждения.
		/// </remarks>
		/// <example>
		/// Мобильный телефон
		/// </example>
		public string Caption { get; set; }

		/// <summary>
		/// Описание типа утверждения.
		/// </summary>
		/// <remarks>
		/// Описание назначения утверждения.
		/// </remarks>
		/// <example>
		/// Обычно используется для СМС оповещений
		/// </example>
		public string Description { get; set; }
	}
}