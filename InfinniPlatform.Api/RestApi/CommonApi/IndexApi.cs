using InfinniPlatform.Api.Dynamic;
using Newtonsoft.Json.Linq;
using System;

namespace InfinniPlatform.Api.RestApi.CommonApi
{
    public sealed class IndexApi
    {
        private readonly string _version;

        public IndexApi(string version = null)
        {
            _version = version;
        }

        public void RebuildIndex(string configuration, string metadata)
        {
            var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "index", "rebuildindex",null, new
            {
                Configuration = configuration,
                Metadata = metadata
            },_version);
            if (!response.IsAllOk)
            {
                throw new ArgumentException(response.Content);
            }
        }

		public bool IndexExists(string configuration, string metadata)
		{
			var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "index", "indexexists",null, new
			{
			    Configuration = configuration,
			    Metadata = metadata
            }, _version);
			if (!response.IsAllOk)
			{
				throw new ArgumentException(response.Content);
			}
			dynamic result = response.Content.ToDynamic();
			return result.IndexExists;
		}

        public dynamic GetFromIndex(string id, string configuration, string metadata)
        {
            var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "index", "getfromindex",null, new
            {
                Id = id,
                Configuration = configuration,
                Metadata = metadata
            }, _version);
            if (!response.IsAllOk)
            {
                throw new ArgumentException(response.Content);
            }
            return response.Content.ToDynamic();
        }


        public void InsertDocument(object item, string configuration,string metadata)
        {
            var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "index", "insertindex",null, new
            {
                Item = item,
                Configuration = configuration,
                Metadata = metadata
            }, _version);
            if (!response.IsAllOk)
            {
                throw new ArgumentException(response.Content);
            }
        }

        public void InsertDocumentWithTimestamp(object item, DateTime timeStamp, string configuration, string metadata)
        {
            var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "index", "insertindexwithtimestamp",null, new
            {
                Item = item,
                Configuration = configuration,
                Metadata = metadata,
                TimeStamp = timeStamp
            }, _version);
            if (!response.IsAllOk)
            {
                throw new ArgumentException(response.Content);
            }
        }
    }
}
