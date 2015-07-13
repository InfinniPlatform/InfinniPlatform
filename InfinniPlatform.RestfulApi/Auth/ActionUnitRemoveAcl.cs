using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.SystemConfig.UserStorage;

namespace InfinniPlatform.RestfulApi.Auth
{
	/// <summary>
	///   Удалить ACL
	/// </summary>
	public sealed class ActionUnitRemoveAcl
	{
		public void Action(IApplyContext target)
		{
			var storage = new ApplicationUserStorePersistentStorage();
			storage.RemoveAcl(target.Item.AclId);
			target.Context.GetComponent<ISecurityComponent>().UpdateAcl();
		}
	}
}
