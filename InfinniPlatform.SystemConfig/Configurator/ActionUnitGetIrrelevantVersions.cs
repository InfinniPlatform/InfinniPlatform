using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Configurator
{
    /// <summary>
    /// Получить список неактуальных конфигураций
    /// </summary>
    public sealed class ActionUnitGetIrrelevantVersions
    {
        public void Action(IApplyContext target)
        {
            var isAdminRole = target.Context.GetComponent<ISecurityComponent>().UserRoles.FirstOrDefault(f => f.RoleName == AuthorizationStorageExtensions.AdminRole) != null;
            if (target.UserName == target.Item.UserName || isAdminRole)
            {
                target.Result = target.Context.GetComponent<IVersionStrategy>().GetIrrelevantVersionList(
                    target.Context.GetComponent<IMetadataConfigurationProvider>().ConfigurationVersions, target.Item.UserName);
            }
            else
            {
                target.Result = new DynamicWrapper();
                target.Result.IsValid = false;
                target.Result.ValidationMessage = string.Format(string.Format(Resources.FailToGetIrrelevantVersions),
                                                                target.Item.UserName);
            }
        }
    }
}
