using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.SystemConfig.UserStorage;

namespace InfinniPlatform.RestfulApi.Auth
{
	/// <summary>
	///   Модуль удаления роли у пользователя
	/// </summary>
	public sealed class ActionUnitRemoveUserRole
	{
		public void Action(IApplyContext target)
		{
			var storage = new ApplicationUserStorePersistentStorage();
			var user = storage.FindUserByName(target.Item.UserName);
			if (user == null)
			{
				throw new ArgumentException(string.Format("User {0} not found", target.Item.UserName));
			}

			storage.RemoveUserFromRole(user, target.Item.RoleName);
			target.Context.GetComponent<ISecurityComponent>().UpdateAcl();
			target.Result = new DynamicWrapper();
			target.Result.IsValid = true;
			target.Result.ValidationMessage = "Role deleted.";
		}
	}
}
