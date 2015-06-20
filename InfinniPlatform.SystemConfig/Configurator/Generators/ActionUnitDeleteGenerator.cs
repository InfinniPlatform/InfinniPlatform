using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.Application.Contracts;

namespace InfinniPlatform.SystemConfig.Configurator.Generators
{
    /// <summary>
    ///     Удаление генератора метаданных
    /// </summary>
    public sealed class ActionUnitDeleteGenerator
    {
        public void Action(IApplyContext target)
        {
            //удаляем сгенерированный для генератора автоматически сервис
            var body = new
                {
                    ActionName = target.Item.GeneratorName,
                    target.Item.Configuration,
                    target.Item.Metadata,
                };

            RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "deletegeneratedservice", null, target.Item,
                                          target.Version);

            //удаляем метаданные самого генератора
            MetadataManagerElement generatorManager =
                new ManagerFactoryDocument(target.Version, target.Item.Configuration, target.Item.Metadata)
                    .BuildGeneratorManager();

            dynamic generator = generatorManager.MetadataReader.GetItem(target.Item.GeneratorName);

            if (generator != null)
            {
                generatorManager.DeleteItem(generator);
            }
        }
    }
}