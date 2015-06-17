using System;
using System.Collections.Generic;
using System.Diagnostics;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders
{
	public sealed class MetadataReaderConfiguration : IDataReader
	{
	    private readonly string _version;
	    private readonly bool _doNotCheckVersion;

	    public MetadataReaderConfiguration(string version, bool doNotCheckVersion = false)
	    {
	        _version = version;
	        _doNotCheckVersion = doNotCheckVersion;
	    }

	    public IEnumerable<dynamic> GetItems()
	    {
	        dynamic body = new DynamicWrapper();
	        body.DoNotCheckVersion = _doNotCheckVersion;
	        
            return DynamicWrapperExtensions.ToEnumerable(RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "getregisteredconfiglist", null, body, _version).ToDynamic().ConfigList);
		}

		public dynamic GetItem(string metadataName)
		{
			dynamic bodyQuery = new DynamicWrapper();
			bodyQuery.ConfigId = metadataName;

            dynamic itemResult = RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "getmetadata", null, bodyQuery, _version ).ToDynamic();

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
