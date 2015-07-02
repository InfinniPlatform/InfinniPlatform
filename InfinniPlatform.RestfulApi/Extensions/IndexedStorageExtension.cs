﻿using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.RestApi.AuthApi;
using InfinniPlatform.Index;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using InfinniPlatform.SystemConfig.Multitenancy;

namespace InfinniPlatform.RestfulApi.Extensions
{
    public static class IndexedStorageExtension
    {
        public static void RebuildIndex(
            string indexName,
            string typeName, 
            SearchAbilityType abilityType = SearchAbilityType.KeywordBasedSearch)
        {
            var elasticFactory = new ElasticFactory(new MultitenancyProvider());

            var indexProvider = elasticFactory.BuildVersionBuilder(indexName, typeName, abilityType);
            
            indexProvider.CreateVersion(true);

            var provider = elasticFactory.BuildIndexStateProvider();
            provider.Refresh();
        }

		public static bool IndexExists(string indexName,string typeName)
		{
			var provider = new ElasticFactory(new MultitenancyProvider()).BuildIndexStateProvider();
			return provider.GetIndexStatus(indexName, typeName) == IndexStatus.Exists;
		}

        public static dynamic GetDocument(string id, string index, string typeName)
        {
			var elasticProvider = new ElasticFactory(new MultitenancyProvider()).BuildVersionProvider(index, typeName, AuthorizationStorageExtensions.AnonimousUser);
            return elasticProvider.GetDocument(id);
        }

        public static dynamic GetDocuments(IEnumerable<string> ids, string index, string typeName)
        {
			var elasticProvider = new ElasticFactory(new MultitenancyProvider()).BuildVersionProvider(index, typeName, AuthorizationStorageExtensions.AnonimousUser);
            return elasticProvider.GetDocuments(ids);
        }

        public static void SetDocument(object item, string indexName, string typeName)
        {
			var elasticProvider = new ElasticFactory(new MultitenancyProvider()).BuildCrudOperationProvider(indexName, typeName, AuthorizationStorageExtensions.AnonimousUser);
            elasticProvider.Set(item);
            elasticProvider.Refresh();
        }

        public static void SetDocuments(IEnumerable<object> items, string indexName, string typeName)
        {
			var elasticProvider = new ElasticFactory(new MultitenancyProvider()).BuildCrudOperationProvider(indexName, typeName, AuthorizationStorageExtensions.AnonimousUser);
            elasticProvider.Set(items);
            elasticProvider.Refresh();
        }

        public static void IndexWithTimestamp(object item, string indexName, string typeName, DateTime timeStamp, string userClaim)
        {
            var elasticConnection = new ElasticConnection();
            elasticConnection.ConnectIndex();
            dynamic jInstance = item.ToDynamic();
            
            jInstance["Id"] = jInstance["Id"].ToString().ToLowerInvariant();

            var indexObject1 = new IndexObject
            {
                Id = Guid.NewGuid().ToString().ToLowerInvariant(),
                TimeStamp = DateTime.Now,
                Values = jInstance
            };
            var indexObject = indexObject1;
            indexObject.TimeStamp = timeStamp;

			var elasticProvider = (ElasticSearchProvider)new ElasticFactory(new MultitenancyProvider()).BuildCrudOperationProvider(indexName, typeName, AuthorizationStorageExtensions.AnonimousUser);
	        var typeNameActual = elasticProvider.ActualTypeName;
			elasticConnection.Client.Index(indexObject, d=>d.Index(indexName).Type(typeNameActual).Routing(userClaim));
            elasticConnection.Client.Refresh(f=>f.Force());
        }
    }
}
