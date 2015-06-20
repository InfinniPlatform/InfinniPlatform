﻿using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Api.Metadata.MetadataContainers;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.Application.Contracts;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.SystemConfig.Configurator.ActionUnitsMetadataDataSource
{
    public sealed class ActionUnitGetDocumentListMetadata
    {
        public void Action(IApplyResultContext target)
        {
            dynamic bodyQuery =
                DynamicWrapperExtensions.ToDynamic(
                    QueryMetadata.GetConfigurationMetadataShortListIql(target.Item.ConfigId,
                                                                       new MetadataContainerDocument()
                                                                           .GetMetadataContainerName()));

            dynamic response = RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getmetadata", null, bodyQuery,
                                                             target.Version);

            IEnumerable<dynamic> queryResult = DynamicWrapperExtensions.ToEnumerable(response.ToDynamic().QueryResult);

            if (queryResult.Any())
            {
                target.Result = queryResult.First().Result.Documents;
            }
            else
            {
                target.Result = new List<dynamic>();
            }
        }
    }
}