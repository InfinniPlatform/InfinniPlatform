﻿using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.SystemConfig.Properties;

namespace InfinniPlatform.SystemConfig.Configurator
{
    /// <summary>
    ///     Удаление объекта метаданных
    /// </summary>
    public sealed class ActionUnitDeleteMetadata
    {
        public void Action(IApplyContext target)
        {
            target.Context.GetComponent<DocumentApi>()
                  .DeleteDocument("systemconfig", target.Metadata, target.Item.Id);

            target.Result = new DynamicWrapper();
            target.Result.Name = target.Item.Name;
            target.Result.IsValid = true;
            target.Result.ValidationMessage = Resources.MetadataObjectDeleted;
        }
    }
}