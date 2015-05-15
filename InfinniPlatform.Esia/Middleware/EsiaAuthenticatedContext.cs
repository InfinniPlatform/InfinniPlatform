using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;

namespace InfinniPlatform.Esia.Middleware
{
	/// <summary>
	/// Контекст для обработки события успешной аутентификации пользователя.
	/// </summary>
	public sealed class EsiaAuthenticatedContext : BaseContext
	{
		public EsiaAuthenticatedContext(IOwinContext context, ICollection<KeyValuePair<string, string>> userInfo)
			: base(context)
		{
			Id = GetSimpleValue(userInfo, "personSNILS");
			Snils = GetSimpleValue(userInfo, "personSNILS");
			Email = GetSimpleValue(userInfo, "personEmail");

			FirstName = GetSimpleValue(userInfo, "firstName");
			MiddleName = GetSimpleValue(userInfo, "middleName");
			LastName = GetSimpleValue(userInfo, "lastName");

			RegionCode = GetSimpleValue(userInfo, "SubjectCode");
			RegionName = GetSimpleValue(userInfo, "SubjectName");

			RoleCodes = GetComplexValues(userInfo, "RoleCode");
			RoleNames = GetComplexValues(userInfo, "RoleName");

			SystemCodes = GetComplexValues(userInfo, "misEntityIdCode");
			SystemNames = GetComplexValues(userInfo, "misEntityIdName");

			OrganizationCodes = GetComplexValues(userInfo, "MedicalOrganizationsCode");
			OrganizationNames = GetComplexValues(userInfo, "MedicalOrganizationsName");
		}


		/// <summary>
		/// Учетные данные пользователя.
		/// </summary>
		public ClaimsIdentity Identity { get; set; }

		/// <summary>
		/// Информация о сессии пользователя.
		/// </summary>
		public AuthenticationProperties Properties { get; set; }


		/// <summary>
		/// Уникальный идентификатор пользователя в ЕСИА.
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// Электронная почта пользоватля.
		/// </summary>
		public string Email { get; private set; }

		/// <summary>
		/// СНИЛС пользователя.
		/// </summary>
		public string Snils { get; private set; }


		/// <summary>
		/// Имя пользователя.
		/// </summary>
		public string FirstName { get; private set; }

		/// <summary>
		/// Отчество пользователя.
		/// </summary>
		public string MiddleName { get; private set; }

		/// <summary>
		/// Фамилия пользователя.
		/// </summary>
		public string LastName { get; private set; }


		/// <summary>
		/// Код региона пользователя.
		/// </summary>
		public string RegionCode { get; private set; }

		/// <summary>
		/// Наименование региона пользователя.
		/// </summary>
		public string RegionName { get; private set; }


		/// <summary>
		/// Коды ролей пользователя в ЕСИА.
		/// </summary>
		public IEnumerable<string> RoleCodes { get; private set; }

		/// <summary>
		/// Наименования ролей пользователя в ЕСИА.
		/// </summary>
		public IEnumerable<string> RoleNames { get; private set; }


		/// <summary>
		/// Коды систем пользователя в ЕСИА.
		/// </summary>
		public IEnumerable<string> SystemCodes { get; private set; }

		/// <summary>
		/// Наименования систем пользователя в ЕСИА.
		/// </summary>
		public IEnumerable<string> SystemNames { get; private set; }


		/// <summary>
		/// Коды организаций пользователя в ЕСИА.
		/// </summary>
		public IEnumerable<string> OrganizationCodes { get; private set; }

		/// <summary>
		/// Наименования организаций пользователя в ЕСИА.
		/// </summary>
		public IEnumerable<string> OrganizationNames { get; private set; }


		private static string GetSimpleValue(IEnumerable<KeyValuePair<string, string>> userInfo, string propertyName)
		{
			return userInfo.FirstOrDefault(i => string.Equals(i.Key, propertyName, StringComparison.OrdinalIgnoreCase)).Value;
		}

		private static IEnumerable<string> GetComplexValues(IEnumerable<KeyValuePair<string, string>> userInfo, string propertyName)
		{
			return userInfo.Where(i => string.Equals(i.Key, propertyName, StringComparison.OrdinalIgnoreCase)).Select(i => i.Value);
		}
	}
}