using System.Linq;

using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.SystemConfig.UserStorage
{
	public static class ApplicationUserStorePersistentStorageExtensions
	{
		public static void CheckStorage()
		{
			var adminApi = new AdminApi();
			var aclApi = new AuthApi();

			// Добавляем роль анонимного пользователя.
			// Данная роль имеет право на чтение каталога пользователей и ролей в нее не должен входить ни один пользователь системы.

			var acl = aclApi.GetAcl(false).ToList();

			dynamic anonymousUserRole = acl.FirstOrDefault(a => a.UserName == AuthorizationStorageExtensions.AnonymousUser);

			if (anonymousUserRole == null)
			{
				adminApi.AddAnonimousUserAcl(AuthorizationStorageExtensions.AnonymousUser);
			}

			var roles = aclApi.GetRoles(false);

			// Если отсутствует роль админа, добавляем ее

			dynamic adminRole = roles.FirstOrDefault(a => a.Name == AuthorizationStorageExtensions.AdminRole);

			if (adminRole == null)
			{
				aclApi.AddRole(AuthorizationStorageExtensions.AdminRole, AuthorizationStorageExtensions.AdminRole, AuthorizationStorageExtensions.AdminRole);
			}

			var users = aclApi.GetUsers(false);

			if (users.FirstOrDefault(u => u.UserName == AuthorizationStorageExtensions.AdminUser) == null)
			{
				aclApi.AddUser(AuthorizationStorageExtensions.AdminUser, "Admin");

				// Проверяем, удалось ли добавить пользователя
				users = aclApi.GetUsers(false);

				if (users.FirstOrDefault(u => u.UserName == AuthorizationStorageExtensions.AdminUser) == null)
				{
					// Если не удалось, завершаем
					return;
				}

				aclApi.AddUserToRole(AuthorizationStorageExtensions.AdminUser, AuthorizationStorageExtensions.AdminRole);
			}

			// Проверяем наличие роли администратора системы
			// Если роль администратора отсутствует, то создаем ее заново
			if (acl.FirstOrDefault(a => a.UserName == AuthorizationStorageExtensions.AdminRole) == null)
			{
				adminApi.GrantAdminAcl(AuthorizationStorageExtensions.AdminRole);
				adminApi.SetDefaultAcl();
			}

			aclApi.InvalidateAcl();
		}
	}
}