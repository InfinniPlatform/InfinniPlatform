using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.AuthApi;

namespace InfinniPlatform.ContextComponents
{
	/// <summary>
	///   Компонент безопасности глобалього контекста
	/// </summary>
	public sealed class CachedSecurityComponent : ISecurityComponent
	{
		private static IEnumerable<dynamic> _roles;
		private static IEnumerable<dynamic> _acl;
		private static IEnumerable<dynamic> _users;


		public void UpdateRoles()
		{
			InternalUpdateRoles();
		}

		private static void InternalUpdateRoles()
		{
			_roles = new AclApi().GetUserRoles();
		}

		public void UpdateUsers()
		{
			InternalUpdateUsers();
		}

		private static void InternalUpdateUsers()
		{
			_users = new AclApi().GetUsers();
		}

		public IEnumerable<dynamic> Roles
		{
			get
			{
				return _roles;
			}
		}


		public void UpdateAcl()
		{
			InternalUpdateAcl();
		}

		private static void InternalUpdateAcl()
		{
			_acl = new AclApi().GetAcl();
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
			InternalUpdateRoles();
			InternalUpdateAcl();
		}
	}
}
