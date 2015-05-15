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
		void UpdateRoles();
		void UpdateUsers();
		void UpdateAcl();
		IEnumerable<object> Acl { get; }
		IEnumerable<object> Users { get; }
		IEnumerable<object> Roles { get; }
		string GetClaim(string claimType, string userName);
		void UpdateClaim(string userName, string claimType, string claimValue);
	}
}
