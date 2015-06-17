using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.Api.RestApi.DataApi
{
	public sealed class MetadataApi
	{
	    private readonly string _version;

	    public MetadataApi(string version)
	    {
	        _version = version;
	    }

	    public dynamic GetMetadataList(string configuration = null, string metadata = null, string metadataType = null, string metadataId = null)
		{

			return RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "getconfigmetadatalist", null, new
			{
			    Configuration = configuration,
			    Metadata = metadata,
			    MetadataType = metadataType,
			    MetadataName = metadataId,
			}, _version).ToDynamicList();
		}

		public dynamic GetDocumentSchema(string configuration, string metadata)
		{

			return RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "getconfigmetadatalist", null, new
			{
			    Configuration = configuration,
			    Metadata = metadata,
			    MetadataType = MetadataType.Schema
			}, _version).ToDynamicList().FirstOrDefault();
		}
	}
}
