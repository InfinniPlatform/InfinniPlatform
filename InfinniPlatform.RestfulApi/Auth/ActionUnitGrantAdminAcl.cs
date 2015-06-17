using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.DataApi;

namespace InfinniPlatform.RestfulApi.Auth
{
	public sealed class ActionUnitGrantAdminAcl
	{

		public void Action(IApplyContext target)
		{
			var userName = target.Item.UserName;

			var aclApi = new AuthApi();

			aclApi.GrantAccess(userName, AuthorizationStorageExtensions.AuthorizationConfigId);
			aclApi.GrantAccess(userName, AuthorizationStorageExtensions.AdministrationConfigId);			

			//ищем конфигурацию авторизации и добавляем права на меню авторизации
            var metadataComponent = target.Context.GetComponent<IMetadataComponent>(target.Version);

			var menuAuth = metadataComponent.GetMetadata(target.Version, AuthorizationStorageExtensions.AuthorizationConfigId, "Common", MetadataType.Menu, "MainMenu");
			var menuAdmin = metadataComponent.GetMetadata(target.Version, AuthorizationStorageExtensions.AdministrationConfigId, "Common", MetadataType.Menu, "MainMenu");
			IEnumerable<dynamic> configs = metadataComponent.GetConfigMetadata(target.Version);

			var configAuth = configs.FirstOrDefault(c => c.Name == AuthorizationStorageExtensions.AuthorizationConfigId);
			var configAdmin = configs.FirstOrDefault(c => c.Name == AuthorizationStorageExtensions.AdministrationConfigId);

			if (menuAuth != null && menuAdmin != null)
			{
				aclApi.GrantAccess(userName, "SystemConfig", "Metadata", "GetDocument",
								   menuAuth.Id);
				aclApi.GrantAccess(userName, "SystemConfig", "Metadata", "GetDocument",
								   menuAdmin.Id);

				aclApi.GrantAccess(userName, "SystemConfig", "MenuMetadata", "GetDocument",
								   menuAuth.Id);
				aclApi.GrantAccess(userName, "SystemConfig", "MenuMetadata", "GetDocument",
								   menuAdmin.Id);
				aclApi.DenyAccessAll("SystemConfig", "MenuMetadata", "GetDocument",
								   menuAuth.Id);
				aclApi.DenyAccessAll("SystemConfig", "MenuMetadata", "GetDocument",
								   menuAdmin.Id);

			}

			if (configAdmin != null && configAuth != null)
			{
				aclApi.GrantAccess(userName, "SystemConfig", "Metadata", "GetDocument",
									configAdmin.Id);
				aclApi.GrantAccess(userName, "SystemConfig", "Metadata", "GetDocument",
								   configAuth.Id);

				aclApi.GrantAccess(userName, "SystemConfig", "Metadata", "GetDocument",
								   configAdmin.Id);
				aclApi.GrantAccess(userName, "SystemConfig", "Metadata", "GetDocument",
								   configAuth.Id);
				aclApi.DenyAccessAll("SystemConfig", "Metadata", "GetDocument",
								   configAdmin.Id);
				aclApi.DenyAccessAll("SystemConfig", "Metadata", "GetDocument",
								   configAuth.Id);
			}
		}
	}
}
