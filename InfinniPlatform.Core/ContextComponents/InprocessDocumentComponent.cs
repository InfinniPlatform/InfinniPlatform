using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.Core.ContextComponents
{
    /// <summary>
    /// Провайдер получения документов внутри серверного процесса (без обращения по REST)
    /// </summary>
    public sealed class InprocessDocumentComponent
    {
        public InprocessDocumentComponent(IConfigurationObjectBuilder configurationObjectBuilder, IIndexFactory indexFactory)
        {
            _configurationObjectBuilder = configurationObjectBuilder;
            _indexFactory = indexFactory;
        }


        private readonly IConfigurationObjectBuilder _configurationObjectBuilder;
        private readonly IIndexFactory _indexFactory;


        public IAllIndexesOperationProvider GetAllIndexesOperationProvider()
        {
            return _indexFactory.BuildAllIndexesOperationProvider();
        }

        public IVersionProvider GetDocumentProvider(string configId, string documentType)
        {
            var config = _configurationObjectBuilder.GetConfigurationObject(configId);

            var documentProvider = config?.GetDocumentProvider(documentType);

            return documentProvider;
        }
    }
}