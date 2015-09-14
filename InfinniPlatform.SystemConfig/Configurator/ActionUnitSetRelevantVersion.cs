using System.Linq;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Configurator
{
    public sealed class ActionUnitSetRelevantVersion
    {
        public void Action(IApplyContext target)
        {
            var isAdminRole =
                target.Context.GetComponent<ISecurityComponent>()
                      .UserRoles.FirstOrDefault(f => f.RoleName == AuthorizationStorageExtensions.AdminRole) != null;
            if (target.UserName == target.Item.UserName || isAdminRole)
            {
                target.Context.GetComponent<IVersionStrategy>()
                                      .SetRelevantVersion(target.Item.Version, target.Item.ConfigurationId, target.Item.UserName);
                target.Result = new DynamicWrapper();
                target.Result.IsValid = true;
            }
            else
            {
                target.Result = new DynamicWrapper();
                target.Result.IsValid = false;
                target.Result.ValidationMessage = string.Format(Resources.DoesNotAllowedToSetVersion,
                                                                target.Item.UserName);
            }
        }
    }
}
