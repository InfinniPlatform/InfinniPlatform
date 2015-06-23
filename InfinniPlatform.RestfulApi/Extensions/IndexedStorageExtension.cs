using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Index;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.SystemConfig.RoutingFactory;

namespace InfinniPlatform.RestfulApi.Extensions
{
    public static class IndexedStorageExtension
    {
        public static void RebuildIndex(
            string indexName,
            string typeName,
            SearchAbilityType abilityType = SearchAbilityType.KeywordBasedSearch)
        {
            var elasticFactory = new ElasticFactory(new RoutingFactoryBase());

            IVersionBuilder indexProvider = elasticFactory.BuildVersionBuilder(indexName, typeName, abilityType);

            indexProvider.CreateVersion(true);

            IIndexStateProvider provider = elasticFactory.BuildIndexStateProvider();
            provider.Refresh();
        }

        public static bool IndexExists(string indexName, string typeName)
        {
            IIndexStateProvider provider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();
            return provider.GetIndexStatus(indexName, typeName) == IndexStatus.Exists;
        }

        public static dynamic GetDocument(string id, string index, string typeName)
        {
            IVersionProvider elasticProvider = new ElasticFactory(new RoutingFactoryBase()).BuildVersionProvider(index,
                                                                                                                 typeName,
                                                                                                                 AuthorizationStorageExtensions
                                                                                                                     .AnonimousUser);
            return elasticProvider.GetDocument(id);
        }

        public static dynamic GetDocuments(IEnumerable<string> ids, string index, string typeName)
        {
            IVersionProvider elasticProvider = new ElasticFactory(new RoutingFactoryBase()).BuildVersionProvider(index,
                                                                                                                 typeName,
                                                                                                                 AuthorizationStorageExtensions
                                                                                                                     .AnonimousUser);
            return elasticProvider.GetDocuments(ids);
        }

        public static void SetDocument(object item, string indexName, string typeName)
        {
            ICrudOperationProvider elasticProvider =
                new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider(indexName, typeName,
                                                                                        AuthorizationStorageExtensions
                                                                                            .AnonimousUser);
            elasticProvider.Set(item);
            elasticProvider.Refresh();
        }

        public static void SetDocuments(IEnumerable<object> items, string indexName, string typeName)
        {
            ICrudOperationProvider elasticProvider =
                new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider(indexName, typeName,
                                                                                        AuthorizationStorageExtensions
                                                                                            .AnonimousUser);
            elasticProvider.Set(items);
            elasticProvider.Refresh();
        }

        public static void IndexWithTimestamp(object item, string indexName, string typeName, DateTime timeStamp,
                                              string userClaim)
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
            IndexObject indexObject = indexObject1;
            indexObject.TimeStamp = timeStamp;

            var elasticProvider =
                (ElasticSearchProvider)
                new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider(indexName, typeName,
                                                                                        AuthorizationStorageExtensions
                                                                                            .AnonimousUser);
            string typeNameActual = elasticProvider.ActualTypeName;
            elasticConnection.Client.Index(indexObject, d => d.Index(indexName).Type(typeNameActual).Routing(userClaim));
            elasticConnection.Client.Refresh(f => f.Force());
        }
    }
}