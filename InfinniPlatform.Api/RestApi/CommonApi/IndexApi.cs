using InfinniPlatform.Api.Dynamic;
using Newtonsoft.Json.Linq;
using System;

namespace InfinniPlatform.Api.RestApi.CommonApi
{
    public static class IndexApi
    {
        public static void RebuildIndex(string configuration, string metadata)
        {
            var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "index", "rebuildindex",null, new
                {
                    Configuration = configuration,
                    Metadata = metadata
                });
            if (!response.IsAllOk)
            {
                throw new ArgumentException(response.Content);
            }
        }

		public static bool IndexExists(string configuration, string metadata)
		{
			var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "index", "indexexists",null, new
			{
				Configuration = configuration,
				Metadata = metadata
			});
			if (!response.IsAllOk)
			{
				throw new ArgumentException(response.Content);
			}
			dynamic result = response.Content.ToDynamic();
			return result.IndexExists == true;
		}

        public static dynamic GetFromIndex(string id, string configuration, string metadata)
        {
            var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "index", "getfromindex",null, new
                {
                    Id = id,
                    Configuration = configuration,
                    Metadata = metadata
                });
            if (!response.IsAllOk)
            {
                throw new ArgumentException(response.Content);
            }
            return response.Content.ToDynamic();
        }


        public static void InsertDocument(object item, string configuration,string metadata)
        {
            var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "index", "insertindex",null, new
                {
                    Item = item,
                    Configuration = configuration,
                    Metadata = metadata
                });
            if (!response.IsAllOk)
            {
                throw new ArgumentException(response.Content);
            }
        }

        public static void InsertDocumentWithTimestamp(object item, DateTime timeStamp, string configuration, string metadata)
        {
            var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "index", "insertindexwithtimestamp",null, new
                {
                    Item = item,
                    Configuration = configuration,
                    Metadata = metadata,
                    TimeStamp = timeStamp
                });
            if (!response.IsAllOk)
            {
                throw new ArgumentException(response.Content);
            }
        }
    }
}
