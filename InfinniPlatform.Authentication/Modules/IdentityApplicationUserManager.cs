using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.Api.Security;
using InfinniPlatform.Authentication.Properties;
using InfinniPlatform.Sdk.ContextComponents;

using Microsoft.AspNet.Identity;

namespace InfinniPlatform.Authentication.Modules
{
	sealed class IdentityApplicationUserManager : IApplicationUserManager
	{
		public IdentityApplicationUserManager(Func<UserManager<IdentityApplicationUser>> userManagerFunc)
		{
			_userManager = new Lazy<UserManager<IdentityApplicationUser>>(userManagerFunc);
		}


		private readonly Lazy<UserManager<IdentityApplicationUser>> _userManager;


		public UserManager<IdentityApplicationUser> UserManager
		{
			get { return _userManager.Value; }
		}


		public object GetCurrentUser()
		{
			return InvokeUserManager((m, userId) => m.FindByIdAsync(userId));
		}


		// Password

		public bool HasPassword()
		{
			return InvokeUserManager((m, userId) => m.HasPasswordAsync(userId));
		}

		public void AddPassword(string password)
		{
			if (string.IsNullOrEmpty(password))
			{
				throw new ArgumentNullException("password");
			}

			InvokeUserManager((m, userId) => m.AddPasswordAsync(userId, password));
		}

		public void RemovePassword()
		{
			InvokeUserManager((m, userId) => m.RemovePasswordAsync(userId));
		}

		public void ChangePassword(string currentPassword, string newPassword)
		{
			if (string.IsNullOrEmpty(currentPassword))
			{
				throw new ArgumentNullException("currentPassword");
			}

			if (string.IsNullOrEmpty(newPassword))
			{
				throw new ArgumentNullException("newPassword");
			}

			InvokeUserManager((m, userId) => m.ChangePasswordAsync(userId, currentPassword, newPassword));
		}


		// SecurityStamp

		public string GetSecurityStamp()
		{
			return InvokeUserManager((m, userId) => m.GetSecurityStampAsync(userId));
		}

		public void UpdateSecurityStamp()
		{
			InvokeUserManager((m, userId) => m.UpdateSecurityStampAsync(userId));
		}


		// Email

		public string GetEmail()
		{
			return InvokeUserManager((m, userId) => m.GetEmailAsync(userId));
		}

		public void SetEmail(string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				throw new ArgumentNullException("email");
			}

			InvokeUserManager((m, userId) => m.SetEmailAsync(userId, email));

			var currentIdentity = GetCurrentIdentity();
			currentIdentity.AddOrUpdateClaim(ClaimTypes.Email, email);
		}

		public bool IsEmailConfirmed()
		{
			return InvokeUserManager((m, userId) => m.IsEmailConfirmedAsync(userId));
		}


		// PhoneNumber

		public string GetPhoneNumber()
		{
			return InvokeUserManager((m, userId) => m.GetPhoneNumberAsync(userId));
		}

		public void SetPhoneNumber(string phoneNumber)
		{
			if (string.IsNullOrEmpty(phoneNumber))
			{
				throw new ArgumentNullException("phoneNumber");
			}

			InvokeUserManager((m, userId) => m.SetPhoneNumberAsync(userId, phoneNumber));

			var currentIdentity = GetCurrentIdentity();
			currentIdentity.AddOrUpdateClaim(ClaimTypes.MobilePhone, phoneNumber);
		}

		public bool IsPhoneNumberConfirmed()
		{
			return InvokeUserManager((m, userId) => m.IsPhoneNumberConfirmedAsync(userId));
		}


		// Logins

		public void AddLogin(string loginProvider, string providerKey)
		{
			if (string.IsNullOrEmpty(loginProvider))
			{
				throw new ArgumentNullException("loginProvider");
			}

			if (string.IsNullOrEmpty(providerKey))
			{
				throw new ArgumentNullException("providerKey");
			}

			InvokeUserManager((m, userId) => m.AddLoginAsync(userId, new UserLoginInfo(loginProvider, providerKey)));
		}

		public void RemoveLogin(string loginProvider, string providerKey)
		{
			if (string.IsNullOrEmpty(loginProvider))
			{
				throw new ArgumentNullException("loginProvider");
			}

			if (string.IsNullOrEmpty(providerKey))
			{
				throw new ArgumentNullException("providerKey");
			}

			InvokeUserManager((m, userId) => m.RemoveLoginAsync(userId, new UserLoginInfo(loginProvider, providerKey)));
		}


		// Claims

		public void AddClaim(string claimType, string claimValue)
		{
			if (string.IsNullOrEmpty(claimType))
			{
				throw new ArgumentNullException("claimType");
			}

			if (string.IsNullOrEmpty(claimValue))
			{
				throw new ArgumentNullException("claimValue");
			}

			var claim = new Claim(claimType, claimValue);
			InvokeUserManager((m, userId) => m.AddClaimAsync(userId, claim));

			var currentIdentity = GetCurrentIdentity();
			currentIdentity.AddOrUpdateClaim(claimType, claimValue);
		}

		public void RemoveClaim(string claimType, string claimValue)
		{
			if (string.IsNullOrEmpty(claimType))
			{
				throw new ArgumentNullException("claimType");
			}

			if (string.IsNullOrEmpty(claimValue))
			{
				throw new ArgumentNullException("claimValue");
			}

			var claim = new Claim(claimType, claimValue);
			InvokeUserManager((m, userId) => m.RemoveClaimAsync(userId, claim));

			var currentIdentity = GetCurrentIdentity();
			currentIdentity.RemoveClaims(claimType, v => string.Equals(claimValue, v));
		}


		// Roles

		public bool IsInRole(string role)
		{
			if (string.IsNullOrEmpty(role))
			{
				throw new ArgumentNullException("role");
			}

			return InvokeUserManager((m, userId) => m.IsInRoleAsync(userId, role));
		}

		public void AddToRole(string role)
		{
			if (string.IsNullOrEmpty(role))
			{
				throw new ArgumentNullException("role");
			}

			InvokeUserManager((m, userId) => m.AddToRoleAsync(userId, role));
			AddRolesToClaims(role);
		}

		public void AddToRoles(params string[] roles)
		{
			if (roles != null && roles.Length > 0)
			{
				InvokeUserManager((m, userId) => m.AddToRolesAsync(userId, roles));
				AddRolesToClaims(roles);
			}
		}

		public void AddToRoles(IEnumerable<string> roles)
		{
			if (roles != null)
			{
				var arrayRoles = roles.ToArray();

				if (arrayRoles.Length > 0)
				{
					InvokeUserManager((m, userId) => m.AddToRolesAsync(userId, arrayRoles));
					AddRolesToClaims(arrayRoles);
				}
			}
		}

		private static void AddRolesToClaims(params string[] roles)
		{
			var currentIdentity = GetCurrentIdentity();

			foreach (var role in roles)
			{
				currentIdentity.AddOrUpdateClaim(ClaimTypes.Role, role);
			}
		}

		public void RemoveFromRole(string role)
		{
			if (string.IsNullOrEmpty(role))
			{
				throw new ArgumentNullException("role");
			}

			InvokeUserManager((m, userId) => m.RemoveFromRoleAsync(userId, role));
			RemoveRolesFromClaims(role);

		}

		public void RemoveFromRoles(params string[] roles)
		{
			if (roles != null && roles.Length > 0)
			{
				InvokeUserManager((m, userId) => m.RemoveFromRolesAsync(userId, roles));
				RemoveRolesFromClaims(roles);
			}
		}

		public void RemoveFromRoles(IEnumerable<string> roles)
		{
			if (roles != null)
			{
				var rolesArray = roles.ToArray();

				if (rolesArray.Length > 0)
				{
					InvokeUserManager((m, userId) => m.RemoveFromRolesAsync(userId, rolesArray));
					RemoveRolesFromClaims(rolesArray);
				}
			}
		}

		private static void RemoveRolesFromClaims(params string[] roles)
		{
			var currentIdentity = GetCurrentIdentity();
			currentIdentity.RemoveClaims(ClaimTypes.Role, roles.Contains);
		}


		// Helpers

		private TResult InvokeUserManager<TResult>(Func<UserManager<IdentityApplicationUser>, string, Task<TResult>> actionForUserId)
		{
			var currentUserId = GetCurrentUserId();
			var task = actionForUserId(UserManager, currentUserId);
			return task.Result;
		}

		private void InvokeUserManager(Func<UserManager<IdentityApplicationUser>, string, Task<IdentityResult>> actionForUserId)
		{
			var currentUserId = GetCurrentUserId();
			var task = actionForUserId(UserManager, currentUserId);
			ThrowIfError(task);
		}

		private static string GetCurrentUserId()
		{
			var currentIdentity = GetCurrentIdentity();
			var currentUserId = currentIdentity.GetUserId();

			if (string.IsNullOrEmpty(currentUserId))
			{
				throw new InvalidOperationException(Resources.UserIdNotFound);
			}

			return currentUserId;
		}

		private static IIdentity GetCurrentIdentity()
		{
			if (Thread.CurrentPrincipal == null || Thread.CurrentPrincipal.Identity == null)
			{
				throw new InvalidOperationException(Resources.RequestIsNotAuthenticated);
			}

			return Thread.CurrentPrincipal.Identity;
		}

		private static void ThrowIfError(Task<IdentityResult> task)
		{
			var result = task.Result;

			if (!result.Succeeded)
			{
				throw new AggregateException(string.Join(Environment.NewLine, result.Errors));
			}
		}
	}
}