using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.Configurator.Generators
{
    /// <summary>
    /// Удаление генератора метаданных
    /// </summary>
    public sealed class ActionUnitDeleteGenerator
    {
        public ActionUnitDeleteGenerator(RestQueryApi restQueryApi)
        {
            _restQueryApi = restQueryApi;
        }

        private readonly RestQueryApi _restQueryApi;

        public void Action(IApplyContext target)
        {
            //удаляем сгенерированный для генератора автоматически сервис
            var body = new
                       {
                           ActionName = target.Item.GeneratorName,
                           target.Item.Configuration,
                           target.Item.Metadata
                       };

            _restQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "deletegeneratedservice", null, target.Item);

            //удаляем метаданные самого генератора
            var generatorManager =
                new ManagerFactoryDocument(target.Item.Configuration, target.Item.Metadata)
                    .BuildGeneratorManager();

            dynamic generator = generatorManager.MetadataReader.GetItem(target.Item.GeneratorName);

            if (generator != null)
            {
                generatorManager.DeleteItem(generator);
            }
        }
    }
}