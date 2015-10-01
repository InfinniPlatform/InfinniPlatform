using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Configurator.ActionUnitsMetadataDataSource
{
    /// <summary>
    ///   Получить список метаданных решений
    /// </summary>
    public sealed class ActionUnitGetSolutionList
    {
        public void Action(IApplyResultContext target)
        {
            target.Result = new DynamicWrapper();
            target.Result.SolutionList = QueryMetadata.QueryConfiguration(QueryMetadata.GetSolutionListIql(),true);
        }
    }
}
