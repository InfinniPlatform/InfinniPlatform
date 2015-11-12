using InfinniPlatform.Factories;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.ContextComponents
{
    /// <summary>
    ///     Провайдер получения документов внутри серверного процесса (без обращения по REST)
    /// </summary>
    public sealed class InprocessDocumentComponent
    {
        private readonly IConfigurationMediatorComponent _configurationMediatorComponent;
        private readonly IIndexFactory _indexFactory;

	    public InprocessDocumentComponent(IConfigurationMediatorComponent configurationMediatorComponent, IIndexFactory indexFactory)
        {
            _configurationMediatorComponent = configurationMediatorComponent;
	        _indexFactory = indexFactory;

        }

        public IAllIndexesOperationProvider GetAllIndexesOperationProvider()
        {
            return _indexFactory.BuildAllIndexesOperationProvider();
        }

        private string GetUserRouting()
        {
			return GlobalContext.GetTenantId();
		}

        public IVersionProvider GetDocumentProvider(string version, string configId, string documentId, string userName)
        {
            //получаем конструктор метаданных конфигураций
            var configBuilder = _configurationMediatorComponent.ConfigurationBuilder;

            //получаем конфигурацию, указанную в метаданных запроса
            var config = configBuilder.GetConfigurationObject(configId);

            if (config != null)
            {
                return config.GetDocumentProvider(documentId);
            }
            return null;
        }
    }
}