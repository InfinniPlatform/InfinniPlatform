using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.Versioning;
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
        private readonly ISecurityComponent _securityComponent;

        public InprocessDocumentComponent(IConfigurationMediatorComponent configurationMediatorComponent,
            ISecurityComponent securityComponent, IIndexFactory indexFactory)
        {
            _configurationMediatorComponent = configurationMediatorComponent;
            _securityComponent = securityComponent;
            _indexFactory = indexFactory;

        }

        public IAllIndexesOperationProvider GetAllIndexesOperationProvider(string userName)
        {
            return _indexFactory.BuildAllIndexesOperationProvider(GetUserRouting(userName));
        }

        private string GetUserRouting(string userName)
        {
            return _securityComponent.GetClaim(AuthorizationStorageExtensions.OrganizationClaim, userName) ??
                   AuthorizationStorageExtensions.AnonimousUser;
        }

        public IVersionProvider GetDocumentProvider(string version, string configId, string documentId, string userName)
        {
            //получаем конструктор метаданных конфигураций
            var configBuilder = _configurationMediatorComponent.ConfigurationBuilder;

            //получаем конфигурацию, указанную в метаданных запроса
            var config = configBuilder.GetConfigurationObject(version, configId);

            if (config != null)
            {
                return config.GetDocumentProvider(documentId, version, GetUserRouting(userName));
            }
            return null;
        }
    }
}