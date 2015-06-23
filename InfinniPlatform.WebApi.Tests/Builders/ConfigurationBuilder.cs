using System;
using System.IO;
using InfinniPlatform.Api.Factories;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.BlobStorage;
using InfinniPlatform.Cassandra.Client;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.SystemConfig.RoutingFactory;

namespace InfinniPlatform.WebApi.Tests.Builders
{
    public sealed class ConfigurationBuilder
    {
        public static void InitScriptStorage()
        {
            const string assemblyName = "InfinniPlatform.Metadata.Tests.dll";

            IIndexStateProvider indexUpdater = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();
            indexUpdater.RecreateIndex("update_package", "update_package");
            indexUpdater.RecreateIndex("update_configuration", "update_configuration");

            dynamic itemConfig = new DynamicWrapper();
            itemConfig.Id = Guid.NewGuid();
            itemConfig.ConfigurationName = "QueryExecutorTest";
            itemConfig.Version = "version_metadatatests";

            Guid itemId = Guid.NewGuid();
            Guid contentId = Guid.NewGuid();

            dynamic item = new DynamicWrapper();
            item.Id = itemId;
            item.ConfigurationName = "QueryExecutorTest";
            item.ModuleId = assemblyName;
            item.Version = "version_federal";
            item.ContentId = contentId;

            ICrudOperationProvider providerConfig =
                new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider("update_configuration",
                                                                                        "update_configuration",
                                                                                        AuthorizationStorageExtensions
                                                                                            .AnonimousUser, null);
            providerConfig.Set(itemConfig);
            providerConfig.Refresh();

            ICrudOperationProvider provider =
                new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider("update_package",
                                                                                        "update_package",
                                                                                        AuthorizationStorageExtensions
                                                                                            .AnonimousUser, null);
            provider.Set(item);
            provider.Refresh();

            byte[] assemblyData = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), assemblyName));
            IBlobStorage blobStorage = CreateBlobStorage();
            blobStorage.SaveBlob(contentId, assemblyName, assemblyData);
        }

        private static IBlobStorage CreateBlobStorage()
        {
            var cassandraFactory = new CassandraDatabaseFactory();
            var blobStorageFactory = new CassandraBlobStorageFactory(cassandraFactory);
            return blobStorageFactory.CreateBlobStorage();
        }
    }
}