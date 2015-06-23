using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.Configurator.Generators
{
    /// <summary>
    ///     Удалить сгенерированный сервис генератора метаданных
    /// </summary>
    public sealed class ActionUnitDeleteGeneratedService
    {
        public void Action(IApplyContext target)
        {
            var metadataFactory = new ManagerFactoryDocument(target.Version, target.Item.Configuration,
                                                             target.Item.Metadata);

            MetadataManagerElement scenarioManager = metadataFactory.BuildScenarioManager();
            MetadataManagerElement processManager = metadataFactory.BuildProcessManager();
            MetadataManagerElement serviceManager = metadataFactory.BuildServiceManager();

            //удаляем сгенерированный сценарий	

            dynamic scenario = scenarioManager.MetadataReader.GetItem(target.Item.GeneratorName);
            scenarioManager.DeleteItem(scenario);

            //удаляем сгенерированный процесс
            dynamic process = processManager.MetadataReader.GetItem(target.Item.GeneratorName);
            processManager.DeleteItem(process);

            //удаляем сгенерированный сервис
            dynamic service = serviceManager.MetadataReader.GetItem(target.Item.GeneratorName);
            serviceManager.DeleteItem(service);
        }
    }
}