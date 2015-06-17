﻿using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.SystemConfig.Configurator.ActionUnitsMetadataDataSource
{
    public sealed class ActionUnitGetConfigurationMetadata
    {
        public void Action(IApplyResultContext target)
        {
            dynamic bodyQuery = new DynamicWrapper();
            bodyQuery.ConfigId = target.Item.ConfigId;

            var response = RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getmetadata", null, bodyQuery, target.Version);
            IEnumerable<dynamic> result = DynamicWrapperExtensions.ToEnumerable(response.ToDynamic().QueryResult);
            target.Result = result.Select(r => r.Result).FirstOrDefault();
        }
    }
}
