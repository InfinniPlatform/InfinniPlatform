using System;

using InfinniPlatform.Api.Deprecated;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Hosting;
using InfinniPlatform.SystemConfig.Properties;

namespace InfinniPlatform.SystemConfig.Configurator.Generators
{
    /// <summary>
    /// Модуль создания нового генератора метаданных
    /// </summary>
    public sealed class ActionUnitCreateGenerator
    {
        public ActionUnitCreateGenerator(RestQueryApi restQueryApi)
        {
            _restQueryApi = restQueryApi;
        }

        private readonly RestQueryApi _restQueryApi;

        public void Action(IApplyContext target)
        {
            if (string.IsNullOrEmpty(target.Item.Configuration))
            {
                throw new ArgumentException(Resources.ErrorConfigurationNameNotFound);
            }

            if (string.IsNullOrEmpty(target.Item.GeneratorName))
            {
                throw new ArgumentException(Resources.ErrorCreatedGeneratorNameNotSpecified);
            }

            if (string.IsNullOrEmpty(target.Item.ActionUnit))
            {
                throw new ArgumentException(Resources.ErrorActionModuleNotSpecified);
            }

            if (string.IsNullOrEmpty(target.Item.MetadataType))
            {
                throw new ArgumentException(Resources.ErrorGeneratorMetadataTypeNotSpecified);
            }

            if (string.IsNullOrEmpty(target.Item.Metadata))
            {
                throw new ArgumentException(Resources.ErrorMetadataNotSpecified);
            }

            //генерируем сервис
            _restQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "generateservicewithoutstate", null, new
                                                                                                            {
                                                                                                                ActionName = target.Item.GeneratorName,
                                                                                                                target.Item.Configuration,
                                                                                                                target.Item.ActionUnit,
                                                                                                                ContextTypeKind = ContextTypeKind.ApplyMove,
                                                                                                                target.Item.Metadata
                                                                                                            });
            //создаем генератор
            dynamic generator = MetadataBuilderExtensions.BuildGenerator(target.Item.GeneratorName,
                target.Item.GeneratorName,
                target.Item.ActionUnit,
                target.Item.MetadataType);

            var manager =
                new ManagerFactoryDocument(target.Item.Configuration, target.Item.Metadata)
                    .BuildGeneratorManager();

            //создаем новый генератор в конфигурации
            manager.MergeItem(generator);
        }
    }
}