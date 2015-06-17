using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.Auth;

namespace InfinniPlatform.ContextComponents
{
	/// <summary>
	///   Компонент безопасности глобалього контекста
	/// </summary>
	public sealed class CachedSecurityComponent : ISecurityComponent
	{
		private static IEnumerable<dynamic> _userRoles;
		private static IEnumerable<dynamic> _acl;
		private static IEnumerable<dynamic> _users;
	    private static IEnumerable<dynamic> _roles;

		public void UpdateUserRoles()
		{
			InternalUpdateUserRoles();
		}

		private static void InternalUpdateUserRoles()
		{
			_userRoles = new AuthApi(null).GetUserRoles(false);
		}

		public void UpdateUsers()
		{
			InternalUpdateUsers();
		}

		private static void InternalUpdateUsers()
		{
			_users = new AuthApi(null).GetUsers(false);
		}

		public IEnumerable<dynamic> Roles
		{
			get
			{
				return _roles;
			}
		}

	    public IEnumerable<dynamic> UserRoles
	    {
	        get { return _userRoles; }
	    }


	    public void UpdateAcl()
		{
			InternalUpdateAcl();
		}

	    public void UpdateRoles()
	    {
	        InternalUpdateRoles();
	    }

	    private static void InternalUpdateAcl()
		{
			_acl = new AuthApi(null).GetAcl(false);
		}

	    private static void InternalUpdateRoles()
	    {
	        _roles = new AuthApi(null).GetRoles(false);
	    }

		public IEnumerable<dynamic> Acl
		{
			get
			{
				return _acl;
			}
		}

		public IEnumerable<dynamic> Users
		{
			get { return _users; }
		}


		public void UpdateClaim(string userName, string claimType, string claimValue)
		{
			if (_users != null)
			{
				var user = _users.FirstOrDefault(u => u.UserName == userName);
				if (user != null)
				{
					dynamic claim = null;
					if (user.Claims == null)
					{
						user.Claims = new List<dynamic>();
					}
					else
					{
						claim = ((IEnumerable<dynamic>) user.Claims).FirstOrDefault(c => c.Type.DisplayName == claimType);
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


					List<dynamic> users = _users.Where(u => u.UserName != userName).ToList();
					users.Add(user);
					_users = users;
				}
			}
		}


		public string GetClaim(string claimType, string userName)
		{
			if (_users != null)
			{
				var user = _users.FirstOrDefault(u => u.UserName == userName);
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

		/// <summary>
		///   Прогрев Acl на старте сервера
		/// </summary>
		public static void WarmUpAcl()
		{
			InternalUpdateUsers();
			InternalUpdateUserRoles();
			InternalUpdateAcl();
            InternalUpdateRoles();
		}
	}
}
