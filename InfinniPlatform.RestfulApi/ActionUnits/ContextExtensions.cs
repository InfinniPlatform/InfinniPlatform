using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.RestApi.AuthApi;
using InfinniPlatform.Index;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
	public sealed class ContextExtensions
	{
		private readonly IApplyContext _target;
		public ContextExtensions(IApplyContext target)
		{
			_target = target;
		}

		public IVersionProvider GetDocumentProvider()
		{

			//получаем конструктор метаданных конфигураций
			var configBuilder = _target.Context.GetComponent<IConfigurationMediatorComponent>().ConfigurationBuilder;

			//получаем конфигурацию, указанную в метаданных запроса
			var config = configBuilder.GetConfigurationObject(_target.Item.Configuration);

			if (config != null)
			{

				return config.GetDocumentProvider(_target.Item.Metadata,
				                                  _target.Context.GetComponent<ISecurityComponent>().GetClaim(AuthorizationStorageExtensions.OrganizationClaim,
				                                                           _target.UserName));
			}
			return null;
		}

	}
}
