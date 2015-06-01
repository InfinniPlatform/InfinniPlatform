using System;

using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.DataApi;

namespace InfinniPlatform.SystemConfig.Administration.MenuPermission
{
	public sealed class ActionUnitSaveMenuPermission
	{
		public void Action(IApplyContext target)
		{
			if (target.Item.Document.Role == null)
			{
				target.IsValid = false;
				target.ValidationMessage = "Role for menu should not be empty.";
				return;
			}

			var documentApi = target.Context.GetComponent<DocumentApi>();

			var aclApi = target.Context.GetComponent<AuthApi>();

			try
			{
				//предоставляем доступ одному пользователю
				aclApi.GrantAccess(target.Item.Document.Role.DisplayName, "SystemConfig", "MenuMetadata", "getdocument",
								   target.Item.Document.Menu.Id);

				//добавляем права администратору
				aclApi.GrantAccess(AuthorizationStorageExtensions.AdminRole, "SystemConfig", "MenuMetadata", "getdocument",
								   target.Item.Document.Menu.Id);

				//запрещаем доступ всем остальным пользователям к меню
				aclApi.DenyAccess("Default", "SystemConfig", "MenuMetadata", "GetDocument",
				                  target.Item.Document.Menu.Id);
			}
			catch (Exception e)
			{
				target.IsValid = false;
				target.ValidationMessage = string.Format("User {0} access denied to change user access.", target.UserName);
				return;
				
			}

			documentApi.SetDocument("Administration", "MenuPermission", target.Item.Document);
		}
	}
}
