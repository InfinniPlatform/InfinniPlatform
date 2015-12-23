using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.ContextComponents
{
    /// <summary>
    /// Провайдер получения документов внутри серверного процесса (без обращения по REST)
    /// </summary>
    public sealed class InprocessDocumentComponent
    {
        public InprocessDocumentComponent(IConfigurationMediatorComponent configurationMediatorComponent, IIndexFactory indexFactory)
        {
            _configurationMediatorComponent = configurationMediatorComponent;
            _indexFactory = indexFactory;
        }


        private readonly IConfigurationMediatorComponent _configurationMediatorComponent;
        private readonly IIndexFactory _indexFactory;


        public IAllIndexesOperationProvider GetAllIndexesOperationProvider()
        {
            return _indexFactory.BuildAllIndexesOperationProvider();
        }

        public IVersionProvider GetDocumentProvider(string configId, string documentType)
        {
            // получаем конструктор метаданных конфигураций
            var configBuilder = _configurationMediatorComponent.ConfigurationBuilder;

            // получаем конфигурацию, указанную в метаданных запроса
            var config = configBuilder.GetConfigurationObject(configId);

            var documentProvider = config?.GetDocumentProvider(documentType);

            return documentProvider;
        }
    }
}