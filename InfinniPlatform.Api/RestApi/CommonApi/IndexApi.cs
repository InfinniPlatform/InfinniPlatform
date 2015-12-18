using System;

namespace InfinniPlatform.Api.RestApi.CommonApi
{
    public sealed class IndexApi
    {
        public IndexApi(RestQueryApi restQueryApi)
        {
            _restQueryApi = restQueryApi;
        }

        private readonly RestQueryApi _restQueryApi;

        public void RebuildIndex(string configuration, string metadata)
        {
            _restQueryApi.QueryPostJsonRaw("RestfulApi", "index", "rebuildindex", null, new
                                                                                        {
                                                                                            Configuration = configuration,
                                                                                            Metadata = metadata
                                                                                        });
        }

        public bool IndexExists(string configuration, string metadata)
        {
            var response = _restQueryApi.QueryPostJsonRaw("RestfulApi", "index", "indexexists", null, new
                                                                                                      {
                                                                                                          Configuration = configuration,
                                                                                                          Metadata = metadata
                                                                                                      });

            dynamic result = response.ToDynamic();
            return result.IndexExists;
        }

        public dynamic GetFromIndex(string id, string configuration, string metadata)
        {
            var response = _restQueryApi.QueryPostJsonRaw("RestfulApi", "index", "getfromindex", null, new
                                                                                                       {
                                                                                                           Id = id,
                                                                                                           Configuration = configuration,
                                                                                                           Metadata = metadata
                                                                                                       });

            return response.ToDynamic();
        }

        public void InsertDocument(object item, string configuration, string metadata)
        {
            _restQueryApi.QueryPostJsonRaw("RestfulApi", "index", "insertindex", null, new
                                                                                       {
                                                                                           Item = item,
                                                                                           Configuration = configuration,
                                                                                           Metadata = metadata
                                                                                       });
        }

        public void InsertDocumentWithTimestamp(object item, DateTime timeStamp, string configuration, string metadata)
        {
            _restQueryApi.QueryPostJsonRaw("RestfulApi", "index", "insertindexwithtimestamp", null, new
                                                                                                    {
                                                                                                        Item = item,
                                                                                                        Configuration = configuration,
                                                                                                        Metadata = metadata,
                                                                                                        TimeStamp = timeStamp
                                                                                                    });
        }
    }
}