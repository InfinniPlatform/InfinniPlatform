using InfinniPlatform.Api.Security;

using Microsoft.AspNet.Identity;

namespace InfinniPlatform.Authentication.Modules
{
	/// <summary>
	/// Сведения о роли системы с реализацией интерфейса <see cref="IRole"/>.
	/// </summary>
	sealed class IdentityApplicationRole : ApplicationRole, IRole
	{
	}
}