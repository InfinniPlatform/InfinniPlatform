using System;
using System.Collections.Generic;

using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Cache;
using InfinniPlatform.Sdk.ContextComponents;

namespace InfinniPlatform.ContextComponents
{
	/// <summary>
	///     Компонент безопасности глобального контекста
	/// </summary>
	[Obsolete]
	public sealed class CachedSecurityComponent : ISecurityComponent
    {
        public static readonly CachedSecurityComponent Instance = new CachedSecurityComponent();


        private readonly SecurityCache _securityCache;

        public CachedSecurityComponent()
        {
            if (_securityCache == null)
            {
                _securityCache = new SecurityCache();
            }
        }

        private SecurityCache SecurityCache
        {
            get { return _securityCache; }
        }

        public IEnumerable<dynamic> Versions
        {
            get { return SecurityCache.Versions; }
        }

        public void UpdateUserRoles()
        {
            InternalUpdateUserRoles();
        }

        public void UpdateUsers()
        {
            InternalUpdateUsers();
        }

        public IEnumerable<dynamic> Roles
        {
            get { return SecurityCache.Roles; }
        }

        public IEnumerable<dynamic> UserRoles
        {
            get { return SecurityCache.UserRoles; }
        }

        public void UpdateAcl()
        {
            InternalUpdateAcl();
        }

        public void UpdateRoles()
        {
            InternalUpdateRoles();
        }

        public void UpdateVersions()
        {
            InternalUpdateVersions();
        }

        public IEnumerable<dynamic> Acl
        {
            get { return SecurityCache.Acl; }
        }

        public IEnumerable<dynamic> Users
        {
            get { return SecurityCache.Users; }
        }

		private void InternalUpdateUserRoles()
        {
            SecurityCache.UserRoles = new AuthApi().GetUserRoles(false);
        }

        private void InternalUpdateUsers()
        {
            SecurityCache.Users = new AuthApi().GetUsers(false);
        }

        private void InternalUpdateAcl()
        {
            SecurityCache.Acl = new AuthApi().GetAcl(false);
        }

        private void InternalUpdateRoles()
        {
            SecurityCache.Roles = new AuthApi().GetRoles(false);
        }

        private void InternalUpdateVersions()
        {
            SecurityCache.Versions = new AuthApi().GetVersions(false);
        }

        /// <summary>
        ///     Прогрев Acl на старте сервера
        /// </summary>
        public void WarmUpAcl()
        {
            InternalUpdateUsers();
            InternalUpdateUserRoles();
            InternalUpdateAcl();
            InternalUpdateRoles();
        }
    }
}