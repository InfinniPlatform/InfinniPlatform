using System;
using System.Collections.Generic;

using InfinniPlatform.Factories;
using InfinniPlatform.Index;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.RestfulApi.Extensions
{
    public static class IndexedStorageExtension
    {
        public static void RebuildIndex(string indexName, string typeName)
        {
            var elasticFactory = new ElasticFactory();

            var indexProvider = elasticFactory.BuildVersionBuilder(indexName, typeName);
            
            indexProvider.CreateVersion(true);

            var provider = elasticFactory.BuildIndexStateProvider();
            provider.Refresh();
        }

		public static bool IndexExists(string indexName,string typeName)
		{
			var provider = new ElasticFactory().BuildIndexStateProvider();
			return provider.GetIndexStatus(indexName, typeName) == IndexStatus.Exists;
		}

        public static dynamic GetDocument(string id, string index, string typeName)
        {
			var elasticProvider = new ElasticFactory().BuildVersionProvider(index, typeName);
            return elasticProvider.GetDocument(id);
        }

        public static dynamic GetDocuments(IEnumerable<string> ids, string index, string typeName)
        {
			var elasticProvider = new ElasticFactory().BuildVersionProvider(index, typeName);
            return elasticProvider.GetDocuments(ids);
        }

        public static void SetDocument(object item, string indexName, string typeName)
        {
			var elasticProvider = new ElasticFactory().BuildCrudOperationProvider(indexName, typeName, null);
            elasticProvider.Set(item);
            elasticProvider.Refresh();
        }

        public static void SetDocuments(IEnumerable<object> items, string indexName, string typeName)
        {
			var elasticProvider = new ElasticFactory().BuildCrudOperationProvider(indexName, typeName, null);
            elasticProvider.Set(items);
            elasticProvider.Refresh();
        }

        public static string GetStatus()
        {
            return new ElasticConnection().GetStatus();
        }

        public static void IndexWithTimestamp(object item, string indexName, string typeName, DateTime timeStamp)
        {
            var elasticConnection = new ElasticConnection();
            dynamic jInstance = item.ToDynamic();
            
            jInstance["Id"] = jInstance["Id"].ToString().ToLowerInvariant();

            var indexObject1 = new IndexObject
            {
                Id = Guid.NewGuid().ToString().ToLowerInvariant(),
                TimeStamp = DateTime.Now,
                Values = jInstance,
                TenantId = GlobalContext.GetTenantId()
            };
            var indexObject = indexObject1;
            indexObject.TimeStamp = timeStamp;

			var elasticProvider = (ElasticSearchProvider)new ElasticFactory().BuildCrudOperationProvider(indexName, typeName, null);
	        var typeNameActual = elasticProvider.ActualTypeName;
			elasticConnection.Client.Index(indexObject, d=>d.Index(indexName).Type(typeNameActual));
            elasticConnection.Refresh();
        }
    }
}
