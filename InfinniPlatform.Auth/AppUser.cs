using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using InfinniPlatform.DocumentStorage;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth
{
    [DocumentType("UserStore")]
    public class AppUser : Document
    {
        public AppUser()
        {
            Roles = new List<string>();
            Logins = new List<AppUserLogin>();
            Claims = new List<AppUserClaim>();
            Tokens = new List<AppUserToken>();
        }

        public string Id
        {
            get => _id?.ToString();
            set => _id = value;
        }

        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }

        public string SecurityStamp { get; set; }

        public string Email { get; set; }

        public string NormalizedEmail { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        public List<string> Roles { get; set; }

        public string PasswordHash { get; set; }

        public List<AppUserLogin> Logins { get; set; }

        public List<AppUserClaim> Claims { get; set; }

        public List<AppUserToken> Tokens { get; set; }

        public void AddRole(string role)
        {
            Roles.Add(role);
        }

        public void RemoveRole(string role)
        {
            Roles.Remove(role);
        }

        public void AddLogin(UserLoginInfo login)
        {
            Logins.Add(new AppUserLogin(login));
        }

        public void RemoveLogin(string loginProvider, string providerKey)
        {
            Logins.RemoveAll(l =>
                             {
                                 if (l.LoginProvider == loginProvider)
                                 {
                                     return l.ProviderKey == providerKey;
                                 }
                                 return false;
                             });
        }

        public bool HasPassword()
        {
            return false;
        }

        public void AddClaim(Claim claim)
        {
            Claims.Add(new AppUserClaim(claim));
        }

        public void RemoveClaim(Claim claim)
        {
            Claims.RemoveAll(c =>
                             {
                                 if (c.Type == claim.Type)
                                 {
                                     return c.Value == claim.Value;
                                 }
                                 return false;
                             });
        }

        public void ReplaceClaim(Claim existingClaim, Claim newClaim)
        {
            if (!Claims.Any(c =>
                            {
                                if (c.Type == existingClaim.Type)
                                {
                                    return c.Value == existingClaim.Value;
                                }
                                return false;
                            }))
            {
                return;
            }
            RemoveClaim(existingClaim);
            AddClaim(newClaim);
        }

        private AppUserToken GetToken(string loginProider, string name)
        {
            return Tokens.FirstOrDefault(t =>
                                         {
                                             if (t.LoginProvider == loginProider)
                                             {
                                                 return t.Name == name;
                                             }
                                             return false;
                                         });
        }

        public void SetToken(string loginProider, string name, string value)
        {
            var token = GetToken(loginProider, name);
            if (token != null)
            {
                token.Value = value;
            }
            else
            {
                Tokens.Add(new AppUserToken
                           {
                               LoginProvider = loginProider,
                               Name = name,
                               Value = value
                           });
            }
        }

        public string GetTokenValue(string loginProider, string name)
        {
            var token = GetToken(loginProider, name);

            return token?.Value;
        }

        public void RemoveToken(string loginProvider, string name)
        {
            Tokens.RemoveAll(t =>
                             {
                                 if (t.LoginProvider == loginProvider)
                                 {
                                     return t.Name == name;
                                 }
                                 return false;
                             });
        }

        public override string ToString()
        {
            return UserName;
        }
    }
}