namespace InfinniPlatform.Esia.Middleware
{
	/// <summary>
	/// Типы утверждений для пользователей Единой системы идентификации и аутентификации (ЕСИА).
	/// </summary>
	public static class EsiaClaimTypes
	{
		private const string ClaimTypeNamespace = "urn:esia:claims:";


		/// <summary>
		/// СНИЛС пользователя.
		/// </summary>
		public const string Snils = ClaimTypeNamespace + "snils";


		/// <summary>
		/// Имя пользователя.
		/// </summary>
		public const string FirstName = ClaimTypeNamespace + "firstname";

		/// <summary>
		/// Отчество пользователя.
		/// </summary>
		public const string MiddleName = ClaimTypeNamespace + "middlename";

		/// <summary>
		/// Фамилия пользователя.
		/// </summary>
		public const string LastName = ClaimTypeNamespace + "lastname";


		/// <summary>
		/// Код региона пользователя.
		/// </summary>
		public const string RegionCode = ClaimTypeNamespace + "regioncode";

		/// <summary>
		/// Наименование региона пользователя.
		/// </summary>
		public const string RegionName = ClaimTypeNamespace + "regionname";


		/// <summary>
		/// Код роли пользователя в ЕСИА.
		/// </summary>
		public const string RoleCode = ClaimTypeNamespace + "rolecode";

		/// <summary>
		/// Наименование роли пользователя в ЕСИА.
		/// </summary>
		public const string RoleName = ClaimTypeNamespace + "rolename";


		/// <summary>
		/// Код системы пользователя в ЕСИА.
		/// </summary>
		public const string SystemCode = ClaimTypeNamespace + "systemcode";

		/// <summary>
		/// Наименование системы пользователя в ЕСИА.
		/// </summary>
		public const string SystemName = ClaimTypeNamespace + "systemname";


		/// <summary>
		/// Код организации пользователя в ЕСИА.
		/// </summary>
		public const string OrganizationCode = ClaimTypeNamespace + "organizationcode";

		/// <summary>
		/// Наименование организации пользователя в ЕСИА.
		/// </summary>
		public const string OrganizationName = ClaimTypeNamespace + "organizationname";
	}
}