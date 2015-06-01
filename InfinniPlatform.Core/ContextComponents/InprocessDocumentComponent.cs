using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.RestApi.Auth;

namespace InfinniPlatform.ContextComponents
{
	/// <summary>
	///   Провайдер получения документов внутри серверного процесса (без обращения по REST)
	/// </summary>
	public sealed class InprocessDocumentComponent
	{
		private readonly IConfigurationMediatorComponent _configurationMediatorComponent;
		private readonly ISecurityComponent _securityComponent;
	    private readonly IIndexFactory _indexFactory;

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

		public IVersionProvider GetDocumentProvider(string configId, string documentId, string userName)
		{
			//получаем конструктор метаданных конфигураций
			var configBuilder =_configurationMediatorComponent.ConfigurationBuilder;

			//получаем конфигурацию, указанную в метаданных запроса
			var config = configBuilder.GetConfigurationObject(configId);

			if (config != null)
			{

				return config.GetDocumentProvider(documentId,GetUserRouting(userName));
			}
			return null;
		}

	}
}
