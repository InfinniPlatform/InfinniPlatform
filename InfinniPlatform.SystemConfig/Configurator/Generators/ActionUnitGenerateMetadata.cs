using System;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.SystemConfig.Properties;

namespace InfinniPlatform.SystemConfig.Configurator.Generators
{
    public sealed class ActionUnitGenerateMetadata
    {
        public void Action(IApplyContext target)
        {
            //ищем зарегистрированный генератор
            if (target.Item.Metadata == null || string.IsNullOrEmpty(target.Item.GeneratorName))
            {
                throw new ArgumentException(Resources.ErrorMetadataGeneratorNotSpecified);
            }

            Func<dynamic, bool> generatorSelector = generator => generator.Name == target.Item.GeneratorName;

            //получаем метаданные генератора по указанному наименованию
            dynamic generatorMetadata =
                target.Context.GetComponent<IMetadataComponent>()
                      .GetMetadataItem(target.Context.GetVersion(target.Item.Configuration,target.UserName), target.Item.Configuration, target.Item.Metadata,
                                       MetadataType.Generator, generatorSelector);
                //generatorReader.GetItem(target.Item.GeneratorName);

            if (generatorMetadata != null)
            {
                //получаем JSON метаданных, сгенерированный соответствующим сервисом, указанным в метаданных генератора
                RestQueryResponse response = RestQueryApi.QueryPostJsonRaw(target.Item.Configuration,
                                                                           target.Item.Metadata,
                                                                           generatorMetadata.Service,
                                                                           null, target.Item.Parameters);
                target.Result = response.Content.ToDynamic();
                target.ValidationMessage = "Metadata successfully generated.";
            }
            else
            {
                target.Result = new DynamicWrapper();
                target.Result.ErrorMessage = string.Format(Resources.GeneratorMetadataNotFound,
                                                           target.Item.GeneratorName);
            }
        }
    }
}