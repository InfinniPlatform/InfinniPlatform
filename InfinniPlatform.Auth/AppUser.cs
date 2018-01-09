using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using InfinniPlatform.DocumentStorage;

using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// Application user.
    /// </summary>
    [DocumentType("UserStore")]
    public class AppUser : Document
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AppUser" />.
        /// </summary>
        public AppUser()
        {
            Roles = new List<string>();
            Logins = new List<AppUserLogin>();
            Claims = new List<AppUserClaim>();
            Tokens = new List<AppUserToken>();
        }

        /// <summary>
        /// User name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User name normalized for search.
        /// </summary>
        public string NormalizedUserName { get; set; }

        /// <summary>
        /// A random value that must change whenever a users credentials change (password changed, login removed).
        /// </summary>
        public string SecurityStamp { get; set; }

        /// <summary>
        /// User password hash.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// User email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User email address normalized for search.
        /// </summary>
        public string NormalizedEmail { get; set; }

        /// <summary>
        /// Flag indicating if a user has confirmed their email address.
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// User phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Flag indicating if a user has confirmed their phone number.
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Flag indicating if two factor authentication is enabled for this user.
        /// </summary>
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// Date and time, in UTC, when any user lockout ends.
        /// </summary>
        public DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        /// Flag indicating if the user could be locked out.
        /// </summary>
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// Number of failed login attempts for the current user.
        /// </summary>
        public int AccessFailedCount { get; set; }

        /// <summary>
        /// List of user roles.
        /// </summary>
        public List<string> Roles { get; set; }

        /// <summary>
        /// List of user logins.
        /// </summary>
        public List<AppUserLogin> Logins { get; set; }

        /// <summary>
        /// List of user claims.
        /// </summary>
        public List<AppUserClaim> Claims { get; set; }

        /// <summary>
        /// List of user tokens.
        /// </summary>
        public List<AppUserToken> Tokens { get; set; }

        /// <summary>
        /// Adds role to user roles.
        /// </summary>
        /// <param name="role">Role name.</param>
        public void AddRole(string role)
        {
            Roles.Add(role);
        }

        /// <summary>
        /// Adds role from user roles.
        /// </summary>
        public void RemoveRole(string role)
        {
            Roles.Remove(role);
        }

        /// <summary>
        /// Adds login to user logins.
        /// </summary>
        public void AddLogin(UserLoginInfo login)
        {
            Logins.Add(new AppUserLogin(login));
        }


        /// <summary>
        /// Removes login from user logins.
        /// </summary>
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

        /// <summary>
        /// Indicates whether user has a password.
        /// </summary>
        public bool HasPassword()
        {
            //TODO Why?
            return false;
        }

        /// <summary>
        /// Adds claim to user claims.
        /// </summary>
        public void AddClaim(Claim claim)
        {
            Claims.Add(new AppUserClaim(claim));
        }

        /// <summary>
        /// Removes claim from user claims.
        /// </summary>
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

        /// <summary>
        /// Replace existing claim with new claim.
        /// </summary>
        /// <param name="existingClaim">Existing claim representation.</param>
        /// <param name="newClaim">New claim representation.</param>
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

        /// <summary>
        /// Gets login provider token.
        /// </summary>
        /// <param name="loginProider">Login provider name.</param>
        /// <param name="name">Token name.</param>
        public AppUserToken GetToken(string loginProider, string name)
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

        /// <summary>
        /// Sets login provider token.
        /// </summary>
        /// <param name="loginProider">Login provider name.</param>
        /// <param name="name">Token name.</param>
        /// <param name="value">Token value.</param>
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

        /// <summary>
        /// Gets login provider token value.
        /// </summary>
        /// <param name="loginProider">Login provider name.</param>
        /// <param name="name">Token name.</param>
        public string GetTokenValue(string loginProider, string name)
        {
            var token = GetToken(loginProider, name);

            return token?.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginProvider">Login provider name.</param>
        /// <param name="name">Token name.</param>
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

        /// <summary>
        /// Return string representation of user.
        /// </summary>
        public override string ToString()
        {
            return UserName;
        }
    }
}