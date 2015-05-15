using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.SearchOptions;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers
{
    public sealed class ManagerIdentifiersStandard : IManagerIdentifiers
    {

	    private static IEnumerable<dynamic> ConfigList
	    {
		    get
		    {
			    return DynamicWrapperExtensions.ToEnumerable(
				            RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "getregisteredconfiglist", null, null)
				                        .ToDynamic()
				                        .ConfigList);
		    }
	    }

        /// <summary>
        ///   Получить идентификатор элемента конфигурации
        /// </summary>
        /// <param name="name">Наименование элемента</param>
        /// <returns>Идентификатор элемента</returns>
        public string GetConfigurationUid(string name)
        {
			return ConfigList.Where(c => c.Name.ToLowerInvariant() == name.ToLowerInvariant()).Select(c => c.Id).FirstOrDefault();
        }


        /// <summary>
        ///   Получить идентификатор документа
        /// </summary>
        public string GetDocumentUid(string configurationId, string documentId)
        {
			var config = ConfigList.FirstOrDefault(c => c.Name.ToLowerInvariant() == configurationId.ToLowerInvariant());
			if (config != null)
			{
				IEnumerable<dynamic> documents = config.Documents;
				foreach (var document in documents)
				{
					if (document.Name.ToLowerInvariant() == documentId.ToLowerInvariant())
					{
						return document.Id;
					}
				}

			}
	        return null;
        }
    }
}
