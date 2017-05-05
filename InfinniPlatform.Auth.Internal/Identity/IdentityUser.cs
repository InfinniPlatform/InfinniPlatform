using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using InfinniPlatform.DocumentStorage;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity
{
    [DocumentType("UserStore")]
    // Add profile data for application users by adding properties to the IdentityUser class
    public class IdentityUser : Document
    {
        public IdentityUser()
        {
            Roles = new List<string>();
            Logins = new List<IdentityUserLogin>();
            Claims = new List<IdentityUserClaim>();
            Tokens = new List<IdentityUserToken>();
        }

        public string Id
        {
            get { return _id?.ToString(); }
            set { _id = value; }
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

        public List<IdentityUserLogin> Logins { get; set; }

        public List<IdentityUserClaim> Claims { get; set; }

        public List<IdentityUserToken> Tokens { get; set; }

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
            Logins.Add(new IdentityUserLogin(login));
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
            Claims.Add(new IdentityUserClaim(claim));
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

        private IdentityUserToken GetToken(string loginProider, string name)
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
                Tokens.Add(new IdentityUserToken
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