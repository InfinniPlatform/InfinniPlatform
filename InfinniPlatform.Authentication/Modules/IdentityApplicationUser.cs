using InfinniPlatform.Api.Security;

using Microsoft.AspNet.Identity;

namespace InfinniPlatform.Authentication.Modules
{
	/// <summary>
	/// Сведения о пользователе системы с реализацией интерфейса <see cref="IUser"/>.
	/// </summary>
	sealed class IdentityApplicationUser : ApplicationUser, IUser
	{
	}
}