using System;
using System.Linq;

using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.SearchOptions.Builders;
using InfinniPlatform.Api.Security;
using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.SystemConfig.UserStorage
{
    internal sealed class ApplicationUserStorePersistentStorage : IApplicationUserStore
    {
        public ApplicationUserStorePersistentStorage(IAppConfiguration appConfiguration)
        {
            var settings = appConfiguration.GetSection<UserStorageSettings>(UserStorageSettings.SectionName);

            _userCache = new ApplicationUserStoreCache(settings.UserCacheTimeout);
        }


        private readonly ApplicationUserStoreCache _userCache;


        public void CreateUser(ApplicationUser user)
        {
            if (string.IsNullOrEmpty(user.Id))
            {
                user.Id = CreateUnique();
            }

            InsertUser(user);
            UpdateUserInCache(user);
        }

        public void UpdateUser(ApplicationUser user)
        {
            DeleteUser(user);
            InsertUser(user);
            UpdateUserInCache(user);
        }

        private static void InsertUser(ApplicationUser user)
        {
            user.SecurityStamp = CreateUnique();
            SetDocument(AuthorizationStorageExtensions.UserStore, ConvertToDynamic(user));
        }

        public void DeleteUser(ApplicationUser user)
        {
            DeleteDocument(AuthorizationStorageExtensions.UserStore, user.Id);
            UpdateUserInCache(user);
        }

        public ApplicationUser FindUserById(string userId)
        {
            return FindUserInCache(c => c.FindUserById(userId), () => GetDocument<ApplicationUser>(AuthorizationStorageExtensions.UserStore, "Id", userId));
        }

        public ApplicationUser FindUserByUserName(string userName)
        {
            return FindUserInCache(c => c.FindUserByUserName(userName), () => GetDocument<ApplicationUser>(AuthorizationStorageExtensions.UserStore, "UserName", userName));
        }

        public ApplicationUser FindUserByEmail(string email)
        {
            return FindUserInCache(c => c.FindUserByEmail(email), () => GetDocument<ApplicationUser>(AuthorizationStorageExtensions.UserStore, "Email", email));
        }

        public ApplicationUser FindUserByPhoneNumber(string phoneNumber)
        {
            return FindUserInCache(c => c.FindUserByPhoneNumber(phoneNumber), () => GetDocument<ApplicationUser>(AuthorizationStorageExtensions.UserStore, "PhoneNumber", phoneNumber));
        }

        public ApplicationUser FindUserByLogin(ApplicationUserLogin userLogin)
        {
            return FindUserInCache(c => c.FindUserByLogin(userLogin), () => GetDocument<ApplicationUser>(AuthorizationStorageExtensions.UserStore, "Logins.ProviderKey", userLogin.ProviderKey));
        }

        public ApplicationUser FindUserByName(string name)
        {
            return FindUserByUserName(name) ?? FindUserByEmail(name) ?? FindUserByPhoneNumber(name);
        }

        public void AddUserToRole(ApplicationUser user, string roleName)
        {
            if (user.Id == null)
            {
                throw new ArgumentException(Properties.Resources.CantAddUnsavedUserToRole);
            }

            dynamic role = GetDocument(AuthorizationStorageExtensions.RoleStore, "Name", roleName);

            if (role == null)
            {
                throw new ArgumentException(string.Format(Properties.Resources.RoleNotFound, roleName));
            }

            var roles = user.Roles.ToList();

            if (roles.All(r => r.Id != role.Id))
            {
                // Обновление сведений пользователя
                roles.Add(new ForeignKey { Id = role.Id, DisplayName = role.Name });
                user.Roles = roles;
                UpdateUser(user);

                // Добавление связки пользователь-роль
                dynamic userRole = new DynamicWrapper();
                userRole.UserName = user.UserName;
                userRole.RoleName = role.Name;

                SetDocument(AuthorizationStorageExtensions.UserRoleStore, userRole);
            }
        }

        public void RemoveUserFromRole(ApplicationUser user, string roleName)
        {
            if (user.Id == null)
            {
                throw new ArgumentException(Properties.Resources.CantRemoveUnsavedUserFromRole);
            }

            dynamic role = GetDocument(AuthorizationStorageExtensions.RoleStore, "Name", roleName);

            if (role == null)
            {
                throw new ArgumentException(string.Format(Properties.Resources.RoleNotFound, roleName));
            }

            var roles = user.Roles.ToList();

            if (roles.Any(r => r.Id == role.Id))
            {
                // Обновление сведений пользователя
                user.Roles = roles.Where(r => r.Id != role.Id).ToList();
                UpdateUser(user);

                // Удаление связки пользователь-роль

                dynamic userRole = GetDocument(AuthorizationStorageExtensions.UserRoleStore, "RoleName", roleName, "UserName", user.UserName);

                if (userRole != null)
                {
                    DeleteDocument(AuthorizationStorageExtensions.UserRoleStore, userRole.Id);
                }
            }
        }

        public void AddUserClaim(ApplicationUser user, string claimType, string claimValue)
        {
            if (!user.Claims.Any(c => c.Type.DisplayName == claimType && c.Value == claimValue))
            {
                var claims = user.Claims.ToList();
                claims.Add(new ApplicationUserClaim { Type = new ForeignKey { Id = claimType, DisplayName = claimType }, Value = claimValue });
                user.Claims = claims;
                UpdateUser(user);
            }
        }

        public void RemoveUserClaim(ApplicationUser user, string claimType, string claimValue)
        {
            if (user.Claims.Any(c => c.Type.DisplayName == claimType && c.Value == claimValue))
            {
                user.Claims = user.Claims.Where(c => !(c.Type.DisplayName == claimType && c.Value == claimValue)).ToList();
                UpdateUser(user);
            }
        }

        public void RemoveUserClaim(ApplicationUser user, string claimType)
        {
            if (user.Claims.Any(c => c.Type.DisplayName == claimType))
            {
                user.Claims = user.Claims.Where(c => c.Type.DisplayName != claimType).ToList();
                UpdateUser(user);
            }
        }

        public void AddUserLogin(ApplicationUser user, ApplicationUserLogin userLogin)
        {
            var logins = user.Logins.ToList();

            if (!logins.Any(f => f.Provider == userLogin.ProviderKey && f.ProviderKey == userLogin.ProviderKey))
            {
                logins.Add(userLogin);
                user.Logins = logins;
                UpdateUser(user);
            }
        }

        public void RemoveUserLogin(ApplicationUser user, ApplicationUserLogin userLogin)
        {
            if (user.Logins.Any(f => f.Provider == userLogin.Provider && f.ProviderKey == userLogin.ProviderKey))
            {
                user.Logins = user.Logins.Where(f => !(f.Provider == userLogin.Provider && f.ProviderKey == userLogin.ProviderKey)).ToList();
                UpdateUser(user);
            }
        }

        public void AddRole(string name, string caption, string description)
        {
            dynamic role = new DynamicWrapper();
            role.Name = name;
            role.Caption = caption;
            role.Description = description;

            dynamic existsRole = GetDocument(AuthorizationStorageExtensions.RoleStore, "Name", name);

            if (existsRole != null)
            {
                role.Id = existsRole.Id;
            }

            SetDocument(AuthorizationStorageExtensions.RoleStore, role);
        }

        public void RemoveRole(string name)
        {
            dynamic role = GetDocument(AuthorizationStorageExtensions.RoleStore, "Name", name);

            if (role == null)
            {
                throw new ArgumentException(string.Format(Properties.Resources.RoleNotFound, name));
            }

            dynamic roleLinks = GetDocument(AuthorizationStorageExtensions.UserStore, "Roles.DisplayName", name);

            if (roleLinks != null)
            {
                throw new ArgumentException(string.Format(Properties.Resources.RoleIsUsed, name));
            }

            DeleteDocument(AuthorizationStorageExtensions.RoleStore, role.Id);
        }

        public dynamic FindClaimType(string claimType)
        {
            return GetDocument<ApplicationClaimType>(AuthorizationStorageExtensions.ClaimStore, "Name", claimType);
        }

        public void AddClaimType(string claimType)
        {
            dynamic claim = FindClaimType(claimType);

            if (claim == null)
            {
                claim = new DynamicWrapper();
                claim.Name = claimType;

                SetDocument(AuthorizationStorageExtensions.ClaimStore, claim);
            }
        }

        public void RemoveClaimType(string claimType)
        {
            dynamic claim = FindClaimType(claimType);

            if (claim != null)
            {
                dynamic claimLinks = GetDocument(AuthorizationStorageExtensions.UserStore, "Claims.Type.DisplayName", claimType);

                if (claimLinks != null)
                {
                    throw new ArgumentException(string.Format(Properties.Resources.ClaimIsUsed, claimType));
                }

                DeleteDocument(AuthorizationStorageExtensions.ClaimStore, claim.Id);
            }
        }

        private static object GetDocument(string documentType, string propertyName, string propertyValue)
        {
            return GetDocument(documentType, f => f.AddCriteria(cr => cr.Property(propertyName).IsEquals(propertyValue)));
        }

        private static T GetDocument<T>(string documentType, string propertyName, string propertyValue)
        {
            var document = GetDocument(documentType, f => f.AddCriteria(cr => cr.Property(propertyName).IsEquals(propertyValue)));
            return ConvertFromDynamic<T>(document);
        }

        private static object GetDocument(string documentType, params string[] documentFilters)
        {
            return GetDocument(documentType, f =>
                                             {
                                                 for (var i = 0; i < documentFilters.Length; i += 2)
                                                 {
                                                     var propertyName = documentFilters[i];
                                                     var propertyValue = documentFilters[i + 1];
                                                     f.AddCriteria(cr => cr.Property(propertyName).IsEquals(propertyValue));
                                                 }
                                             });
        }

        private static object GetDocument(string documentType, Action<FilterBuilder> documentFilters)
        {
            var documentApi = new DocumentApiUnsecured();
            var documents = documentApi.GetDocument(AuthorizationStorageExtensions.AuthorizationConfigId, documentType, documentFilters, 0, 1);
            return (documents != null) ? documents.FirstOrDefault() : null;
        }

        private static void SetDocument(string documentType, object document)
        {
            var documentApi = new DocumentApiUnsecured();
            documentApi.SetDocument(AuthorizationStorageExtensions.AuthorizationConfigId, documentType, document, false, true);
        }

        private static void DeleteDocument(string documentType, string documentId)
        {
            var documentApi = new DocumentApiUnsecured();
            documentApi.DeleteDocument(AuthorizationStorageExtensions.AuthorizationConfigId, documentType, documentId);
        }

        private static T ConvertFromDynamic<T>(object dynamicObject)
        {
            return (T)JsonObjectSerializer.Default.ConvertFromDynamic(dynamicObject, typeof(T));
        }

        private static object ConvertToDynamic(object typeObject)
        {
            return JsonObjectSerializer.Default.ConvertToDynamic(typeObject);
        }

        private void UpdateUserInCache(ApplicationUser user)
        {
            _userCache.AddOrUpdateUser(user);
        }

        private ApplicationUser FindUserInCache(Func<ApplicationUserStoreCache, ApplicationUser> cacheSelector, Func<ApplicationUser> storageSelector)
        {
            var user = cacheSelector(_userCache);

            if (user == null)
            {
                user = storageSelector();

                if (user != null)
                {
                    _userCache.AddOrUpdateUser(user);
                }
            }

            return user;
        }

        private static string CreateUnique()
        {
            return Guid.NewGuid().ToString();
        }
    }
}