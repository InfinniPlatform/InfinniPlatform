using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders
{
	public sealed class MetadataReaderConfiguration : IDataReader
	{
		public IEnumerable<dynamic> GetItems()
		{
            return DynamicWrapperExtensions.ToEnumerable(RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "getregisteredconfiglist", null, null).ToDynamic().ConfigList);
		}

		public dynamic GetItem(string metadataName)
		{
			dynamic bodyQuery = new DynamicWrapper();
			bodyQuery.ConfigId = metadataName;

            dynamic itemResult = RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "getmetadata", null, bodyQuery).ToDynamic();

			if (itemResult.QueryResult == null)
			{
				throw new ArgumentException("Fail to make metadata request. Error: {0}",itemResult);
			}

			if (itemResult.QueryResult.Count > 0)
			{
				return itemResult.QueryResult[0].Result;
			}
			return null;
		}


	}
}
