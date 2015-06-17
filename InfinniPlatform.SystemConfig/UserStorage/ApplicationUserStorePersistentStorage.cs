using System;
using System.Diagnostics;
using System.Linq;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Security;
using InfinniPlatform.Api.Settings;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.SystemConfig.UserStorage
{
	public sealed class ApplicationUserStorePersistentStorage : IApplicationUserStore
	{
		public void Dispose()
		{

		}

		public void CreateUser(ApplicationUser user)
		{
			InsertUser(user);
		}

		private void InsertUser(ApplicationUser user)
		{
		    user.SecurityStamp = Guid.NewGuid().ToString();
            var instance = DynamicWrapperExtensions.ToDynamic(user);
            
			new DocumentApiUnsecured(null).SetDocument(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.UserStore, instance, false, true);
		}

		public void UpdateUser(ApplicationUser user)
		{
			DeleteUser(user);
			InsertUser(user);
		}

		public void DeleteUser(ApplicationUser user)
		{
			new DocumentApiUnsecured(null).DeleteDocument(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.UserStore, user.Id);
		}

		public ApplicationUser FindUserById(string userId)
		{
            dynamic user = new DocumentApiUnsecured(null).GetDocument(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.UserStore,
										  f => f.AddCriteria(cr => cr.Property("Id").IsEquals(userId)), 0, 1).FirstOrDefault();
            if (user != null)
            {
                return (JObject.FromObject(user)).ToObject<ApplicationUser>();
            }
            return null;
		}

		public ApplicationUser FindUserByName(string userName)
		{
			dynamic user = new DocumentApiUnsecured(null).GetDocument(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.UserStore,
										 f => f.AddCriteria(cr => cr.Property("UserName").IsEquals(userName)), 0, 1).FirstOrDefault();
			if (user != null)
			{
				return (JObject.FromObject(user)).ToObject<ApplicationUser>();
			}
			return null;

		}

		public ApplicationUser FindUserByLogin(ApplicationUserLogin userLogin)
		{
			dynamic user = new DocumentApiUnsecured(null).GetDocument(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.UserStore,
										 f => f.AddCriteria(cr => cr.Property("Logins.ProviderKey").IsEquals(userLogin.ProviderKey)), 0, 1).FirstOrDefault();

			if (user != null)
			{
				return (JObject.FromObject(user)).ToObject<ApplicationUser>();
			}
			return null;
		}

		public void AddUserToRole(ApplicationUser user, string roleName)
		{
			if (user.Id == null)
			{
				throw new ArgumentException(Resources.CantAddUnsavedUserToRole);
			}

			dynamic role = new DocumentApiUnsecured(null).GetDocument(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.RoleStore,
													 f => f.AddCriteria(cr => cr.Property("Name").IsEquals(roleName)), 0, 1).FirstOrDefault();

			if (role == null)
			{
				throw new ArgumentException(string.Format("role with name \"{0}\" not found.",roleName));
			}

			var roles = user.Roles.ToList();
			if (roles.FirstOrDefault(r => r.Id == role.Id) == null)
			{        
                //список ролей пользователя не используется в текущей реализации. Сделано только для обеспечения совместимости с внешними конфигурациями авторизации
				var link = new ForeignKey();
				link.Id = role.Id;
				link.DisplayName = role.Name;

				roles.Add(link);

				user.Roles = roles;

				UpdateUser(user);

                //добавляем связку в хранилище
			    dynamic userRoleInstance = new DynamicWrapper();
			    userRoleInstance.UserName = user.UserName;
			    userRoleInstance.RoleName = role.Name;

				new DocumentApiUnsecured(null).SetDocument(AuthorizationStorageExtensions.AuthorizationConfigId,
			                                  AuthorizationStorageExtensions.UserRoleStore, userRoleInstance, false, true);
			}

		}

		public void RemoveUserFromRole(ApplicationUser user, string roleName)
		{
			if (user.Id == null)
			{
				throw new ArgumentException(Resources.CantRemoveUnsavedUserFromRole);
			}

			dynamic role = new DocumentApiUnsecured(null).GetDocument(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.RoleStore,
										 f => f.AddCriteria(cr => cr.Property("Name").IsEquals(roleName)), 0, 1).FirstOrDefault();

			var roles = user.Roles.ToList();
			if (role != null && roles.FirstOrDefault(r => r.Id == role.Id) != null)
			{
				user.Roles = roles.Where(r => r.Id != role.Id).ToList();

				UpdateUser(user);

				dynamic userRole = new DocumentApiUnsecured(null).GetDocument(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.UserRoleStore,
								f => f.AddCriteria(cr => cr.Property("RoleName").IsEquals(roleName))
									.AddCriteria(cr => cr.Property("UserName").IsEquals(user.UserName)), 0, 1).FirstOrDefault();

				if (userRole != null)
				{
					new DocumentApiUnsecured(null).DeleteDocument(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.UserRoleStore, userRole.Id);
				}

			}
		}

		public void AddUserClaim(ApplicationUser user, string claimType, string claimValue)
		{
			var claims = user.Claims.ToList();

			ApplicationClaimType claim = FindClaimType(claimType);
			if (claim != null)
			{
				var userClaim = new ApplicationUserClaim();
				userClaim.Type = new ForeignKey()
									 {
										 DisplayName = claim.Name,
										 Id = claim.Id
									 };
				userClaim.Value = claimValue;

				claims.Add(userClaim);
				user.Claims = claims;
				UpdateUser(user);
			}
			else
			{
				throw new ArgumentException(string.Format("User claim not found: {0}", claimType));
			}
		}

		public void RemoveUserClaim(ApplicationUser user, string claimType)
		{
			var claims = user.Claims.ToList().Where(c => c.Type.DisplayName != claimType).ToList();
			user.Claims = claims;
			UpdateUser(user);

		}

		public void AddUserLogin(ApplicationUser user, ApplicationUserLogin userLogin)
		{
			var logins = user.Logins.ToList();
			if (logins.FirstOrDefault(f => f.Provider == userLogin.ProviderKey && f.ProviderKey == userLogin.ProviderKey) == null)
			{
				logins.Add(userLogin);
				user.Logins = logins;
				UpdateUser(user);
			}
		}

		public void RemoveUserLogin(ApplicationUser user, ApplicationUserLogin userLogin)
		{
			var logins = user.Logins.ToList().Where(f => !(f.Provider == userLogin.Provider && f.ProviderKey == userLogin.ProviderKey)).ToList();
			user.Logins = logins;
			UpdateUser(user);

		}

		public void AddRole(string roleName, string caption, string description)
		{
			dynamic instance = new DynamicWrapper();
			instance.Name = roleName;
			instance.Caption = caption;
			instance.Description = description;

			dynamic role = new DocumentApiUnsecured(null).GetDocument(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.RoleStore,
										 f => f.AddCriteria(cr => cr.Property("Name").IsEquals(roleName)), 0, 1).FirstOrDefault();
			if (role != null)
			{
				instance.Id = role.Id;
			}

			new DocumentApiUnsecured(null).SetDocument(AuthorizationStorageExtensions.AuthorizationConfigId,
										  AuthorizationStorageExtensions.RoleStore, instance, false, true);

		}

		public void RemoveRole(string roleName)
		{
			dynamic role = new DocumentApiUnsecured(null).GetDocument(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.RoleStore,
													 f => f.AddCriteria(cr => cr.Property("Name").IsEquals(roleName)), 0, 1).FirstOrDefault();

			dynamic roleLinks = new DocumentApiUnsecured(null).GetDocument(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.UserStore,
										 f => f.AddCriteria(cr => cr.Property("Roles.DisplayName").IsEquals(roleName)), 0, 1).FirstOrDefault();

			if (role == null)
			{
				throw new ArgumentException(string.Format("Role with name \"{0}\" not found.", roleName));
			}

			if (roleLinks != null)
			{
				throw new ArgumentException(string.Format("Role with name \"{0}\" has user linked and should not be deleted", roleName));
			}

			new DocumentApiUnsecured(null).DeleteDocument(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.RoleStore, role.Id);
		}


		public dynamic FindClaimType(string claimType)
		{
			dynamic claim =
				new DocumentApiUnsecured(null).GetDocument(AuthorizationStorageExtensions.AuthorizationConfigId,
											  AuthorizationStorageExtensions.ClaimStore,
											  f => f.AddCriteria(cr => cr.Property("Name").IsEquals(claimType)), 0, 1)
								 .FirstOrDefault();
			if (claim != null)
			{
				return JObject.FromObject(claim).ToObject<ApplicationClaimType>();
			}
			return null;
		}


		public void AddClaimType(string claimType)
		{
			dynamic claim = FindClaimType(claimType);

			if (claim == null)
			{
				claim = new DynamicWrapper();
				claim.Name = claimType;

				new DocumentApiUnsecured(null).SetDocument(AuthorizationStorageExtensions.AuthorizationConfigId,
											  AuthorizationStorageExtensions.ClaimStore, claim, false, true);
			}
		}

		public void RemoveClaimType(string claimType)
		{
			dynamic claim = FindClaimType(claimType);

			if (claim != null)
			{
				dynamic claimLinks =
					new DocumentApiUnsecured(null).GetDocument(AuthorizationStorageExtensions.AuthorizationConfigId,
					                              AuthorizationStorageExtensions.UserStore,
					                              f => f.AddCriteria(cr => cr.Property("Claims.Type.DisplayName").IsEquals(claimType)), 0, 1)
					                 .FirstOrDefault();

				if (claimLinks != null)
				{
					throw new ArgumentException(
						string.Format("Can't delete user claim \"{0}\": existing users have link to claim type.", claimType));
				}
				new DocumentApiUnsecured(null).DeleteDocument(AuthorizationStorageExtensions.AuthorizationConfigId,
				                                 AuthorizationStorageExtensions.ClaimStore, claim.Id);
			}
		}


		public void RemoveAcl(string aclId)
		{
			new DocumentApiUnsecured(null).DeleteDocument(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.AclStore, aclId);
		}
	}

	public static class ApplicationUserStorePersistentStorageExtensions
	{
		public static void CheckStorage()
		{
			var adminApi = new AdminApi();
			var aclApi = new AuthApi(null);

			//добавляем роль анонимного пользователя. Данная роль имеет право на чтение каталога пользователей и ролей
			//в нее не должен входить ни один пользователь системы.

			var acl = aclApi.GetAcl(false).ToList();

			var rolesAnonimous = acl.FirstOrDefault(a => a.UserName == AuthorizationStorageExtensions.AnonimousUser);
			if (rolesAnonimous == null)
			{
				adminApi.AddAnonimousUserAcl(AuthorizationStorageExtensions.AnonimousUser);
			}

			var roles = aclApi.GetRoles(false);
			//если отсутствует роль админа, добавляем ее
			var roleAdmin = roles.FirstOrDefault(a => a.Name == AuthorizationStorageExtensions.AdminRole);

			if (roleAdmin == null)
			{
				aclApi.AddRole(AuthorizationStorageExtensions.AdminRole, AuthorizationStorageExtensions.AdminRole,AuthorizationStorageExtensions.AdminRole);
			}

			var users = aclApi.GetUsers(false);
			if (users.FirstOrDefault(u => u.UserName == AuthorizationStorageExtensions.AdminUser) == null)
			{
				aclApi.AddUser(AuthorizationStorageExtensions.AdminUser, AppSettings.GetValue("AdminPassword","Admin"));

				//проверяем, удалось ли добавить пользователя
				users = aclApi.GetUsers(false);
				if (users.FirstOrDefault(u => u.UserName == AuthorizationStorageExtensions.AdminUser) == null)
				{
					//если не удалось, завершаем
					return;
				}

				aclApi.AddUserToRole(AuthorizationStorageExtensions.AdminUser, AuthorizationStorageExtensions.AdminRole);
			
			}



		    //проверяем наличие роли администратора системы
			//если роль администратора отсутствует, то создаем ее заново
			if (acl.FirstOrDefault(a => a.UserName == AuthorizationStorageExtensions.AdminRole) == null)
			{
				adminApi.GrantAdminAcl(AuthorizationStorageExtensions.AdminRole);

			    adminApi.SetDefaultAcl();
			}


			aclApi.InvalidateAcl();

		}
	}
}
