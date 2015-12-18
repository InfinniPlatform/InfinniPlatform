using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.Auth
{
    public sealed class ActionUnitGrantAdminAcl
    {
        public ActionUnitGrantAdminAcl(AuthApi authApi)
        {
            _authApi = authApi;
        }

        private readonly AuthApi _authApi;

        public void Action(IApplyContext target)
        {
            var userName = target.Item.UserName;

            _authApi.GrantAccess(userName, AuthorizationStorageExtensions.AuthorizationConfigId);
            _authApi.GrantAccess(userName, AuthorizationStorageExtensions.AdministrationConfigId);

            //ищем конфигурацию авторизации и добавляем права на меню авторизации
            var metadataComponent = target.Context.GetComponent<IMetadataComponent>();

            var menuAuth = metadataComponent.GetMetadata(AuthorizationStorageExtensions.AuthorizationConfigId, "Common",
                MetadataType.Menu, "MainMenu");
            var menuAdmin = metadataComponent.GetMetadata(AuthorizationStorageExtensions.AdministrationConfigId,
                "Common", MetadataType.Menu, "MainMenu");
            IEnumerable<dynamic> configs = metadataComponent.GetConfigMetadata();

            dynamic configAuth =
                configs.FirstOrDefault(c => c.Name == AuthorizationStorageExtensions.AuthorizationConfigId);
            dynamic configAdmin =
                configs.FirstOrDefault(c => c.Name == AuthorizationStorageExtensions.AdministrationConfigId);

            if (menuAuth != null && menuAdmin != null)
            {
                _authApi.GrantAccess(userName, "SystemConfig", "Metadata", "GetDocument", menuAuth.Id);
                _authApi.GrantAccess(userName, "SystemConfig", "Metadata", "GetDocument", menuAdmin.Id);
                _authApi.GrantAccess(userName, "SystemConfig", "MenuMetadata", "GetDocument", menuAuth.Id);
                _authApi.GrantAccess(userName, "SystemConfig", "MenuMetadata", "GetDocument", menuAdmin.Id);
                _authApi.DenyAccessAll("SystemConfig", "MenuMetadata", "GetDocument", menuAuth.Id);
                _authApi.DenyAccessAll("SystemConfig", "MenuMetadata", "GetDocument", menuAdmin.Id);
            }

            if (configAdmin != null && configAuth != null)
            {
                _authApi.GrantAccess(userName, "SystemConfig", "Metadata", "GetDocument", configAdmin.Id);
                _authApi.GrantAccess(userName, "SystemConfig", "Metadata", "GetDocument", configAuth.Id);
                _authApi.GrantAccess(userName, "SystemConfig", "Metadata", "GetDocument", configAdmin.Id);
                _authApi.GrantAccess(userName, "SystemConfig", "Metadata", "GetDocument", configAuth.Id);
                _authApi.DenyAccessAll("SystemConfig", "Metadata", "GetDocument", configAdmin.Id);
                _authApi.DenyAccessAll("SystemConfig", "Metadata", "GetDocument", configAuth.Id);
            }
        }
    }
}