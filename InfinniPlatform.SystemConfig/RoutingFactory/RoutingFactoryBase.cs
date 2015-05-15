using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.RestApi.AuthApi;
using InfinniPlatform.Factories;

namespace InfinniPlatform.SystemConfig.RoutingFactory
{
	public sealed class RoutingFactoryBase : IIndexRoutingFactory
	{
		public string GetRouting(string userRouting, string indexName, string indexType)
		{
			if (indexName.ToLowerInvariant() == "systemconfig" || indexName.ToLowerInvariant() == "authorization" || indexName.ToLowerInvariant() == "restfulapi" || indexName.ToLowerInvariant() == "update")
			{
				return RoutingExtensions.SystemRouting;
			}
			if (string.IsNullOrEmpty(userRouting))
			{
				return AuthorizationStorageExtensions.AnonimousUser;
			}

			return userRouting;
		}
	}
}
