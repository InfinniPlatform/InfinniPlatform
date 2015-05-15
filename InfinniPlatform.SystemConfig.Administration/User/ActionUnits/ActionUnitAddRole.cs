using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.RestApi.AuthApi;
using InfinniPlatform.Api.RestApi.DataApi;

namespace InfinniPlatform.SystemConfig.Administration.User.ActionUnits
{
	/// <summary>
	///   Добавление новой роли в конфигурации администрирования
	/// </summary>
	public sealed class ActionUnitAddRole
	{
		public void Action(IApplyContext target)
		{
			var aclApi = target.Context.GetComponent<AclApi>();

			var role = target.Item.Document;

			if (role == null || string.IsNullOrEmpty(role.Name))
			{
				target.IsValid = false;
				target.ValidationMessage = "Role name is not specified";
				return;
			}

			var roleFound = aclApi.GetRoles().FirstOrDefault(r => r.Name.ToLowerInvariant() == role.Name.ToLowerInvariant());

			if (roleFound != null)
			{
				target.IsValid = false;
				target.ValidationMessage = "Role with name " + role.Name + " already exists.";
				return;
			}

			aclApi.AddRole(role.Name, role.Name, role.Name);

			target.Context.GetComponent<DocumentApi>()
				.SetDocument(AuthorizationStorageExtensions.AdministrationConfigId, "Role",
				   target.Item.Document);
		}
	}
}
