using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Api.RestQuery.RestQueryBuilders;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers
{
	/// <summary>
	///   Удалить при дальнейшем рефакторинге системы
	/// </summary>
    public static class CommonMetadataApi 
    {

        /// <summary>
        ///   Получить доступные типы сервисов
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<dynamic> GetServiceTypeMetadata()
        {
            return RestQueryApi.QueryGetRaw("SystemConfig", "metadata", "getservicemetadata",null, 0, 1000).ToDynamicList();
        }

    }



}
