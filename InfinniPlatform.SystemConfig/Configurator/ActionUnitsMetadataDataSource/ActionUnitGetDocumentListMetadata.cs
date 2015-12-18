using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.Configurator.ActionUnitsMetadataDataSource
{
    [Obsolete]
    public sealed class ActionUnitGetDocumentListMetadata
    {
        public void Action(IApplyResultContext target)
        {
            target.Result = new List<object>();
        }
    }
}