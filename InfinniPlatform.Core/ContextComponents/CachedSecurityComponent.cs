using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Cache;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.ContextComponents
{
    /// <summary>
    ///     Компонент безопасности глобалього контекста
    /// </summary>
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



        public void UpdateClaim(string userName, string claimType, string claimValue)
        {
            if (Users != null)
            {
                var user = Users.FirstOrDefault(u => u.UserName == userName);
                if (user != null)
                {
                    dynamic claim = null;
                    if (user.Claims == null)
                    {
                        user.Claims = new List<dynamic>();
                    }
                    else
                    {
                        claim = ((IEnumerable<dynamic>)user.Claims).FirstOrDefault(c => c.Type.DisplayName == claimType);
                    }

                    if (claim == null)
                    {
                        claim = new DynamicWrapper();
                        user.Claims.Add(claim);
                    }

                    claim.Type = new DynamicWrapper();
                    claim.Type.Id = Guid.NewGuid().ToString();
                    claim.Type.DisplayName = claimType;
                    claim.Value = claimValue;


                    var users = Users.Where(u => u.UserName != userName).ToList();
                    users.Add(user);
                    SecurityCache.Users = users;
                }
            }
        }

        public string GetClaim(string claimType, string userName)
        {
            if (Users != null)
            {
                var user = Users.FirstOrDefault(u => u.UserName == userName);
                if (user != null)
                {
                    IEnumerable<dynamic> claims = user.Claims;
                    if (claims != null)
                    {
                        return claims.Where(c => c.Type.DisplayName == claimType).Select(c => c.Value).FirstOrDefault();
                    }
                }
                return null;
            }
            return null;
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