using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.Configurator
{
    /// <summary>
    /// Получить список неактуальных конфигураций
    /// </summary>
    public sealed class ActionUnitGetIrrelevantVersions
    {
        public void Action(IApplyContext target)
        {
            target.Result = target.Context.GetComponent<IVersionStrategy>().GetIrrelevantVersionList(
                target.Context.GetComponent<IMetadataConfigurationProvider>().ConfigurationVersions, target.UserName);
        }
    }
}
