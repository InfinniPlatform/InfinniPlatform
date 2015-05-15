using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.RestApi.AuthApi;

namespace InfinniPlatform.ContextComponents
{
	/// <summary>
	///   Провайдер получения документов внутри серверного процесса (без обращения по REST)
	/// </summary>
	public sealed class InprocessDocumentComponent
	{
		private readonly IConfigurationMediatorComponent _configurationMediatorComponent;
		private readonly ISecurityComponent _securityComponent;

		public InprocessDocumentComponent(IConfigurationMediatorComponent configurationMediatorComponent,
		                                 ISecurityComponent securityComponent)
		{
			_configurationMediatorComponent = configurationMediatorComponent;
			_securityComponent = securityComponent;
		}

		public IVersionProvider GetDocumentProvider(string configId, string documentId, string userName)
		{
			//получаем конструктор метаданных конфигураций
			var configBuilder =_configurationMediatorComponent.ConfigurationBuilder;

			//получаем конфигурацию, указанную в метаданных запроса
			var config = configBuilder.GetConfigurationObject(configId);

			if (config != null)
			{

				return config.GetDocumentProvider(documentId,_securityComponent.GetClaim(AuthorizationStorageExtensions.OrganizationClaim,userName) ?? AuthorizationStorageExtensions.AnonimousUser);
			}
			return null;
		}

	}
}
