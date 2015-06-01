using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.ContextComponents
{
	/// <summary>
	///   Компонент безопасности глобального контекста
	/// </summary>
	public interface ISecurityComponent
	{
		void UpdateUserRoles();
		void UpdateUsers();
		void UpdateAcl();

	    void UpdateRoles();

		IEnumerable<dynamic> Acl { get; }
		IEnumerable<dynamic> Users { get; }
		IEnumerable<dynamic> Roles { get; }

        IEnumerable<dynamic> UserRoles { get;  }

		string GetClaim(string claimType, string userName);
		void UpdateClaim(string userName, string claimType, string claimValue);
	}
}
