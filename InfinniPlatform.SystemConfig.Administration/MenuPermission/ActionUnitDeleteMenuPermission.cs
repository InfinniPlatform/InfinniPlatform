using System;

using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.RestApi.AuthApi;
using InfinniPlatform.Api.RestApi.DataApi;

namespace InfinniPlatform.SystemConfig.Administration.MenuPermission
{
	public sealed class ActionUnitDeleteMenuPermission
	{
		public void Action(IApplyContext target)
		{
			if (target.Item.Document.Menu == null)
			{
				target.IsValid = false;
				target.ValidationMessage = "Menu to delete permission should not be empty.";
				return;
			}

			var documentApi = target.Context.GetComponent<DocumentApi>();

			var aclApi = target.Context.GetComponent<AclApi>();

			try
			{
				//запрещаем доступ пользователю
				aclApi.DenyAccess(target.Item.Document.Role.DisplayName, "SystemConfig", "MenuMetadata", "getdocument",
								   target.Item.Document.Menu.Id);

			}
			catch (Exception e)
			{
				target.IsValid = false;
				target.ValidationMessage = string.Format("User {0} access denied to change user access. ", target.UserName);
				return;
			}

			documentApi.DeleteDocument("Administration", "MenuPermission", target.Item.Document.Id);
		}
	}
}
