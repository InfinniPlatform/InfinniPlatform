using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Factories;

namespace InfinniPlatform.SystemConfig.RoutingFactory
{
	public sealed class RoutingFactoryBase : IIndexRoutingFactory
	{
	    /// <summary>
	    ///   Получить роутинг запросов к индексу для указанных типов
	    /// </summary>
	    /// <param name="userRouting">Роутинг, предоставленный пользователем</param>
	    /// <param name="indexName">Индекс для формирования роутинга</param>
	    /// <param name="indexType">Тип в индексе для формирования роутинга</param>
	    /// <returns>Строка роутинга для запросов к индексу</returns>
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

        /// <summary>
        ///   Получить роутинг для формирования запроса к данным без указания конкретного индекса и типа (запроса по всем индексам и типам)
        /// </summary>
        /// <param name="userRouting">Роутинг, сопоставленный пользователю</param>
        /// <returns>Результирующий роутинг пользователя</returns>
	    public string GetRoutingUnspecifiedType(string userRouting)
	    {
            if (string.IsNullOrEmpty(userRouting))
            {
                return AuthorizationStorageExtensions.AnonimousUser;
            }

            return userRouting;	        
	    }
	}
}
